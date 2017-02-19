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
    
    
    INFO: Camera Shake Effect

    Written by: Amir Badamchi
    
*/


using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float m_SwaySpeed = 3f;
    [SerializeField] private float m_BaseSwayAmount = 6f;

	void FixedUpdate ()
    {
        float bx = (Mathf.PerlinNoise(0, Time.time*m_SwaySpeed) - 0.5f);
        float by = (Mathf.PerlinNoise(0, (Time.time*m_SwaySpeed))) - 0.5f;

        bx *= m_BaseSwayAmount;
        by *= m_BaseSwayAmount;

        float tx = (Mathf.PerlinNoise(0, Time.time*m_SwaySpeed) - 0.5f);
        float ty = ((Mathf.PerlinNoise(0, (Time.time*m_SwaySpeed))) - 0.5f);

        transform.Rotate(bx + tx, by + ty, 0);
	}
}

