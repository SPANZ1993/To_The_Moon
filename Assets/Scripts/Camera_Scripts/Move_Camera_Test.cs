using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


public class Move_Camera_Test : MonoBehaviour
{
    
    // If DebugMode, then print info about where the camera is / where the camera is moving to
    public bool DebugMode = false;

    public float panSpeed;

    private int cur_i;
    private int cur_dest_i;
    private Vector3 position;
    private Vector3[] locs = new Vector3[4];

    private int ROOM = 0;
    private int COMPUTER = 1;
    private int ROCKET = 2;
    private int MINES = 3;


    private Game_Scaler game_scaler;

    // Start is called before the first frame update
    void Start()
    {
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();

        locs[ROOM] = GameObject.Find("Room_Waypoint").transform.position; // Room
        locs[COMPUTER] = GameObject.Find("Computer_Waypoint").transform.position; // Comp
        locs[ROCKET] = GameObject.Find("Rocket_Waypoint").transform.position; // Rocket
        locs[MINES] = GameObject.Find("Mine_Waypoint").transform.position; // Mines

        locs[ROOM] = new Vector3(locs[ROOM].x, locs[ROOM].y, -10f);
        locs[COMPUTER] = new Vector3(locs[COMPUTER].x, locs[COMPUTER].y, -10f);
        locs[ROCKET] = new Vector3(locs[ROCKET].x, locs[ROCKET].y, -10f);
        locs[MINES] = new Vector3(locs[MINES].x, locs[MINES].y, -10f);

        cur_i = 0;
        cur_dest_i = 0;
        this.transform.position = locs[cur_i];   
    }

    void Update()
    {
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
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                if (DebugMode)
                    Debug.Log("UP");
                if (cur_i == COMPUTER){
                    cur_dest_i = ROOM;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (DebugMode)
                    Debug.Log("DOWN");
                if (cur_i == ROOM){
                    if (DebugMode)
                        Debug.Log("MOVING TO COMPUTER");
                    cur_dest_i = COMPUTER;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (DebugMode)
                    Debug.Log("LEFT");
                if (cur_i == ROOM){
                    cur_dest_i = MINES;
                }
                else if (cur_i == ROCKET){
                    cur_dest_i = ROOM;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (DebugMode)
                    Debug.Log("RIGHT");
                if (cur_i == MINES){
                    cur_dest_i = ROOM;
                }
                else if (cur_i == ROOM){
                    cur_dest_i = ROCKET;
                }
            }
        }
        else
        {
            if (DebugMode){
                Debug.Log("CUR I " + cur_i);
                Debug.Log("CUR DEST" + cur_dest_i);
            }
            moveTowards(this.transform.position, locs[cur_dest_i], panSpeed * Time.deltaTime);
            if (locs.Contains(this.transform.position))
                cur_i = cur_dest_i;
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

}
