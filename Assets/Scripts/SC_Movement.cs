using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f; //Movement speed

    public float turnSmoothTime = 0.1f; //Smooth animation when rotating
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //Left, Right
        float vertical = Input.GetAxisRaw("Vertical"); //Back, Forward
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //Vector operations //.normalized when go diagonally for don't move faster

        if(direction.magnitude >= 0.1f) //Check if we're moving in any direction is greater or equal then 0.1f
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //Turn character to target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //Turn character by given angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //Performs a rotation

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime); //Simple move character
        }
    }
}
