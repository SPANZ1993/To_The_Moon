using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Minecart_Controller : MonoBehaviour, ITappable
{

    [HideInInspector]
    public double lastEmptiedTimeUnix;
    public double curCoins;
    public double coinsCapacity;
    public double coinsPerSecond;
    public double nextFullTimeUnix {get; set;}
    public double secondsLeft {get; private set;} // Don't set this from the outside
    public bool isFull {get; private set;}

    private bool tapInitiated = false; // The current tap started on the minecart
    private bool tapped = false; // The screen is held down currently on the minecart
    private List<Vector2> curDragLocs = new List<Vector2>();

    
    private int curAnim = 0;

    private Animator minecartAnimator;

    [SerializeField]
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;
    private Game_Manager gameManager;

    public bool startComplete = false;



    public delegate void MinecartTapped(double coins);
    public static event MinecartTapped MinecartTappedInfo;




    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        minecartAnimator = GetComponent<Animator>();
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
        //lastEmptiedTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(); // REMOVE
        startComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If it hasn't yet been initialized by the game manager, then just chill
        if (nextFullTimeUnix != 0){
            curDragLocs = touchDetection.dragLocs;
            calculateCurCoins();
            updateAnimation();
            secondsLeft = calculateSecondsLeft();
            //Debug.Log("CUR COINS: " + curCoins);
        }
        //Debug.Log("IS FULL??: " + isFull + " SECONDS LEFT: " + secondsLeft);
    }


    public void onTapStart()
    {

        // Debug.Log("LBC START");
        
        //If we dragged onto
        if (curDragLocs.Count > 1){
            tapped = true;
        }
        // If we tapped onto
        else{
            tapInitiated = true;
            tapped = true;
        }

    }
    
    public void onTapStay()
    {
        // Debug.Log("LBC STAY");
    }



    // We want to do this so we can do different things when the user taps and drags off the sprite
    // and when the user taps and lets go on the sprite
    // (Both of these events call onTapEnd())
    IEnumerator _onTapEnd(bool first)
    {
        yield return new WaitForSeconds(0);
        if(touchDetection.tapInProgress){
            // Debug.Log("Dragged Off");
            if (first){
                tapped = false;
            }
            StartCoroutine(_onTapEnd(false));
        }
        else
        {
            if (tapInitiated && tapped){
                Debug.Log("TAPPED THE MINECART");
                Debug.Log("CURCOINS: " + curCoins);
                if (curCoins != 0){
                    double curCoinsTmp = curCoins;
                    curCoins = 0.0;
                    lastEmptiedTimeUnix = gameManager.gameTimeUnix;
                    calculateNextFullTime();
                    MinecartTappedInfo(curCoinsTmp);
                }
            }
            tapped = false;
            tapInitiated = false;

        }
    }


    public void onTapEnd()
    {
        // Debug.Log("LBC END");
        // If we lifted our finger, else if we dragged off the object
        StartCoroutine(_onTapEnd(true));
    }


    public double calculateSecondsLeft(){
        double coinsLeft = coinsCapacity - curCoins;
        double _secondsLeft = Math.Ceiling(coinsLeft/coinsPerSecond);
        return _secondsLeft;
    }

    public void calculateNextFullTime(){
        double _secondsLeft = calculateSecondsLeft();
        nextFullTimeUnix = Math.Floor(gameManager.gameTimeUnix) + _secondsLeft;
    }
    
    // public void calculateNextFullTime(){
    //     double timeToFill = coinsCapacity / coinsPerSecond;
    //     nextFullTimeUnix = lastEmptiedTimeUnix + timeToFill;
    // }

    
    public void initializeCurCoins(double prevCoins){
    //This will only be called by the game manager to calculate the coins
    //at the start of the game after an initialization
        double secondsSinceEmptied = gameManager.gameTimeUnix - lastEmptiedTimeUnix;
        double curCoinsCalc = prevCoins + Math.Floor(secondsSinceEmptied * coinsPerSecond);
        curCoinsCalc = Math.Max(curCoinsCalc, 0.0);
        if (curCoinsCalc >= coinsCapacity){
            curCoins = coinsCapacity;
            isFull = true;
        }
        else{
            curCoins = curCoinsCalc;
            isFull = false;
        }
    }

    // public void initializeCurCoins(double prevCoins){
    // //This will only be called by the game manager to calculate the coins
    // //at the start of the game after an initialization
    //     // double secondsSinceEmptied = gameManager.gameTimeUnix - lastEmptiedTimeUnix;
    //     // double curCoinsCalc = prevCoins + Math.Floor(secondsSinceEmptied * coinsPerSecond);
    //     // curCoinsCalc = Math.Max(curCoinsCalc, 0.0);
    //     // if (curCoinsCalc >= coinsCapacity){
    //     //     curCoins = coinsCapacity;
    //     //     isFull = true;
    //     // }
    //     // else{
    //     //     curCoins = curCoinsCalc;
    //     //     isFull = false;
    //     // }
    //     calculateCurCoins();
    // }

    public double spoofLastEmptiedTime(double coins){
    //This will be only called by the game manager when we are in offline mode
    // Spoofs the CartLastEmptied time to yield a given coin value
        double secondsAgo = Math.Floor(coins / coinsPerSecond);
        double spoofedTime = gameManager.gameTimeUnix - secondsAgo;
        Debug.Log("SPOOFED LAST EMPTIED TIME: " + spoofedTime);
        return spoofedTime;
    }


    private void calculateCurCoins(){
        double secondsSinceEmptied = gameManager.gameTimeUnix - lastEmptiedTimeUnix;
        double secondsUntilNextFull = nextFullTimeUnix - gameManager.gameTimeUnix;
        //Debug.Log("SSE: " + secondsSinceEmptied);
        double curCoinsCalc = Math.Floor((secondsSinceEmptied/(secondsSinceEmptied + secondsUntilNextFull)) * coinsCapacity);
        if (gameManager.gameTimeUnix >= nextFullTimeUnix){
            curCoinsCalc = coinsCapacity;
        }
        curCoinsCalc = Math.Max(curCoinsCalc, 0.0);
        //Debug.Log("CCC: " + curCoinsCalc);
        if (curCoinsCalc >= coinsCapacity){
            curCoins = coinsCapacity;
            isFull = true;
        }
        else{
            curCoins = curCoinsCalc;
            isFull = false;
        }
        gameManager.mineCartCurCoins = curCoins;
    }

    private void updateAnimation(){
        if (isFull){
            curAnim = 9;
        }
        else {
            float percentFull = (float)(curCoins/coinsCapacity)*100.0f;
            float percentStep = 100.0f/9.0f; // 9 Other Animations
            //Debug.Log("PF: " + percentFull);
            //Debug.Log(percentFull/percentStep);
            curAnim = Mathf.FloorToInt(percentFull/percentStep);
        }
        //Debug.Log("ANIM: " + curAnim);
        minecartAnimator.SetInteger("Anim_Num", curAnim);
    }
}
