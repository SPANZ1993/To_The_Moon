using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_Game_Button_Handlers : MonoBehaviour
{
    // Start is called before the first frame update
    UI_Controller uiController;
    Game_Manager gameManager;

    void Start(){
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
    }


    public void onRewardedAdYesButtonPressed(){
        //Debug.Log("SAID YES");
        uiController.startRewardedAd();
    }

    public void onRewardedAdNoButtonPressed(){
        //Debug.Log("SAID NO");
        uiController.passRewardedAd();
    }
}
