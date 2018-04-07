using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flash : MonoBehaviour
{
    public List<AudioClip> thunders;

    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(thunders[Random.Range(0, thunders.Count - 1)]);
        Invoke("flashed", 500f / 1000f);
    }

    void flashed()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
