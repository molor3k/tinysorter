using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_AnimationController : MonoBehaviour {
    
    private Animator anim;


    void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void SetState(int newState) {
        anim.SetInteger("currentState", newState);
    }
}