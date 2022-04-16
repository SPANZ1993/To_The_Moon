using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Timer_Rocket_Button : Countdown_Timer_Controller_Base
{

    Game_Manager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        base.Start();
    }


    protected override void setTimeLeft(){
        timeLeft = (gameManager.prevLaunchTimeUnix + gameManager.launchRefreshTime) - gameManager.gameTimeUnix;
    }

    protected override void setIsReady(){
        isReady = gameManager.remainingLaunches >= 1;
    }
}
