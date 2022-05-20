using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI; // Remove

public class Mine_Shaft_Controller : MonoBehaviour, ITappable
{

    // Tap Stuff
    [SerializeField]
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;
    private bool tapInitiated = false; // The current tap started on the minecart
    private bool tapped = false; // The screen is held down currently on the minecart
    private List<Vector2> curDragLocs = new List<Vector2>();


    private Game_Manager gameManager;

    [HideInInspector]
    public double mineGameLastPlayedUnix;
    public double mineGameRefreshTime;
    public double nextRefreshTimeUnix {get; set;}
    public bool mineGameIsReady {get; private set;}

    public bool initialized = false;
    public bool refreshCalculated = false;

        
    public delegate void MineShaftTapped();
    public static event MineShaftTapped MineShaftTappedInfo;

    public delegate void MineGameReady();
    public static event MineGameReady MineGameReadyInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
        mineGameIsReady = false;
    }

    private GameObject mineIndicator; // Remove
    // Update is called once per frame
    void Update()
    {

        if (initialized && refreshCalculated){
            checkMineGameIsReady();
        }

        try{
            if (mineIndicator is null){
                mineIndicator = GameObject.Find("Mine_Indicator");
            }

            if(mineGameIsReady){
                mineIndicator.GetComponent<Image>().color = new Color(0, 255, 0, 255);
            }
            else{
                mineIndicator.GetComponent<Image>().color = new Color(255, 0, 0, 255);
            }
        }
        catch(System.Exception e){

        }
    }



    public double spoofLastMineGamePlayedTime(bool setToReady = true){
    // This will be only called by the game manager when we are in offline mode
    // Spoofs the LastMineGamePlayedTime to Now
        double spoofedTime;
        if (setToReady){
            spoofedTime = gameManager.gameTimeUnix - (31536000*10); // Say we last played ten years ago (We want to be able to play on the start of the game)
        }
        else{
            spoofedTime = gameManager.gameTimeUnix;
        }
        // Debug.Log("SPOOFED LAST MINE GAME TIME: " + spoofedTime);
        return spoofedTime;
    }




    private void checkMineGameIsReady(){
        //Debug.Log("NEXT REFRESH TIME: " + nextRefreshTimeUnix);
        //Debug.Log("CUR REFRESH TIME: " + gameManager.gameTimeUnix);
        if (gameManager.gameTimeUnix > nextRefreshTimeUnix && !mineGameIsReady){
            // Debug.Log("NEXT REFRESH TIME: " + nextRefreshTimeUnix);
            // Debug.Log("CUR REFRESH TIME: " + gameManager.gameTimeUnix);
            mineGameIsReady = true;
            if(MineGameReadyInfo != null){
                MineGameReadyInfo();
            }
        }
    }


    // Sometimes at the start of a scene this will get called before we have the gameManager reference so just wait until we have it from Start()
    IEnumerator _calculateNextGameTime(){ 
        //Debug.Log("CALCULATING NEXT TIME!!!!");
        yield return new WaitForSeconds(0);
        if(gameManager != null){
            //Debug.Log("CALCULATING NEXT TIME!!!!  --- DID IT");
            nextRefreshTimeUnix = mineGameLastPlayedUnix + gameManager.mineGameRefreshTime;
            //Debug.Log("SETTING NEXT REFRESH TIME: " + nextRefreshTimeUnix + "\n CURTIME: " + gameManager.gameTimeUnix);
            refreshCalculated = true;
        }
        else
        {
            gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>(); // New
            StartCoroutine(_calculateNextGameTime());
        }
    }

    
    public void calculateNextGameTime(){
        StartCoroutine(_calculateNextGameTime());
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
                //Debug.Log("Tapped The Mineshaft");
                if (MineShaftTappedInfo != null){
                    //Debug.Log("ALERTING TAPPED THE MINESHAFT");
                    MineShaftTappedInfo();
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




}
