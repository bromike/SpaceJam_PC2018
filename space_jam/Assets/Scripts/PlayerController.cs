using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;
    void Update() {


        CharacterController controller = GetComponent<CharacterController>();

        lastDirection = moveDirection;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.rotation = Quaternion.LookRotation(moveDirection);
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15F);

        //Vector3 facingrotation = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        // if (facingrotation != Vector3.zero)         //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
        //     transform.forward = facingrotation;

        //;
        //Debug.Log(moveDirection);

        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
