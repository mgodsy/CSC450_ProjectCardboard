using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : MonoBehaviour {

    public GameObject player;
    bool moving;

    void Update()
    {
        //Calculate how far the menu is from the center of the screen

        float diff = player.transform.eulerAngles.y - transform.eulerAngles.y;

        if (moving)
        {
            //Menu canvas is in a 'moving' state;
            //check to see if it's time to stop moving,
            //or what direction it needs to be moving in

            if(Mathf.Abs(diff) < 1)
            {
                //Menu has moved back in front of the user; stop moving

                moving = false;
            }
            else
            {
                //Menu is still away from user;
                //if the difference between the player and menu rotations is
                //less than 180 degrees, move right; else move left

                Move(diff < 180 ? diff : -diff);
            }
        }
        else
        {
            //Menu canvas is not in a 'moving' state
            //Check to see if it needs to start moving

            if (Mathf.Abs(diff) > 60)
            {
                //Only start moving if the menu is more than 60 degrees off-center

                moving = true;
            }
        }        
    }

    void Move(float dir)
    {
        //Amount of movement is relative to the difference between the player and menu rotations
        //This causes the motion to be less abrupt; it moves quickly when the user looks away,
        //and comes to a gradual stop in front of them

        transform.Rotate(dir * 2 * Vector3.up * Time.deltaTime);
    }
}
