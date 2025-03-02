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
    [SerializeField] private Vector3 defaultGravity = new Vector3(0, -9.81f, 0);
    [SerializeField] private Vector3 invertedGravity = new Vector3(0, 9.81f, 0);
    
    [Header("Referencias")]
    [SerializeField] private GameObject cam;




    private Rigidbody rb;
    private bool canDoubleJump = false;
    private bool wasGrounded;
    private float hInput, vInput;
    private bool isPaused = false;
    private Vector3 lastCheckpoint;
    private int dashCount = 0;
    private bool jumpRequested = false;
    private bool doubleJumpRequested = false;
    private bool isGravityInverted = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wasGrounded = IsGrounded(); // Inicializar estado
        rb.useGravity = false; // Desactivar gravedad de Unity
        lastCheckpoint = GameManager.instance.GetLastCheckpoint(); // Obtener el último checkpoint
        transform.position = lastCheckpoint; // Mover al jugador al último checkpoint
    }

    void Update()
    {
        // Entrada y dirección de movimiento
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape)) // Pausar el juego
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }

    
        if (Input.GetKeyDown(KeyCode.Space)) // Saltar
        {
            if (IsGrounded()) // Si está en el suelo
            {
                jumpRequested = true;
            }
            else if (canDoubleJump && !wasGrounded) // Si puede hacer doble salto, y no estaba en el suelo
            {
                doubleJumpRequested = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Level3") // Cambiar gravedad en el nivel 3 con la tecla E
        {
            Debug.Log("Gravity change request");
            if(IsGrounded()){
                isGravityInverted = !isGravityInverted; // Cambiar gravedad invertida o no invertida, depende del estado actual
            }
        }
        
        HandleSkill(); // Manejar habilidades
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
        if(horizontalVelocity.magnitude > maxHorizontalSpeed) // Limitar la velocidad horizontal máxima
        {
            horizontalVelocity = horizontalVelocity.normalized * maxHorizontalSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

    

        if(jumpRequested){
            JumpPhysics();
            jumpRequested = false;
        }

        if(doubleJumpRequested){
            DoubleJumpPhysics();
            doubleJumpRequested = false;
        }

        rb.AddForce(isGravityInverted ? invertedGravity : defaultGravity, ForceMode.Acceleration); // Aplicar gravedad
        

        wasGrounded = IsGrounded(); // Actualizar estado de si está en el suelo
    }

    private void HandleSkill() // Manejar habilidades, progresion de habilidades en los niveles
    {
        if(SceneManager.GetActiveScene().name == "Level1"){ // En el nivel 1 solo habra doble salto
            
        }
        else if(SceneManager.GetActiveScene().name == "Level2"){ // En el nivel 2 solo habra dash
            HandleDash();
        }
        else if(SceneManager.GetActiveScene().name == "Level3"){ // En el nivel 3 habra gravedad invertida, doble salto y dash
            HandleDash();
        }
    }

    private void DoubleJumpPhysics()
    {

        if((canDoubleJump && SceneManager.GetActiveScene().name == "Level1") || (SceneManager.GetActiveScene().name == "Level3" && canDoubleJump)){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Resetear la velocidad en Y
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse); // Aplicar fuerza de doble salto
            canDoubleJump = false; // Desactivar doble salto
        }

    }

    private void JumpPhysics()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Resetear la velocidad en Y
        if(isGravityInverted){
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse); // Aplicar fuerza de salto, si la gravedad está invertida se aplica hacia abajo
        }
        else{
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplicar fuerza de salto, si la gravedad no está invertida se aplica hacia arriba
        }
        
        if(SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level3")
        {
            canDoubleJump = true; // Activar doble salto si está en el nivel 1 o 3
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0) // Si se pulsa el botón de dash y hay dash disponible
        {
            StartCoroutine(Dash()); // Realizar el dash, mediante una corrutina. Las corrutinas son funciones que se pueden pausar y reanudar
            dashCount--;
        }
    }

    private IEnumerator Dash(){ // Corrutina para el dash
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 moveDirection = (camForward * vInput + camRight * hInput).normalized;
        rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1); // Esperar 1 segundo antes de poder hacer otro dash
        rb.linearVelocity = Vector3.zero; // Resetear la velocidad del jugador para no mantener el impulso dado por el dash
    }

    public void AddDash()
    {
        dashCount++;
    }

    private bool IsGrounded()
    {
        if(isGravityInverted){ // Segun la gravedad
            return Physics.Raycast(transform.position, Vector3.up, 0.6f); // Raycast hacia arriba para comprobar si está en el suelo
        }
        else{
            return Physics.Raycast(transform.position, Vector3.down, 0.6f); // Raycast hacia abajo para comprobar si está en el suelo
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("resetLayer"))  
        {
            ResetPlayer(); // Resetear al jugador
            GameManager.instance.ReloadLevel(); // Recargar el nivel
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("checkpointLayer"))
        {
            GameManager.instance.SaveCheckpoint(other.transform.position); // Guardar el checkpoint
            ScoreManager.instance.setHasCheckpoint(true); // Activar el checkpoint
        }   
    }
    private void ResetPlayer()
    {
        transform.position = GameManager.instance.GetLastCheckpoint(); // Mover al jugador al último checkpoint
        rb.linearVelocity = Vector3.zero; // Resetear la velocidad
        rb.angularVelocity = Vector3.zero; // Resetear la velocidad angular
        isGravityInverted = false; // Resetear la gravedad invertida
        rb.AddForce(defaultGravity, ForceMode.Acceleration); // Aplicar gravedad
        if(!ScoreManager.instance.getHasCheckpoint()){
            ScoreManager.instance.ResetScores();
        }
    }
}