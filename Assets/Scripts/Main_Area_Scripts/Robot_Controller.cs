using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Robot_Controller : MonoBehaviour, ITappable
{

    public GameObject RobotClothes;
    public GameObject Waypoint_Bot;
    public GameObject Waypoint_Top;

    private GameObject Robot_Rings;
    private Vector3 Robot_Rings_Offset;
    
    private GameObject Robot_Screen;
    private Vector3 Robot_Screen_Offset;

    private GameObject Robot_Clothes;
    private Vector3 Robot_Clothes_Offset;


    private Vector3 Robot_Clothes_Waypoint_Top;
    private Vector3 Robot_Clothes_Waypoint_Bot;

    private Vector3 Robot_Screen_Waypoint_Top;
    private Vector3 Robot_Screen_Waypoint_Bot;

    private Vector3 Robot_Rings_Waypoint_Top;
    private Vector3 Robot_Rings_Waypoint_Bot;


    private Animator RobotAnimator;

    private int tweenID = -1;
    private LeanTween LeanTween;


    [SerializeField]
    private float secondsBetweenWaypoints = 0.5f;

    [SerializeField]
    public bool wave = false;

    public bool topIsRecentWaypoint = false;
    private bool tweenStartsThisFrame = false;



    // Tap Stuff
    [SerializeField]
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;
    private bool tapInitiated = false; // The current tap started on the roboot
    private bool tapped = false; // The screen is held down currently on the robot
    private List<Vector2> curDragLocs = new List<Vector2>();

    private Game_Manager gameManager;
    





    public delegate void _RobotTapped(); // Internal Use... Only Alerts Robot Manager that the robot was tapped
    public static event _RobotTapped _RobotTappedInfo;


    bool startDone = false;




    private Game_Scaler game_scaler;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0);
        //Your Function You Want to Call
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        RobotAnimator = GetComponent<Animator>();

        Robot_Screen = GameObject.Find("Robot_Screen");
        Robot_Clothes = GameObject.Find("Robot_Clothes");
        Waypoint_Bot = GameObject.Find("Robot_Waypoint_Bottom");
        Waypoint_Top = GameObject.Find("Robot_Waypoint_Top");
        Robot_Rings = GameObject.Find("Robot_Rings");


        Robot_Rings_Offset = transform.position - Robot_Rings.transform.position;
        Robot_Screen_Offset = transform.position - Robot_Screen.transform.position;
        Robot_Clothes_Offset = transform.position - Robot_Clothes.transform.position;

        //
        Robot_Clothes_Waypoint_Top = Waypoint_Top.transform.position - Robot_Clothes_Offset;
        Robot_Clothes_Waypoint_Bot = Waypoint_Bot.transform.position - Robot_Clothes_Offset;

        Robot_Screen_Waypoint_Top = Waypoint_Top.transform.position - Robot_Screen_Offset;
        Robot_Screen_Waypoint_Bot = Waypoint_Bot.transform.position - Robot_Screen_Offset;

        Robot_Rings_Waypoint_Top = Waypoint_Top.transform.position - Robot_Rings_Offset;
        Robot_Rings_Waypoint_Bot = Waypoint_Bot.transform.position - Robot_Rings_Offset;
        //
        transform.position = Waypoint_Bot.transform.position;

        LeanTween = GetComponent<LeanTween>();
        
        startDone = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (startDone){
            curDragLocs = touchDetection.dragLocs;
            updateAnimation();
            if (topIsRecentWaypoint){
                moveTowardsWaypoint(gameObject, transform.position, Waypoint_Bot.transform.position);
                moveTowardsWaypoint(Robot_Clothes, Robot_Clothes.transform.position, Robot_Clothes_Waypoint_Bot);
                moveTowardsWaypoint(Robot_Screen, Robot_Screen.transform.position, Robot_Screen_Waypoint_Bot);
                moveTowardsWaypoint(Robot_Rings, Robot_Rings.transform.position, Robot_Rings_Waypoint_Bot);
            }
            else{
                moveTowardsWaypoint(gameObject, transform.position, Waypoint_Top.transform.position);
                moveTowardsWaypoint(Robot_Clothes, Robot_Clothes.transform.position, Robot_Clothes_Waypoint_Top);
                moveTowardsWaypoint(Robot_Screen, Robot_Screen.transform.position, Robot_Screen_Waypoint_Top);
                moveTowardsWaypoint(Robot_Rings, Robot_Rings.transform.position, Robot_Rings_Waypoint_Top);
            }
        }
    }



    private void moveTowardsWaypoint(GameObject obj, Vector3 Location, Vector3 destination){
        bool notTweening = false;

        if (obj == gameObject){
            if (!LeanTween.isTweening(tweenID)){
                notTweening = true;
                tweenID = LeanTween.move(obj, destination, secondsBetweenWaypoints).setEase(LeanTweenType.easeInOutSine).id;
                tweenStartsThisFrame = true;
            }
            else{
                tweenStartsThisFrame = false;
            }

            if (notTweening && (transform.position == Waypoint_Bot.transform.position || transform.position == Waypoint_Top.transform.position)){
                topIsRecentWaypoint = !topIsRecentWaypoint;
            }
        }
        else if (tweenStartsThisFrame){
            LeanTween.move(obj, destination, secondsBetweenWaypoints).setEase(LeanTweenType.easeInOutSine);
        }
        
    }


    private void moveChildren(){
        Robot_Rings.transform.position = transform.position - Robot_Rings_Offset;
        Robot_Screen.transform.position = transform.position - Robot_Screen_Offset;
        Robot_Clothes.transform.position = transform.position - Robot_Clothes_Offset;
    }

    private void updateAnimation(){
        if (!LeanTween.isTweening(tweenID) && transform.position == Waypoint_Bot.transform.position){
            RobotAnimator.SetBool("Wave", wave);
        }
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
                //Debug.Log("TAPPED THE ROBOT");
                _RobotTappedInfo();
            }
            tapped = false;
            tapInitiated = false;

        }
    }


    public void onTapEnd(bool wasFirst)
    {
        // Debug.Log("LBC END");
        // If we lifted our finger, else if we dragged off the object
        if(wasFirst){
            StartCoroutine(_onTapEnd(true));
        }
    }





}
