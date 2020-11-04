using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    private Animator anim;

    // Movement speed
    public float walkSpeed = 15f; 
    public float runSpeed = 20f; 

    // Turn settings
    public float turnSmoothTime = 0.1f;     // Rotation smoothing time
    float turnSmoothVelocity;               // Rotation smoothing

    // Input 
    float inputDirection;

    // States
    bool isRunning;
    bool isWalking


    void Start() {
        anim = controller.GetComponent<Animator>();
    }

    void Update() {
        inputDirection = getInputDirection();

        isRunning = Input.GetButton("ButtonRun");
        isWalking = inputDirection.magnitude >= 0.1f;

        moveCharacter();
        animateCharacter();
    }

    private moveCharacter() {
        if (isWalking) {
            var speed = isRunning ? runSpeed : walkSpeed;
                
            // Get a target angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 

            // Get angle of smoothed transition between character's and target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 

            // Perform a rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 

            // Move in a direction of rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
            controller.Move(moveDir.normalized * speed * Time.deltaTime); 
        }
    }

    private animateCharacter() {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isRunning", isWalking ? isRunning : false);
    }

    private float getInputDirection() {
        var inputHorizontalAxis = Input.GetAxisRaw("Horizontal");
        var inputVerticalAxis = Input.GetAxisRaw("Vertical");

        // Return normalized value, so character won't move faster if there's a diagonal movement
        return new Vector3(inputHorizontalAxis, 0f, inputVerticalAxis).normalized; 
    }
}
