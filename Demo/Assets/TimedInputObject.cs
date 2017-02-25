using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class TimedInputObject : MonoBehaviour, TimedInputHandler{

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.white; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleTimedInput() {
        GetComponent<Renderer>().material.color = Color.blue;
    }
}
