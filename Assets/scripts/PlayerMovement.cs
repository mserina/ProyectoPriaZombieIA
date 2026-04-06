using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Cámara")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    [Header("Suelo")]
    public float groundCheckDistance = 41.7f; // distancia del rayo hacia abajo
    public LayerMask groundLayer;            // asigna la layer del suelo en el inspector

    private Rigidbody rb;
    private bool jumpRequested = false; // se activa en Update, se consume en FixedUpdate

    [Header("Gravedad")]
    public float fallMultiplier = 3f;    // gravedad extra al caer
    public float lowJumpMultiplier = 2f; // gravedad extra al subir (corta la flotación)
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Rotación horizontal con ratón
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseDelta.x);

        // Capturar intención de salto en Update, donde GetKeyDown es fiable
        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        
        // Gravedad extra al caer
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Physics.gravity * fallMultiplier, ForceMode.Acceleration);
        }
        // Gravedad extra al subir, para que no flote
        else if (rb.linearVelocity.y > 0)
        {
            rb.AddForce(Physics.gravity * lowJumpMultiplier, ForceMode.Acceleration);
        }
        // Movimiento WASD
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        Vector3 move = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 velocity = move * speed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // Aplicar salto si fue solicitado en Update
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpRequested = false; // consumir la petición
        }
    }

    private bool IsGrounded()
    {
        // Lanza un rayo corto hacia abajo desde el centro del jugador
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}