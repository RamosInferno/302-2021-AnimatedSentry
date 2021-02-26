using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CharacterController pawn;
    public float walkSpeed = 5;

    public Transform leg1;
    public Transform leg2;

    public float gravityMultiplyer = 10;
    public float jumpImpulse = -5;

    private Vector3 inputDirection = new Vector3();

    private float timeLeftGrounded = 0;

    public bool isGrounded
    {
        get // return true is pawn on ground or coyote-time isn't zero
        {
            return pawn.isGrounded || timeLeftGrounded > 0;
        }
    }

    // how fast the player is currently
    private float verticalVelocity = 0;


    void Start()
    {
        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (timeLeftGrounded > 0) timeLeftGrounded -= Time.deltaTime;

        MovePlayer();
        if (isGrounded) WiggleLegs(); // idle + walk
        else AirLegs(); // jump (or falling)
    }

    private void WiggleLegs()
    {

        float degrees = 45;
        float speed = 10;


        Vector3 inputDirLocal = transform.InverseTransformDirection(inputDirection);
        Vector3 axis = Vector3.Cross(inputDirLocal, Vector3.up);

        //check the allignment of inputDirLocal against foward vector
        float allignment = Vector3.Dot(inputDirLocal, Vector3.forward);


        //if (allignment < 0) allignment *= -1;

        allignment = Mathf.Abs(allignment);

        degrees *= AnimMath.Lerp(0.25f, 1, allignment); //decrease 'degrees' when strafing

        float wave = Mathf.Sin(Time.time * speed) * degrees;

        leg1.localRotation = AnimMath.Slide(leg1.localRotation, Quaternion.AngleAxis(wave, axis), .001f);
        leg2.localRotation = AnimMath.Slide(leg2.localRotation, Quaternion.AngleAxis(-wave, axis), .001f);

    }


    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isJumpHeld = Input.GetButton("Jump");
        bool onJumpPress = Input.GetButtonDown("Jump");


        //float yawOfInput = Mathf.Atan2(v, h);
        //float yawOfCamera = cam.transform.eulerAngles.y;

        bool isTryingToMove = (h != 0 || v != 0);
        if (isTryingToMove)
        {
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), .02f);
        }


        inputDirection = transform.forward * v + transform.right * h;

        if (inputDirection.sqrMagnitude > 1) inputDirection.Normalize();



        if(pawn.isGrounded)
        {

            verticalVelocity = 0; // on ground zero-out vertical-velocity

            if (isJumpHeld)
            {
                verticalVelocity = 5;
            }
        }
   
        //apply gravity
        verticalVelocity += gravityMultiplyer * Time.deltaTime;

        // adds lateral movement to vertical movement
        Vector3 moveDelta = inputDirection * walkSpeed + verticalVelocity * Vector3.down;

        // move pawn
        CollisionFlags flags = pawn.Move(moveDelta * Time.deltaTime); // 0, -1, 0



         if(isGrounded)
        {
            verticalVelocity = 0; // on ground zero-out vertical-velocity 
            verticalVelocity = 5;
            timeLeftGrounded = .2f;
            
        }
        
        if(isGrounded)
        {
            if (isJumpHeld)
            {
                verticalVelocity = -jumpImpulse; // on ground zero-out vertical-velocity
                timeLeftGrounded = 0;
            }
        }
    }

    private void AirLegs()
    {
        leg1.localRotation = AnimMath.Slide(leg1.localRotation, Quaternion.Euler(30, 0, 0), .001f);
        leg2.localRotation = AnimMath.Slide(leg2.localRotation, Quaternion.Euler(-30, 0, 0), .001f);
    }

}
