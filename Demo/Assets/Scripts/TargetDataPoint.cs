using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDataPoint : MonoBehaviour {

	[SerializeField]
	private string dataPointName;	// The name of the targeted data point

	public static GameObject targetDataObj;	// Ref to the only copy of this Go that should exist, see below in Awake()

	void Awake () {
		// Ensures only one copy of this Go exists
		if (targetDataObj != null) {
			Destroy (gameObject);
		} else {
			targetDataObj = gameObject;
			DontDestroyOnLoad (targetDataObj);
		}

		dataPointName = "";
	}

	public void SetDataPoint(string name) {
		// Sets the the name of the targeted data point
		dataPointName = name;
	}

	public string GetDataPoint() {
		// Outputs the name of the currently selected target data point
		return dataPointName;
	}

	public bool isSet () {
		// Returns True of a data point has been set and false otherwise
		return dataPointName != null && dataPointName != "";
	}
}
