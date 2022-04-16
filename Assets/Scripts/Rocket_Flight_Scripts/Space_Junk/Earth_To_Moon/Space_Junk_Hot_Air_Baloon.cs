using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Hot_Air_Baloon : Space_Junk_Base
{

    private float maxRotation = 15.0f; // 15 degrees
    //Rigidbody2D rb;

    [SerializeField]
    private float thrustVert = 1.0f;
    [SerializeField]
    private float topSpeedVert = 0.2f; 

    private float speedVert = 0.0f;
    private float sideToSideDist = 0.5f;
    private float sideToSideTime = 2.0f;

    private bool hitHappened = false;
    private Vector2 ForceComponents;
    private Vector2 thrustVecHor;
    private Vector2 thrustVecVert;

    private Vector2 curForceVec;
    private float accelerationMultVert;

    private float spawnX;

    bool tweenPaused;
    int x_tween_id;

    Vector3 curRotationVec;

    bool firstInstance;
    Game_Scaler gameScaler;

    void Awake(){
        if (Random.value < 0.5){
            sideToSideDist = -sideToSideDist;
        }
        firstInstance = true;
    }

    void OnEnable()
    {
        base.OnEnable();
        tweenPaused = false;
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info += onSpaceJunkCollision;

        if (rb is null){
            rb = GetComponent<Rigidbody2D>();
        }
        
        if (gameScaler is null){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        }

        if (firstInstance){
            thrustVert = thrustVert * gameScaler.ScaleY;
            topSpeedVert = topSpeedVert * gameScaler.ScaleY;
            sideToSideDist = sideToSideDist * gameScaler.ScaleX;
            firstInstance = false;
        }

        thrustVecVert = new Vector2(0.0f, thrustVert);
        curForceVec = new Vector2();
        curRotationVec = transform.eulerAngles;

        spawnX = transform.position.x;
        initializeHotAirBaloonMovementHor();


        
    }

    void OnDisable(){
        base.OnDisable();
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info -= onSpaceJunkCollision;
        
        if (LeanTween.isTweening(x_tween_id)){
            LeanTween.cancel(x_tween_id);
        }

        hitHappened = false;

    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    //float accelerationMultHor;

    void Update(){

        if (!hitHappened){
            speedVert = rb.velocity.y;
            accelerationMultVert = 1.0f - (rb.velocity.y / topSpeedVert);


            if(Mathf.Abs(speedVert) < topSpeedVert){
                curForceVec = thrustVecVert * accelerationMultVert;
                rb.AddForce(curForceVec);
            }

            calculateHotAirBaloonRotation();
            transform.rotation = Quaternion.Euler(curRotationVec.x, curRotationVec.y, curRotationVec.z);
        }
    }



    private void initializeHotAirBaloonMovementHor(){
        Vector3 startLoc = transform.position;
        //Debug.Log("Baloon Tweening From " + startLoc.x + " To " + startLoc.x + (sideToSideDist/2.0f) + " DISTANCE IS " + Mathf.Abs());
        x_tween_id = LeanTween.moveX(gameObject, startLoc.x + (sideToSideDist/2.0f), sideToSideTime/2.0f).setEase(LeanTweenType.easeInOutSine).id;

        IEnumerator _HotAirBaloonMovementHor(){
            yield return new WaitForSeconds(0);
            if(LeanTween.isTweening(x_tween_id) || tweenPaused){
                StartCoroutine(_HotAirBaloonMovementHor());
            }
            else{
                sideToSideDist = -sideToSideDist;
                x_tween_id = LeanTween.moveX(gameObject, startLoc.x + sideToSideDist, sideToSideTime).setEase(LeanTweenType.easeInOutSine).id;
                StartCoroutine(_HotAirBaloonMovementHor());
            }
        }
        StartCoroutine(_HotAirBaloonMovementHor());
    }



    private void calculateHotAirBaloonRotation(){
        curRotationVec.z = transform.position.x > spawnX ? ((transform.position.x - spawnX) / Mathf.Abs(sideToSideDist)) * maxRotation : -((spawnX - transform.position.x) / Mathf.Abs(sideToSideDist)) * maxRotation;
    }



    private void onSpaceJunkCollision(GameObject obj){
        if (GameObject.ReferenceEquals(gameObject, obj)){
            hitHappened = true;
            if (LeanTween.isTweening(x_tween_id)){
                LeanTween.cancel(x_tween_id);
            }
        }
    }

}