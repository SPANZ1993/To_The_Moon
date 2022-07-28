using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    



    GameObject Logo;
    Material LogoMaterial;
    RectTransform LogoRect;
    GameObject BackgroundPanel;
    Material BackgroundMaterial;




    [SerializeField]
    LeanTweenType logoEaseType;
    [SerializeField]
    LeanTweenType colorEaseType;

    System.Action<float> updateLogoAction;
    System.Action<float> updateBackgroundAction;


    public delegate void EndLandingPageScene();
    public static event EndLandingPageScene EndLandingPageSceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();


        Logo = GameObject.Find("Logo");
        LogoMaterial = Logo.GetComponent<Image>().material;
        LogoRect = Logo.GetComponent<RectTransform>();
        BackgroundPanel = GameObject.Find("Background_Panel");
        BackgroundMaterial = BackgroundPanel.GetComponent<CanvasRenderer>().GetMaterial();
        updateLogoAction = _updateLogo;


        //tweenBackgroundColor();
        tweenLogoPosition();

        Invoke("InitializeGamePlayFab", 3f);
        //Invoke("_EndLandingPageScene", 100000f);
    }

    bool firstUpdate = true;
    void Update(){
        minRemainingSceneDisplayTime -= Time.deltaTime;
        
        // if(firstUpdate){
        //     tweenBackgroundColor();
        //     firstUpdate = false;
        // }
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


        // THIS NEEDS TO HAPPEN BEFORE THE MAIN AREA IS LOADED ON GAME STARTUP
        Game_Manager.instance.initializeProgressionManager(
            Game_Manager.instance.loadedGame.SerializedEventsState, 
            Game_Manager.instance.loadedGame.CurrentLevelId, 
            Game_Manager.instance.loadedGame.HighestLevelId, 
            Game_Manager.instance.loadedGame.RocketGameFreePlayMode,
            Game_Manager.instance.loadedGame.RocketGameFreePlayModeManuallySet);

        StartCoroutine(waitForSceneDisplayTime());
    }



    void tweenLogoPosition(){
        LeanTween.value(Logo, updateLogoAction, 0f, 1f, 3f).setEase(logoEaseType).setOnUpdate(
            updateLogoAction
        );
        LeanTween.move(Logo, new Vector2(0f,0f), 3f).setEase(logoEaseType);
    }

    float prevVal = -1;
    float prevPrevVal = -1;
    int bounceCount = 0;
    float[] bounceVolumeDecrements = {0f, .2f, .2f, .2f};
    void _updateLogo(float value){
        LogoMaterial.SetFloat("_ShadowY", Mathf.LerpUnclamped(-0.129f, -0.027f, value));
        if(value < prevVal && prevPrevVal < prevVal){
            if(bounceCount < bounceVolumeDecrements.Length){
                Audio_Manager.instance.SetVolume("UI_Logo_Thud", Audio_Manager.instance.GetVolume("UI_Logo_Thud")-bounceVolumeDecrements[bounceCount]);
            }
            Audio_Manager.instance.Play("UI_Logo_Thud");
            bounceCount++;
        }
        prevPrevVal = prevVal;
        prevVal = value;
    }


    void tweenBackgroundColor(){
        Image img = BackgroundPanel.GetComponent<Image>();
        LeanTween.value(BackgroundPanel, setColorCallback, new Color(255f/255f,255/255f,255/255f,255/255f), new Color(103f/255f, 115f/255f, 212f/255f, 255f/255f), 3f).setEase(colorEaseType);

        void setColorCallback( Color c )
        {
            img.color = c;
    
        // For some reason it also tweens my image's alpha so to set alpha back to 1 (I have my color set from inspector). You can use the following
    
            Color tempColor = img.color;
            tempColor.a = 1f;
            img.color = tempColor;
            //Debug.Log("SETTING BACKGROUND COLOR: " +  BackgroundPanel.GetComponent<Image>().color);
        }
    }
}
