using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour {

    public float rotationSpeed = 180f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Rotate loading icon
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
		
	}
}
