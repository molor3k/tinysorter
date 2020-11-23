using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_AIAnimation : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation() {
        anim.SetInteger("currentState", (int)SC_States.States.WALK);
    }
}
