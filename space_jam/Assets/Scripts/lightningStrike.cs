using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightningStrike : MonoBehaviour
{
    CharacterController cc;
    public float pushForce = 20.0f;
    private bool pushActive = false;

    public float pushTime = 250.0f;   // in ms
    private Vector3 pushDirection = Vector3.zero;
    public int duration; //in ms.

    private void Start()
    {
        Invoke("finished", duration / 1000.0f);
    }

    void Update()
    {
        if (pushActive && cc)
        {
            cc.Move(pushDirection);
            Debug.Log("pushed");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        cc = col.gameObject.GetComponent<CharacterController>();
        if (cc)
        {
            GameObject parent = col.transform.root.gameObject;
            pushDirection = new Vector3(parent.transform.position.x, 0, parent.transform.position.z) * Time.deltaTime * pushForce;
            pushActive = true;
            Invoke("stopPush", pushTime / 1000.0f);
        }
    }

    void finished()
    {
        Destroy(gameObject);
    }

    void stopPush()
    {
        pushActive = false;
        cc = null;
    }
}
