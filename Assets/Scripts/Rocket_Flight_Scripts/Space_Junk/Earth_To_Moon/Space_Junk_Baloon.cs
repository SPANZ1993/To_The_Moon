using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Baloon : Space_Junk_Base
{

    private float maxRotation = 15.0f; // 15 degrees
    //Rigidbody2D rb;
    private bool goingRight;
    private float offsetAngle;
    private Vector3 prevHorPos;
    private float thrustVert = 0.5f;
    private float thrustHor = 0.5f;
    private float topSpeedHor = 2.0f;
    private float topSpeedVert = 1.0f; 

    [SerializeField]
    private float speedVert = 0.0f;
    [SerializeField]
    private float speedHor = 0.0f;
    [SerializeField]
    private float sideToSideDist = 1.0f;

    private bool hitHappened = false;
    private Vector2 ForceComponents;
    private Vector2 thrustVecHor;
    private Vector2 thrustVecVert;

    void Awake(){
        //offsetAngle = Random.Range(-maxRotation, maxRotation);
        if (Random.value < 0.5){
            goingRight = false; 
        }
        else{
            goingRight = true;
        }
    }

    void OnEnable()
    {
        base.OnEnable();
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info += onSpaceJunkCollision;
    }

    void OnDisable(){
        base.OnEnable();
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info -= onSpaceJunkCollision;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //Debug.Log("DIRECTION: " + offsetAngle);
        //Debug.Log("DIR GOING RIGHT: " + goingRight);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, offsetAngle);
        //CalculateXYMovementComponents(offsetAngle, goingRight);
        thrustVecHor = new Vector2(thrustHor, 0.0f);
        thrustVecVert = new Vector2(0.0f, thrustVert);
        prevHorPos = transform.position;
        // if (!goingRight){
        //     Vector3 curScale = transform.localScale;
        //     transform.localScale = new Vector3(curScale.x * -1f, curScale.y, curScale.z);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitHappened){
            float accelerationMultVert = 1 - (rb.velocity.y / topSpeedHor);
            float accelerationMultHor = 1 - (rb.velocity.x / topSpeedVert);
            speedHor = rb.velocity.x;
            speedVert = rb.velocity.y;
            if (Mathf.Abs(speedVert) < topSpeedVert){
                rb.AddForce(thrustVecVert * accelerationMultVert);
            }
            if (Mathf.Abs(speedHor) < topSpeedHor){
                rb.AddForce(thrustVecHor * accelerationMultHor * (goingRight ? 1.0f : -1.0f));
            }
        }
    }

    private void checkHorizontalPosition(){
        if (Mathf.Abs(transform.position.x - prevHorPos.x)  > sideToSideDist){
            prevHorPos = transform.position;
            goingRight = !goingRight;
        }
    }

    // private void CalculateXYMovementComponents(float angle, bool goingRight){
    //     float xComp = Mathf.Cos(Mathf.Abs(angle  * Mathf.Deg2Rad));
    //     float yComp = Mathf.Sin(Mathf.Abs(angle  * Mathf.Deg2Rad));
    //     if (angle >= 0.0f && goingRight){
    //         ForceComponents = new Vector2(xComp, yComp).normalized;
    //     }
    //     else if(angle < 0.0f && goingRight){
    //         ForceComponents = new Vector2(xComp, -yComp).normalized;
    //     }
    //     else if(angle >= 0.0f && !goingRight){
    //         ForceComponents = new Vector2(-xComp, -yComp).normalized;
    //     }
    //     else{
    //         ForceComponents = new Vector2(-xComp, yComp).normalized;
    //     }
    // }

    private void onSpaceJunkCollision(GameObject obj){
        if (GameObject.ReferenceEquals(gameObject, obj)){
            //Debug.Log("HEY WE GOT HIT");
            hitHappened = true;
        }
    }

}