using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_UFO : Space_Junk_Base
{

    [SerializeField]
    bool DebugMode = false;


    //private Rigidbody2D rb;
    [SerializeField]
    private float maxRotation = 15.0f; // 15 degrees
    [SerializeField]
    private float wiggleTime = 0.5f;
    private int wiggleTweenId;
    private bool curWiggleRight = true;


    [SerializeField]
    private float minPossibleMoveTime, maxPossibleMoveTime;

    [SerializeField]
    private float minPossibleDistanceTraveled, maxPossibleDistanceTraveled;
    private float possibleDistanceTraveled;

    [SerializeField]
    private float minPossibleWaitTime, maxPossibleWaitTime;
    private float curWaitTime, curWaitTimeLeft;

    private Vector2 nextDestination;
    private bool nextDestinationChosen = false;
    private int moveTweenId;
    private bool onFinalTween;


    private bool hitHappened = false;

    private Game_Scaler gameScaler;

    void OnEnable()
    {
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info += onSpaceJunkCollision;

        transform.rotation = new Quaternion();

        if (!DebugMode && gameScaler == null){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        }

        wiggle();
    }

    void OnDisable(){
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info -= onSpaceJunkCollision;


    
        if (LeanTween.isTweening(wiggleTweenId)){
            LeanTween.cancel(wiggleTweenId);
        }
        if (LeanTween.isTweening(moveTweenId)){
            LeanTween.cancel(moveTweenId);
        }
    }


    // Start is called before the first frame update
    void Start()
    {  

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void chooseNextDestination(){
        
        IEnumerator _chooseNextDestination(){
            yield return new WaitForSeconds(0);
            possibleDistanceTraveled = Random.Range(minPossibleDistanceTraveled, maxPossibleDistanceTraveled);
            nextDestination = Random.insideUnitCircle;
            nextDestination = nextDestination * (possibleDistanceTraveled / nextDestination.magnitude);
            if (!DebugMode){
                nextDestination.x = nextDestination.x * gameScaler.ScaleX;
                nextDestination.y = nextDestination.y * gameScaler.ScaleY;
            }
            nextDestination = (Vector2)transform.position + nextDestination;
            nextDestinationChosen = true;
        }

        StartCoroutine(_chooseNextDestination());
    }

    private void wiggleWait(){

        IEnumerator _wiggleWait(){
            yield return new WaitForSeconds(0);
            if(curWaitTimeLeft >= 0.0f){
                curWaitTimeLeft -= Time.deltaTime;
                StartCoroutine(_wiggleWait());
            }
        }

        nextDestinationChosen = false;
        chooseNextDestination();


        curWaitTime = Random.Range(minPossibleWaitTime, maxPossibleWaitTime);
        curWaitTimeLeft = curWaitTime;

        
        StartCoroutine(_wiggleWait());
    }


    private void wiggle(){
        wiggleWait();
        onFinalTween = false;
        _wiggle();
    }


    private void _wiggle(){

        IEnumerator _waitForFinalWiggle(){
            yield return new WaitForSeconds(0);
            if (LeanTween.isTweening(wiggleTweenId)){
                StartCoroutine(_waitForFinalWiggle());
            }
            else{
                moveToNextDestination();
            }
        }
        
        if (curWaitTimeLeft > 0.0f || !nextDestinationChosen){
            wiggleTweenId = LeanTween.rotateZ(gameObject, curWiggleRight ? maxRotation : -maxRotation, wiggleTime).setEase(LeanTweenType.easeInOutSine).id;
            curWiggleRight = !curWiggleRight;
            LeanTween.descr(wiggleTweenId).setOnComplete(_wiggle);
        }
        else if (curWaitTimeLeft <= 0.0f && !onFinalTween){
            wiggleTweenId = LeanTween.rotateZ(gameObject, 0.0f, wiggleTime/2.0f).setEase(LeanTweenType.easeInSine).id;
            curWiggleRight = !curWiggleRight;
            LeanTween.descr(wiggleTweenId).setOnComplete(_wiggle);
            onFinalTween = true;
        }
        else {
            if (LeanTween.isTweening(wiggleTweenId)){
                StartCoroutine(_waitForFinalWiggle());
            }
            else{
                moveToNextDestination();
            }
        }
    }

    private void moveToNextDestination(){
        moveTweenId = LeanTween.move(gameObject, nextDestination, Random.Range(minPossibleMoveTime, maxPossibleMoveTime)).setEase(LeanTweenType.easeInOutSine).id;
        LeanTween.descr(moveTweenId).setOnComplete(wiggle);
    }
    


    private void onSpaceJunkCollision(GameObject obj){
        if (GameObject.ReferenceEquals(gameObject, obj)){
            hitHappened = true;
            if (LeanTween.isTweening(wiggleTweenId)){
                LeanTween.cancel(wiggleTweenId);
            }
            if (LeanTween.isTweening(moveTweenId)){
                LeanTween.cancel(moveTweenId);
            }
        }
    }

}
