using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {

    public float speed;
    private int clkWise;

    void Start()
    {
        clkWise = Random.value > 0.5f ? 1 : -1;
    }
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, clkWise * speed * Time.deltaTime);
		
	}
}
