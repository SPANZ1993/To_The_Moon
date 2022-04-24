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
    




    public delegate void EndLandingPageScene();
    public static event EndLandingPageScene EndLandingPageSceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        Invoke("InitializeGamePlayFab", 3f);
        //Invoke("_EndLandingPageScene", 100000f);
    }

    void Update(){
        minRemainingSceneDisplayTime -= Time.deltaTime;
    }

    
    public void InitializeGamePlayFab(){
        PlayFab_Initializer playFabInitializer = gameObject.AddComponent<PlayFab_Initializer>();
        playFabInitializer.callBack = _EndLandingPageScene;
    }

    void _EndLandingPageScene(){
        IEnumerator waitForSceneDisplayTime(){
            while (minRemainingSceneDisplayTime > 0){
                yield return new WaitForSeconds(0);
            }
            if (EndLandingPageSceneInfo != null){
                EndLandingPageSceneInfo();
            }
        }

        StartCoroutine(waitForSceneDisplayTime());
    }
}
