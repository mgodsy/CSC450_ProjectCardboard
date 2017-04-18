using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionTip : MonoBehaviour {

    public Text text;  //reference to UI text

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();  //initialize text

        // Load instructions from external text file.  Instructions should be
        // located in the Resources folder, in the file 'Instructions.txt'.
        // Instructions.txt should contain one instruction per line.  Ensure
        // that the file contains no blank lines.
        TextAsset instructionsFull = Resources.Load("Instructions") as TextAsset;

        // Select a random line from the full text to display.
        string[] instructions = instructionsFull.text.Split('\n');
        int index = Random.Range(0, instructions.Length);

        text.text = instructions[index];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
