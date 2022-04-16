using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Ship_Controls_PC : MonoBehaviour
{
    private double posx, posy, vertical_delta, horizontal_delta, orig_vertical_delta,
    orig_horizontal_delta, speed;
    public double acceleration, topspeed;

    void updateSpeed()
    {
        // Big ups to my homie Pythagoras
        speed = System.Math.Sqrt((vertical_delta * vertical_delta) + (horizontal_delta * horizontal_delta));
        speed = System.Math.Abs(speed);
    }


    // Start is called before the first frame update
    void Start()
    {
        vertical_delta = 0.0;
        horizontal_delta = 0.0;
        orig_vertical_delta = 0.0;
        orig_horizontal_delta = 0.0;
    }

    // Update is called once per frame
    void Update()
    {
        orig_vertical_delta = vertical_delta;
        orig_horizontal_delta = horizontal_delta;


        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
        // Move to the right
            if (Math.Abs(horizontal_delta + acceleration) <= topspeed)
            {
                horizontal_delta += acceleration;
            }
        } 
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
        // Move to the left
            if (Math.Abs(horizontal_delta - acceleration) <= topspeed)
            {
                horizontal_delta -= acceleration;
            }
        }

        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0) {
        // Move up
            if (Math.Abs(vertical_delta + acceleration) <= topspeed)
            {
                vertical_delta += acceleration;
            }
        } 
        else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0) {
        // Move down
            if (Math.Abs(vertical_delta - acceleration) <= topspeed)
            {
                vertical_delta -= acceleration;
            }
        }

        updateSpeed();


        transform.position = new Vector3(transform.position.x + (float)horizontal_delta, 
                                            transform.position.y + (float)vertical_delta,
                                            transform.position.z);
        
        // else
        // {
        //     transform.position = new Vector3(transform.position.x + (float)orig_horizontal_delta, 
        //                         transform.position.y + (float)orig_vertical_delta,
        //                         transform.position.z);
        //     vertical_delta = orig_vertical_delta;
        //     horizontal_delta = orig_horizontal_delta;
        // }
    }
}
