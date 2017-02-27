using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Only specifying the sceneName or sceneBuildIndex will load the scene with the Single mode
       
    }

    public void LoadVR()
    {
        SceneManager.LoadSceneAsync("TableScene");
    }

    public void LoadSplash()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }


    // Update is called once per frame
    void Update () {
		
	}
}
