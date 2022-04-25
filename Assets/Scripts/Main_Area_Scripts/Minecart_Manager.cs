using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

using System;

public class Minecart_Manager : MonoBehaviour
{

    [HideInInspector]
    public double lastEmptiedTimeUnix;
    public double curCoins;
    public double coinsCapacity;
    public double coinsPerSecond;
    public double nextFullTimeUnix {get; set;}
    public double secondsLeft {get; private set;} // Don't set this from the outside
    public bool isFull {get; private set;}


    public bool startComplete = false;

    private int curAnim = 0;

    private Animator minecartAnimator;

    private Game_Manager gameManager;


    public static Minecart_Manager instance;



    public delegate void MinecartTapped(double coins); // Only minecart manager will be subscribed to this
    public static event MinecartTapped MinecartTappedInfo;


    void OnEnable(){
        Minecart_Controller._MinecartTappedInfo += onMinecartTapped;
    }

    void OnDisable(){
        Minecart_Controller._MinecartTappedInfo -= onMinecartTapped;
    }






    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }


    void OnLevelWasLoaded(){
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            minecartAnimator = GameObject.Find("Minecart").GetComponent<Animator>();
            Debug.Log("MCA: " + GameObject.Find("Minecart") + " -- " + minecartAnimator);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        minecartAnimator = GameObject.Find("Minecart").GetComponent<Animator>();

        //lastEmptiedTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(); // REMOVE
        startComplete = true;
    }


    // Update is called once per frame
    void Update()
    {
        // If it hasn't yet been initialized by the game manager, then just chill
        Debug.Log("NEXT FULL TIME: " + nextFullTimeUnix + " --- MCA: " + minecartAnimator);
        if (SceneManager.GetActiveScene().name == "Main_Area" && nextFullTimeUnix != 0 && minecartAnimator != null){
            Debug.Log("IN HERE YINZO");
            calculateCurCoins();
            updateAnimation();
            secondsLeft = calculateSecondsLeft();
            //Debug.Log("CUR COINS: " + curCoins);
        }
        else if (minecartAnimator != null){
            minecartAnimator = null;
        }
        //Debug.Log("IS FULL??: " + isFull + " SECONDS LEFT: " + secondsLeft);
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
        Debug.Log("UPDATING MINECART ANIMATION");
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


    void onMinecartTapped(){
        if (curCoins != 0){
            double curCoinsTmp = curCoins;
            curCoins = 0.0;
            lastEmptiedTimeUnix = gameManager.gameTimeUnix;
            calculateNextFullTime();
            if(MinecartTappedInfo != null){
                MinecartTappedInfo(curCoinsTmp);
            }
        }
    }
}
