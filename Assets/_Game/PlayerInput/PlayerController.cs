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

    [Header("Left/Right movement")]
    [SerializeField]
    private float m_moveAcceleration = 100f;
    [SerializeField]
    private float m_maxSpeed = 100f;
    [SerializeField]
    private float m_inAirMovementMultiplier = .5f;

    [Header("Jump")]
    private float m_jumpForce = 500f;
    [SerializeField]

    [Header("Auto run")]
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
        RaycastHit hitInfo;
        float multiplier = m_isGrounded ? 1f : m_inAirMovementMultiplier;

        if (Physics.SphereCast(m_groundCheck.position, m_groundCheckRadius, Vector3.down, out hitInfo, m_groundCheckLayers))
        {
            m_isGrounded = false;
        }
        else
        {
            m_isGrounded = true;
        }

        if (Mathf.Abs(m_rigidbody.velocity.x) < m_maxSpeed)
            m_rigidbody.AddForce(transform.right * m_moveAcceleration * m_rigidbodyMass * m_inputX * multiplier * Time.fixedDeltaTime, ForceMode.Acceleration);

        if (m_rigidbody.velocity.z < m_maxRunSpeed)
        {
            // Apply constant force backwards for auto running
            m_rigidbody.AddForce(Vector3.forward * m_runAcceleration * m_rigidbodyMass, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        if (m_isGrounded)
        {
            m_rigidbody.AddForce(transform.up * m_jumpForce / m_rigidbodyMass, ForceMode.Impulse);
        }
    }

    #region DEBUG
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(m_groundCheck.position, m_groundCheckRadius);
    }
    #endregion
}
