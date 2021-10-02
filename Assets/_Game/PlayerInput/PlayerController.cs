using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 10f;
    [SerializeField]
    private float m_maxSpeed = 100f;
    [SerializeField]
    private Rigidbody m_rigidbody;
    [SerializeField]
    private Transform m_groundCheck;
    [SerializeField]
    private float m_groundCheckRadius;
    [SerializeField]
    private LayerMask m_groundCheckLayers;
    [SerializeField]
    private float m_jumpForce = 500f;

    [HideInInspector]
    public PlayerInputActions m_inputActions;

    private float m_inputX;
    private bool m_isGrounded;

    private void Awake()
    {
        m_inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        m_inputActions.Enable();

        m_inputActions.Gameplay.Jump.started += ctx => Jump();
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

        if(Physics.SphereCast(m_groundCheck.position, m_groundCheckRadius, Vector3.down, out hitInfo, m_groundCheckLayers))
        {
            m_isGrounded = false;
        } else
        {
            m_isGrounded = true;
        }

        if (Mathf.Abs(m_rigidbody.velocity.x) < m_maxSpeed)
            m_rigidbody.AddForce(transform.right * m_speed * m_inputX * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void Jump()
    {
        if(m_isGrounded)
        {
            m_rigidbody.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
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
