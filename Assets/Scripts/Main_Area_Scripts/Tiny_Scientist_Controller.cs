using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Tiny_Scientist_Controller : MonoBehaviour
{

    [SerializeField]
    private bool DebugMode = false;

    [HideInInspector]
    public float walkSpeed {get; set;}
    [HideInInspector]
    public float runSpeed {get; set;}
    [HideInInspector]
    public float minWaitSecs {get; set;}
    [HideInInspector]
    public float maxWaitSecs {get; set;}

    [HideInInspector]
    public bool isReadyForRandomWaypoint {get; set;}

    public bool isAtFinalWaypoint; // After the launch, have we made it back in the building?

    public GameObject Target_Waypoint {get; set;}


    private Vector2 prevLoc = new Vector2();
    

    // Used to determine which waypoint to head to next on the way to Target_Waypoint
    public GameObject Cur_Target_Waypoint {get; set;}
    private int target_waypoint_i;
    public List<GameObject> path_order;




    public bool launched = false;

    public float hvel;
    public float vvel;


    private Animator scientist_animator;

    private Game_Scaler game_scaler;


    private float cur_wait_time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        game_scaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        
        
        scientist_animator = GetComponent<Animator>();
        scientist_animator.SetFloat("Walk_Vel", 0.0f);
        scientist_animator.SetBool("Standing", true);
        scientist_animator.SetBool("Run", false);


        isReadyForRandomWaypoint = true;
        prevLoc.x = transform.position.x;
        prevLoc.y = transform.position.y;

        isAtFinalWaypoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        upDateVelVals();
        upDateWaypoints();
        //Debug.Log(Target_Waypoint.name);
        if (transform.position != Cur_Target_Waypoint.transform.position && transform.position != Target_Waypoint.transform.position){
            isReadyForRandomWaypoint = false;
            if (!launched){
                if (cur_wait_time <= 0.0f){
                    moveTowards(transform.position, Cur_Target_Waypoint.transform.position, walkSpeed);
                }
                else{
                    cur_wait_time -= Time.deltaTime;
                }
            }
            else{
                moveTowards(transform.position, Cur_Target_Waypoint.transform.position, runSpeed);
            }
        }
        else if (transform.position == Cur_Target_Waypoint.transform.position && transform.position != Target_Waypoint.transform.position){
            path_order.RemoveAt(0);
            Cur_Target_Waypoint = path_order[0];
            if (!launched){
                moveTowards(transform.position, Cur_Target_Waypoint.transform.position, walkSpeed);
            }
            else{
                moveTowards(transform.position, Cur_Target_Waypoint.transform.position, runSpeed);
            }
        }
        else{
            if (!launched){
                isReadyForRandomWaypoint = true;
                cur_wait_time = Random.Range(minWaitSecs, maxWaitSecs);
            }
            else{
                isAtFinalWaypoint = true;
            }
        }

        scientist_animator.SetFloat("Walk_Vel", hvel);
        scientist_animator.SetBool("Standing", hvel==0.0f);
        scientist_animator.SetBool("Run", launched);
    }




    void moveTowards(Vector3 Location, Vector3 destination, float speed){
        float step = speed * Time.deltaTime;
        step = game_scaler.Scale_Value_To_Screen_Width(step);

        // move sprite towards the target location
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
    }


    void upDateWaypoints(){
        // This is a little fucky, but we want to turn around and run down the stairs immediately on launch press
        
        if (launched && vvel > 0.0f){
            path_order.RemoveAt(0);
        }

        Cur_Target_Waypoint = path_order[0];
        Target_Waypoint = path_order[path_order.Count - 1];
    }



    void upDateVelVals(){
        hvel = transform.position.x - prevLoc.x;
        vvel = transform.position.y - prevLoc.y;
        //Debug.Log("VVEL: " + vvel);
        //Debug.Log("HVEL: " + hvel);
        prevLoc.x = transform.position.x;
        prevLoc.y = transform.position.y;
    }
    
}
