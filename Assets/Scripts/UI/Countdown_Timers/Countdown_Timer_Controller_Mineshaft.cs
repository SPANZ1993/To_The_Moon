using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Timer_Controller_Mineshaft : Countdown_Timer_Controller_Base
{
    Mine_Shaft_Controller mineShaftController;
    Game_Manager gameManager;

    void Start(){
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        mineShaftController = GameObject.Find("Mine_Shaft").GetComponent< Mine_Shaft_Controller>();

        base.Start();
    }

    protected override void setTimeLeft(){
        timeLeft = mineShaftController.nextRefreshTimeUnix - gameManager.gameTimeUnix;
        Debug.Log("TimeLeft Mineshaft: " + timeLeft + " ---- " + mineShaftController.nextRefreshTimeUnix + " ---- " + gameManager.gameTimeUnix);
    }

    protected override void setIsReady(){
        isReady = mineShaftController.mineGameIsReady;
    }
}
