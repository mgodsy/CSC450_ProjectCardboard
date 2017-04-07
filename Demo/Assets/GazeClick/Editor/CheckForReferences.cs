using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class CheckForReferences {
	static CheckForReferences()
	{
		try {
			StreamReader stream = File.OpenText (System.IO.Path.Combine(Application.dataPath, "GoogleVR/Scripts/GvrViewer.cs"));
			if (stream==null) {
				Debug.Log ("GoogleVR is needed in order to use the AutoClick unity package!" +
					"You can download it at https://developers.google.com/vr/unity/download ");
			}
		} catch (System.Exception) {
			Debug.Log ("GoogleVR is needed in order to use the AutoClick unity package!" +
				"You can download it at https://developers.google.com/vr/unity/download ");
		}	
	}
}