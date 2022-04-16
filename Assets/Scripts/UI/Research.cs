using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Research
{

    public int researchId;
    public string researchName;
    public double expectedTime;
    public double expectedThrust;
    public double price;
    public Sprite researchSprite;
    
    public Researcher assignedResearcher;
    public double assignedTime;
    public double timeLeft; // How much time in the future from the assigned time will this be complete?
    public bool researchComplete;
    public double thrustReward;
    
    Game_Manager gameManager;

    

    public Research(int ResearchId, string ResearchName, double ExpectedTime, double ExpectedThrust, double Price, Sprite ResearchSprite){
        researchId = ResearchId;
        researchName = ResearchName;
        expectedTime = ExpectedTime;
        expectedThrust = ExpectedThrust;
        price = Price;
        researchSprite = ResearchSprite;
    }


    public void assignResearcher(Researcher AssignedResearcher){
        if (gameManager is null){
            gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        }
        assignedResearcher = AssignedResearcher;
        assignedTime = gameManager.gameTimeUnix;
        timeLeft = Math.Round(AssignedResearcher.generateTimeMultiplier() * expectedTime);
        thrustReward = Math.Round(AssignedResearcher.generateThrustMultiplier() * expectedThrust);
    }
    
    
    public void assignResearcher(Researcher AssignedResearcher, double AssignedTime, double TimeLeft,  double ThrustReward){
        assignedResearcher = AssignedResearcher;
        assignedTime = AssignedTime;
        timeLeft = TimeLeft;
        thrustReward = ThrustReward;
    }

    public void deassignResearcher(){
        assignedResearcher = null;
        assignedTime = 0.0;
        timeLeft = 0.0;
        thrustReward = 0.0;
        researchComplete = false;
    }

    public bool isResearcherAssigned(){
        return assignedResearcher != null;
    }

    public double calculateTimeRemaining(){
        if (gameManager is null){
            gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        }
        double timeRemaining = 0.0;
        if (isResearcherAssigned()){
            //timeRemaining = Math.Max(Math.Round(assignedTime + timeLeft - gameManager.gameTimeUnix), 0.0);
            timeRemaining = Math.Max(assignedTime + timeLeft - gameManager.gameTimeUnix, 0.0);
            // Debug.Log("ASSIGNED TIME: " + assignedTime + " TIME LEFT: " + timeLeft + " GAME TIME: " + gameManager.gameTimeUnix + " TIME REMAINING: " + timeRemaining);
            if (timeRemaining <= 0.0f){
                researchComplete = true;
            }
        }
        return timeRemaining;
    }

}
