using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
