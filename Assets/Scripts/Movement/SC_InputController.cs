using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_InputController : MonoBehaviour
{
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isOpeningInventory = false;

    private SC_StateController stateController;

    private Vector3 inputDirection;
    public Vector3 InputDirection{ get { return inputDirection; } }


    void Start() {
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    void Update() {
        inputDirection = getInputDirection();

        isWalking = inputDirection.magnitude >= 0.1f;
        isRunning = Input.GetButton("ButtonRun") && isWalking;

        isOpeningInventory = Input.GetButtonDown("ButtonInventory");

        inputToState();
    }

    void inputToState() {
        SC_StateController.States currentState = stateController.getCurrentState();

        if (isWalking) {
            if (isRunning) {
                stateController.onRun();
            } else {
                stateController.onWalk();
            }
        }
        
        if (currentState == SC_StateController.States.OPEN_INVENTORY) {
            if (isOpeningInventory) {
                stateController.onIdle();
            }
        } else {
            if (isOpeningInventory) {
                stateController.onOpenInventory();
            } else {
                if (!isWalking && !isRunning) {
                    stateController.onIdle();
                }
            }
        }
    }
    
    private Vector3 getInputDirection() {
        var inputHorizontalAxis = Input.GetAxisRaw("Horizontal");
        var inputVerticalAxis = Input.GetAxisRaw("Vertical");

        // Return normalized value, so character won't move faster if there's a diagonal movement
        return new Vector3(inputHorizontalAxis, 0f, inputVerticalAxis).normalized; 
    }
}
