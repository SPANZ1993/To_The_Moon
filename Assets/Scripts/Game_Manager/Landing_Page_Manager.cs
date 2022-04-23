using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_Page_Manager : MonoBehaviour
{
    PlayFab_Manager playFabManager;
    Game_Manager gameManager;
    UI_Controller uiController;

    bool loggedInPlayFabServer = false;
    double? serverTime;
    SaveGameObject loadedData;
    bool failedLogInPlayFabServer = false;
    bool failedGetServerTime = false;
    bool failedGetLoadedData = false;
    bool startedSceneTransition = false;

    bool retryConnectBoxDisplayed = false;
    bool waitingForResponsePlayFabLogin = false;
    bool waitingForResponsePlayFabTime = false;
    bool waitingForResponsePlayFabData = false;


    [SerializeField]
    float minRemainingSceneDisplayTime = 5f;
    


    void OnEnable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo += onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo += onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo += onPlayFabLoginFailure;
        PlayFab_Manager.PlayFabGetUnixTimeSuccessInfo += onGetServerTimeSuccess;
        PlayFab_Manager.PlayFabGetUnixTimeFailureInfo += onGetServerTimeFailure;
        PlayFab_Manager.PlayFabGetSaveDataSuccessInfo += onGetSaveDataSuccess;
        PlayFab_Manager.PlayFabGetSaveDataFailureInfo += onGetSaveDataFailure;
    }

    void OnDisable(){
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo -= onPlayFabAccountCreate;
        PlayFab_Manager.PlayFabLoginSuccessInfo -= onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginFailureInfo -= onPlayFabLoginFailure;
        PlayFab_Manager.PlayFabGetUnixTimeSuccessInfo -= onGetServerTimeSuccess;
        PlayFab_Manager.PlayFabGetUnixTimeFailureInfo -= onGetServerTimeFailure;
        PlayFab_Manager.PlayFabGetSaveDataSuccessInfo -= onGetSaveDataSuccess;
        PlayFab_Manager.PlayFabGetSaveDataFailureInfo -= onGetSaveDataFailure;
    }


    public delegate void EndLandingPageScene();
    public static event EndLandingPageScene EndLandingPageSceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        Invoke("Login", 3f);
        //Invoke("_EndLandingPageScene", 100000f);
    }
    
    public void Login(){
        playFabManager.Login();

        uiController.disableLandingPageRetryConnectBox();
        retryConnectBoxDisplayed = false;
        waitingForResponsePlayFabLogin = true;
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
        Debug.Log("LOADED DATA: " + loadedData);
        Debug.Log("SERVER TIME: " + serverTime);
        if (minRemainingSceneDisplayTime <= 0f && loggedInPlayFabServer && serverTime != null && loadedData != null){
            gameManager.loadedGame = loadedData;
            gameManager.gameTimeUnix = serverTime ?? 0;
            gameManager.gameStartTimeUnix = serverTime ?? 0;
            gameManager.doneLoading = true;
            _EndLandingPageScene();
        }
        else if (failedLogInPlayFabServer && !retryConnectBoxDisplayed && !waitingForResponsePlayFabLogin){
            uiController.enableLandingPageRetryConnectBox();
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
    }
    
    void onPlayFabLoginSuccess(){
        loggedInPlayFabServer = true;
        failedLogInPlayFabServer = false;
        waitingForResponsePlayFabLogin = false;
    }

    void onPlayFabLoginFailure(){
        failedLogInPlayFabServer = true;
        waitingForResponsePlayFabLogin = false;
    }

    void onGetServerTimeSuccess(double unixTime){
        Debug.Log("GOT UNIX TIME HERE: " + unixTime);
        serverTime = unixTime;
        waitingForResponsePlayFabTime = false;
    }

    void onGetServerTimeFailure(){
        Debug.Log("FAILED UNIX TIME HERE");
        waitingForResponsePlayFabTime = false;
    }

    void onGetSaveDataSuccess(SaveGameObject loadedGame){
        Debug.Log("GOT SAVE DATA HERE: " + loadedGame);
        loadedData = loadedGame;
        waitingForResponsePlayFabData = false;
    }

    void onGetSaveDataFailure(){
        Debug.Log("FAILED SAVE DATA HERE");
        loadedData = new SaveGameObject();
        waitingForResponsePlayFabData = false;
    }
}
