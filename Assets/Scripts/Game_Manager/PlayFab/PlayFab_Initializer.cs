using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class PlayFab_Initializer : MonoBehaviour
{
    PlayFab_Manager playFabManager;
    Game_Manager gameManager;
    UI_Controller uiController;

    bool loggedInPlayFabServer = false;
    public double? serverTime {get; private set;}
    public SaveGameObject loadedData {get; private set;}
    bool failedLogInPlayFabServer = false;
    bool failedGetServerTime = false;
    bool failedGetLoadedData = false;
    bool startedSceneTransition = false;

    string displayName; // The user's display name

    bool retryConnectBoxDisplayed = false;
    bool waitingForResponsePlayFabLogin = false;
    bool waitingForResponsePlayFabTime = false;
    bool waitingForResponsePlayFabData = false;

    public System.Action callBack {private get; set;}


    public delegate void StartingPlayFabInitiation();
    public static event StartingPlayFabInitiation StartingPlayFabInitiationInfo;

    public delegate void EndingPlayFabInitiation();
    public static event EndingPlayFabInitiation EndingPlayFabInitiationInfo;

    void OnEnable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo += onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo += onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo += onPlayFabLoginFailure;
        PlayFab_Manager.PlayFabGetUnixTimeSuccessInfo += onGetServerTimeSuccess;
        PlayFab_Manager.PlayFabGetUnixTimeFailureInfo += onGetServerTimeFailure;
        PlayFab_Manager.PlayFabGetSaveDataSuccessInfo += onGetSaveDataSuccess;
        PlayFab_Manager.PlayFabGetSaveDataFailureInfo += onGetSaveDataFailure;

        Retry_Connect_Box_Button_Handlers.RetryConnectBoxButtonHandlerPressedInfo += onRetryConnectButtonPressed;
    }

    void OnDisable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo -= onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo -= onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo -= onPlayFabLoginFailure;
        PlayFab_Manager.PlayFabGetUnixTimeSuccessInfo -= onGetServerTimeSuccess;
        PlayFab_Manager.PlayFabGetUnixTimeFailureInfo -= onGetServerTimeFailure;
        PlayFab_Manager.PlayFabGetSaveDataSuccessInfo -= onGetSaveDataSuccess;
        PlayFab_Manager.PlayFabGetSaveDataFailureInfo -= onGetSaveDataFailure;
    
        Retry_Connect_Box_Button_Handlers.RetryConnectBoxButtonHandlerPressedInfo -= onRetryConnectButtonPressed;
    }


    // Start is called before the first frame update
    void Start()
    {
        if(StartingPlayFabInitiationInfo != null){
            StartingPlayFabInitiationInfo();
        }

        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        StartCoroutine(_firstLoginAttempt());
        //Invoke("_EndLandingPageScene", 100000f);
    }

    IEnumerator _firstLoginAttempt(){
        while(callBack == null){
            yield return new WaitForSeconds(0);
        }
        Login();
    }

    
    public void Login(){
        playFabManager.Login();

        uiController.disableRetryConnectBox();
        retryConnectBoxDisplayed = false;
        waitingForResponsePlayFabLogin = true;
    }


    // Update is called once per frame
    void Update()
    {
        // Debug.Log("LOADED DATA: " + loadedData);
        // Debug.Log("SERVER TIME: " + serverTime);
        if (loggedInPlayFabServer && serverTime != null && loadedData != null){
            if (SceneManager.GetActiveScene().name == "Landing_Page"){ // It should always be this scene if we are creating a user
                loadedData.Metrics.numGameStartups += 1;
            }
            gameManager.loadedGame = loadedData;
            gameManager.metrics = loadedData.Metrics;
            gameManager.gameTimeUnix = serverTime ?? 0;
            gameManager.gameStartTimeUnix = serverTime ?? 0;
            gameManager.userDisplayName = displayName;
            gameManager.doneLoading = true;
            callBack();
            if(EndingPlayFabInitiationInfo != null){
                EndingPlayFabInitiationInfo();
            }
            Destroy(this);
        }
        else if (failedLogInPlayFabServer && !retryConnectBoxDisplayed && !waitingForResponsePlayFabLogin){
            //Debug.Log("TRYING TO ENABLE CONNECTION BOX");
            uiController.enableRetryConnectBox();
            retryConnectBoxDisplayed = true;
        }
        if (loggedInPlayFabServer && serverTime == null && !waitingForResponsePlayFabTime){
            playFabManager.GetServerTime();
            waitingForResponsePlayFabTime = true;
        }
        if (loggedInPlayFabServer && loadedData == null && !waitingForResponsePlayFabData){
            playFabManager.LoadData();
            waitingForResponsePlayFabData = true;
        }
    }

    void onPlayFabAccountCreate(){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFabLogin = false;
        loadedData = new SaveGameObject();
        if (SceneManager.GetActiveScene().name == "Landing_Page"){ // It should always be this scene if we are creating a user
            loadedData.Metrics.numGameStartups = 0;
        }
    }
    
    void onPlayFabLoginSuccess(LoginResult result){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFabLogin = false;
        displayName = null;
        if (result.InfoResultPayload.PlayerProfile != null){
            displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
    }

    void onPlayFabLoginFailure(){
        failedLogInPlayFabServer = true;
        waitingForResponsePlayFabLogin = false;
    }

    void onGetServerTimeSuccess(double unixTime){
        //Debug.Log("GOT UNIX TIME HERE: " + unixTime);
        serverTime = unixTime;
        waitingForResponsePlayFabTime = false;
    }

    void onGetServerTimeFailure(){
        //Debug.Log("FAILED UNIX TIME HERE");
        waitingForResponsePlayFabTime = false;
    }

    void onGetSaveDataSuccess(SaveGameObject loadedGame){
        //Debug.Log("GOT SAVE DATA HERE: " + loadedGame);
        loadedData = loadedGame;
        waitingForResponsePlayFabData = false;

    }

    void onGetSaveDataFailure(){
        //Debug.Log("FAILED SAVE DATA HERE");
        loadedData = new SaveGameObject();
        waitingForResponsePlayFabData = false;
    }

    void onRetryConnectButtonPressed(){
        Login();
    }
}
