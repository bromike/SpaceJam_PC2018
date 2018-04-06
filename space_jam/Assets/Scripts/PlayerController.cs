using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0F;
    //public float jumpSpeed = 8.0F;
    //public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public int playerId = 1;
    public bool isRdy = false;

    void Update() {

        if (Input.GetButton("Submit"))
            isRdy = true;

        CharacterController controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(Input.GetAxis("Horizontal_" + playerId), 0, Input.GetAxis("Vertical_" + playerId));

        //transform.rotation = Quaternion.LookRotation(moveDirection);
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15F);

        //;
        //Debug.Log(moveDirection);

        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
