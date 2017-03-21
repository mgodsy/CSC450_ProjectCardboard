using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScene : MonoBehaviour {

    public Text text;  //reference to the canvas text

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();  //initialize text

        TextAsset credits = Resources.Load("Credits") as TextAsset;

        text.text = credits.text;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
