using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float doubleJumpForce = 1.5f;
    [SerializeField] private float maxHorizontalSpeed = 8f;
    [SerializeField] private float dashForce = 10f;
    
    [Header("Referencias")]
    [SerializeField] private GameObject cam;
    [SerializeField] private TMP_Text coinText;


    private Rigidbody rb;
    private bool canDoubleJump = false;
    private bool wasGrounded;
    private float hInput, vInput;
    private bool isPaused = false;
    private Vector3 lastCheckpoint;
    private int dashCount = 0;
    private bool jumpRequested = false;
    private bool doubleJumpRequested = false;


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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }

    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                jumpRequested = true;
            }
            else if (canDoubleJump && !wasGrounded)
            {
                doubleJumpRequested = true;
            }
        }
        
        HandleSkill();

        
    }

    void FixedUpdate()
    {
        // Dirección de movimiento en base a la cámara
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 moveDirection = (camForward * vInput + camRight * hInput).normalized;

        // Mover al jugador
        rb.AddForce(moveDirection * moveSpeed);
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if(horizontalVelocity.magnitude > maxHorizontalSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxHorizontalSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

        // bool isGrounded = IsGrounded();
        // Logica salto
        if(jumpRequested){
            JumpPhysics();
            jumpRequested = false;
        }

        if(doubleJumpRequested){
            DoubleJumpPhysics();
            doubleJumpRequested = false;
        }
        wasGrounded = IsGrounded(); // Actualizar estado anterior

        // Logica dash y doble salto
        // HandleSkill();

        
    }

    private void HandleSkill()
    {
        if(SceneManager.GetActiveScene().name == "Level1"){
            
        }
        else if(SceneManager.GetActiveScene().name == "Level2"){
            HandleDash();
        }
    }

    private void DoubleJumpPhysics()
    {

        if(canDoubleJump && SceneManager.GetActiveScene().name == "Level1"){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            canDoubleJump = false;
        }

    }

    private void JumpPhysics()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            canDoubleJump = true;
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0)
        {
            StartCoroutine(Dash());
            dashCount--;
        }
    }

    private IEnumerator Dash(){
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 moveDirection = (camForward * vInput + camRight * hInput).normalized;
        rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        rb.linearVelocity = Vector3.zero;
    }

    public void AddDash()
    {
        dashCount++;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("resetLayer"))  // SE RESETEAN LAS BARRERAS DE STARTTRIGGER Y ENDTRIGGER
        {
            transform.position = lastCheckpoint;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ScoreManager.instance.ResetScores();
            GameManager.instance.ReloadLevel();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("checkpointLayer"))
        {
            lastCheckpoint = other.transform.position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }   
    }
}