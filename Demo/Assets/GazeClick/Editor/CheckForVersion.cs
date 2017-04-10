using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class CheckForVersion
{
	static CheckForVersion ()
	{
		#if !AutoClickDevelopment
		if (float.Parse (GvrViewer.GVR_SDK_VERSION) < 1.1f) {

			string content = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/Version1.0.txt"));
			string file = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"));
			if (file.Contains ("Basic Version")) {	
				Debug.Log ("Google VR Version " + GvrViewer.GVR_SDK_VERSION + " found. Importing old GazeClick Version.");
				File.WriteAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"), content);
				AssetDatabase.Refresh ();
			}
		} else if (float.Parse (GvrViewer.GVR_SDK_VERSION) < 1.2f) {

			string content = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/Version1.1.txt"));
			string file = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"));
			if (file.Contains ("Basic Version")) {	
				Debug.Log ("Google VR Version " + GvrViewer.GVR_SDK_VERSION + " found. Importing original GazeClick Version.");
				File.WriteAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"), content);
				AssetDatabase.Refresh ();
			}
		} else if (float.Parse (GvrViewer.GVR_SDK_VERSION) == 1.2f) {
			string content = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/Version1.2.txt"));
			string file = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"));
			if (file.Contains ("Basic Version")) {	
				Debug.Log ("Google VR Version " + GvrViewer.GVR_SDK_VERSION + " found. Importing new GazeClick Version.");
				File.WriteAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"), content);
				AssetDatabase.Refresh ();
			}
		} else if (float.Parse (GvrViewer.GVR_SDK_VERSION) == 1.3f) {
			string content = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/Version1.3.txt"));
			string file = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"));
			if (file.Contains ("Basic Version")) {	
				Debug.Log ("Google VR Version " + GvrViewer.GVR_SDK_VERSION + " found. Importing new GazeClick Version.");
				File.WriteAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"), content);
				AssetDatabase.Refresh ();
			}
		} else {

			string content = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/Version1.3.txt"));
			string file = File.ReadAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"));
			if (file.Contains ("Basic Version")) {	
				Debug.Log ("Google VR Version " + GvrViewer.GVR_SDK_VERSION + " found. This version might not be supported yet.");
				File.WriteAllText (System.IO.Path.Combine (Application.dataPath, "GazeClick/Implementation/GazeClick.cs"), content);
				AssetDatabase.Refresh ();
			}
		}
		#endif
	}
}