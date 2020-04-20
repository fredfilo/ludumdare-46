using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Monkey : MonoBehaviour, Notifiable
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
    [SerializeField] private bool m_isGrounded;
    
    [SerializeField] private float m_jumpForce = 3f;
    [SerializeField] private int m_jumpMaxIterations = 4;
    [SerializeField] private float m_maxFallVelocity = 12f;
    private int m_jumpIterations;
    [SerializeField] private bool m_isJumping;
    [SerializeField] private bool m_jumpHold;
    [SerializeField] private bool m_isDiving;

    [SerializeField] private Transform m_fireTarget;
    [SerializeField] private Transform m_waterTarget;
    private Transform m_currentTarget;
    [SerializeField] private Types.PathTo m_currentPathTo;

    [SerializeField] private int m_currentState;
    [SerializeField] private float m_currentStateStartedAt;
    [SerializeField] private float m_currentStateExpiresAt;

    [SerializeField] private bool m_isFull;

    private float m_wasIdleAt;
    [SerializeField] private float m_maxDurationWithoutIdle = 5f;

    [SerializeField] private int m_combustiblePerPee = 1;

    private bool m_isControllable = true;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public Types.PathTo currentPathTo
    {
        get => m_currentPathTo;
        set => m_currentPathTo = value;
    }

    public bool isFull
    {
        get => m_isFull;
        set => m_isFull = value;
    }

    public Transform spriteContainer => m_spritesContainer;

    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void ChangeOrientation()
    {
        m_isFacingRight = !m_isFacingRight;
        Vector3 scale = m_spritesContainer.localScale;
        scale.x = Mathf.Abs(scale.x) * (m_isFacingRight ? 1f : -1f);
        m_spritesContainer.localScale = scale;
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        
        DecideState();
        
        GameController.instance.notifier.Subscribe(Notification.Type.WIN, this);
        GameController.instance.notifier.Subscribe(Notification.Type.LOSE, this);
    }

    private void OnDestroy()
    {
        GameController.instance.notifier.Unsubscribe(Notification.Type.WIN, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.LOSE, this);
    }

    private void Update()
    {
        CheckOrientation();
        CheckGround();
        CheckInput();
        SetAnimatorParameters();

        if (Time.time > m_currentStateExpiresAt) {
            DecideState();
        }
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
            ChangeOrientation();
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
        if (!m_isControllable) {
            m_input.x = 0;
            m_velocity.x = 0;
            return;
        }
        
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
        if (m_rigidBody.velocity.y < 0) {
            m_animator.SetInteger(AnimatorParameters.State, STATE_FALL);
            return;
        }
        
        if (m_rigidBody.velocity.y > 0) {
            m_animator.SetInteger(AnimatorParameters.State, STATE_JUMP);
            return;
        }
        
        m_animator.SetInteger(AnimatorParameters.State, m_currentState);
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
                return 0.3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_currentPathTo == Types.PathTo.FIRE && other.CompareTag("FireForMonkey")) {
            GameController.instance.fire.IncreaseCombustible(-m_combustiblePerPee);
            SetState(STATE_PEE);
            m_isFull = false;
            return;
        }
        
        if (m_currentPathTo == Types.PathTo.WATER && other.CompareTag("WaterForMonkey")) {
            SetState(STATE_DIVE);
            ChangeOrientation();
            m_isFull = true;
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        MonkeyJumper jumper = other.GetComponent<MonkeyJumper>();        
        if (jumper && jumper.pathTo == m_currentPathTo) {
            SetState(STATE_JUMP);
            m_jumpHold = true;
        }
    }

    private void SetState(int state)
    {
        //Debug.Log("SetState: " + state);
        m_currentState = state;

        switch (m_currentState) {
            case STATE_RUN:
            case STATE_JUMP:
            case STATE_FALL:
                m_input.x = m_isFacingRight ? 1f : -1f;
                break;
            case STATE_IDLE:
                m_wasIdleAt = Time.time;
                break;
            default:
                m_input.x = 0;
                break;
        }

        m_currentStateExpiresAt = Time.time + GetStateDuration(m_currentState);
    }

    private void DecideState()
    {
        m_currentPathTo = m_isFull ? Types.PathTo.FIRE : Types.PathTo.WATER;

        switch (m_currentState) {
            case STATE_PEE:
                SetState(STATE_HAPPY);
                return;
            case STATE_HAPPY:
                SetState(STATE_RUN);
                return;
            case STATE_RUN:
                if (m_isGrounded && Time.time - m_wasIdleAt > m_maxDurationWithoutIdle) {
                    SetState(STATE_IDLE);
                    return;
                }
                break;
        }

        SetState(STATE_RUN);
    }

    public void OnNotification(Notification notification, Notifier notifier)
    {
        if (notification.type == Notification.Type.WIN || notification.type == Notification.Type.LOSE) {
            m_isControllable = false;
        }
    }
}
