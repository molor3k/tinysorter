using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_States;

public class SC_StateController : MonoBehaviour {
    
    private SC_AnimationController animationController;
    
    public States playingState = States.IDLE;
    public States previousState = States.IDLE;
    public States currentState = States.IDLE;


    void Start() {
        animationController = gameObject.GetComponent<SC_AnimationController>();
    }

    void Update() {
        if (playingState != currentState) {
            animationController.SetState((int)currentState);
        }

        playingState = currentState;
    }

    public States getPreviousState() {
        return previousState;
    }

    public States getCurrentState() {
        return currentState;
    }

    public void setCurrentState(States newState) {
        previousState = currentState;
        currentState = newState;
    }

    public void onResetState() {
        setCurrentState(States.STATE_RESET);
    }

    public void onIdle() {
        setCurrentState(States.IDLE);
    }

    public void onWalk() {
        if (currentState == States.IDLE) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            setCurrentState(States.WALK);
                        }
                    })
                )
            );
        } else if (currentState == States.RUN || currentState == States.RUN_END) {
            setCurrentState(States.WALK);
        }
    }

    public void onRun() {
        if (currentState == States.IDLE) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            setCurrentState(States.RUN);
                        }
                    })
                )
            );
        } else if (currentState == States.WALK) {
            setCurrentState(States.RUN);
        }
    }

    public void onRunEnd() {
        if (currentState == States.RUN) {
            setCurrentState(States.RUN_END);

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
        if (currentState != States.OPEN_INVENTORY || currentState != States.DROP_ITEM) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            setCurrentState(States.PICK_ITEM);

                            StartCoroutine(
                                OnCompleteAnimation(
                                    (res => {
                                        if (res) {
                                            onIdle();
                                        }
                                    })
                                )
                            );
                        }
                    })
                )
            );
        }
    }

    public void onDropItem() {
        setCurrentState(States.DROP_ITEM);

        StartCoroutine(
            OnCompleteAnimation(
                (result => {
                    if (result) {
                        if (previousState == States.OPEN_INVENTORY) {
                            setCurrentState(States.OPEN_INVENTORY);
                        } else if (previousState == States.RECYCLE) {
                            setCurrentState(States.RECYCLE);
                        }
                    }
                })
            )
        );
    }

    public void onOpenInventory() {
        if (currentState != States.RECYCLE && currentState != States.CLOSE_INVENTORY && currentState != States.DROP_ITEM) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            setCurrentState(States.OPEN_INVENTORY);
                        }
                    })
                )
            );
        }
    }

    public void onRecycle() {
        if (currentState != States.OPEN_INVENTORY && currentState != States.CLOSE_INVENTORY && currentState != States.DROP_ITEM) {
            StartCoroutine(
                OnResetStateComplete(
                    (result => {
                        if (result) {
                            setCurrentState(States.RECYCLE);
                        }
                    })
                )
            );
        }
    }

    public void onCloseInventory() {
        setCurrentState(States.CLOSE_INVENTORY);
        
        StartCoroutine(
            OnWait(0.5f, 
                (result => {
                    if (result) {
                        onIdle();
                    }
                })
            )
        );
    }

    IEnumerator OnWait(float time, Action<bool> onComplete) {
        yield return new WaitForSeconds(time);

        onComplete(true);
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
