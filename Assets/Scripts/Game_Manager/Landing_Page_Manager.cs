using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_Page_Manager : MonoBehaviour
{
    PlayFab_Manager playFabManager;
    UI_Controller uiController;

    bool loggedInPlayFabServer = false;
    bool failedLogInPlayFabServer = false;
    bool startedSceneTransition = false;

    bool retryConnectBoxDisplayed = false;
    bool waitingForResponsePlayFab = false;


    [SerializeField]
    float minRemainingSceneDisplayTime = 5f;
    


    void OnEnable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo += onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo += onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo += onPlayFabLoginFailure;
    }

    void OnDisable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo -= onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo -= onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo -= onPlayFabLoginFailure;
    }


    public delegate void EndLandingPageScene();
    public static event EndLandingPageScene EndLandingPageSceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();

        Invoke("Login", 3f);
        //Invoke("_EndLandingPageScene", 100000f);
    }
    
    public void Login(){
        playFabManager.Login();
        uiController.disableLandingPageRetryConnectBox();
        retryConnectBoxDisplayed = false;
        waitingForResponsePlayFab = true;
    }

    void _EndLandingPageScene(){
        if (EndLandingPageSceneInfo != null){
            EndLandingPageSceneInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        minRemainingSceneDisplayTime -= Time.deltaTime;
        if (minRemainingSceneDisplayTime <= 0f && loggedInPlayFabServer){
            _EndLandingPageScene();
        }
        else if (failedLogInPlayFabServer && !retryConnectBoxDisplayed && !waitingForResponsePlayFab){
            uiController.enableLandingPageRetryConnectBox();
            retryConnectBoxDisplayed = true;
        }
    }

    void onPlayFabAccountCreate(){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFab = false;
    }
    
    void onPlayFabLoginSuccess(){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFab = false;
    }

    void onPlayFabLoginFailure(){
        failedLogInPlayFabServer = true;
        waitingForResponsePlayFab = false;
    }
}
