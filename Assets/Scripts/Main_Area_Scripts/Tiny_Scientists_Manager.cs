using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny_Scientists_Manager : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float minWaitSecs;
    [SerializeField]
    private float maxWaitSecs;


    private bool launchInitiated = false;
    private List<GameObject> scientists =  new List<GameObject>();
    private List<Tiny_Scientist_Controller> scientists_controllers = new List<Tiny_Scientist_Controller>();
    private Dictionary<string, GameObject> waypoints = new Dictionary<string, GameObject>();


    private System.Random random = new System.Random();

    private bool launched = false;
    public bool launchComplete = false;


    private List<string> waypoints_main_stairs_order = new List<string>{"TS_0R",
                                                                        "TS_0RBS",
                                                                        "TS_0LTS",
                                                                        "TS_1L",
                                                                        "TS_1LBS",
                                                                        "TS_1RTS",
                                                                        "TS_2R",
                                                                        "TS_2RBS",
                                                                        "TS_2LTS",
                                                                        "TS_3L",
                                                                        "TS_3LBS",
                                                                        "TS_3RTS",
                                                                        "TS_4R",
                                                                        "TS_4RBS",
                                                                        "TS_4LTS",
                                                                        "TS_5L"};

    private List<string> waypoints_random_choices = new List<string>{"TS_0R",
                                                                    "TS_0L",
                                                                    "TS_1R",
                                                                    "TS_1L",
                                                                    "TS_2L",
                                                                    "TS_2R",
                                                                    "TS_3L",
                                                                    "TS_3R",
                                                                    "TS_4L",
                                                                    "TS_4R",
                                                                    "TS_5L",
                                                                    "TS_5R"};


    // If we are heading towards one of these during a launch OR we are moving towards a waypoint that is higher than us, then don't include that waypoint in the new path
    private Dictionary<string, string> side_waypoint_to_main_stair = new Dictionary<string, string>()
        {
            ["TS_5R"] = "TS_5L",
            ["TS_4L"] = "TS_4R",
            ["TS_3R"] = "TS_3L",
            ["TS_2L"] = "TS_2R",
            ["TS_1R"] = "TS_1L",
            ["TS_0L"] = "TS_0R",
        };








    void OnEnable()
    {
        Scene_Manager.InitiateLaunchInfo += onLaunchInitiated;
    }

    void OnDisable()
    {
        Scene_Manager.InitiateLaunchInfo -= onLaunchInitiated;
    }









    // Start is called before the first frame update
    void Awake()
    {

        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Tiny_Scientists"){
                foreach (Transform grandchild in child){
                    scientists.Add(grandchild.gameObject);
                    scientists_controllers.Add(grandchild.gameObject.GetComponent<Tiny_Scientist_Controller>());
                }
            }
            else if (child.gameObject.name == "Tiny_Scientists_Waypoints"){
                foreach (Transform grandchild in child){
                    waypoints.Add(grandchild.gameObject.name, grandchild.gameObject);
                }
            }
        }
        // Set each child at a random waypoint
        int index;
        for (int i = 0; i < scientists.Count; i++){
            index = random.Next(waypoints_random_choices.Count);
            scientists[i].transform.position = waypoints[waypoints_random_choices[index]].transform.position;
            scientists_controllers[i].Cur_Target_Waypoint = waypoints[waypoints_random_choices[index]];
            scientists_controllers[i].path_order = new List<GameObject>() {waypoints[waypoints_random_choices[index]]};
            scientists_controllers[i].walkSpeed = walkSpeed;
            scientists_controllers[i].runSpeed = runSpeed;
            scientists_controllers[i].minWaitSecs = minWaitSecs;
            scientists_controllers[i].maxWaitSecs = maxWaitSecs;
        }
    }

 
    void OnLevelWasLoaded(){
        launched = false;
        launchComplete = false;
    }
 

   // Update is called once per frame
    void Update()
    {
        if (!launched){
            for (int i = 0; i < scientists.Count; i++){
                if (scientists_controllers[i].isReadyForRandomWaypoint){
                    supplyRandomTargetWaypoint(scientists_controllers[i]);
                }
            }
        }
        else{
            bool all_made_it = true;
            for (int i = 0; i < scientists.Count; i++){
                // If some scientist hasn't yet made it to the building waypoint
                if (!scientists_controllers[i].isAtFinalWaypoint){
                    all_made_it = false;
                }
            }
            if (all_made_it){
                launchComplete = true;
            }
        }
    }



    void supplyRandomTargetWaypoint(Tiny_Scientist_Controller controller)
    {
        int i = random.Next(waypoints_random_choices.Count);
        controller.path_order = calculateWaypointPath(controller.Cur_Target_Waypoint, waypoints[waypoints_random_choices[i]]);
    }

    List<GameObject> calculateWaypointPath(GameObject prevWayPoint, GameObject targetWayPoint){
        List<string> newPath;
        List<GameObject> newPathGameObj = new List<GameObject>();
        string extraWaypoint = null;
        int start_i;
        int end_i;
        bool goingDown = false;
        bool launchSequence = false;

        // if (prevWayPoint.name == "TS_BUILDING" && targetWayPoint.name == "TS_BUILDING"){
        //     newPathGameObj.Add(targetWayPoint);
        //     return newPathGameObj;
        // }


        if (targetWayPoint.name == "TS_BUILDING"){
            launchSequence = true;
            targetWayPoint = waypoints["TS_0L"];
        }



        if (prevWayPoint.name == targetWayPoint.name){
            newPath = new List<String>();
            newPath.Add(targetWayPoint.name);
            foreach (string wp in newPath){
                newPathGameObj.Add(waypoints[wp]);
            }
            return newPathGameObj;
        }


        if (!waypoints_main_stairs_order.Contains(prevWayPoint.name)){
            start_i = waypoints_main_stairs_order.IndexOf(side_waypoint_to_main_stair[prevWayPoint.name]);
        }
        else
        {
            start_i = waypoints_main_stairs_order.IndexOf(prevWayPoint.name);
        }

        if (!waypoints_main_stairs_order.Contains(targetWayPoint.name)){
            end_i = waypoints_main_stairs_order.IndexOf(side_waypoint_to_main_stair[targetWayPoint.name]);
            extraWaypoint = targetWayPoint.name;
        }
        else{
            end_i = waypoints_main_stairs_order.IndexOf(targetWayPoint.name);
        }
        
        if (end_i < start_i){
            int tmp = start_i;
            start_i = end_i;
            end_i = tmp;
            goingDown = true;
        }
        

        newPath = waypoints_main_stairs_order.GetRange(start_i, end_i-start_i+1);

        if (goingDown){
            newPath.Reverse();
        }


        if (!(extraWaypoint is null)){
            newPath.Add(extraWaypoint);
        }


        if (launchSequence){
            newPath.Add("TS_BUILDING");
        }

        // If we are going to an out of the way waypoint on a launch
        if (launchSequence && side_waypoint_to_main_stair.ContainsKey(newPath[0])){
            newPath.RemoveAt(0);
        }

        // Sometimes we do this weird thing where we step down one stair, and then go back on the path
        // This will prevent that
        if (newPath.Contains(prevWayPoint.name)){
            int pwpi = newPath.IndexOf(prevWayPoint.name);
            for (int i=pwpi; i>=0; i--){
                newPath.RemoveAt(i);
            }
        }


        foreach (string wp in newPath){
            newPathGameObj.Add(waypoints[wp]);
        }
        //Debug.Log("PATH FROM " + prevWayPoint.name + " TO " + targetWayPoint.name + " IS " + string.Join(", ", newPath));
        return newPathGameObj;
    }


    void onLaunchInitiated(){
        Debug.Log("CALLING THIS");
        launched = true;
        for (int i = 0; i < scientists.Count; i++){
            scientists_controllers[i].path_order = calculateWaypointPath(scientists_controllers[i].Cur_Target_Waypoint, waypoints["TS_BUILDING"]);
            Debug.Log("SUPPLYING WAYPOINT PATH: " + string.Join(", ", scientists_controllers[i].path_order));
            scientists_controllers[i].launched = true;
        }
    }


}
