using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Satellite : Space_Junk_Flyer
{

    [SerializeField]
    private float dishPivotDegrees, dishPivotTime;
    private bool firstDishPivot;
    private int dishPivotId;

    private float curDishAngle;

    private GameObject dish;
    private GameObject dishPivotPoint;


    bool first;

    protected override void OnEnable(){


        dish = transform.GetChild(0).gameObject;
        dishPivotPoint = transform.GetChild(2).gameObject;
        firstDishPivot = true;
        // thrust = Random.Range(minPossibleThrust, maxPossibleThrust);
        // topSpeed = Random.Range(minPossibleTopSpeed, maxPossibleTopSpeed);
        // offsetAngle = Random.Range(-maxRotation, maxRotation);
        base.OnEnable();
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // if (!goingRight){ // Fixes it from being flipped if it's going left
        //     Vector3 curScale = transform.localScale;
        //     transform.localScale = new Vector3(curScale.x * -1f, curScale.y, curScale.z);
        // }

        first = true;

    }


    // Start is called before the first frame update
    protected override void Start()
    {
        // dish = transform.GetChild(0).gameObject;
        // dishPivotPoint = transform.GetChild(2).gameObject;
        // firstDishPivot = true;
        // // thrust = Random.Range(minPossibleThrust, maxPossibleThrust);
        // // topSpeed = Random.Range(minPossibleTopSpeed, maxPossibleTopSpeed);
        // // offsetAngle = Random.Range(-maxRotation, maxRotation);
        // base.Start();
        // transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // if (!goingRight){ // Fixes it from being flipped if it's going left
        //     Vector3 curScale = transform.localScale;
        //     transform.localScale = new Vector3(curScale.x * -1f, curScale.y, curScale.z);
        // }
        // //rotateDish();
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();
        if (first){
            rotateDish();
            first = false;
        }   
    }

    // private void rotateDish(){
    //     if (!firstDishPivot){
    //         dishPivotId = LeanTween.rotateAround(dish, rotateVec, dishPivotDegrees, dishPivotTime).setEase(LeanTweenType.easeInOutSine).id;
    //         firstDishPivot = false;
    //     }
    //     else{
    //         dishPivotId = LeanTween.rotateAround(dish, rotateVec, dishPivotDegrees/2.0f, dishPivotTime/2.0f).setEase(LeanTweenType.easeOutSine).id;
    //     }
    //     dishPivotDegrees = -dishPivotDegrees;
    //     LeanTween.descr(dishPivotId).setOnComplete(rotateDish);
    // }

    float p;
    float q;
    float angle;
    float angleToAdd;

    private void rotateDish(){

        float mod(float x, float m) {
            return (x%m + m)%m;
        }

        float curRotationAngle = 0.0f;
        bool dishGoingRight = false;

        Vector2 Point_1;
        Vector2 Point_2;
        float angle;
        float angleToAdd;


        IEnumerator _rotateDish(){
            yield return new WaitForSeconds(0);
            Point_1 = (Vector2) dishPivotPoint.transform.position;
            Point_2 = (Vector2) dish.transform.position;
            angle = Mathf.Atan2(Point_2.y - Point_1.y , Point_2.x-Point_1.x) * 180 / Mathf.PI;
            // angle = mod(angle, 360.0f);
            //angle = Mathf.fmodf(angle, 360.0f);
            //Debug.Log("ANGLE: " + angle);
            p = dishGoingRight ? dishPivotDegrees : -dishPivotDegrees;
            q = Time.deltaTime / dishPivotTime;
            angleToAdd = p * q;
            //angleToAdd = dishGoingRight ? 0.02f : -0.02f;
            //Debug.Log("A2A Before: " + angleToAdd + " DTIME: " + Time.deltaTime);
            //Debug.Log("ANGLE TO ADD: " + angleToAdd);
            // if (dishGoingRight){
            //     if (curRotationAngle + angleToAdd >= dishPivotDegrees){
            //         angleToAdd = dishPivotDegrees - curRotationAngle;
            //         dishGoingRight = false;
            //     }

            // }
            // else{
            //     if (curRotationAngle + angleToAdd <= -dishPivotDegrees){
            //         angleToAdd = -dishPivotDegrees + curRotationAngle;
            //         dishGoingRight = true;
            //     }
            // }

            if (dishGoingRight && curRotationAngle + angleToAdd >= dishPivotDegrees){
                angleToAdd = dishPivotDegrees - curRotationAngle;
                ///Debug.Log("HERE GOING RIGHT!!!: " + angleToAdd);
                dishGoingRight = false;
            }
            else if (!dishGoingRight && curRotationAngle + angleToAdd <= -dishPivotDegrees){
                angleToAdd = -dishPivotDegrees - curRotationAngle;
                //Debug.Log("HERE GOING LEFT!!!: " + angleToAdd);
                dishGoingRight = true;
            }

            //Debug.Log("A2A After: " + angleToAdd);
            curRotationAngle = curRotationAngle + angleToAdd;


            //dish.transform.RotateAround(dishPivotPoint.transform.position, Vector3.forward, angleToAdd);
            //dish.transform.eulerAngles = new Vector3(0.0f, 0.0f, curRotationAngle);
            dish.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, curRotationAngle);
            StartCoroutine(_rotateDish());
        }

        StartCoroutine(_rotateDish());
    }



}
