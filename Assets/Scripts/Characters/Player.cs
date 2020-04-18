using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private const float MIN_INPUT_FOR_RUN = 0.25f;
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private Collider2D m_collisionCollider;
    [SerializeField] private bool m_isFacingRight = true;
    [SerializeField] private Transform m_spritesContainer;
    
    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private Vector3 m_velocity;
    private Vector3 m_input;
    private Vector3 m_inputAbsolute;

    [SerializeField] private LayerMask m_groundLayers;
    private float m_checkGroundDistance = 0.1f;
    private readonly Vector2 m_checkGroundDirection = Vector2.down;
    private bool m_isGrounded;
    private float m_wasGroundedAt;
    private Vector3 m_wasGroundedAtPosition;

    [SerializeField] private float m_jumpForce = 3f;
    [SerializeField] private int m_jumpMaxIterations = 4;
    [SerializeField] private float m_jumpCoyoteDuration = 0.1f;
    [SerializeField] private float m_maxFallVelocity = 12f;
    private int m_jumpIterations;
    private bool m_isJumping;
    private bool m_jumpHold;

    private bool m_isDrowning;
    private bool m_isThrowing;

    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void OnCannotWalk()
    {
        m_isDrowning = true;
    }

    public void OnDrownFinished()
    {
        m_isDrowning = false;
        transform.position = m_wasGroundedAtPosition;
    }
    
    public void OnThrowFinished()
    {
        m_isThrowing = false;
    }
    
    public void OnInputMove(InputAction.CallbackContext context)
    {
        m_input.x = context.ReadValue<Vector2>().x;
        m_inputAbsolute.x = Mathf.Abs(m_input.x);
    }
    
    public void OnInputJump(InputAction.CallbackContext context)
    {
        switch (context.phase) {
            case InputActionPhase.Started:
                m_jumpHold = true;
                break;
            case InputActionPhase.Performed:
                m_jumpHold = true;
                break;
            case InputActionPhase.Canceled:
                m_jumpHold = false;
                break;
        }
    }
    
    public void OnInputUseEquipped(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            m_isThrowing = true;
        }
    }

    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckOrientation();
        CheckGround();
        CheckInput();
        SetAnimatorParameters();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_rigidBody.velocity;
        velocity.x = m_velocity.x;
        
        if (m_isJumping) {
            velocity.y += m_jumpForce;
            m_jumpIterations++;
            if (m_jumpIterations >= m_jumpMaxIterations) {
                m_isJumping = false;
                m_jumpIterations = 0;
            }
        }

        if (velocity.y < -m_maxFallVelocity) {
            velocity.y = -m_maxFallVelocity;
        }

        m_rigidBody.velocity = velocity;
    }

    private void CheckOrientation()
    {
        if (m_input.x > 0 && !m_isFacingRight || m_input.x < 0 && m_isFacingRight) {
            m_isFacingRight = !m_isFacingRight;
            Vector3 scale = m_spritesContainer.localScale;
            scale.x = Mathf.Abs(scale.x) * (m_isFacingRight ? 1f : -1f);
            m_spritesContainer.localScale = scale;
        }
    }

    private void CheckGround()
    {
        m_isGrounded = false;

        Vector2 origin = m_collisionCollider.bounds.min;
        RaycastHit2D hit = Physics2D.Raycast(origin, m_checkGroundDirection, m_checkGroundDistance, m_groundLayers);
        if (hit.collider) {
            m_isGrounded = true;
            m_wasGroundedAt = Time.time;
            m_wasGroundedAtPosition = transform.position;
            return;
        }

        origin.x += m_collisionCollider.bounds.size.x;
        hit = Physics2D.Raycast(origin, m_checkGroundDirection, m_checkGroundDistance, m_groundLayers);
        if (hit.collider) {
            m_isGrounded = true;
            m_wasGroundedAt = Time.time;
            m_wasGroundedAtPosition = transform.position;
        }
    }

    private void CheckInput()
    {
        // Reset
        m_velocity.x = 0;

        // Movement
        if (!m_isThrowing && m_inputAbsolute.x >= MIN_INPUT_FOR_RUN && !m_isDrowning) {
            m_velocity.x = m_input.x * m_movementSpeed;
        }

        // Jump
        if (!m_isJumping && m_jumpHold && (m_isGrounded || (Time.time - m_wasGroundedAt) <= m_jumpCoyoteDuration)) {
            m_isJumping = true;
            m_jumpIterations = 0;
        }

        if (m_isJumping && !m_jumpHold) {
            m_isJumping = false;
            m_jumpIterations = 0;
        }
    }
    
    private void SetAnimatorParameters()
    {
        m_animator.SetBool(AnimatorParameters.IsGrounded, m_isGrounded);
        m_animator.SetBool(AnimatorParameters.IsRunning, m_inputAbsolute.x >= MIN_INPUT_FOR_RUN);
        m_animator.SetBool(AnimatorParameters.IsDrowning, m_isDrowning);
        m_animator.SetBool(AnimatorParameters.IsThrowing, m_isThrowing);
        m_animator.SetFloat(AnimatorParameters.InputX, m_input.x);
        m_animator.SetFloat(AnimatorParameters.VelocityY, m_rigidBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pickup pickup = other.GetComponent<Pickup>();
        if (pickup != null) {
            Debug.Log(pickup.type);
        }
    }
}
