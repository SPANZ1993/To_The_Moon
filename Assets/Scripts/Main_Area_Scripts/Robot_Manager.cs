using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;


public enum RobotStates {
    idle,
    thinking,
    pickax,
    alert
};

public class Robot_Manager : MonoBehaviour
{

    private Game_Manager gameManager;

    private Robot_Controller robotController;
    private Animator robotAnimator;
    private Animator screenAnimator;

    // So we can monitor whether the mine game is ready
    private Mine_Shaft_Controller mineShaftController;



    public RobotStates RobotState {get; private set;}

    public Queue<Speech_Object> messageQueue;
        
    public delegate void RobotTapped();
    public static event RobotTapped RobotTappedInfo;


    public static Robot_Manager instance;


    void Awake(){
        if (!instance){
            instance = this;
            messageQueue = new Queue<Speech_Object>();
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }


    }



    void OnEnable()
    {
        Robot_Controller._RobotTappedInfo += onRobotTapped;
        //Mine_Shaft_Controller.MineGameReadyInfo += onMineGameReady;
    }

    void OnDisable()
    {
        Robot_Controller._RobotTappedInfo -= onRobotTapped;
        //Mine_Shaft_Controller.MineGameReadyInfo -= onMineGameReady;
    }

    void OnLevelWasLoaded(){
        if(SceneManager.GetActiveScene().name == "Main_Area"){
            Start();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RobotState = RobotStates.idle;

        GameObject robot = GameObject.Find("Robot");
        robotController = robot.GetComponent<Robot_Controller>();
        robotAnimator = robot.GetComponent<Animator>();
        screenAnimator = GameObject.Find("Robot_Screen").GetComponent<Animator>();
        mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();
    
        gameManager = Game_Manager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Main_Area"){
            updateRobotState();
            updateScreenAnimation();
            updateRobotAnimation();
        }
    }


    void updateRobotAnimation(){
        switch(RobotState){
            case RobotStates.idle:
                robotController.wave = false;
                break;
            case RobotStates.thinking:
                robotController.wave = false;
                break;
            case RobotStates.pickax:
                robotController.wave = true;
                break;
            case RobotStates.alert:
                robotController.wave = true;
                break;
            default:
                robotController.wave = false;
                break;
        }
    }


    void updateScreenAnimation(){
        void falsify(){
            foreach(int i in Enum.GetValues(typeof(RobotStates)))
            {
                screenAnimator.SetBool(Enum.GetName(typeof(RobotStates), i), false);
            }
        }

        
        falsify();
        screenAnimator.SetBool(Enum.GetName(typeof(RobotStates), RobotState), true);
    }

    
    void onRobotTapped(){
        if(!Audio_Manager.instance.IsPlaying("Robot_Tapped")){
            Audio_Manager.instance.Play("Robot_Tapped");
        }
        //Debug.Log("ROBOT TAPPED FROM MANAGER... STATE IS: " + Enum.GetName(typeof(RobotStates), RobotState));
        if(RobotTappedInfo != null){
            RobotTappedInfo();
        }
    }


    // void onMineGameReady(){
    //     if (RobotState == RobotStates.idle){
    //         RobotState = RobotStates.pickax;
    //     }
    // }


    void updateRobotState(){
        if(messageQueue.Count != 0){
            RobotState = RobotStates.alert;
        }
        else if(mineShaftController != null && mineShaftController.mineGameIsReady){
            RobotState = RobotStates.pickax;
        }

        else RobotState = RobotStates.idle;
    }

}
