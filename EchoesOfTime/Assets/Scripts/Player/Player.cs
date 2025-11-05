using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]InputHandler inputReader;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    private Vector3 forcesPlayerOnFrame;


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
       
        forcesPlayerOnFrame += AddVector2To3(inputReader.MoveVector * movementSpeed);


        AddForces(); //Add all the forces this frame has done

    }

    private void FixedUpdate()
    {
        
    }

    private void AddForces()
    {
        rb.AddForce(forcesPlayerOnFrame);

        forcesPlayerOnFrame = Vector3.zero;
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
        
    }

    private void OnDisable()
    {
        inputReader.JumpEvent -= InputReader_JumpEvent;
    }

}
