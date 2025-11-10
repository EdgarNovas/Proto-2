using System;
using UnityEditor.ShaderGraph;
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

    #region WallRunning
    RaycastHit leftWallHit;

    RaycastHit rightWallHit;
    public LayerMask groundMask;
    public LayerMask isWallGroundMask;
    [SerializeField] float raycastLenght = 2f;
    [SerializeField] float groundRaycastLenght = .2f;

    #endregion


    [SerializeField] Transform camTrans;
    [SerializeField] Transform camMove;
    bool isWallRight = false;
    bool isWallLeft = false;
    float rotateCameraZ = 0f;
    bool isGrounded;
    [SerializeField]float rotationSpeed = 5f;

    bool isWall => isWallLeft || isWallRight;




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
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
        CheckForWall();
        IsWallRunning();
        PlayerLook();
    }

    private void FixedUpdate()
    {

        AddForces(); //Add all the forces this frame has done
    }

    private void AddForces()
    {
        if(!isWall)
        {
            Vector3 moveInput = AddVector2To3(inputReader.MoveVector * movementSpeed);
            rb.AddForce(moveInput.x * camTrans.right + moveInput.z * camTrans.forward);
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
        if(isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, -45f, lerpSpeed);
            rotateCameraZ = Mathf.Clamp(rotateCameraZ, -45f, 0f); // Corrected clamp
            rb.useGravity = false;
        }
        else if (isWallRight)
        {
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, 45f, lerpSpeed);
            rotateCameraZ = Mathf.Clamp(rotateCameraZ, 0f, 45f);
            rb.useGravity = false;
        }
        else
        {
            // This is the missing part: Lerp back to 0
            rotateCameraZ = Mathf.Lerp(rotateCameraZ, 0f, lerpSpeed);
            rb.useGravity = true;
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
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        if (Vector3.Dot(wallForward, transform.forward) < 0)
        {
            wallForward = -wallForward; // Reverse if we are running backwards relative to the wall
        }

        // 2. Add Forward Movement Force (Scaled by Movement Speed)
        // We only allow forward input (moveVector.y, which is 'W' or 'S' on a typical WASD setup)
        float forwardInput = inputReader.MoveVector.y;
        rb.AddForce(wallForward * forwardInput * movementSpeed * 5f, ForceMode.Force); // Increased force multiplier for movement

        // 3. Add Force Into the Wall (Attachment Force)
        // This force pushes the player towards the wall to keep them attached.
        // Use ForceMode.Force for continuous push.
        rb.AddForce(-wallNormal * 10f, ForceMode.Force); // You may need to tune the '10f' value
    }

    public void PlayerLook()
    {
        Vector2 rotateVector = inputReader.LookVector;

        float horizontalInput = rotateVector.x; // For character rotation (Y-axis)

        // Rotate the character around the Y-axis (horizontal input)
        if (horizontalInput != 0)
        {
            // Calculate the desired rotation angle
            float rotationAngle = horizontalInput * rotationSpeed * Time.deltaTime;

            // Apply the rotation around the Y-axis
            transform.Rotate(0f, rotationAngle, 0f);
        }
    }
}
