using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPush : MonoBehaviour
{
    public GameObject lookout;
    public float hammerForce = 20.0f;
    private bool dashActive = false;

    public float dashTime = 250.0f;   // in ms
    private Vector3 dashDirection = Vector3.zero;

    CharacterController cc;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("OnCollisionEnter collision");
    }

    void OnCollisionEnter(Collider col)
    {
        Debug.Log("OnCollisionEnter collider");
    }

    void Update()
    {
        if (dashActive && cc)
        {
            cc.Move(dashDirection);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        cc = col.gameObject.GetComponent<CharacterController>();
        if (cc)
        {
            dashDirection = new Vector3(-lookout.transform.right.z, 0, lookout.transform.right.x) * Time.deltaTime * hammerForce;
            Invoke("ResetDash", dashTime / 1000.0f);
            dashActive = true;
        }
    }

    void ResetDash()
    {
        dashActive = false;
        cc = null;
        // Animator: reset bool/trigger here
    }
}
