using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Golf_Ball : MonoBehaviour
{

    Space_Junk_Flyer flyer;

    private bool Launched = false;
    private int Golf_Ball_Growth_Tween_Id = -1;

    private float wipeTime = 1f;

    void OnEnable(){
        flyer = gameObject.GetComponent<Space_Junk_Flyer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(flyer == null){
            flyer = gameObject.GetComponent<Space_Junk_Flyer>();
        }
        flyer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(float offsetAngle, float thrust, float topSpeed, bool goingRight){
        flyer.enabled = true;
        flyer.offsetAngle = offsetAngle;
        flyer.thrust = thrust;
        flyer.topSpeed = topSpeed;
        flyer.goingRight = goingRight;
        transform.SetParent(null);
        flyer.CalculateXYMovementComponents(flyer.offsetAngle, flyer.goingRight);
        flyer.CalculateThrustVec();
        Launched = true;
        Golf_Ball_Growth_Tween_Id = LeanTween.scale(gameObject, gameObject.transform.transform.localScale * 5f, wipeTime).setEase(LeanTweenType.easeOutSine).id;
    }

    public void OnCollisionEnter2D(){
        if(Launched && LeanTween.isTweening(Golf_Ball_Growth_Tween_Id)){
            LeanTween.cancel(Golf_Ball_Growth_Tween_Id);
        }
    }
}
