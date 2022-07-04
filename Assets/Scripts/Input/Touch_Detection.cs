using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;


public enum Swipe{
    NOSWIPE = 0,
    UPSWIPE = 1,
    DOWNSWIPE = 2,
    LEFTSWIPE = 3,
    RIGHTSWIPE = 4
} 

public class Touch_Detection : MonoBehaviour
{

    // If true, display touch location on screen
    [SerializeField]
    private bool DebugMode = true;
    [SerializeField]
    private bool RenderReticle = false;

    private Vector2 startPos;
    private Vector2 direction;
    private Vector2 curPos;
    private Vector3 curGamePos;

    public string m_Text;
    public string m_Text_Pos;
    public string message;

    private Game_Scaler game_scaler;

    private GameObject cam;
    private Transform cam_transform;

    [HideInInspector]
    public List<Vector2> dragLocs = new List<Vector2>();
    [HideInInspector]
    public List<TouchPhase> touchPhases = new List<TouchPhase>(); // Correspond to draglocs
    [HideInInspector]
    public int touchId {get; private set;} // Just increment every time we start a new touch
    [HideInInspector]
    public double dragStartTime;
    [HideInInspector]
    public double dragEndTime;

    private Swipe swipeval;


    // Values that define what a "Swipe" is
    private double MaxSwipeTime = 1;
    private float VSwipeMinPercent = 50;
    private float HSwipeMinPercent = 25;
    public bool currentlySwiping {get; private set;}


    // A GameObject With A Hitbox That Will Be Active During a Touch
    private GameObject reticle;
    private SpriteRenderer reticleRenderer;
    private CircleCollider2D reticleCollider;

    public static Touch_Detection instance;

    private bool enableReticleAfterCurTap = false;
    private Dictionary<Swipe, bool> enableSwipesAfterCurTap;
    [SerializeField]
    private bool reticleDisabled = false;
    private List<Vector3[]> reticleDisabledBoxes = new List<Vector3[]>();




    private Dictionary<Swipe, bool> enabledSwipes;

    

    // Swipe Delegates

    public delegate void SwipedUp();
    public static event SwipedUp SwipedUpInfo;

    public delegate void SwipedDown();
    public static event SwipedDown SwipedDownInfo;

    public delegate void SwipedLeft();
    public static event SwipedLeft SwipedLeftInfo;

    public delegate void SwipedRight();
    public static event SwipedRight SwipedRightInfo;





    [HideInInspector]
    public bool tapInProgress = false;

    public GameObject current_game_object;



    public delegate void TapStart(GameObject other);
    public static event TapStart TapStartInfo;

    public delegate void TapStay(GameObject other);
    public static event TapStay TapStayInfo;

