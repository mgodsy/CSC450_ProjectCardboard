/*
    _____  _____  _____  _____  ______
        |  _____ |      |      |  ___|
        |  _____ |      |      |     |
    
     U       N       I       T      Y
                                         
    
    TerraUnity Co. - Earth Simulation Tools - 2016
    
    http://terraunity.com
    info@terraunity.com
    
    This script is written for Unity 3D Engine.
    Unity 3D Version: Unity 5.x
    
    
    INFO: Wiggle 3D effect for camera

    Written by: Amir Badamchi
    
*/


using UnityEngine;
using System.Collections;

public class Wiggle3D : MonoBehaviour
{
	public float pan = 0.1f;
	public float speed = 7f;
	public float focalDistance = 5f;
    private Vector3 focalPoint;
	
	void FixedUpdate ()
	{
        focalPoint = new Vector3
            (
                focalPoint.x,
                focalPoint.y,
                -focalDistance
            );

		activate3D();
	}
	
	void activate3D ()
    {
		float offset = transform.position.x + (Mathf.Sin(Time.fixedTime * speed) * pan);
		transform.position = new Vector3(offset, transform.position.y, transform.position.z);
        transform.LookAt(focalPoint);
	}
}

