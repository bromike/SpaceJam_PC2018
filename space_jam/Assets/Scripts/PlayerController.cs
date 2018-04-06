using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //public float jumpSpeed = 8.0F;
    //public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public int playerId = 1;

    [HideInInspector]
    public bool isRdy = false;

    // TODO: put these in common class (GameManager?)
    public float gravity = 20.0F;
    public float speed = 6.0F;
    public float cooldownTime = 1000.0f;    // in ms
    public float dashSpeed = 50.0f;
    public float dashTime = 50.0f;          // in ms

    private bool cooldown = false;
    private bool dashActive = false;

    private CharacterController controller;
    //private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetButton("Submit"))
            isRdy = true;

        if (controller.isGrounded)
        {
            if (!dashActive)
                moveDirection = new Vector3(Input.GetAxis("Horizontal_" + playerId), 0, Input.GetAxis("Vertical_" + playerId));

            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15f);

            if (Input.GetButton("Fire1_" + playerId) && !cooldown)
            {
                Invoke("ResetCooldown", cooldownTime / 1000.0f);
                cooldown = true;
                Invoke("ResetDash", dashTime / 1000.0f);
                dashActive = true;
            }

            if (dashActive)
            {
                // Animator: set bool/trigger here
                controller.Move(new Vector3(-transform.right.z, 0, transform.right.x) * dashSpeed * Time.deltaTime);
                return;
            }
            moveDirection *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    void ResetDash()
    {
        dashActive = false;
        // Animator: reset bool/trigger here
    }
    void ResetCooldown()
    {
        // Cooldown UI : hide here
        cooldown = false;
    }

    void OnCollisionEnter (Collision col)
    {
        Debug.Log("PlayerController.OnCollisionEnter");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject other = hit.gameObject;
        if (other)
        {
            if (other.CompareTag("Weapon"))
            {
                //other.SendMessage("ReceiveMessage", hit.gameObject.GetInstanceID());
                Debug.Log("Parent from Weapon is " + other.transform.parent);
                controller.Move(new Vector3(-hit.moveDirection.x, 0, -hit.moveDirection.z) * Time.deltaTime * 100);
            }
            CharacterController otherController = other.GetComponent<CharacterController>();
            if(otherController)
            {
                otherController.Move(new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * Time.deltaTime * speed);
            }
        }
    }
}
