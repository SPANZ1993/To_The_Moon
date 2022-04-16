using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Cow : Space_Junk_Flyer
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    protected override void Update()
    {
        if (!hitHappened){
            if (gameScaler is null){
                accelerationMult = 1 - (rb.velocity.magnitude / gameScaler.Scale_Value_To_Screen_Width(topSpeed));
            }
            else{
                accelerationMult = 1 - (rb.velocity.magnitude / topSpeed);
            }
            speed = rb.velocity.magnitude;
            if (Mathf.Abs(speed) < topSpeed){
                rb.AddForce(thrustVec * accelerationMult);
            }
            if (shouldFlipSprite){
                if (ForceComponents.x >= 0 && !rend.flipX){
                    //flipSprite();
                    rend.flipX = true;
                }
                else if (ForceComponents.x < 0 && rend.flipX){
                    //flipSprite();
                    rend.flipX = false;
                }
            }
        }
    }
}
