using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        ////OLD POLAR BEAR CODE
        //if (other.CompareTag("Player"))
        //{
        //    //GetComponent<Animator>().SetTrigger("EatingRange");
        //    //other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        //    bearManager.PlayOneShot(snapSound);
        //    StartCoroutine(WaitAndHideMesh(other.gameObject, fishDissapearDelay));
        //}
    }
}
