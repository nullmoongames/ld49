using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody m_rigidbody;
    [SerializeField]
    private Transform m_groundCheck;
    [SerializeField]
    private float m_groundCheckRadius;
    [SerializeField]
    private LayerMask m_groundCheckLayers;
    [SerializeField]
    private Transform m_frontCheck;
    [SerializeField]
    private float m_frontCheckRadius;
    [SerializeField]
    private LayerMask m_frontCheckLayerMask;
    [SerializeField]
    Animator m_animator;
    [SerializeField]
    Transform m_pirateRunnerMesh;

    [Header("Left/Right movement")]
    [SerializeField]
    private float m_moveAcceleration = 100f;
    [SerializeField]
    private float m_maxSpeed = 100f;
    [SerializeField]
    private float m_inAirMovementMultiplier = .5f;

    [Header("Jump")]
    [SerializeField]
    private float m_jumpForce = 500f;
    [SerializeField]
    private float m_gravity = 9.81f;

    [Header("Auto run")]
    [SerializeField]
    private float m_maxRunSpeed = 100f;
    [SerializeField]
    private float m_runAcceleration = 5f;

    [Header("VFX/SFX")]
    [SerializeField]
    private VisualEffect m_speedVFX;
    [SerializeField]
    private CinemachineImpulseSource m_cinemachineImpulseSource;
    [SerializeField]
    private AudioSource m_speedBoostAudioSource;
    [SerializeField]
    private AudioSource m_ImpactAudioSource;
    [SerializeField]
    private AudioSource m_ouchAudioSource;

    [HideInInspector]
    public PlayerInputActions m_inputActions;

    private float m_inputX;
    private bool m_isGrounded;
    private float m_rigidbodyMass;
    private float forwardMultiplier = 1f;

    private void Awake()
    {
        m_inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        m_inputActions.Enable();

        m_inputActions.Gameplay.Jump.started += ctx => Jump();
        m_rigidbodyMass = m_rigidbody.mass;

        m_speedVFX.Stop();
    }

    private void OnDisable()
    {
        m_inputActions.Disable();
    }

    private void Update()
    {
        m_inputX = m_inputActions.Gameplay.Move.ReadValue<Vector2>().x;
        UpdateAnimator();

        Vector3 angles = m_pirateRunnerMesh.eulerAngles;
        angles.y = 24 * m_inputX;
        m_pirateRunnerMesh.eulerAngles = angles;
    }

    private void FixedUpdate()
    {
        float multiplier = m_isGrounded ? 1f : m_inAirMovementMultiplier;
        Vector3 velocity = m_rigidbody.velocity;

        Collider[] colliders = Physics.OverlapSphere(m_groundCheck.position, m_groundCheckRadius, m_groundCheckLayers);

        if (colliders.Length > 0)
        {
            m_isGrounded = true;

            foreach(Collider collider in colliders)
            {
                if (collider.CompareTag("SpeedBoost"))
                {
                    StartSpeedBoost();
                }
            }
        }
        else
        {
            m_isGrounded = false;
        }

        if (Mathf.Abs(m_rigidbody.velocity.x) < m_maxSpeed)
            m_rigidbody.AddForce(transform.right * m_moveAcceleration * m_rigidbodyMass * m_inputX * multiplier * Time.fixedDeltaTime, ForceMode.Acceleration);


        if (m_rigidbody.velocity.z < m_maxRunSpeed)
        {
            // Apply constant force backwards for auto running
            m_rigidbody.AddForce(transform.forward * m_runAcceleration * forwardMultiplier, ForceMode.Force);
        }

        // Apply gravity
        if(!m_isGrounded)
        {
            velocity.y -= m_gravity;
            m_rigidbody.velocity = velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            if (LevelGenerationManager.instance == null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
            GameEventController.instance.DeathEvent();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boat"))
            GameEventController.instance.NewEntryBoat(other.transform.parent);
        else if(other.gameObject.layer == LayerMask.NameToLayer("SpeedBoost"))
        {
            StartSpeedBoost();
            Destroy(other.gameObject, 0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Obstacle"))
        {
            m_animator.SetTrigger("Stunned");

            m_cinemachineImpulseSource.GenerateImpulse(Vector3.one);

            m_ouchAudioSource.Play();
            m_ImpactAudioSource.pitch = Random.Range(1f, 1.5f);
            m_ImpactAudioSource.Play();
        }
    }

    private void Jump()
    {
        if (m_isGrounded)
        {
            m_rigidbody.AddForce(transform.up * m_jumpForce / m_rigidbodyMass, ForceMode.Impulse);
        }
    }

    public void HitObstacle(float obstacleForce)
    {
        m_rigidbody.AddForce(Vector3.back * obstacleForce * m_rigidbodyMass, ForceMode.Impulse);
    }

    public void ResetPlayerVelocity() 
    {
        m_rigidbody.velocity = Vector3.zero;
    }

    private void UpdateAnimator()
    {
        m_animator.SetFloat("VelocityY", m_rigidbody.velocity.y);
        m_animator.SetBool("Grounded", m_isGrounded);
    }

    private void StartSpeedBoost()
    {
        forwardMultiplier = 10f;
        if (m_speedVFX.aliveParticleCount < 1)
            m_speedVFX.Play();
        m_speedVFX.playRate = 3f;

        if (!m_speedBoostAudioSource.isPlaying)
        {
            m_speedBoostAudioSource.pitch = Random.Range(.8f, 1.2f);
            m_speedBoostAudioSource.Play();
        }

        Invoke("StopSpeedBoost", 2f);
    }

    private void StopSpeedBoost()
    {
        m_speedVFX.Stop();
        forwardMultiplier = 1f;
    }

    #region DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(m_groundCheck.position, m_groundCheckRadius);
        Gizmos.DrawSphere(m_frontCheck.position, m_frontCheckRadius);
        Ray ray = new Ray();
        ray.origin = m_groundCheck.position;
        ray.direction = Vector3.down;
        Gizmos.DrawRay(ray);
    }
    #endregion
}
