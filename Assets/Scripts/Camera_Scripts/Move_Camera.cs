using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Move_Camera : MonoBehaviour
{


    // If DebugMode, then print info about where the camera is / where the camera is moving to
    public bool DebugMode = false;

    public float panSpeed;

    //public bool currentlyMoving = false;
    public int movingTweenId = -1;

    private int cur_i;
    private int cur_dest_i;
    private Vector3 position;
    private Vector3[] locs = new Vector3[4];

    private int ROOM = 0;
    private int COMPUTER = 1;
    private int ROCKET = 2;
    private int MINES = 3;

    private bool _SwipedUp = false;
    private bool _SwipedDown = false;
    private bool _SwipedLeft = false;
    private bool _SwipedRight = false;


    private UI_Controller uiController;
    private Scene_Manager sceneManager;
    private bool screenFrozen = false; // Flag that says if the screen is allowed to move or not


    private Game_Scaler game_scaler;


    void OnEnable()
    {
        Touch_Detection.SwipedUpInfo += SwipedUpListener;
        Touch_Detection.SwipedDownInfo += SwipedDownListener;
        Touch_Detection.SwipedLeftInfo += SwipedLeftListener;
        Touch_Detection.SwipedRightInfo += SwipedRightListener;
    }

    void OnDisable()
    {
        Touch_Detection.SwipedUpInfo -= SwipedUpListener;
        Touch_Detection.SwipedDownInfo -= SwipedDownListener;
        Touch_Detection.SwipedLeftInfo -= SwipedLeftListener;
        Touch_Detection.SwipedRightInfo -= SwipedRightListener;
    }




    void SwipedUpListener(){
        _SwipedUp = true;
    }

    void SwipedDownListener(){
        _SwipedDown = true;
    }

    void SwipedLeftListener(){
        _SwipedLeft = true;
    }

    void SwipedRightListener(){
        _SwipedRight = true;
    }





    // Start is called before the first frame update
    void Start()
    {
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();

        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        sceneManager = GameObject.Find("Scene_Manager").GetComponent<Scene_Manager>();

        cur_i = 0;
        cur_dest_i = 0;
        this.transform.position = new Vector3(locs[cur_i].x, locs[cur_i].y, -10f); 
    }

    private bool firstUpdate = true;







    void Update()
    {
        screenFrozen = isFrozen();
        if (firstUpdate)
        {
            
            locs[ROOM] = GameObject.Find("Room_Waypoint").transform.position; // Room
            locs[COMPUTER] = GameObject.Find("Computer_Waypoint").transform.position; // Comp
            locs[ROCKET] = GameObject.Find("Rocket_Waypoint").transform.position; // Rocket
            locs[MINES] = GameObject.Find("Mine_Waypoint").transform.position; // Mines

            locs[ROOM] = new Vector3(locs[ROOM].x, locs[ROOM].y, -10f);
            locs[COMPUTER] = new Vector3(locs[COMPUTER].x, locs[COMPUTER].y, -10f);
            locs[ROCKET] = new Vector3(locs[ROCKET].x, locs[ROCKET].y, -10f);
            locs[MINES] = new Vector3(locs[MINES].x, locs[MINES].y, -10f);

            firstUpdate = false;
        }

        if (DebugMode){
            print("CUR CAM LOC: " + this.transform.position);
            print("CONTAINS? " + locs.Contains(this.transform.position));
        }
        if (locs.Contains(this.transform.position) && (cur_i == cur_dest_i))
        {
            if (DebugMode){
                print("DETECTING...");
                print("CUR I..." + cur_i);
            }
            // Detect input
            if(Input.GetKeyDown(KeyCode.UpArrow) || _SwipedUp) {
                if (DebugMode)
                    Debug.Log("UP");
                if (cur_i == COMPUTER){
                    cur_dest_i = ROOM;
                }
                _SwipedUp = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || _SwipedDown) {
                if (DebugMode)
                    Debug.Log("DOWN");
                if (cur_i == ROOM){
                    if (DebugMode)
                        Debug.Log("MOVING TO COMPUTER");
                    //cur_dest_i = COMPUTER; // We don't want to be able to go the computer. At least not yet.
                }
                _SwipedDown = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || _SwipedLeft) {
                if (DebugMode)
                    Debug.Log("LEFT");
                if (cur_i == ROOM){
                    cur_dest_i = MINES;
                }
                else if (cur_i == ROCKET){
                    cur_dest_i = ROOM;
                }
                _SwipedLeft = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || _SwipedRight) {
                if (DebugMode)
                    Debug.Log("RIGHT");
                if (cur_i == MINES){
                    cur_dest_i = ROOM;
                }
                else if (cur_i == ROOM){
                    cur_dest_i = ROCKET;
                }
                _SwipedRight = false;
            }
            if(screenFrozen){
                _SwipedUp = false;
                _SwipedDown = false;
                _SwipedLeft = false;
                _SwipedRight = false;
                cur_dest_i = cur_i;
            }
        }
        else
        {
            if (DebugMode){
                Debug.Log("CUR I " + cur_i);
                Debug.Log("CUR DEST" + cur_dest_i);
            }
            //moveTowards(this.transform.position, locs[cur_dest_i], panSpeed * Time.deltaTime);
            if (!LeanTween.isTweening(movingTweenId)){
                closeMenusOnSwipe();
                movingTweenId = LeanTween.move(this.gameObject, locs[cur_dest_i], panSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(closeMenusOnSwipe).id;
            }
            if (locs.Contains(this.transform.position))
                cur_i = cur_dest_i;
        }
    }

    void closeMenusOnSwipe(){
        if(Game_Manager.instance.gameObject.GetComponent<IEvent>() == null){
            UI_Controller.instance.closeMenus();
        }
        else{
            Debug.Log("SKIPPING CLOSE MENUS BC IN EVENT");
        }
    }

    void moveTowards(Vector3 location, Vector3 destination, float speed)
    {
        if (DebugMode)
            Debug.Log("MOVING TO: " + destination);
        if (!location.Equals(destination))
        {
            float[] possible_speedsx = new float[] {speed, Mathf.Abs(location.x - destination.x)};
            float[] possible_speedsy = new float[] {speed, Mathf.Abs(location.y - destination.y)};

            float y_speed_multiplier = game_scaler.DevScreenHeight / game_scaler.ScreenHeight;
            float x_speed_multiplier = game_scaler.DevScreenWidth / game_scaler.ScreenWidth;


            float speedx = possible_speedsx.Min();
            if (destination.x < location.x)
                speedx = speedx * -1f;
            speedx = speedx * x_speed_multiplier;

            float speedy = possible_speedsy.Min();
            if (destination.y < location.y)
                speedy = speedy * -1f;
            speedy = speedy * y_speed_multiplier;

            //this.transform.position.x += speedx;
            //this.transform.position.y += speedy;
            this.transform.position = new Vector3(this.transform.position.x + speedx, this.transform.position.y + speedy, -10f);
        }
    }


        // UI_Controller
    //public bool rocketBuildingMenuDisplayed = false;
    //public bool researchMenuDisplayed = false;
    
    // Scene_Manager
    // public bool startedRocketSceneTransition {get; private set;}
    // public bool startedMineSceneTransition {get; private set;}

    public void setToLocation(int loci){
        cur_i = loci;
        cur_dest_i = loci;
        gameObject.transform.position = locs[loci];
    }


    bool isFrozen(){
        //Debug.Log("UHHH: " + !(!uiController.rocketBuildingMenuDisplayed && !uiController.researchMenuDisplayed && !sceneManager.startedRocketSceneTransition && !sceneManager.startedMineSceneTransition));
        return uiController.rocketBuildingMenuDisplayed || uiController.researchMenuDisplayed || sceneManager.startedRocketSceneTransition || sceneManager.startedMineSceneTransition;
    }

}
