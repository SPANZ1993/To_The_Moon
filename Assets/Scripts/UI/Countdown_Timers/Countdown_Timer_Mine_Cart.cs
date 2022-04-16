using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Timer_Mine_Cart : Countdown_Timer_Controller_Base
{
    private Minecart_Controller minecartController;
    private Game_Manager gameManager;

    void Start(){
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        minecartController = GameObject.Find("Minecart").GetComponent< Minecart_Controller>();

        base.Start();
    }

    protected override void setTimeLeft(){
        timeLeft = minecartController.secondsLeft;
    }

    protected override void setIsReady(){
        isReady = minecartController.isFull;
    }
}
