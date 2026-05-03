using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;
    public float acceleration = 10f;

    [Header("Cámara")]
    public float mouseSensitivity = 2f;

    [Header("Salto")]
    public float jumpForce = 6f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool isJumping = false;
    private float jumpCooldown = 0f;

    [Header("Suelo")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Arma")]
    public GameObject weaponInHand;
    private bool hasWeapon = false;
    public bool HasWeapon => hasWeapon;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask enemyLayer;

    private Rigidbody rb;
    private Animator animator;

    [Header("Gravedad")]
    public float fallMultiplier = 4f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;

        if (weaponInHand != null)
            weaponInHand.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ROTACIÓN RATÓN (SOLO EJE X)
        float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // INPUT SALTO (BUFFER)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // DETECCIÓN DE SUELO
        bool grounded = IsGrounded();
        animator.SetBool("isGrounded", grounded);

        // Cooldown del salto
        jumpCooldown -= Time.deltaTime;

        // Resetea isJumping al aterrizar
        if (grounded && rb.linearVelocity.y <= 0.1f && jumpCooldown <= 0f)
            isJumping = false;

        // COYOTE TIME
        if (grounded && !isJumping)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // SALTO
        if (jumpBufferTimer > 0f && coyoteTimer > 0f && !isJumping)
        {
            Jump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // ATAQUE
        if (hasWeapon && Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("attack");
            Attack();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        Vector3 moveDir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;

        Vector3 velocity = rb.linearVelocity;
        Vector3 targetVelocity = moveDir * speed;

        Vector3 smooth = Vector3.Lerp(
            new Vector3(velocity.x, 0, velocity.z),
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(smooth.x, velocity.y, smooth.z);

        animator.SetBool("isWalking", moveInput.magnitude > 0.1f);

        // GRAVEDAD MEJORADA
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("jump");
        isJumping = true;
        jumpCooldown = 3f;
    }

    void Attack()
    {
        if (attackPoint == null)
        {
            Debug.Log("attackPoint es null");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        Debug.Log($"Hits detectados: {hits.Length}");

        foreach (Collider hit in hits)
        {
            Debug.Log($"Hit: {hit.gameObject.name}");
            ZombieJump zj = hit.GetComponentInParent<ZombieJump>();
            if (zj != null)
            {
                zj.TakeHit();
                continue;
            }

            ZombieGordo zg = hit.GetComponentInParent<ZombieGordo>();
            if (zg != null)
            {
                zg.TakeHit();
            }
        }
    }

    public void PickWeapon()
    {
        hasWeapon = true;

        if (weaponInHand != null)
            weaponInHand.SetActive(true);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        return Physics.CheckSphere(groundCheck.position, groundRadius, ~0);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}