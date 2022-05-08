using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Timer_Mine_Cart : Countdown_Timer_Controller_Base
{
    private Minecart_Manager minecartManager;
    private Game_Manager gameManager;

    void Start(){
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        minecartManager = GameObject.Find("Minecart_Manager").GetComponent< Minecart_Manager>();

        base.Start();
    }

    protected override void setTimeLeft(){
        timeLeft = minecartManager.secondsLeft;
        //Debug.Log("TIME LEFT MINE CART SECONDS: " + timeLeft);
    }

    protected override void setIsReady(){
        isReady = minecartManager.isFull;
    }
}
