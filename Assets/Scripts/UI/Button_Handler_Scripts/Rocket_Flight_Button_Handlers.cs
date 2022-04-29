using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Flight_Button_Handlers : MonoBehaviour
{
    Rocket_Control rocketControl;
    UI_Controller uiController;
    Rocket_Game_Manager rocketGameManager;
    Game_Manager gameManager;
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        rocketControl = GameObject.Find("Rocket").GetComponent<Rocket_Control>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
    }


    public void cutTheEngines(){
        rocketControl.thrust = 0.5;
    }

    public void onRewardedAdYesButtonPressed(){
        //Debug.Log("SAID YES");
        uiController.startRewardedAd();
    }

    public void onRewardedAdNoButtonPressed(){
        //Debug.Log("SAID NO");
        uiController.passRewardedAd();
    }

    public void pauseButton(){
        paused = !paused;
        rocketGameManager.SendAlertPause(paused);
    }

    public void onAppPause(){
        gameManager.onApplicationPause();
    }
}
