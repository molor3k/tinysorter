using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_States;

public class SC_InputController : MonoBehaviour {
    
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isOpeningInventory = false;
    public bool isSelectingStack = false;
    public bool isAction = false;

    private bool isCPU = false;
    public Vector2 cpuChangeDirectionDelay = new Vector2(300, 600);
    private float cpuChangeDirectionTime = 0;

    private SC_Interactions interaction;
    private SC_Movement movementController;
    private SC_StateController stateController;

    private Vector3 inputDirection;
    public Vector3 InputDirection{ get { return inputDirection; } }


    void Start() {
        interaction = gameObject.GetComponent<SC_Interactions>();
        movementController = gameObject.GetComponent<SC_Movement>();
        stateController = gameObject.GetComponent<SC_StateController>();

        isCPU = movementController.isCPU;
    }

    void Update() {
        if (isCPU) {
            CPUMoveTimer();
            stateController.onWalk();
        } else {
            inputDirection = getInputDirection();

            isWalking = inputDirection.magnitude >= 0.1f;
            isRunning = Input.GetButton("ButtonRun") && isWalking;

            isOpeningInventory = Input.GetButtonDown("ButtonInventory");
            isSelectingStack = Input.GetButton("ButtonInventoryStackSelect");
            isAction = Input.GetButtonDown("ButtonAction");

            inputToState();
        }
    }

    void inputToState() {
        States currentState = stateController.getCurrentState();
        bool isNotPickingOrDropping = (currentState != States.PICK_ITEM) && (currentState != States.DROP_ITEM);
        bool isInInventory = (currentState == States.OPEN_INVENTORY) || (currentState == States.CLOSE_INVENTORY) || (currentState == States.RECYCLE) || (currentState == States.DROP_ITEM);
        
        bool isClosingRecycling = isAction && (currentState == States.RECYCLE);
        bool isClosingInventory = isOpeningInventory && (currentState == States.OPEN_INVENTORY);

        bool isNono = (currentState == States.NONO);

        if (!isNono) {
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
    }

    void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            if (contact.otherCollider.gameObject.layer == 9) {      // Layer 9 is Obstacle layer
                if (isCPU) {
                    CPUChangeDirection();
                }
            }
        }
    }

    private void CPUChangeDirection() {
        var horizontalAxis = UnityEngine.Random.Range(-45, 45);
        var verticalAxis = UnityEngine.Random.Range(-65, 65);

        inputDirection = new Vector3(horizontalAxis, 0f, verticalAxis).normalized; 
        cpuChangeDirectionTime = UnityEngine.Random.Range(cpuChangeDirectionDelay.x, cpuChangeDirectionDelay.y);
    }

    private void CPUMoveTimer() {
        if (cpuChangeDirectionTime <= 0) {
            CPUChangeDirection();
        } else {
            cpuChangeDirectionTime--;
        }
    }
    
    private Vector3 getInputDirection() {
        var inputHorizontalAxis = Input.GetAxisRaw("Horizontal");
        var inputVerticalAxis = Input.GetAxisRaw("Vertical");

        // Return normalized value, so character won't move faster if there's a diagonal movement
        return new Vector3(inputHorizontalAxis, 0f, inputVerticalAxis).normalized; 
    }
}
