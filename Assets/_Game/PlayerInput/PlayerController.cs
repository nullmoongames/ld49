using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Left/Right movement")]
    [SerializeField]
    private float m_moveAcceleration = 100f;
    [SerializeField]
    private float m_maxSpeed = 100f;
    [SerializeField]
    private float m_inAirMovementMultiplier = .5f;

    [Header("Jump")]
    private float m_jumpForce = 500f;

    [Header("Auto run")]
    [SerializeField]
    private float m_maxRunSpeed = 100f;
    [SerializeField]
    private float m_runAcceleration = 5f;

    [HideInInspector]
    public PlayerInputActions m_inputActions;

    private float m_inputX;
    private bool m_isGrounded;
    private float m_rigidbodyMass;

    private void Awake()
    {
        m_inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        m_inputActions.Enable();

        m_inputActions.Gameplay.Jump.started += ctx => Jump();
        m_rigidbodyMass = m_rigidbody.mass;
    }

    private void OnDisable()
    {
        m_inputActions.Disable();
    }

    private void Update()
    {
        m_inputX = m_inputActions.Gameplay.Move.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        float multiplier = m_isGrounded ? 1f : m_inAirMovementMultiplier;

        Collider[] colliders = Physics.OverlapSphere(m_groundCheck.position, m_groundCheckRadius, m_groundCheckLayers);

        if (colliders.Length > 0)
        {
            m_isGrounded = true;
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
            m_rigidbody.AddForce(Vector3.forward * m_runAcceleration, ForceMode.Acceleration);
        }

        Debug.Log($"Grounded: {m_isGrounded}");
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
            GameManager.instance.DeathEvent();
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
