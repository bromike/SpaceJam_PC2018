using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour {

    //void OnColliderEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject);
    //}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("OnCollisionEnter collision");
    }

    void OnCollisionEnter(Collider col)
    {
        Debug.Log("OnCollisionEnter collider");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject != transform.parent)
        {
            Debug.Log(col.gameObject);
            Debug.Log(transform.parent.gameObject);
            col.gameObject.GetComponent<CharacterController>().Move(new Vector3(transform.right.z, 0, -transform.right.x) * Time.deltaTime * 100);
        }
        Debug.Log("CollisionTest.OnTriggerEnter");
    }

    void ReceiveMessage(int id)  
    {
        if (id == gameObject.GetInstanceID())
            Debug.Log(id);
    }
}
