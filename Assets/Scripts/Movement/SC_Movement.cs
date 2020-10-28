using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f; //Movement speed

    public float turnSmoothTime = 0.1f; //Smooth time when rotating
    float turnSmoothVelocity; //Smooth animation when rotating

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //Left, Right
        float vertical = Input.GetAxisRaw("Vertical"); //Back, Forward
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //Vector operations //.normalized when go diagonally for don't move faster

        //Check if we're moving in any direction is greater or equal then 0.1f
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //Turn character to target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //Turn character by given angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //Performs a rotation

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //Move declaration
            controller.Move(moveDir.normalized * speed * Time.deltaTime); //Simple move character

            anim.SetBool("isRunning", true); //Move animation
        } else {
            anim.SetBool("isRunning", false); //Idle animation
        }
    }
}
