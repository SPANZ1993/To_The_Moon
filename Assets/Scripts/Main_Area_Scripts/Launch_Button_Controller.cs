using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch_Button_Controller : MonoBehaviour, ITappable
{

    private bool coverOpen = false;
    private bool buttonPressed = false;
    private bool buttonPressStarted = false;

    [SerializeField]
    private GameObject Launch_Cover;
    [SerializeField]
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;

    private List<Vector2> curDragLocs = new List<Vector2>();
    private List<Vector2> prevDragLocs;
    private double curDragStartTime;
    private double curDragEndTime;
    private double prevDragStartTime;
    private double prevDragEndTime;



    private Animator coverAnimator;
    private Animator buttonAnimator;
    

    private Game_Manager gameManager;
    private Game_Scaler game_scaler;



    [SerializeField]
    private float min_v_swipe_percent_lid = 10.0f;
    [SerializeField]
    private float max_h_swipe_percent_lid = 25.0f;
    [SerializeField]
    private double max_swipe_time_lid = 0.5;






    private bool launched = false;

    public delegate void InitiateLaunch();
    public static event InitiateLaunch InitiateLaunchInfo;






    // Start is called before the first frame update
    void Start()
    {
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
        coverAnimator = Launch_Cover.GetComponent<Animator>();
        buttonAnimator = GetComponent<Animator>();
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
    }


    public void onTapStart()
    {
        if (!launched){
            // Debug.Log("LBC START");
            double dragStartTime = touchDetection.dragStartTime;

            if (coverOpen && curDragLocs.Count > 1){
                // Debug.Log("Dragged On");
                // Animate the button press again if we drag off then back onto the sprite
                if (buttonPressStarted){
                    buttonPressed = true;
                }
            }
            else if (coverOpen){
                // Debug.Log("Pressed On");
                buttonPressStarted = true;
                buttonPressed = true;
            }
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
                buttonPressed = false;
            }
            StartCoroutine(_onTapEnd(false));
        }
        else
        {
            //Debug.Log("Finger Lifted");
            if (coverOpen && buttonPressStarted && buttonPressed){
                // Check if we pressed the button
                if (gameManager.remainingLaunches >= 1){
                    //Debug.Log("LAUNCH TIME BABY!");
                    launched = true;
                    InitiateLaunchInfo();
                }
                else{
                    buttonPressed = false;
                }
            }

            if (!launched){
                if (checkLidRemoveSwipe(prevDragLocs, prevDragStartTime, prevDragEndTime)){
                    coverOpen = true;
                }
                else if (checkLidReplaceSwipe(prevDragLocs, prevDragStartTime, prevDragEndTime)){
                    coverOpen = false;
                }
            
                curDragLocs = new List<Vector2>();
                prevDragLocs = new List<Vector2>();
                curDragStartTime = 0.0f;
                curDragEndTime = 0.0f;
                prevDragStartTime = 0.0f;
                prevDragEndTime = 0.0f;
                buttonPressStarted = false;
            }
        }
    }


    public void onTapEnd()
    {
        if (!launched){
            // Debug.Log("LBC END");
            // If we lifted our finger, else if we dragged off the object
            StartCoroutine(_onTapEnd(true));
        }
    }



    private bool checkLidRemoveSwipe(List<Vector2> swipeDragLocs, double swipeDragStartTime, double swipeDragEndTime){
        if (swipeDragLocs.Count >= 1 && swipeDragEndTime - swipeDragStartTime <= max_swipe_time_lid)
        {
            Vector2 dragStartLoc = swipeDragLocs[0];
            Vector2 dragEndLoc = swipeDragLocs[swipeDragLocs.Count - 1];

            float VDragPercent = (dragEndLoc.y - dragStartLoc.y) / game_scaler.ScreenHeightPx * 100.0f;
            float HDragPercent = (Mathf.Abs(dragStartLoc.x - dragEndLoc.x) / game_scaler.ScreenWidthPx) * 100.0f;

            if (VDragPercent >= min_v_swipe_percent_lid && HDragPercent <= max_h_swipe_percent_lid)
            {
            return true;
            }
        }
        // If We Don't Get a Good Swipe
        return false;
    }
    
    private bool checkLidReplaceSwipe(List<Vector2> swipeDragLocs, double swipeDragStartTime, double swipeDragEndTime){
        if (swipeDragLocs.Count >= 1 && swipeDragEndTime - swipeDragStartTime <= max_swipe_time_lid)
        {
            Vector2 dragStartLoc = swipeDragLocs[0];
            Vector2 dragEndLoc = swipeDragLocs[swipeDragLocs.Count - 1];

            float VDragPercent = (dragEndLoc.y - dragStartLoc.y) / game_scaler.ScreenHeightPx * 100.0f;
            float HDragPercent = (Mathf.Abs(dragStartLoc.x - dragEndLoc.x) / game_scaler.ScreenWidthPx) * 100.0f;

            if (VDragPercent <=  -1.0f * min_v_swipe_percent_lid && HDragPercent <= max_h_swipe_percent_lid)
            {
            return true;
            }
        }
        // If We Don't Get a Good Swipe
        return false;
    }

 



    // Update is called once per frame
    void Update()
    {
        if (!launched){
            prevDragLocs = curDragLocs;
            curDragLocs = touchDetection.dragLocs;
            prevDragStartTime = curDragStartTime;
            prevDragEndTime = curDragEndTime;
            curDragStartTime = touchDetection.dragStartTime;
            curDragEndTime = touchDetection.dragEndTime;

            // Update animations
            coverAnimator.SetBool("isOpen", coverOpen);
            buttonAnimator.SetBool("isPressed", buttonPressed);
        }
    }
}