    public delegate void TapEnd(GameObject other);
    public static event TapEnd TapEndInfo;






    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);


            enabledSwipes = new Dictionary<Swipe, bool>(){
                {Swipe.UPSWIPE, true},
                {Swipe.DOWNSWIPE, true},
                {Swipe.LEFTSWIPE, true},
                {Swipe.RIGHTSWIPE, true}
            };

            enableSwipesAfterCurTap = new Dictionary<Swipe, bool>(){
                {Swipe.UPSWIPE, false},
                {Swipe.DOWNSWIPE, false},
                {Swipe.LEFTSWIPE, false},
                {Swipe.RIGHTSWIPE, false}
            };
            touchId = 0;
        }
        else{
            Destroy(this.gameObject);
        }
    }


    void OnEnable()
    {
        Reticle.TapStartInfo += TapStartListener;
        Reticle.TapStayInfo += TapStayListener;
        Reticle.TapEndInfo += TapEndListener;
    }

    void OnDisable()
    {
        Reticle.TapStartInfo -= TapStartListener;
        Reticle.TapStayInfo -= TapStayListener;
        Reticle.TapEndInfo -= TapEndListener;
    }


    // Start is called before the first frame update
    void Start()
    {
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();

        reticle = GameObject.Find("Reticle");
        reticleRenderer = reticle.GetComponent<SpriteRenderer>();
        reticleCollider = reticle.GetComponent<CircleCollider2D>();




        cam = GameObject.Find("Main Camera");
        cam_transform = cam.GetComponent<Transform>();        
        currentlySwiping = false;
    }

    void OnLevelWasLoaded(){
        cam = GameObject.Find("Main Camera");
        cam_transform = cam.GetComponent<Transform>();
        enableReticle(immediately:true);
        enableSwipes(immediately:true);
    }

    public Touch touch {get; private set;}
    // Update is called once per frame
    void Update()
    {

        //Update the Text on the screen depending on current TouchPhase, and the current direction vector
        m_Text = "Touch : " + message + "in direction" + direction;
        m_Text_Pos = "Pos: " + curPos;

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            tapInProgress = true;

            touch = Input.GetTouch(0);


            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {    
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    currentlySwiping = false;
                    // Record initial touch position.
                    startPos = touch.position;
                    dragLocs.Add(touch.position);
                    touchPhases.Add(TouchPhase.Began);
                    touchId = (touchId + 1) % 10;
                    dragStartTime = Time.timeAsDouble;
                    message = "Begun ";
                    if (RenderReticle){
                        reticleRenderer.enabled = true;
                    }
                    if (!reticleDisabled && !reticleInsideDisabledBox(touch.position)){
                        reticleCollider.enabled = true;
                    }
                    else{
                        reticleCollider.enabled = false;
                    }
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    currentlySwiping = DetectSwipe(dragLocs, dragStartTime, Time.timeAsDouble) != Swipe.NOSWIPE;
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    dragLocs.Add(touch.position);
                    touchPhases.Add(TouchPhase.Moved);
                    message = "Moving ";
                    if (!reticleDisabled && !reticleInsideDisabledBox(touch.position)){
                        reticleCollider.enabled = true;
                    }
                    else{
                        reticleCollider.enabled = false;
                    }
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    dragEndTime = Time.timeAsDouble;
                    message = "Ending ";
                    reticleRenderer.enabled = false;
                    reticleCollider.enabled = false;

                    // Check to see if we've got any swipes
                    swipeval = DetectSwipe(dragLocs, dragStartTime, dragEndTime);
                    if (swipeval != Swipe.NOSWIPE){
                        if (DebugMode)
                            Debug.Log("SWIPE FROM TD!!!");
                        if (swipeval == Swipe.UPSWIPE && enabledSwipes[Swipe.UPSWIPE] == true &&  SwipedUpInfo != null){
                            //Debug.Log("UTD");
                            SwipedUpInfo();
                        }
                        else if (swipeval == Swipe.DOWNSWIPE && enabledSwipes[Swipe.DOWNSWIPE] == true && SwipedDownInfo != null){
                            //Debug.Log("DTD");
                            SwipedDownInfo();
                        }
                        else if (swipeval == Swipe.LEFTSWIPE && enabledSwipes[Swipe.LEFTSWIPE] == true && SwipedLeftInfo != null){
                            //Debug.Log("LTD");
                            SwipedLeftInfo();
                        }
                        else if (swipeval == Swipe.RIGHTSWIPE && enabledSwipes[Swipe.RIGHTSWIPE] == true && SwipedRightInfo != null){
                            //Debug.Log("RTD");
                            SwipedRightInfo();
                        }
                    }




                    // If we got a swipe, reset all the swipe vars
                    // dragLocs = new List<Vector2>();
                    // dragStartTime = 0;
                    // dragEndTime = 0;
                    // swipeval = 0;

                    if (enableReticleAfterCurTap){
                        reticleDisabled = false;
                        enableReticleAfterCurTap = true;
                    }
                    foreach(Swipe pos_swipe in new Swipe[] {Swipe.UPSWIPE, Swipe.DOWNSWIPE, Swipe.LEFTSWIPE, Swipe.RIGHTSWIPE}){
                        if(enableSwipesAfterCurTap[pos_swipe]){
                            //Debug.Log("REENEABLING FROM ENABLE AFTER CUR TAP");
                            enabledSwipes[pos_swipe] = true;
                            enableSwipesAfterCurTap[pos_swipe] = false;
                        }
                    }

                    break;
            }
            curPos = touch.position;
            curGamePos = TouchPos2GamePos(curPos); 
            reticle.transform.position = curGamePos;
        }
        // No Input
        else
        {
            tapInProgress = false;
            currentlySwiping = false;
            dragLocs = new List<Vector2>();
            dragStartTime = 0;
            dragEndTime = 0;
            swipeval = 0;  
        }
    }


    public Vector3 TouchPos2GamePos(Vector2 touchPos){
        float x = ((touchPos.x / game_scaler.ScreenWidthPx) * game_scaler.ScreenWidth) + (cam_transform.position.x - (game_scaler.ScreenWidth / 2f));
        float y = ((touchPos.y / game_scaler.ScreenHeightPx) * game_scaler.ScreenHeight) + (cam_transform.position.y - (game_scaler.ScreenHeight / 2f));

        return new Vector3(x, y, 0f);
    }



    public Vector2 TouchPos2ScreenPos(Vector2 touchPos){
        // Returns in terms of a cartesian plane with origin at center of screen and screen edges at -1.0, 1.0
        float x = (2.0f * touchPos.x / game_scaler.ScreenWidthPx) - 1.0f;
        float y = (2.0f * touchPos.y / game_scaler.ScreenHeightPx) - 1.0f;
        return new Vector2(x, y);
    }

    


    Swipe DetectSwipe(List<Vector2> dragLocs, double dragStartTime, double dragEndTime){
        if (dragEndTime - dragStartTime <= MaxSwipeTime){
            Vector2 dragStartLoc = dragLocs[0];
            Vector2 dragEndLoc = dragLocs[dragLocs.Count - 1];

            float VDragPercent = (Mathf.Abs(dragStartLoc.y - dragEndLoc.y) / game_scaler.ScreenHeightPx) * 100.0f;
            float HDragPercent = (Mathf.Abs(dragStartLoc.x - dragEndLoc.x) / game_scaler.ScreenWidthPx) * 100.0f;

            // HORIZONTAL SWIPE TAKES PRIORITY
            
            if (HDragPercent >= HSwipeMinPercent){
                //Debug.Log("HDRAG PERCENT: " + HDragPercent + " --- " + HSwipeMinPercent + " TIME: " + (dragEndTime - dragStartTime) +  "---- HSWIPE!!!");
                if (dragStartLoc.x > dragEndLoc.x)
                {
                    return Swipe.RIGHTSWIPE;
                }
                else 
                {
                    return Swipe.LEFTSWIPE;
                }
            }
            else if (VDragPercent >= VSwipeMinPercent){
                if (dragStartLoc.y > dragEndLoc.y)
                {
                    return Swipe.UPSWIPE;
                }
                else 
                {
                    return Swipe.DOWNSWIPE;
                }
            }
            else
            {
                //Debug.Log("HDRAG PERCENT: " + HDragPercent + " --- " + HSwipeMinPercent + " TIME: " + (dragEndTime - dragStartTime) +  "---- NOSWIPE");
                return Swipe.NOSWIPE; // NO SWIPE... DIDN'T SWIPE FAR ENOUGH
            }

        }
        else
        {
            // NO SWIPE... Took Too Long
            return Swipe.NOSWIPE;
        }
    }

    
    public void disableReticle(bool disableswipes=true){
        //reticleCollider.enabled = false;
        reticleDisabled = true;
        enableReticleAfterCurTap = false;
        if(disableswipes){
            disableSwipes();
        }
    }

    public void enableReticle(bool immediately=false, bool enableswipes=true){
        //reticleCollider.enabled = true;
        //reticleDisabled = false;
        //Debug.Log("ENABLING RETICLE TD");
        if(!immediately){
            enableReticleAfterCurTap = true;
        }
        else{
            reticleDisabled = false;
            enableReticleAfterCurTap = false;
        }
        if(enableswipes){
            enableSwipes(immediately:immediately);
        }
    }

    public void enableReticleInstant(){
        // A little dangerous but we gotta do it sometimes
        reticleDisabled = false;
        enableReticleAfterCurTap = true;
    }

    public void enableSwipes(bool immediately = false){
        enableSwipes(new [] {Swipe.UPSWIPE, Swipe.LEFTSWIPE, Swipe.RIGHTSWIPE, Swipe.DOWNSWIPE}, immediately);
    }

    public void enableSwipes(List<Swipe> swipes, bool immediately = false){
        enableSwipes(swipes.ToArray(), immediately);
    }

    public void enableSwipes(Swipe[] swipes, bool immediately = false){
        foreach(Swipe swipe in swipes){
            enableSwipes(swipe, immediately);
        }
    }

    public void enableSwipes(Swipe swipe, bool immediately = false){
        if(enableSwipesAfterCurTap != null){
            //Debug.Log("ENABLING " + swipe);
            if(!immediately){
                enableSwipesAfterCurTap[swipe] = true;
            }
            else{
                enabledSwipes[swipe] = true;
                enableSwipesAfterCurTap[swipe] = false;
            }
        }
        else{
            StartCoroutine(_enableSwipes(swipe, immediately));
        }
    }

    // Just in case this gets called before it's initialized
    IEnumerator _enableSwipes(Swipe swipe, bool immediately){
        while(enableSwipesAfterCurTap == null){
            yield return new WaitForSeconds(0);
        }
        //Debug.Log("ENABLING " + swipe);
        if(!immediately){
            enableSwipesAfterCurTap[swipe] = true;
        }
        else{
            enabledSwipes[swipe] = true;
            enableSwipesAfterCurTap[swipe] = false;
        }
    }



    public void disableSwipes(){
        disableSwipes(new Swipe[] {Swipe.UPSWIPE, Swipe.LEFTSWIPE, Swipe.RIGHTSWIPE, Swipe.DOWNSWIPE});
    }

    public void disableSwipes(List<Swipe> swipes){
        disableSwipes(swipes.ToArray());
    }

    public void disableSwipes(Swipe[] swipes){
        foreach(Swipe swipe in swipes){
            disableSwipes(swipe);
        }
    }

    public void disableSwipes(Swipe swipe){
        if(enabledSwipes != null){
            //Debug.Log("DISABLING " + swipe);
            enabledSwipes[swipe] = false;
            enableSwipesAfterCurTap[swipe] = false;
        }
        StartCoroutine(_disableSwipes(swipe));
    }

    // Just in case this gets called before it's initialized
    IEnumerator _disableSwipes(Swipe swipe){
        while(enabledSwipes == null){
            yield return new WaitForSeconds(0);
        }
        //Debug.Log("DISABLING " + swipe);
        //Debug.Log(swipe + " DISABLED");
        enabledSwipes[swipe] = false;
        enableSwipesAfterCurTap[swipe] = false;
    }






    public void disableReticleInBoundingBox(Vector3[] boundingBox){
        if (boundingBox.Length != 4){
            throw new ArgumentException("Bounding Box Must Contain 4 Vectors");
        }
        Vector3[] screenBoundingBox = new Vector3[4];
        for(int i = 0; i < 4; i++){
            Vector3 bound = boundingBox[i];
            //Debug.Log("SCREEN POINT: " + game_scaler.WorldToScreenPoint(bound));
            screenBoundingBox[i] = game_scaler.WorldToScreenPoint(bound);
        }
        reticleDisabledBoxes.Add(screenBoundingBox);
        //Debug.Log("ADDED BOUNDING BOX");
        //Debug.Log("RETICLE DISABLED BOXES LEN: " + reticleDisabledBoxes.Count);
        foreach(Vector3[]bb in reticleDisabledBoxes){
            //foreach(Vector3 b in bb){
                //Debug.Log("BB: " + b);
            //}
        }
    }

    public void enableReticleInBoundingBox(Vector3[] boundingBox){
        if (boundingBox.Length != 4){
            throw new ArgumentException("Bounding Box Must Contain 4 Vectors");
        }
        //Debug.Log("REMOVED BOUNDING BOX");
        Vector3[] screenBoundingBox = new Vector3[4];
        List<int> iToRemove = new List<int>();
        for(int i = 0; i < reticleDisabledBoxes.Count; i++){
            if(boundingBoxEquals(boundingBox, reticleDisabledBoxes[i])){
                iToRemove.Add(i);
            }
        }
        iToRemove.Reverse();
        foreach(int i in iToRemove){
            reticleDisabledBoxes.RemoveAt(i);
        }
    }


    private bool boundingBoxEquals(Vector3[] bbA, Vector3[] bbB){
        int matching_corners = 0;
        foreach(Vector3 boundA in bbA){
            foreach(Vector3 boundB in bbA){
                if (boundA.Equals(boundB)){
                    matching_corners++;
                }
            }
        }
        if (matching_corners > 4 && matching_corners != 16){ // If we got 16 then both boxes are probably just localScale = (0,0,0), which is fine
            throw new InvalidOperationException("GOT MORE THAN 4 MATCHING CORNERS WHEN CHECKING FOR BOUNDING BOX EQUALITY...?");
        }
        return matching_corners == 4;
    }


    public bool reticleInsideDisabledBox(Vector2 touchPos){
        // Have to give this the touch pos bc reticle might not be where the touch is at the start of the frame
        if(reticleDisabledBoxes.Count == 0){
            return false;
        }
        bool reticleInside = false;
        Vector3 reticleScreenLoc = game_scaler.WorldToScreenPoint(TouchPos2GamePos(touchPos));
        foreach(Vector3[] bb in reticleDisabledBoxes)
        {
            float? minX = null;
            float? minY = null;
            float? maxX = null;
            float? maxY = null;
            foreach(Vector3 b in bb){
                if (minX == null || b.x < minX){
                    minX = b.x;
                }

                if (maxX == null || b.x > maxX){
                    maxX = b.x;
                }

                if (minY == null || b.y < minY){
                    minY = b.y;
                }

                if (maxY == null || b.y > maxY){
                    maxY = b.y;
                }
            }
            //Debug.Log("CHECKING IF RETICLE IN BOUNDING BOX");
            reticleInside = (reticleScreenLoc.x >= minX) && (reticleScreenLoc.x < maxX) && 
            (reticleScreenLoc.y >= minY) && (reticleScreenLoc.y < maxY);
                
            if (reticleInside)
            {
                //Debug.Log("RETICLE IS!");
                return reticleInside;
            }
        }
        //Debug.Log("RETICLE IS NOT");
        return reticleInside;
    }




    void TapStartListener(GameObject other){
        if (TapStartInfo != null && !reticleDisabled)
            TapStartInfo(other);
    }
    
    void TapStayListener(GameObject other){
        if (TapStayInfo != null && !reticleDisabled)
            TapStayInfo(other);
    }

    void TapEndListener(GameObject other){
        if (TapEndInfo != null && !reticleDisabled)
            TapEndInfo(other);
    }

}
