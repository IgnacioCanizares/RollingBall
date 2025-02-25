using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float doubleJumpForce = 1.5f;
    [SerializeField] private float maxHorizontalSpeed = 8f;
    [SerializeField] private GameObject cam;


    private Rigidbody rb;
    private bool canDoubleJump = false;
    private bool wasGrounded;
    private float hInput, vInput;
    private bool isPaused = false;
    private Vector3 lastCheckpoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wasGrounded = IsGrounded(); // Inicializar estado
        lastCheckpoint = transform.position;

    }

    void Update()
    {
        // Entrada y dirección de movimiento
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        // Get escape key input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused){
                Time.timeScale = 0;
                isPaused = true;

            }
            else{
                Time.timeScale = 1;
                isPaused = false;
            }
        }

        // Lógica de saltos
        HandleJump();
    }

    void FixedUpdate()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        // // Movimiento horizontal
        Vector3 moveDirection = (camForward * vInput + camRight * hInput).normalized;
        
        rb.AddForce(moveDirection * moveSpeed);
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if(horizontalVelocity.magnitude > maxHorizontalSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxHorizontalSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }

    private void HandleJump()
    {
        bool isGrounded = IsGrounded();

        // Activar doble salto al caer sin saltar
        if (wasGrounded && !isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = true;
        }

        // Lógica de salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                canDoubleJump = false;
            }
        }

        wasGrounded = isGrounded; // Actualizar estado anterior
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("resetLayer"))
        {
            transform.position = lastCheckpoint;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}