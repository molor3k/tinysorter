using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_States;

public class SC_Movement : MonoBehaviour {
    
    public CharacterController controller;
    public Transform cam;
    public GameObject dustParticles;

    private SC_CameraController cameraController;
    private SC_InputController inputController;
    private SC_StateController stateController;

    // States
    private States currentState;
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
    private float targetAngle = 180.0f;

    // Gravity
    private float gravityAcceleration = 0.1f;


    void Start() {
        cameraController = gameObject.GetComponent<SC_CameraController>();
        inputController = gameObject.GetComponent<SC_InputController>();
        stateController = gameObject.GetComponent<SC_StateController>();

        dustParticles.GetComponent<ParticleSystem>().Stop();
    }

    void Update() {
        inputDirection = inputController.InputDirection;

        getStates();

        rotateCharacter();
        moveCharacter();

        applyGravity();

        applyDustParticlesEffect();
    }

    private void getStates() {
        currentState = stateController.getCurrentState();

        isWalking = currentState == States.WALK;
        isRunning = currentState == States.RUN;
        isOpeningInventory = (currentState == States.OPEN_INVENTORY) || (currentState == States.RECYCLE);
    }

    private void rotateCharacter() {
        if (isOpeningInventory) {
            targetAngle = 0.0f;
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

    private void applyGravity() {
        bool isStanding = isOnGround();
        bool isRising = isPreparedToRise();

        if (isRising) {
            controller.Move(Vector3.up.normalized * 15.0f * Time.deltaTime); 
            gravityAcceleration = 0.1f;
        } else {
            if (!isStanding) {
                gravityAcceleration *= 1.5f;
                controller.Move(Vector3.down.normalized * gravityAcceleration * Time.deltaTime); 
            } else {
                gravityAcceleration = 0.1f;
            }
        }
    }

    private bool isPreparedToRise() {
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward + new Vector3(0, 0.15f, 0);

        return getRayCollision(startPos, direction, 3.0f, 8);
    }

    private bool isOnGround() {
        Vector3 startPos = transform.position + new Vector3(0, 1.0f, 0);
        Vector3 direction = Vector3.down;

        return getRayCollision(startPos, direction, 1.0f, 8);
	}

    private bool getRayCollision(Vector3 startPos, Vector3 direction, float rayLength, int layerNumber) {
        int layerMask = 1 << layerNumber;

        if (!Physics.Raycast(startPos, direction, out RaycastHit hit, rayLength, layerMask)) {
			return false;
		}

        return true;
    }

    private void applyDustParticlesEffect() {
        if(isRunning) {
            dustParticles.GetComponent<ParticleSystem>().Play();
        } else {
            dustParticles.GetComponent<ParticleSystem>().Stop();
        }
    }
}