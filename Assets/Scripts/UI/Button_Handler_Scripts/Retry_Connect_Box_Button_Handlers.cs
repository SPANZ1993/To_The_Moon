using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry_Connect_Box_Button_Handlers : MonoBehaviour
{
    // Start is called before the first frame update
    UI_Controller uiController;
    

    public delegate void RetryConnectBoxButtonHandlerPressed();
    public static event RetryConnectBoxButtonHandlerPressed RetryConnectBoxButtonHandlerPressedInfo;


    void Start(){
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
    }


    public void onRetryConnectButtonPressed(){
        Audio_Manager.instance.Play("UI_Button_No_Effect");
        if(RetryConnectBoxButtonHandlerPressedInfo != null){
            RetryConnectBoxButtonHandlerPressedInfo();
        }
        uiController.disableRetryConnectBox();
    }
}
