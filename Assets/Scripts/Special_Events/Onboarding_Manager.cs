using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onboarding_Manager : MonoBehaviour
{

    private GameObject nameInputBox;
    private GameObject coinNameInputBox;
    private GameObject screenTint;
    
    int currentSpeechNum = 1;
    string displayName;
    string coinName;

    public delegate void OnboardingStarted();
    public static event OnboardingStarted OnboardingStartedInfo;

    public delegate void OnboardingEnded();
    public static event OnboardingEnded OnboardingEndedInfo;



    // Sequence Variables
    // private bool waitingForNameSubmit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        UI_Controller.NameInputBoxSubmitButtonPressedInfo += onNameSubmitButtonPressed;
        UI_Controller.CoinNameInputBoxSubmitButtonPressedInfo += onCoinNameSubmitButtonPressed;
    }

    void OnDisable(){
        UI_Controller.NameInputBoxSubmitButtonPressedInfo -= onNameSubmitButtonPressed;
        UI_Controller.CoinNameInputBoxSubmitButtonPressedInfo -= onCoinNameSubmitButtonPressed;
    }


    public void ExecuteOnboarding(float delay){
        if(OnboardingStartedInfo != null){
            OnboardingStartedInfo();
        }
        StartCoroutine(_executeOnboarding(delay));
    }

    IEnumerator _executeOnboarding(float delay){
        yield return new WaitForSeconds(delay);

        nameInputBox = GameObject.Find("Name_Input_Box");
        coinNameInputBox = GameObject.Find("Coin_Name_Input_Box");
        screenTint = GameObject.Find("Screen_Tint");

        UI_Controller.instance.EnableUIElement(screenTint);
        UI_Controller.instance.EnableUIElement(nameInputBox);
        UI_Controller.instance.DisableUIElement(screenTint, touchOnly:true);

        //waitingForNameSubmit = true;
        StartCoroutine(_waitForNameSubmit());
        //TouchScreenKeyboard.Open("Name", keyboardType = TouchScreenKeyboardType.ASCIICapable;
    }

    private IEnumerator _waitForNameSubmit(){
        yield return new WaitForSeconds(0);
        if(displayName == null){
            StartCoroutine(_waitForNameSubmit());
        }
        else{
            if(BadwordsFilter.instance.checkWordContainsBadWords(displayName)){
                // Display Something About it having bad words
                Debug.Log("BAD WORDS BABBY");
                displayName = null;
                StartCoroutine(_waitForNameSubmit());
            }
            else{
                Debug.Log("MADE IT HERE");
                UI_Controller.instance.DisableUIElement(screenTint);
                UI_Controller.instance.DisableUIElement(nameInputBox);
                Game_Manager.instance.userDisplayName = displayName;
                startOnboardSpeech(1);
            }
        }
    }

    private void promptForCoinName(){
        Debug.Log("PROMPTING FOR COIN NAME!");
        StartCoroutine(_promptForCoinName(.25f));
    }

    private IEnumerator _promptForCoinName(float delay){
        yield return new WaitForSeconds(delay);
        UI_Controller.instance.EnableUIElement(coinNameInputBox);
        UI_Controller.instance.EnableUIElement(screenTint);
        StartCoroutine(_waitForCoinNameSubmit());
    }

    private IEnumerator _waitForCoinNameSubmit(){
        yield return new WaitForSeconds(0);
        if(coinName == null){
            StartCoroutine(_waitForCoinNameSubmit());
        }
        else{
            if(BadwordsFilter.instance.checkWordContainsBadWords(coinName)){
                // Display Something About it having bad words
                Debug.Log("BAD WORDS BABBY");
                coinName = null;
                StartCoroutine(_waitForCoinNameSubmit());
            }
            else{
                Debug.Log("MADE IT HERE COINS");
                UI_Controller.instance.DisableUIElement(screenTint);
                UI_Controller.instance.DisableUIElement(coinNameInputBox);
                Game_Manager.instance.coinName = coinName + Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.General.Coin").ToLower();
                startOnboardSpeech(2);
            }
        }
    }


    private void startTour(){
        Debug.Log("Starting Tour!");
    }

    private void startOnboardSpeech(int onboardingEventSpeechNum){
        string keystring = "";
        if(onboardingEventSpeechNum == 1){
            keystring = "Events_Script.Onboarding.1.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:true, keyString:keystring, formatFunc:null), callBack:promptForCoinName);
        }
        if(onboardingEventSpeechNum == 2){
            keystring = "Events_Script.Onboarding.2.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:true, keyString:keystring, formatFunc:null), callBack:startTour);
        }
    }

    private void onNameSubmitButtonPressed(string enteredName){
        Debug.Log("YO YO YO YO");
        displayName = enteredName;
    }

    private void onCoinNameSubmitButtonPressed(string enteredName){
        Debug.Log("YO YO YO YO JOE");
        coinName = enteredName;
    }

}
