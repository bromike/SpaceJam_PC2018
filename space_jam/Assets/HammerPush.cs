using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPush : MonoBehaviour
{
    private GameObject parent;
    private Vector3 dashDirection = Vector3.zero;
    private bool dashActive = false;
    private CharacterController cc;

    public float dashTime = 250.0f;   // in ms
    public float hammerForce = 20.0f;

    private void Start()
    {
        parent = transform.root.gameObject;
    }

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
        if (cc == parent.gameObject.GetComponent<CharacterController>())
            return;

        if (cc)
        {
            dashDirection = new Vector3(-parent.transform.right.z, 0, parent.transform.right.x * 1.3f) * Time.deltaTime * hammerForce;
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
