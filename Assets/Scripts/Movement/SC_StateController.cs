using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_StateController : MonoBehaviour
{
    private SC_AnimationController animationController;
    public enum States {
        STATE_RESET = 0,
        IDLE,
        WALK,
        RUN,
        RUN_END,
        PICK_ITEM,
        DROP_ITEM,
        OPEN_INVENTORY
    }

    public States previousState = States.IDLE;
    public States currentState = States.IDLE;


    void Start() {
        animationController = gameObject.GetComponent<SC_AnimationController>();
    }

    void Update() {
        if (previousState != currentState) {
            animationController.SetState((int)currentState);
        }

        previousState = currentState;
    }

    public States getCurrentState() {
        return currentState;
    }

    public void onResetState() {
        currentState = States.STATE_RESET;
    }

    public void onIdle() {
        currentState = States.IDLE;
    }

    public void onWalk() {
        if (currentState == States.IDLE) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            currentState = States.WALK;
                        }
                    })
                )
            );
        } else if (currentState == States.RUN || currentState == States.RUN_END) {
            currentState = States.WALK;
        }
    }

    public void onRun() {
        if (currentState == States.IDLE) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            currentState = States.RUN;
                        }
                    })
                )
            );
        } else if (currentState == States.WALK) {
            currentState = States.RUN;
        }
    }

    public void onRunEnd() {
        if (currentState == States.RUN) {
            currentState = States.RUN_END;

            StartCoroutine(
                OnCompleteAnimation(
                    (result => {
                        if (result) {
                            onIdle();
                        }
                    })
                )
            );
        }
    }

    public void onPickItem() {
        if ((int)currentState < 5) {
            currentState = States.PICK_ITEM;

            StartCoroutine(
                OnCompleteAnimation(
                    (result => {
                        if (result) {
                            onIdle();
                        }
                    })
                )
            );
        }
    }

    public void onDropItem() {
        if (currentState != States.PICK_ITEM) {
            currentState = States.DROP_ITEM;

            StartCoroutine(
                OnCompleteAnimation(
                    (result => {
                        if (result) {
                            onOpenInventory();
                        }
                    })
                )
            );
        }
    }

    public void onOpenInventory() {
        StartCoroutine(
            OnResetStateComplete(
                (result => {
                    if (result) {
                        currentState = States.OPEN_INVENTORY;
                    }
                })
            )
        );
    }

    IEnumerator OnResetStateComplete(Action<bool> onComplete) {
        onResetState();

        yield return new WaitForSeconds(0.05f);

        onComplete(true);
    }

    IEnumerator OnCompleteAnimation(Action<bool> onComplete) {
        Animator anim = gameObject.GetComponent<Animator>();

        while(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        onComplete(true);
    }
}
