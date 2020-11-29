using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_AIMovement : MonoBehaviour {

    public CharacterController controller;

    // Movement 
    public float walkSpeed = 15f; 
    private float movementSpeed = 0f;
    private Vector3 movementDirection;
    private Vector3 inputDirection;

    // Rotation
    public float turnSmoothTime = 0.1f;             // Rotation smoothing time
    private float turnSmoothVelocity;               // Rotation smoothing
    private float targetAngle;

    // Gravity
    private float gravityAcceleration = 0.1f;
    private int count = 0;


    void Start() {
        inputDirection = getInputDirection();
    }

    void Update() {
        if(count > 300) {
            inputDirection = getInputDirection();
            count = 0;
        }

        rotateAI();
        moveAI();

        applyGravity();

        count++;
    }

    private void rotateAI() {
        
        targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg; 

        // Get angle of smoothed transition between character's and target angle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 

        // Perform a rotation
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void moveAI() {
        float targetSpeed = 0f;

        targetSpeed = walkSpeed;
        movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 

        // Speed
        movementSpeed += (targetSpeed - movementSpeed) * .1f;
        
        // Move in a direction of rotation
        controller.Move(movementDirection.normalized * movementSpeed * Time.deltaTime); 
    }

    private Vector3 getInputDirection() {
        var inputHorizontalAxis = Random.Range(-360, 360);
        var inputVerticalAxis = Random.Range(-360, 360);

        // Return normalized value, so character won't move faster if there's a diagonal movement
        return new Vector3(inputHorizontalAxis, 0f, inputVerticalAxis).normalized; 
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
}
