using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_Page_Button_Handlers : MonoBehaviour
{
    // Start is called before the first frame update
    UI_Controller uiController;
    Landing_Page_Manager landingPageManager;

    void Start(){
        landingPageManager = GameObject.Find("Landing_Page_Manager").GetComponent<Landing_Page_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
    }


    public void onRetryConnectButtonPressed(){
        landingPageManager.Login();
        uiController.disableLandingPageRetryConnectBox();
    }
}
