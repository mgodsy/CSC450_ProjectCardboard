using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDataPoint : MonoBehaviour {

	public void SelectPoint() {
		// Locates the TargetData GO and sets the executing Point as the current target
		GameObject targetData = GameObject.Find("TargetData");
		TargetDataPoint targetDataPoint = targetData.GetComponent<TargetDataPoint> ();
		targetDataPoint.SetDataPoint (gameObject.name);

		// Loads the WorldScene
		LoadingScreenManager.LoadScene("WorldScene");
		return;
	}
}
