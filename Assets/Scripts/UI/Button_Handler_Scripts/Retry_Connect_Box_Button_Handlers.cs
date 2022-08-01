using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Retry_Connect_Box_Button_Handlers : MonoBehaviour
{
    // Start is called before the first frame update
    //UI_Controller uiController;
    

    public delegate void RetryConnectBoxButtonHandlerPressed();
    public static event RetryConnectBoxButtonHandlerPressed RetryConnectBoxButtonHandlerPressedInfo;


    void Start(){
        //uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
    }


    public void onRetryConnectButtonPressed(){
        if(!Audio_Manager.instance.IsPlaying("UI_Button_No_Effect")){
            Audio_Manager.instance.Play("UI_Button_No_Effect");
        }
        if(RetryConnectBoxButtonHandlerPressedInfo != null){
            if(GameObject.Find("App_State_Text")!=null){
                GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text += "BPA";
            }
            RetryConnectBoxButtonHandlerPressedInfo();
        }
        UI_Controller.instance.disableRetryConnectBox();
    }
}
