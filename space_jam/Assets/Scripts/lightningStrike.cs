using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightningStrike : MonoBehaviour
{
    CharacterController cc;
    public GameObject scorchMark;
    public float hammerForce = 20.0f;
    private bool dashActive = false;

    public float dashTime = 250.0f;   // in ms
    private Vector3 dashDirection = Vector3.zero;

    private void Start()
    {
        Instantiate(scorchMark, transform.position, transform.rotation);
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
            dashDirection = new Vector3(-cc.velocity.normalized.x, 0, -cc.velocity.normalized.z) * Time.deltaTime * hammerForce;
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
