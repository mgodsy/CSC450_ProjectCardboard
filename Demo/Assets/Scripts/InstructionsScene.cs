using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScene : MonoBehaviour {

    public Text text;  //reference to the canvas text

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();  //initialize text

        TextAsset instructionsFull = Resources.Load("Instructions") as TextAsset;

        string[] instructions = instructionsFull.text.Split('\n');

        for (int i = 0; i < instructions.Length; i++)
        {
            text.text += instructions[i];
            text.text += "\n\n";
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
