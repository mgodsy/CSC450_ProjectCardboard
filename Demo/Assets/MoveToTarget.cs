using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour {

	GameObject targetData;
	TargetDataPoint targetDataPoint;
	string currentDataPoint;
	public float offset = 15.0f;

	void MoveToDataPoint() {
		Transform targetPointTransform = GameObject.Find(targetDataPoint.GetDataPoint ()).transform;
		gameObject.transform.position = new Vector3 (targetPointTransform.position.x, targetPointTransform.position.y, targetPointTransform.position.z - offset);
		currentDataPoint = targetDataPoint.GetDataPoint ();
		return;
	}






	void Start () {
		targetData = GameObject.Find ("TargetData");
		targetDataPoint = targetData.GetComponent<TargetDataPoint>();
		currentDataPoint = null;
	}
	
	void Update () {
		if (currentDataPoint == null || currentDataPoint != targetDataPoint.GetDataPoint ()) {
			MoveToDataPoint ();
		}
	}
}
