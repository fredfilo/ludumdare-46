using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour, Notifiable
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
    [SerializeField] private Tilemap m_groundTilemap;

    [SerializeField] private float m_jumpForce = 3f;
    [SerializeField] private int m_jumpMaxIterations = 4;
    [SerializeField] private float m_jumpCoyoteDuration = 0.1f;
    [SerializeField] private float m_maxFallVelocity = 12f;
    private int m_jumpIterations;
    private bool m_isJumping;
    private bool m_jumpHold;

    private bool m_isControllable = true;
    private bool m_isDrowning;
    private bool m_isThrowing;
    private bool m_isShowingOff;
    private bool m_shouldShowOff;
    [SerializeField] private float m_chanceOfShowOffAfterThrow = 0.1f;
    
    [SerializeField] private PlayerWeapon m_weapon;
    [SerializeField] private Vector3 m_throwForce = new Vector3(300f, 300f, 0);
    
    private readonly List<Interactable> m_interactables = new List<Interactable>();

    private GameObject m_pickedUp;
    private Types.PickupType m_pickedUpType = Types.PickupType.NONE;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public bool Pickup(Types.PickupType pickupPickupType, GameObject pickupObject)
    {
        if (m_pickedUp != null) {
            return false;
        }

        m_pickedUpType = pickupPickupType;
        
        switch (pickupPickupType) {
            case Types.PickupType.WOOD:
                m_pickedUp = pickupObject;
                m_pickedUp.transform.parent = transform;
                m_pickedUp.transform.localPosition = new Vector3(0, 1f, 0);
                m_pickedUp.transform.rotation = Quaternion.identity;
                m_pickedUp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                m_pickedUp.GetComponent<Collider2D>().enabled = false;
                break;
            case Types.PickupType.AXE:
                m_pickedUp = pickupObject;
                m_pickedUp.SetActive(false);
                break;
        }

        return true;
    }
    
    public void OnCannotWalk()
    {
        m_isDrowning = true;

        if (m_pickedUp == null || m_pickedUpType == Types.PickupType.AXE) {
            return;
        }

        Vector3 throwForce = m_throwForce;
        if (!m_isFacingRight) {
            throwForce.x *= -1f;
        }
        
        m_pickedUp.transform.parent = transform.parent;
        m_pickedUp.GetComponent<Collider2D>().enabled = true;
        Rigidbody2D pickedUpRigidBody = m_pickedUp.GetComponent<Rigidbody2D>();
        pickedUpRigidBody.bodyType = RigidbodyType2D.Dynamic;
        pickedUpRigidBody.AddForce(throwForce);

        m_pickedUp = null;
        m_pickedUpType = Types.PickupType.NONE;
    }

    public void OnDrownFinished()
    {
        m_isDrowning = false;
        transform.position = m_wasGroundedAtPosition;
    }
    
    public void OnThrowFinished()
    {
        m_isThrowing = false;

        if (m_shouldShowOff) {
            m_shouldShowOff = false;
            m_isShowingOff = true;
        }
    }

    public void OnShowOffFinished()
    {
        m_isShowingOff = false;
    }

    public void OnThrowAttack()
    {
        if (m_pickedUp == null || m_pickedUpType == Types.PickupType.AXE) {
            m_weapon.Attack();
            GameController.instance.PlayAttackTree();
            return;
        }

        Vector3 throwForce = m_throwForce; 
        
        if (Random.value < m_chanceOfShowOffAfterThrow) {
            m_shouldShowOff = true;
            throwForce = Vector2.up * m_throwForce.magnitude;
            GameController.instance.PlayShowOff();
        }

        if (!m_isFacingRight) {
            throwForce.x *= -1f;
        }
        
        m_pickedUp.transform.parent = transform.parent;
        m_pickedUp.GetComponent<Collider2D>().enabled = true;
        Rigidbody2D pickedUpRigidBody = m_pickedUp.GetComponent<Rigidbody2D>();
        pickedUpRigidBody.bodyType = RigidbodyType2D.Dynamic;
        pickedUpRigidBody.AddForce(throwForce);

        m_pickedUp = null;
        m_pickedUpType = Types.PickupType.NONE;
        
        GameController.instance.PlayThrow();
    }
    
    public void OnInputMove(InputAction.CallbackContext context)
    {
        if (!m_isControllable) {
            return;
        }
        
        m_input.x = context.ReadValue<Vector2>().x;
        m_inputAbsolute.x = Mathf.Abs(m_input.x);
    }
    
    public void OnInputJump(InputAction.CallbackContext context)
    {
        if (!m_isControllable) {
            return;
        }
        
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
        if (!m_isControllable) {
            return;
        }
        
        if (context.phase == InputActionPhase.Performed) {
            m_isThrowing = true;
        }
    }
    
    public void OnInputInteract(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) {
            return;
        }
        
        if (!m_isControllable) {
            GameController.instance.notifier.Notify(new PlayerWantsToInteract());
            return;
        }
        
        if (m_interactables.Count > 0) {
            Interactable interactable = m_interactables[0];
            interactable.Interact(this);
        }
    }
    
    public void OnNotification(Notification notification, Notifier notifier)
    {
        switch (notification.type) {
            case Notification.Type.UI_INTERACTION_STARTED:
                m_isControllable = false;
                break;
            case Notification.Type.UI_INTERACTION_ENDED:
                m_isControllable = true;
                break;
            case Notification.Type.WIN:
                Debug.Log("WIN");
                // m_isControllable = false;
                // m_isShowingOff = true;
                // Invoke(nameof(DontShowOff), 1f);
                break;
            case Notification.Type.LOSE:
                Debug.Log("LOSE");
                // m_isControllable = false;
                break;
        }
    }

    private void DontShowOff()
    {
        m_isShowingOff = false;
    }

    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        
        GameController.instance.notifier.Subscribe(Notification.Type.UI_INTERACTION_STARTED, this);
        GameController.instance.notifier.Subscribe(Notification.Type.UI_INTERACTION_ENDED, this);
        GameController.instance.notifier.Subscribe(Notification.Type.WIN, this);
        GameController.instance.notifier.Subscribe(Notification.Type.LOSE, this);
    }

    private void OnDestroy()
    {
        GameController.instance.notifier.Unsubscribe(Notification.Type.UI_INTERACTION_STARTED, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.WIN, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.LOSE, this);
    }

    private void Update()
    {
        CheckOrientation();
        CheckGround();
        CheckInput();
        SetAnimatorParameters();

        if (m_pickedUp != null && m_pickedUpType != Types.PickupType.AXE) {
            m_pickedUp.transform.localPosition = new Vector3(0, 1f, 0);
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
            if (hit.collider.CompareTag("Ground")) {
                m_wasGroundedAtPosition = transform.position;
            }
            return;
        }

        origin.x += m_collisionCollider.bounds.size.x;
        hit = Physics2D.Raycast(origin, m_checkGroundDirection, m_checkGroundDistance, m_groundLayers);
        if (hit.collider) {
            m_isGrounded = true;
            m_wasGroundedAt = Time.time;
            if (hit.collider.CompareTag("Ground")) {
                m_wasGroundedAtPosition = transform.position;
            }
        }
    }

    private void CheckInput()
    {
        // Reset
        m_velocity.x = 0;

        if (!m_isControllable || m_isShowingOff) {
            return;
        }
        
        // Movement
        if (!m_isThrowing && m_inputAbsolute.x >= MIN_INPUT_FOR_RUN && !m_isDrowning) {
            m_velocity.x = m_input.x * m_movementSpeed;
        }

        // Jump
        if (!m_isJumping && m_jumpHold && (m_isGrounded || (Time.time - m_wasGroundedAt) <= m_jumpCoyoteDuration)) {
            GameController.instance.PlayJump();
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
        m_animator.SetBool(AnimatorParameters.IsShowingOff, m_isShowingOff);
        m_animator.SetFloat(AnimatorParameters.InputX, m_input.x);
        m_animator.SetFloat(AnimatorParameters.VelocityY, m_rigidBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && !m_interactables.Contains(interactable)) {
            m_interactables.Insert(0, interactable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && m_interactables.Contains(interactable)) {
            m_interactables.Remove(interactable);
        }
    }
}
