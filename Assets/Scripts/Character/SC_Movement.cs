/*
MIT License

Copyright (c) 2021 IBPM Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static SC_States;

public class SC_Movement : MonoBehaviour {
    // AI
    public bool isCPU = false;

    //
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
        stateController = gameObject.GetComponent<SC_StateController>();
        inputController = gameObject.GetComponent<SC_InputController>();

        //dustParticles.GetComponent<ParticleSystem>().Stop();
    }

    void Update() {
        inputDirection = inputController.InputDirection;

        getStates();

        rotateCharacter();
        moveCharacter();

        applyGravity();

        //applyDustParticlesEffect();
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
                targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                if (!isCPU) {
                    targetAngle += cam.eulerAngles.y;
                }
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
        if (!isCPU) {
            cameraController.SetCharacterCamera(isRunning);
        }
    }

    private void applyGravity() {
        bool isStanding = isOnGround();
        bool isRising = isPreparedToRise();

        if (isRising) {
            controller.Move(Vector3.up.normalized * 15.0f * Time.deltaTime); 
            gravityAcceleration = 0.1f;
        } else {
            if (!isStanding) {
                gravityAcceleration *= 1.05f;
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

    /*private void applyDustParticlesEffect() {
        if(isRunning) {
            dustParticles.GetComponent<ParticleSystem>().Play();
        } else {
            dustParticles.GetComponent<ParticleSystem>().Stop();
        }
    }*/
}

#if UNITY_EDITOR
[CustomEditor(typeof(SC_Movement))]
public class SC_MovementEditor : Editor {
    public override void OnInspectorGUI() {
        var scr = target as SC_Movement;

        scr.isCPU = GUILayout.Toggle(scr.isCPU, "Is CPU");

        using (new EditorGUI.DisabledScope(scr.isCPU)) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Camera");
            scr.cam = EditorGUILayout.ObjectField(scr.cam , typeof(Transform), true) as Transform;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Controller");
        scr.controller = EditorGUILayout.ObjectField(scr.controller , typeof(CharacterController), true) as CharacterController;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PrefixLabel("Walk speed");
        scr.walkSpeed = EditorGUI.Slider(EditorGUILayout.GetControlRect(), scr.walkSpeed, 5.0f, 50.0f);

        EditorGUILayout.PrefixLabel("Run speed");
        scr.runSpeed = EditorGUI.Slider(EditorGUILayout.GetControlRect(), scr.runSpeed, 5.0f, 50.0f);

        EditorGUILayout.PrefixLabel("Turn ease value");
        scr.turnSmoothTime = EditorGUI.Slider(EditorGUILayout.GetControlRect(), scr.turnSmoothTime, 0.0f, 1.0f);
    }
}
#endif