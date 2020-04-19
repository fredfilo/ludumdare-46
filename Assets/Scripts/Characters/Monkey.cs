using UnityEngine;
using UnityEngine.Tilemaps;

public class Monkey : MonoBehaviour
{
    // STATIC
    // -------------------------------------------------------------------------

    private const int STATE_IDLE = 0; 
    private const int STATE_RUN = 1; 
    private const int STATE_HAPPY = 2; 
    private const int STATE_JUMP = 3; 
    private const int STATE_FALL = 4; 
    private const int STATE_PEE = 5; 
    private const int STATE_DIVE = 6;
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private Collider2D m_collisionCollider;
    [SerializeField] private bool m_isFacingRight = true;
    [SerializeField] private Transform m_spritesContainer;
    
    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private Vector3 m_input;
    private Vector3 m_velocity;

    [SerializeField] private LayerMask m_groundLayers;
    private float m_checkGroundDistance = 0.1f;
    private readonly Vector2 m_checkGroundDirection = Vector2.down;
    private bool m_isGrounded;
    
    [SerializeField] private float m_jumpForce = 3f;
    [SerializeField] private int m_jumpMaxIterations = 4;
    [SerializeField] private float m_maxFallVelocity = 12f;
    private int m_jumpIterations;
    private bool m_isJumping;
    private bool m_jumpHold;

    private bool m_isDiving;

    [SerializeField] private Transform m_fireTarget;
    [SerializeField] private Transform m_waterTarget;
    private Transform m_currentTarget;

    private int m_currentState;
    private float m_currentStateStartedAt;
    private float m_currentStateExpiresAt;
    
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
            return;
        }

        origin.x += m_collisionCollider.bounds.size.x;
        hit = Physics2D.Raycast(origin, m_checkGroundDirection, m_checkGroundDistance, m_groundLayers);
        if (hit.collider) {
            m_isGrounded = true;
        }
    }

    private void CheckInput()
    {
        // Reset
        m_velocity.x = 0;
        
        // Movement
        if (!m_isDiving) {
            m_velocity.x = m_input.x * m_movementSpeed;
        }

        // Jump
        if (!m_isJumping && m_jumpHold && m_isGrounded) {
            m_jumpHold = false;
            m_isJumping = true;
            m_jumpIterations = 0;
        }
    }
    
    private void SetAnimatorParameters()
    {
        m_animator.SetInteger(AnimatorParameters.State, STATE_IDLE);
    }

    private float GetStateDuration(int state)
    {
        switch (state) {
            case STATE_IDLE:
                return 3f;
            case STATE_RUN:
                return 5f;
            case STATE_HAPPY:
                return 1f;
            case STATE_PEE:
                return 2.5f;
            case STATE_DIVE:
                return 3f;
            default:
                return 0;
        }
    }
}
