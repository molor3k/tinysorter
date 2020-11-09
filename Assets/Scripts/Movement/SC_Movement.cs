using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public SC_CameraController vCam;

    private Animator anim;

    // Movement speed
    public float walkSpeed = 15f; 
    public float runSpeed = 20f; 
    private float movementSpeed = 0f;
    private Vector3 movementDirection;

    // Turn settings
    public float turnSmoothTime = 0.1f;     // Rotation smoothing time
    float turnSmoothVelocity;               // Rotation smoothing

    // Input 
    Vector3 inputDirection;

    // States
    bool isRunning;
    bool isRunningStateBefore;

    bool isWalking;


    void Start() {
        anim = controller.GetComponent<Animator>();
    }

    void Update() {
        inputDirection = getInputDirection();

        isWalking = inputDirection.magnitude >= 0.1f;
        isRunning = Input.GetButton("ButtonRun") && isWalking;

        moveCharacter();
        animateCharacter();
    }

    private void moveCharacter() {
        float targetSpeed = 0f;
        float targetAngle = transform.rotation.y;

        if (isWalking) {
            targetSpeed = isRunning ? runSpeed : walkSpeed;
                
            // Get a target angle
            targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 

            // Get angle of smoothed transition between character's and target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 

            // Perform a rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 

            // Move direction
            movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
        }

        // Speed
        movementSpeed += (targetSpeed - movementSpeed) * .1f;
        
        // Move in a direction of rotation
        controller.Move(movementDirection.normalized * movementSpeed * Time.deltaTime); 
        
        // Camera
        if (isRunningStateBefore != isRunning) {
            vCam.SetCharacterCamera(isRunning);
        }

        //
        isRunningStateBefore = isRunning;
    }

    private void animateCharacter() {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isWalking ? isRunning : false);
    }

    private Vector3 getInputDirection() {
        var inputHorizontalAxis = Input.GetAxisRaw("Horizontal");
        var inputVerticalAxis = Input.GetAxisRaw("Vertical");

        // Return normalized value, so character won't move faster if there's a diagonal movement
        return new Vector3(inputHorizontalAxis, 0f, inputVerticalAxis).normalized; 
    }
}