﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_States;

public class SC_InputController : MonoBehaviour
{
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isOpeningInventory = false;
    public bool isAction = false;

    private SC_Interactions interaction;
    private SC_StateController stateController;

    private Vector3 inputDirection;
    public Vector3 InputDirection{ get { return inputDirection; } }


    void Start() {
        interaction = gameObject.GetComponent<SC_Interactions>();
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    void Update() {
        inputDirection = getInputDirection();

        isWalking = inputDirection.magnitude >= 0.1f;
        isRunning = Input.GetButton("ButtonRun") && isWalking;

        isOpeningInventory = Input.GetButtonDown("ButtonInventory");
        isAction = Input.GetButtonDown("ButtonAction");

        inputToState();
    }

    void inputToState() {
        States currentState = stateController.getCurrentState();
        bool isNotPickingOrDropping = (currentState != States.PICK_ITEM) && (currentState != States.DROP_ITEM);
        bool isInInventory = (currentState == States.OPEN_INVENTORY) || (currentState == States.CLOSE_INVENTORY) || (currentState == States.RECYCLE) || (currentState == States.DROP_ITEM);
        
        bool isClosingRecycling = isAction && (currentState == States.RECYCLE);
        bool isClosingInventory = isOpeningInventory && (currentState == States.OPEN_INVENTORY);

        if (isNotPickingOrDropping) {
            if (isWalking) {
                if (isRunning) {
                    stateController.onRun();
                } else {
                    stateController.onWalk();
                }
            }
        }
        
        if (isInInventory) {
            if (isClosingInventory || isClosingRecycling) {
                stateController.onCloseInventory();
            }
        } else {
            if (isOpeningInventory) {
                stateController.onOpenInventory();
            } else {
                if (!isWalking && !isRunning) {
                    if (isNotPickingOrDropping) {
                        stateController.onIdle();
                    }
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
