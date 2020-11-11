using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    private SC_CameraController cameraController;
    private SC_InputController inputController;
    private SC_StateController stateController;

    // States
    private SC_StateController.States currentState;
    private bool isWalking;
    private bool isRunning;
    private bool isOpeningInventory;

    // Movement 
    public float walkSpeed = 15f; 
    public float runSpeed = 20f; 
    private float movementSpeed = 0f;
    private Vector3 movementDirection;
    private Vector3 inputDirection;

    // Rotation
    public float turnSmoothTime = 0.1f;             // Rotation smoothing time
    private float turnSmoothVelocity;               // Rotation smoothing
    private float targetAngle;


    void Start() {
        cameraController = gameObject.GetComponent<SC_CameraController>();
        inputController = gameObject.GetComponent<SC_InputController>();
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    void Update() {
        inputDirection = inputController.InputDirection;

        getStates();

        rotateCharacter();
        moveCharacter();
    }

    private void getStates() {
        currentState = stateController.getCurrentState();

        isWalking = currentState == SC_StateController.States.WALK;
        isRunning = currentState == SC_StateController.States.RUN;
        isOpeningInventory = currentState == SC_StateController.States.OPEN_INVENTORY;
    }

    private void rotateCharacter() {
        if (isOpeningInventory) {
            targetAngle = 180.0f;
        } else {
            if (isWalking || isRunning) {
                targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            }
        }

        // Get angle of smoothed transition between character's and target angle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 

        // Perform a rotation
        transform.rotation = Quaternion.Euler(0f, angle, 0f); 
    }

    private void moveCharacter() {
        float targetSpeed = 0f;

        if (isWalking || isRunning) {
            targetSpeed = isRunning ? runSpeed : walkSpeed;
            movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
        }

        // Speed
        movementSpeed += (targetSpeed - movementSpeed) * .1f;
        
        // Move in a direction of rotation
        controller.Move(movementDirection.normalized * movementSpeed * Time.deltaTime); 
        
        // Camera
        cameraController.SetCharacterCamera(isRunning);
    }

}