using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float rotationSpeed = 40f;
    public bool rotateClockwise = false;
    public bool rotateCounterClockwise = false;

    private float angle = 0;  //used to track rotation amount


    // Function to call to set table to rotate left
    public void rotateLeft()
    {
        rotateClockwise = true;
    }

    // Function to call to set table to rotate right
    public void rotateRight()
    {
        rotateCounterClockwise = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Rotate 90 degrees in clockwise direction
        if (rotateClockwise)
        {
            if (angle <= 90)
            {
                // Apply rotation
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                angle += rotationSpeed * Time.deltaTime;
            } else
            {
                // Rotation finished; reset variables
                angle = 0;
                rotateClockwise = false;
            }
        }

        // Rotate 90 degrees in counterclockwise direction
        if (rotateCounterClockwise)
        {
            if (angle <= 90)
            {
                // Apply rotation
                transform.Rotate(Vector3.up, -(rotationSpeed * Time.deltaTime));
                angle += rotationSpeed * Time.deltaTime;
            }
            else
            {
                // Rotation finished; reset variables
                angle = 0;
                rotateCounterClockwise = false;
            }
        }

    }
}
