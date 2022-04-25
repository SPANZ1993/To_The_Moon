using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;


public class Robot_Manager : MonoBehaviour
{

    private Game_Manager gameManager;

    private Robot_Controller robotController;
    private Animator robotAnimator;
    private Animator screenAnimator;



    public enum RobotStates {
        idle,
        thinking,
        pickax,
        alert
    };

    public RobotStates RobotState;

    
        
    public delegate void RobotTapped();
    public static event RobotTapped RobotTappedInfo;



     void OnEnable()
    {
        Robot_Controller._RobotTappedInfo += onRobotTapped;
        Mine_Shaft_Controller.MineGameReadyInfo += onMineGameReady;
    }

    void OnDisable()
    {
        Robot_Controller._RobotTappedInfo -= onRobotTapped;
        Mine_Shaft_Controller.MineGameReadyInfo -= onMineGameReady;
    }

    // Start is called before the first frame update
    void Start()
    {
        RobotState = RobotStates.idle;

        robotController = transform.GetChild(0).gameObject.GetComponent<Robot_Controller>();
        robotAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        screenAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();
    
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

    }

    // Update is called once per frame
    void Update()
    {
        updateScreenAnimation();
        updateRobotAnimation();

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
        //Debug.Log("ROBOT TAPPED FROM MANAGER... STATE IS: " + Enum.GetName(typeof(RobotStates), RobotState));
        RobotTappedInfo();
    }


    void onMineGameReady(){
        if (RobotState == RobotStates.idle){
            RobotState = RobotStates.pickax;
        }
    }


}
