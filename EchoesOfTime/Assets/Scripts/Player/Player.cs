using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]InputHandler inputReader;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    private Vector3 forcesPlayerOnFrame;
    [SerializeField] float camAngleRotation;
    private float verticalCameraRotation = 0f;
    [SerializeField] float gravityForce = 30f; // tune between 20–40 for realistic feel
    [SerializeField] float fallMultiplier = 2f; // stronger pull when falling

    #region WallRunning
    RaycastHit leftWallHit;

    RaycastHit rightWallHit;
    public LayerMask groundMask;
    public LayerMask isWallGroundMask;
    [SerializeField] float raycastLenght = 2f;
    [SerializeField] float groundRaycastLenght = .2f;

    #endregion

    [Header("Coyote Time")]
    [SerializeField] float coyoteTimeDuration = 0.15f; // 0.15 segundos
    private float coyoteTimeCounter;

    [Space(10)]
    [SerializeField] Transform camTrans;
    [SerializeField] Transform camMove;
    bool isWallRight = false;
    bool isWallLeft = false;
    float rotateCameraZ = 0f;
    bool isGrounded;
    [SerializeField]float rotationSpeed = 5f;

    bool isWall => isWallLeft || isWallRight;

    //TODO: cuando congeles un objeto se queda en kinematic por 2 segundos
    //TODO: cuando rebovines esta en kinematic, cuando pares de rebobinar se queda en kinematic por 2 segundos


    private void OnEnable()
    {
        inputReader.JumpEvent += InputReader_JumpEvent;
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        

    }

  

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        
        CheckForWall();
        IsWallRunning();

        if (isGrounded)
        {
            // Si estamos en el suelo, reiniciamos el contador
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            // Si estamos en el aire, empezamos a restar tiempo
            coyoteTimeCounter -= Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        // 1. Coger el input vertical (Mouse Y)
        Vector2 rotateVector = inputReader.LookVector;
        float verticalInput = rotateVector.y;

        // 2. Calcular y acumular la rotación X (vertical)
        // Es -= porque el input 'Y' del ratón suele estar invertido
        verticalCameraRotation -= verticalInput * rotationSpeed * Time.deltaTime;

        // 3. Limitar (clamp) la rotación vertical para no dar la vuelta
        verticalCameraRotation = Mathf.Clamp(verticalCameraRotation, -90f, 90f);

        // 4. Aplicar TODAS las rotaciones a la cámara
        // Usamos 'camTrans' (tu variable de cámara)
        // Usamos localRotation para que rote relativo al jugador
        camTrans.localRotation = Quaternion.Euler(
            verticalCameraRotation, // Rotación X (arriba/abajo)
            0,                      // Rotación Y (la maneja el cuerpo del jugador)
            rotateCameraZ           // Rotación Z (el tilt que ya calculas en IsWallRunning)
        );
    }

    private void FixedUpdate()
    {

        AddForces(); 
        
        PlayerLookHorizontal();

        ApplyCustomGravity();
    }

    private void AddForces()
    {
        if(!isWall)
        {
            Vector3 moveInput = AddVector2To3(inputReader.MoveVector * movementSpeed);
            // FIX HERE: Flatten camera forward/right to ignore vertical tilt
            Vector3 flatForward = camTrans.forward;
            flatForward.y = 0f;
            flatForward.Normalize();

            Vector3 flatRight = camTrans.right;
            flatRight.y = 0f;
            flatRight.Normalize();
            rb.AddForce(moveInput.x * flatRight + moveInput.z * flatForward);
        }
        if(isWall)
        {
            WallRunningMovement();
        }
        

    }


    public Vector3 AddVector2To3(Vector2 v)
    {
        Vector3 VToReturn = Vector3.zero;
        VToReturn.x = v.x;
        VToReturn.z = v.y;

        return VToReturn;
    }

    private void InputReader_JumpEvent()
    {
        //If is grounded
        if (isWall)
        {
            Vector3 wallNormal = isWallRight ? rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
            if (Vector3.Dot(wallForward, transform.forward) < 0)
            {
                wallForward = -wallForward; 
            }
            float magnitudeJump;
            if (rb.linearVelocity.magnitude > 2f)
            {
                magnitudeJump = 2.3f;
            }
            else if (rb.linearVelocity.magnitude < 1f)
            {
                magnitudeJump = 1f;
            }
            else
            {
                magnitudeJump = rb.linearVelocity.magnitude;
            }


            rb.AddForce((wallForward * jumpForce) + (wallNormal * (jumpForce * magnitudeJump)),ForceMode.Impulse);
        }
        else if (coyoteTimeCounter > 0f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            coyoteTimeCounter = 0f;
        }
       
        
        
    }

    private void OnDisable()
    {
        inputReader.JumpEvent -= InputReader_JumpEvent;

    }



    //PARKOUR FUNCTIONS

    private void CheckForWall()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundRaycastLenght, groundMask);
        //isWallLeft = Physics.CheckBox(leftWallCheck.position, new Vector3(0.2f, 0.7f, 0.4f), Quaternion.identity, isWallGroundMask);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, raycastLenght, isWallGroundMask);
        //isWallRight = Physics.CheckBox(rightWallCheck.position, new Vector3(0.2f, 0.7f, 0.4f), Quaternion.identity, isWallGroundMask);
        isWallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, raycastLenght, isWallGroundMask);
    }

    private void IsWallRunning()
    {
        float lerpSpeed = Time.deltaTime * 10f; // Controls camera tilt speed

        if (isWallLeft)
        {
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, -camAngleRotation, lerpSpeed);
            rotateCameraZ = Mathf.Clamp(rotateCameraZ, -camAngleRotation, 0f); // Corrected clamp
            
        }
        else if (isWallRight)
        {
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, camAngleRotation, lerpSpeed);
            rotateCameraZ = Mathf.Clamp(rotateCameraZ, 0f, camAngleRotation);
            
        }
        else
        {
            
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, 0f, lerpSpeed);
            
        }

        if(isWall)
        {

        }

        camMove.transform.localRotation = Quaternion.Euler(camMove.transform.localRotation.x, camMove.transform.localRotation.y, rotateCameraZ);
    }


    private void WallRunningMovement()
    {
        Vector3 wallNormal = isWallRight ? rightWallHit.normal : leftWallHit.normal;


        // 1. Calculate Wall Forward Direction
        // Vector3.Cross(wallNormal, transform.up) gives a vector along the wall face.
        // We check the dot product to ensure it's pointing in the player's forward direction.
        Vector3 wallForward = Vector3.Cross(wallNormal, Camera.main.transform.up);

        // Reverse if we are running backwards relative to the wall
        wallForward *= Mathf.Sign(Vector3.Dot(wallForward, Camera.main.transform.forward));

        // 2. Add Forward Movement Force (Scaled by Movement Speed)
        // We only allow forward input (moveVector.y, which is 'W' or 'S' on a typical WASD setup)
        float forwardInput = inputReader.MoveVector.y;
        rb.AddForce(wallForward * forwardInput * movementSpeed, ForceMode.Force); // Increased force multiplier for movement

        // 3. Add Force Into the Wall (Attachment Force)
        // This force pushes the player towards the wall to keep them attached.
        // Use ForceMode.Force for continuous push.
        rb.AddForce(-wallNormal * 10f, ForceMode.Force); // You may need to tune the '10f' value
    }

    public void PlayerLook()
    {
        Vector2 rotateVector = inputReader.LookVector;
        float horizontalInput = rotateVector.x;

        if (horizontalInput != 0)
        {

            float rotationAngle = horizontalInput * rotationSpeed * Time.fixedDeltaTime;

            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    private void PlayerLookHorizontal()
    {
        Vector2 rotateVector = inputReader.LookVector;
        float horizontalInput = rotateVector.x;

        if (horizontalInput != 0)
        {
            // 1. Usa Time.fixedDeltaTime
            float rotationAngle = horizontalInput * rotationSpeed * Time.fixedDeltaTime;

            // 2. Calcula la rotación delta
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // 3. Aplica al Rigidbody
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }


    private void ApplyCustomGravity()
    {
        if (isWall)
            return; // no gravity while wallrunning, handled elsewhere

        // Stronger gravity when player is moving downward
        if (rb.linearVelocity.y < 0)
            rb.AddForce(Vector3.down * gravityForce * fallMultiplier, ForceMode.Acceleration);
        else
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
    }


}
