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
    public Dictionary<string, string> titleData {get; private set;}
    bool failedLogInPlayFabServer = false;
    bool failedGetServerTime = false;
    bool failedGetLoadedData = false;
    bool failedGetTitleData = false;
    bool startedSceneTransition = false;

    string displayName; // The user's display name

    bool retryConnectBoxDisplayed = false;
    bool waitingForResponsePlayFabLogin = false;
    bool waitingForResponsePlayFabTime = false;
    bool waitingForResponsePlayFabData = false;
    bool waitingForResponsePlayFabTitleData = false;

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
        PlayFab_Manager.PlayFabGetTitleDataSuccessInfo += onGetTitleDataSuccess;
        PlayFab_Manager.PlayFabGetTitleDataFailureInfo += onGetTitleDataFailure;

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
        PlayFab_Manager.PlayFabGetTitleDataSuccessInfo -= onGetTitleDataSuccess;
        PlayFab_Manager.PlayFabGetTitleDataFailureInfo -= onGetTitleDataFailure;
    
        Retry_Connect_Box_Button_Handlers.RetryConnectBoxButtonHandlerPressedInfo -= onRetryConnectButtonPressed;
    }


    // Start is called before the first frame update
    void Start()
    {
        if(StartingPlayFabInitiationInfo != null){
            StartingPlayFabInitiationInfo();
        }

        //Debug.Log("IS TITLE DATA NULL? " + (titleData == null));

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
        if (loggedInPlayFabServer && serverTime != null && loadedData != null && titleData != null){
            if (SceneManager.GetActiveScene().name == "Landing_Page"){ // It should always be this scene if we are creating a user
                loadedData.Metrics.numGameStartups += 1;
            }
            gameManager.loadedGame = loadedData;
            gameManager.metrics = loadedData.Metrics;
            gameManager.gameTimeUnix = serverTime ?? 0;
            gameManager.gameStartTimeUnix = serverTime ?? 0;
            gameManager.userDisplayName = displayName;
            gameManager.titleData = titleData;
            gameManager.doneLoading = true;
            callBack();
            if(EndingPlayFabInitiationInfo != null){
                EndingPlayFabInitiationInfo();
            }
            Debug.Log("PLAYFAB INITIALIZER DESTROYED");
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
        if (loggedInPlayFabServer && titleData == null && !waitingForResponsePlayFabTitleData){
            playFabManager.GetTitleData();
            waitingForResponsePlayFabTitleData = true;
        }
    }



    //IF WE ARE IN THE LANDING PAGE AND (AN ACCOUNT WAS CREATED OR THE ACCOUNT DOESN'T HAVE A DISPLAY NAME) THEN SET PROGRESSION MANAGER TO INITIALIZED SO IT RUNS THE ONBOARDING SEQUENCE

    void onPlayFabAccountCreate(){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFabLogin = false;
        loadedData = new SaveGameObject();
        if (SceneManager.GetActiveScene().name == "Landing_Page"){ // It should always be this scene if we are creating a user
            loadedData.Metrics.numGameStartups = 0;
            Progression_Manager.instance.initialized = true;
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
        if(displayName == null || displayName == "" && SceneManager.GetActiveScene().name == "Landing_Page"){
            Progression_Manager.instance.initialized = true;
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

        Dictionary<AudioChannel, float> channelVols = new Dictionary<AudioChannel, float>();
        channelVols[AudioChannel.SoundFX] = loadedData.SoundFxSoundLevel;
        channelVols[AudioChannel.Music] = loadedData.MusicSoundLevel;
        Audio_Manager.instance.UpdateChannelVolumes(channelVols);
        
        waitingForResponsePlayFabData = false;

    }

    void onGetSaveDataFailure(){
        //Debug.Log("FAILED SAVE DATA HERE");
        loadedData = new SaveGameObject();
        waitingForResponsePlayFabData = false;
    }


    void onGetTitleDataSuccess(Dictionary<string, string> TitleData){
        titleData = TitleData;
        Debug.Log("GOT TITLE DATA: " + titleData["Robot Message"]);
    }

    void onGetTitleDataFailure(){

    }


    void onRetryConnectButtonPressed(){
        Login();
    }
}
