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

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que la física gire el jugador
    }

    void Update()
    {
        // ===== Rotación con ratón =====
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        // Rotación horizontal (gira el jugador)
        transform.Rotate(Vector3.up * mouseDelta.x);


    }

    void FixedUpdate()
    {
        // ===== Movimiento con WASD =====
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        Vector3 move = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 velocity = move * speed;
        velocity.y = rb.linearVelocity.y; // Mantener la velocidad vertical para gravedad/salto
        rb.linearVelocity = velocity;

        // ===== Salto =====
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Salto");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private bool IsGrounded()
    {
        return GetComponent<Rigidbody>().linearVelocity.y == 0;
    }

}