using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightningStrike : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(destroyAfterStrike());
	}

    IEnumerator destroyAfterStrike()
    {
        yield return new WaitForSeconds((float)0.083);     //this is 5 frame.
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1);                //Need to change seconde in order to match anim time.
        Destroy(gameObject);
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    CharacterController player = hit.gameObject.GetComponent<CharacterController>();
    //    if(player)
    //    {
    //        player.Move(new Vector3(-hit.moveDirection.x + transform.position.x, 0, -hit.moveDirection.z + transform.position.z));
    //    }
    //}
}
