using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Onboarding_Manager : MonoBehaviour
{

    private GameObject nameInputBox;
    private GameObject coinNameInputBox;
    private GameObject screenTint;



    private Collider2D mineshaftCollider, minecartCollider, robotCollider;

    private Collider2D rocketButtonCollider, rocketBuildingCollider;




    private Sprite_Highlighter spriteHighlighter;


    private bool swipedLeftPrevFrame = false;
    private bool swipedRightPrevFrame = false;

    private bool openedLidPrevFrame = false;
    private bool closedLidPrevFrame = false;

    private bool minecartTappedPrevFrame = false;

    private bool displayNameValidated = false;
    private bool waitingForNameValidationResponse = false;
    private bool nameIsUnique = false;



    private bool loggedIn, loginFailed = false;
    private bool displayNameSet, setDisplayNameFailed = false;



    string displayName;
    string coinName;

    public delegate void OnboardingStarted();
    public static event OnboardingStarted OnboardingStartedInfo;


    public delegate void OnboardingDisableNonUITouch();
    public static event OnboardingDisableNonUITouch OnboardingDisableNonUITouchInfo;

    // public delegate void OnboardingEnableNonUITouch();
    // public static event OnboardingEnableNonUITouch OnboardingEnableNonUITouchInfo;

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
        UI_Controller.NameSubmitConfirmationButtonPressedInfo += onNameSubmitConfirmationButtonPressed;
        UI_Controller.CoinNameSubmitConfirmationButtonPressedInfo += onCoinNameSubmitConfirmationButtonPressed;

        Touch_Detection.SwipedLeftInfo += onSwipedLeft;
        Touch_Detection.SwipedRightInfo += onSwipedRight;

        Minecart_Manager.MinecartTappedInfo += onMinecartTapped;

        Launch_Button_Controller.LidOpenedPrevFrameInfo += onLidOpened;
        Launch_Button_Controller.LidClosedPrevFrameInfo += onLidClosed;

        PlayFab_Manager.PlayFabGetAccountInfoSuccessInfo += onGetAccountInfoSuccess;
        PlayFab_Manager.PlayFabGetAccountInfoFailureInfo += onGetAccountInfoFailure;
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo += onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginSuccessInfo += onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabSetDisplayNameSuccessInfo += onPlayFabSetDisplayNameSuccess;
        PlayFab_Manager.PlayFabSetDisplayNameFailureInfo += onPlayFabSetDisplayNameFailure;
        
    }

    void OnDisable(){
        UI_Controller.NameInputBoxSubmitButtonPressedInfo -= onNameSubmitButtonPressed;
        UI_Controller.CoinNameInputBoxSubmitButtonPressedInfo -= onCoinNameSubmitButtonPressed;
        UI_Controller.NameSubmitConfirmationButtonPressedInfo -= onNameSubmitConfirmationButtonPressed;
        UI_Controller.CoinNameSubmitConfirmationButtonPressedInfo -= onCoinNameSubmitConfirmationButtonPressed;

    
        Touch_Detection.SwipedLeftInfo -= onSwipedLeft;
        Touch_Detection.SwipedRightInfo -= onSwipedRight;

        Minecart_Manager.MinecartTappedInfo -= onMinecartTapped;

        Launch_Button_Controller.LidOpenedPrevFrameInfo -= onLidOpened;
        Launch_Button_Controller.LidClosedPrevFrameInfo -= onLidClosed;

        PlayFab_Manager.PlayFabGetAccountInfoSuccessInfo -= onGetAccountInfoSuccess;
        PlayFab_Manager.PlayFabGetAccountInfoFailureInfo -= onGetAccountInfoFailure;
        PlayFab_Manager.PlayFabAccountCreateSuccessInfo -= onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabLoginSuccessInfo -= onPlayFabLoginSuccess;
        PlayFab_Manager.PlayFabSetDisplayNameSuccessInfo -= onPlayFabSetDisplayNameSuccess;
        PlayFab_Manager.PlayFabSetDisplayNameFailureInfo -= onPlayFabSetDisplayNameFailure;
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

        UI_Controller.instance.displayNameInputBox();


        //waitingForNameSubmit = true;
        StartCoroutine(_waitForNameSubmit());
        //TouchScreenKeyboard.Open("Name", keyboardType = TouchScreenKeyboardType.ASCIICapable;
    }


    void onNameSubmitConfirmationButtonPressed(bool selectedYes){
        if(selectedYes){
            UI_Controller.instance.DisableUIElement(GameObject.Find("Name_Input_Confirmation_Box"));
            UI_Controller.instance.DisableUIElement(screenTint);
            startOnboardSpeech(1);
        }
        else{
            UI_Controller.instance.displayNameInputBox(hint:"", potentialName:displayName);
            UI_Controller.instance.DisableUIElement(GameObject.Find("Name_Input_Confirmation_Box"));
            displayName = null;
            StartCoroutine(_waitForNameSubmit());
        }
    }


    private void validateNameSubmit(string name){
        waitingForNameValidationResponse = true;
        PlayFab_Manager.instance.GetAccountInfo(name);
    }



    private IEnumerator _waitForNameSubmit(){
        yield return new WaitForSeconds(0);
        if(displayName == null || waitingForNameValidationResponse){
            StartCoroutine(_waitForNameSubmit());
        }
        else{
            Debug.Log(displayName.Count(char.IsLetterOrDigit));
            if(BadwordsFilter.instance.checkWordContainsBadWords(displayName)){
                // Display Something About it having bad words
                // foreach(Transform child in GameObject.Find("Name_Enter_Box_Hint_Text").transform){
                //     Destroy(child.gameObject);
                // }
                UI_Controller.instance.displayNameInputBox(hint:Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.Onboarding.Name_Input_Box.Hint.Badwords"), potentialName:displayName);
                Debug.Log("BAD WORDS BABBY");
                displayName = null;
                displayNameValidated = false;
                waitingForNameValidationResponse = false;
                
                StartCoroutine(_waitForNameSubmit());
            }
            else if (displayName.Length < 3 || displayName.Count(char.IsLetterOrDigit) < 3){
                UI_Controller.instance.displayNameInputBox(hint:Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.Onboarding.Name_Input_Box.Hint.Too_Short"), potentialName:displayName);
                // Display Something About it Needing to Be Longer than 3 Characters
                Debug.Log("2$HORT");
                displayName = null;
                displayNameValidated = false;
                waitingForNameValidationResponse = false;
                
                StartCoroutine(_waitForNameSubmit());
            }
            else if(!displayNameValidated && !waitingForNameValidationResponse){
                validateNameSubmit(displayName);
                StartCoroutine(_waitForNameSubmit());
            }
            else if(!displayNameValidated && waitingForNameValidationResponse){
                StartCoroutine(_waitForNameSubmit());
            }
            else if(displayNameValidated && !nameIsUnique){
                UI_Controller.instance.displayNameInputBox(hint:Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.Onboarding.Name_Input_Box.Hint.Taken"), potentialName:displayName);
                //Display Something About it Being Taken
                Debug.Log("TAKEN");
                displayName = null;
                displayNameValidated = false;
                waitingForNameValidationResponse = false;
                StartCoroutine(_waitForNameSubmit());
            }
            else{
                Debug.Log("MADE IT HERE");
                UI_Controller.instance.DisableUIElement(screenTint);
                UI_Controller.instance.DisableUIElement(nameInputBox);
                Game_Manager.instance.userDisplayName = displayName;
                UI_Controller.instance.displayNameConfirmationBox(displayName);
                //startOnboardSpeech(1);
            }
        }
    }

    private void promptForCoinName(){
        Debug.Log("PROMPTING FOR COIN NAME!");
        StartCoroutine(_promptForCoinName(0.25f));
    }

    private IEnumerator _promptForCoinName(float delay){
        yield return new WaitForSeconds(delay);
        UI_Controller.instance.displayCoinNameInputBox();
        StartCoroutine(_waitForCoinNameSubmit());
    }


    void onCoinNameSubmitConfirmationButtonPressed(bool selectedYes){
        if(selectedYes){
            UI_Controller.instance.DisableUIElement(GameObject.Find("Coin_Name_Input_Confirmation_Box"));
            UI_Controller.instance.DisableUIElement(screenTint);
            startOnboardSpeech(2);
        }
        else{
            UI_Controller.instance.displayCoinNameInputBox(hint:"", potentialCoinName:coinName);
            UI_Controller.instance.DisableUIElement(GameObject.Find("Coin_Name_Input_Confirmation_Box"));
            coinName = null;
            StartCoroutine(_waitForCoinNameSubmit());
        }
    }



    private IEnumerator _waitForCoinNameSubmit(){
        yield return new WaitForSeconds(0);
        if(coinName == null){
            StartCoroutine(_waitForCoinNameSubmit());
        }
        else{
            if(BadwordsFilter.instance.checkWordContainsBadWords(coinName)){
                // Display Something About it having bad words
                UI_Controller.instance.displayCoinNameInputBox(hint:Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.Onboarding.Coin_Name_Input_Box.Hint.Badwords"), potentialCoinName:coinName);
                Debug.Log("BAD WORDS BABBY");
                coinName = null;
                StartCoroutine(_waitForCoinNameSubmit());
            }
            else if (coinName.Length < 3 || coinName.Count(char.IsLetterOrDigit) < 3){
                // Display Something About it Needing to Be Longer than 3 Characters
                UI_Controller.instance.displayCoinNameInputBox(hint:Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.Onboarding.Name_Input_Box.Hint.Too_Short"), potentialCoinName:coinName);
                Debug.Log("2$HORT");
                coinName = null;
                StartCoroutine(_waitForCoinNameSubmit());
            }
            else{
                Debug.Log("MADE IT HERE COINS");
                UI_Controller.instance.DisableUIElement(screenTint);
                UI_Controller.instance.DisableUIElement(coinNameInputBox);
                Game_Manager.instance.coinName = coinName + Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.General.Coin").ToLower();
                //startOnboardSpeech(2);
                UI_Controller.instance.displayCoinNameConfirmationBox(coinName + Localization_Manager.instance.GetLocalizedString("UI_Banner", "UI.General.Coin").ToLower());
            }
        }
    }


    private void startTour(){
        
        
        Debug.Log("Starting Tour!");
        spriteHighlighter = gameObject.AddComponent<Sprite_Highlighter>();
        if(OnboardingDisableNonUITouchInfo != null){
            OnboardingDisableNonUITouchInfo();
        }
        spriteHighlighter.StartPulsing(.5f);
        spriteHighlighter.AddHighlightedSprites(new GameObject[] {GameObject.Find("Computer"), 
                                                                GameObject.Find("Computer_Screen"), 
                                                                GameObject.Find("Keyboard_Screen"), 
                                                                GameObject.Find("Keyboard"), 
                                                                GameObject.Find("Mouse")}
                                                );

        Invoke("startPostComputerSpeech", 3f);
    }

    private void startPostComputerSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(new GameObject[] {GameObject.Find("Computer"), 
                                                        GameObject.Find("Computer_Screen"), 
                                                        GameObject.Find("Keyboard_Screen"), 
                                                        GameObject.Find("Keyboard"), 
                                                        GameObject.Find("Mouse")}
                                        );
        startOnboardSpeech(3);
    }
    


    private void highlightBookshelf(){
        spriteHighlighter.AddHighlightedSprites(GameObject.Find("Bookshelf"));
        Invoke("startPostBookshelfSpeech", 3f);
    }

    private void startPostBookshelfSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(GameObject.Find("Bookshelf"));
        startOnboardSpeech(4);
    }


    private void demonstrateSwipeToMines(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForSwipe(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!swipedLeftPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForSwipe(timeSinceLastHint + Time.deltaTime));
            }
            else if(!swipedLeftPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayMineSwipeHint();
            }
            else if(swipedLeftPrevFrame){
                UI_Controller.instance.DisableUIElement(GameObject.Find("Left_Swipe_Arrow"));
                startMineExplanation();
                swipedLeftPrevFrame = false;
            }
        }

        void displayMineSwipeHint(){
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(5);
        }



        UI_Controller.instance.EnableUIElement(GameObject.Find("Left_Swipe_Arrow"));
        Touch_Detection.instance.enableSwipes(Swipe.LEFTSWIPE, true);
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForSwipe(0f));
    }

    private void startMineExplanation(){
        IEnumerator _startMinecartSpeech(float waitSecs){
            yield return new WaitForSeconds(waitSecs);
            startOnboardSpeech(6);
        }

        Touch_Detection.instance.disableSwipes();
        Debug.Log("AT THE MINES BABBY");
        StartCoroutine(_startMinecartSpeech(.5f));
    }

    private void highlightMineCart(){
        spriteHighlighter.AddHighlightedSprites(GameObject.Find("Minecart"));
        Invoke("startPostMinecartHighlightSpeech", 3f);
    }


    private void startPostMinecartHighlightSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(GameObject.Find("Minecart"));
        startOnboardSpeech(7);
    }




    private void demonstrateTapMinecart(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForTap(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!minecartTappedPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForTap(timeSinceLastHint + Time.deltaTime));
            }
            else if(!minecartTappedPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayMinecartTapHint();
            }
            else if(minecartTappedPrevFrame){
                // Disable Everything
                Debug.Log("OKAY");
                disableAllMineAreaColliders();
                startMineshaftExplanation();
                minecartTappedPrevFrame = false;
            }
        }

        void displayMinecartTapHint(){
            ///Disable Everything
            disableAllMineAreaColliders();
            startOnboardSpeech(8);
        }



        //UI_Controller.instance.EnableUIElement(GameObject.Find("Left_Swipe_Arrow"));
        //Touch_Detection.instance.enableSwipes(Swipe.LEFTSWIPE);
        //Debug.Log("WAITING FOR SWIPE");
        disableAllMineAreaColliders();
        if(minecartCollider == null){
            minecartCollider = GameObject.Find("Minecart").GetComponent<Collider2D>();
        }
        minecartCollider.enabled = true;
        //Touch_Detection.instance.enableReticle(enableswipes:false);
        Touch_Detection.instance.enableReticleInstant();
        StartCoroutine(waitForTap(0f));
    }





    private void startMineshaftExplanation(){
        Debug.Log("IN HERE MOGGO");
        disableAllMineAreaColliders();
        Touch_Detection.instance.disableReticle(disableswipes:true);
        


        Invoke("givePreMineShaftSpeech", 0.25f);
    }


    private void givePreMineShaftSpeech(){
        startOnboardSpeech(9);
    }






    private void highlightMineshaft(){
        spriteHighlighter.AddHighlightedSprites(GameObject.Find("Mine_Shaft"));
        Invoke("startPostMineshaftSpeech", 3f);
    }

    private void startPostMineshaftSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(GameObject.Find("Mine_Shaft"));
        startOnboardSpeech(10);
    }



    private void highlightRobot(){
        spriteHighlighter.AddHighlightedSprites(new GameObject[] {GameObject.Find("Robot"), 
                                                GameObject.Find("Robot_Screen"), 
                                                GameObject.Find("Robot_Clothes"), 
                                                GameObject.Find("Robot_Rings")}
                                                );
        Invoke("startPostRobotSpeech", 3f);
    }

    private void startPostRobotSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(new GameObject[] {GameObject.Find("Robot"), 
                                        GameObject.Find("Robot_Screen"), 
                                        GameObject.Find("Robot_Clothes"), 
                                        GameObject.Find("Robot_Rings")}
                                        );
        startOnboardSpeech(11);
    }




    private void demonstrateSwipeFromMinesToBase(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForSwipe(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!swipedRightPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForSwipe(timeSinceLastHint + Time.deltaTime));
            }
            else if(!swipedRightPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayHint();
            }
            else if(swipedRightPrevFrame){
                UI_Controller.instance.DisableUIElement(GameObject.Find("Right_Swipe_Arrow"));
                startDogBaseSequence();
                swipedRightPrevFrame = false;
            }
        }

        void displayHint(){
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(12);
        }



        UI_Controller.instance.EnableUIElement(GameObject.Find("Right_Swipe_Arrow"));
        Touch_Detection.instance.enableSwipes(Swipe.RIGHTSWIPE, true);
        disableAllMineAreaColliders();
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForSwipe(0f));
    }


    private void startDogBaseSequence(){


        Touch_Detection.instance.disableReticle(disableswipes:true);
        enableAllMineAreaColliders();
        Invoke("introduceDog", .5f);
    }


    private void introduceDog(){
        startOnboardSpeech(13);
    }





    private void demonstrateSwipeToRocket(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForSwipe(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!swipedRightPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForSwipe(timeSinceLastHint + Time.deltaTime));
            }
            else if(!swipedRightPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayRocketSwipeHint();
            }
            else if(swipedRightPrevFrame){
                UI_Controller.instance.DisableUIElement(GameObject.Find("Right_Swipe_Arrow"));
                startRocketExplanation();
                swipedRightPrevFrame = false;
            }
        }

        void displayRocketSwipeHint(){
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(14);
        }



        UI_Controller.instance.EnableUIElement(GameObject.Find("Right_Swipe_Arrow"));
        Touch_Detection.instance.enableSwipes(Swipe.RIGHTSWIPE, true);
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForSwipe(0f));
    }



   private void startRocketExplanation(){
        Debug.Log("IN HERE MOGGORIFFIC");
        disableAllRocketAreaColliders();
        Touch_Detection.instance.disableReticle(disableswipes:true);
        
        Invoke("givePreRocketSpeech", 0.25f);
    }


    private void givePreRocketSpeech(){
        startOnboardSpeech(15);
    }





    private void highlightLab(){
        spriteHighlighter.AddHighlightedSprites(new GameObject[] {GameObject.Find("Rocket_Building"), 
                                                                GameObject.Find("Rocket_Building_Logo")}
                                                );
        Invoke("startPostLabSpeech", 3f);
    }

    private void startPostLabSpeech(){
        spriteHighlighter.RemoveHighlightedSprites(new GameObject[] {GameObject.Find("Rocket_Building"), 
                                                                GameObject.Find("Rocket_Building_Logo")}
                                                );
        startOnboardSpeech(16);
    }



    private void highlightButton(){
        spriteHighlighter.AddHighlightedSprites(new GameObject[] {GameObject.Find("Rocket_Button"), 
                                                                GameObject.Find("Launch_Cover")}
                                                );
        GameObject.Find("Rocket_Button").GetComponent<Launch_Button_Controller>().disable();
        disableAllRocketAreaColliders();
        Invoke("demonstrateOpenLid", 3f);
    }




    private void demonstrateOpenLid(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForOpen(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!openedLidPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForOpen(timeSinceLastHint + Time.deltaTime));
            }
            else if(!openedLidPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayRocketLidOpenHint();
            }
            else if(openedLidPrevFrame){
                GameObject.Find("Rocket_Launch_Cover_Swipe_Arrow_Up").GetComponent<SpriteRenderer>().enabled = false;
                startBackToBaseSpeech();
                openedLidPrevFrame = false;
            }
        }

        try{
            spriteHighlighter.RemoveHighlightedSprites(new GameObject[] {GameObject.Find("Rocket_Button"), 
                                                        GameObject.Find("Launch_Cover")}
                                                    );
        }
        catch(System.Exception e){}


        if(rocketButtonCollider == null){
            rocketButtonCollider = GameObject.Find("Rocket_Button").GetComponent<Collider2D>();
        }
        rocketButtonCollider.enabled = true;

        void displayRocketLidOpenHint(){
            disableAllRocketAreaColliders();
            Touch_Detection.instance.disableReticle();
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(17);
        }


        GameObject.Find("Rocket_Launch_Cover_Swipe_Arrow_Up").GetComponent<SpriteRenderer>().enabled = true;
        Touch_Detection.instance.enableReticle(immediately:true);
        Touch_Detection.instance.disableSwipes();
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForOpen(0f));
    }




    private void startBackToBaseSpeech(){
        IEnumerator _waitThenTalk(float waittime){
            yield return new WaitForSeconds(waittime);
            startOnboardSpeech(18);
        }
        disableAllRocketAreaColliders();
        StartCoroutine(_waitThenTalk(0.25f));
    }


    private void demonstrateCloseLid(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForClose(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!closedLidPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForClose(timeSinceLastHint + Time.deltaTime));
            }
            else if(!closedLidPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayRocketLidCloseHint();
            }
            else if(closedLidPrevFrame){
                GameObject.Find("Rocket_Launch_Cover_Swipe_Arrow_Down").GetComponent<SpriteRenderer>().enabled = false;
                demonstrateSwipeFromRocketToBase();
                closedLidPrevFrame = false;
            }
        }


        if(rocketButtonCollider == null){
            rocketButtonCollider = GameObject.Find("Rocket_Button").GetComponent<Collider2D>();
        }
        rocketButtonCollider.enabled = true;

        void displayRocketLidCloseHint(){
            Touch_Detection.instance.disableReticle();
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(19);
        }



        GameObject.Find("Rocket_Launch_Cover_Swipe_Arrow_Down").GetComponent<SpriteRenderer>().enabled = true;
        Touch_Detection.instance.enableReticle(immediately:true);
        Touch_Detection.instance.disableSwipes();
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForClose(0f));
    }


    private void demonstrateSwipeFromRocketToBase(){
        
        float hintWaitTime = 10.0f;
        IEnumerator waitForSwipe(float timeSinceLastHint){
            yield return new WaitForSeconds(0);
            if(!swipedLeftPrevFrame && !(timeSinceLastHint >= hintWaitTime)){
                StartCoroutine(waitForSwipe(timeSinceLastHint + Time.deltaTime));
            }
            else if(!swipedLeftPrevFrame && (timeSinceLastHint >= hintWaitTime)){
                displayHint();
            }
            else if(swipedLeftPrevFrame){
                UI_Controller.instance.DisableUIElement(GameObject.Find("Left_Swipe_Arrow"));
                startFinalSpeechSequence();
                swipedLeftPrevFrame = false;
            }
        }

        void displayHint(){
            Touch_Detection.instance.disableSwipes();
            startOnboardSpeech(20);
        }



        UI_Controller.instance.EnableUIElement(GameObject.Find("Left_Swipe_Arrow"));
        Touch_Detection.instance.enableSwipes(Swipe.LEFTSWIPE, true);
        disableAllRocketAreaColliders();
        //Debug.Log("WAITING FOR SWIPE");
        StartCoroutine(waitForSwipe(0f));
    }



    private void startFinalSpeechSequence(){
        Touch_Detection.instance.disableSwipes();
        startOnboardSpeech(21);
    }



    private void setUpPlayFabAccount(){

        Game_Manager.instance.userDisplayName = displayName;
        Game_Manager.instance.coinName = coinName;


        loggedIn = false;
        loginFailed = false;
        displayNameSet = false;
        setDisplayNameFailed = false;
        StartCoroutine(_setUpPlayFab(startGame));

    }

    IEnumerator _setUpPlayFab(System.Action callBack){
        PlayFab_Manager.instance.Login();
        
        
        while(!(loggedIn || loginFailed)){
            yield return new WaitForSeconds(0f);
        }
        if(loginFailed){
            yield return new WaitForSeconds(5f);
            setUpPlayFabAccount();
        }

        PlayFab_Manager.instance.SetDisplayName(Game_Manager.instance.userDisplayName);
        

        while(!(displayNameSet || setDisplayNameFailed)){
            yield return new WaitForSeconds(0f);
        }
        if(setDisplayNameFailed){
            yield return new WaitForSeconds(5f);
            setUpPlayFabAccount();
        }
        
        callBack();
    }


    private void startGame(){
        enableAllMineAreaColliders();
        enableAllRocketAreaColliders();
        Touch_Detection.instance.enableReticle(immediately:true);
        Touch_Detection.instance.enableSwipes(immediately:true);
        Debug.Log("GAME STARTED!");
        Destroy(this);
    }




















    private void startOnboardSpeech(int onboardingEventSpeechNum){
        string keystring = "";
        if(onboardingEventSpeechNum == 1){
            keystring = "Events_Script.Onboarding.1.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:promptForCoinName);
        }
        else if(onboardingEventSpeechNum == 2){
            keystring = "Events_Script.Onboarding.2.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:startTour);
        }
        else if(onboardingEventSpeechNum == 3){
            keystring = "Events_Script.Onboarding.3.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightBookshelf);
        }
        else if(onboardingEventSpeechNum == 4){
            keystring = "Events_Script.Onboarding.4.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeToMines);
        }
        else if(onboardingEventSpeechNum == 5){
            keystring = "Events_Script.Onboarding.5.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeToMines);
        }
        else if(onboardingEventSpeechNum == 6){
            keystring = "Events_Script.Onboarding.6.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightMineCart);
        }
        else if(onboardingEventSpeechNum == 7){
            keystring = "Events_Script.Onboarding.7.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateTapMinecart);
        }
        else if(onboardingEventSpeechNum == 8){
            keystring = "Events_Script.Onboarding.8.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateTapMinecart);
        }
        else if(onboardingEventSpeechNum == 9){
            keystring = "Events_Script.Onboarding.9.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightMineshaft);
        }
        else if(onboardingEventSpeechNum == 10){
            keystring = "Events_Script.Onboarding.10.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightRobot);
        }
        else if(onboardingEventSpeechNum == 11){
            keystring = "Events_Script.Onboarding.11.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeFromMinesToBase);
        }
        else if(onboardingEventSpeechNum == 12){
            keystring = "Events_Script.Onboarding.12.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeFromMinesToBase);
        }
        else if(onboardingEventSpeechNum == 13){
            keystring = "Events_Script.Onboarding.13.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeToRocket);
        }
        else if(onboardingEventSpeechNum == 14){
            keystring = "Events_Script.Onboarding.14.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeToRocket);
        }
        else if(onboardingEventSpeechNum == 15){
            keystring = "Events_Script.Onboarding.15.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightLab);
        }
        else if(onboardingEventSpeechNum == 16){
            keystring = "Events_Script.Onboarding.16.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:highlightButton);
        }
        else if(onboardingEventSpeechNum == 17){
            keystring = "Events_Script.Onboarding.17.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateOpenLid);
        }
        else if(onboardingEventSpeechNum == 18){
            keystring = "Events_Script.Onboarding.18.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateCloseLid);
        }
        else if(onboardingEventSpeechNum == 19){
            keystring = "Events_Script.Onboarding.19.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateCloseLid);
        }
        else if(onboardingEventSpeechNum == 20){
            keystring = "Events_Script.Onboarding.20.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:demonstrateSwipeFromRocketToBase);
        }
        else if(onboardingEventSpeechNum == 21){
            keystring = "Events_Script.Onboarding.21.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:false, keyString:keystring, formatFunc:null), callBack:setUpPlayFabAccount);
        }
    }

    private void onNameSubmitButtonPressed(string enteredName){
        Debug.Log("YO YO YO YO");
        displayName = enteredName;
        displayNameValidated = false;
    }

    private void onCoinNameSubmitButtonPressed(string enteredName){
        Debug.Log("YO YO YO YO JOE");
        coinName = enteredName;
    }

    private void onSwipedLeft(){
        Debug.Log("LEFT!");
        swipedLeftPrevFrame = true;
    }

    private void onSwipedRight(){
        Debug.Log("RIGHT!");
        swipedRightPrevFrame = true;
    }

    private void onMinecartTapped(double coins){
        Debug.Log("MINECART TAPPED");
        minecartTappedPrevFrame = true;
    }

    private void onLidOpened(){
        Debug.Log("OPENED LID");
        openedLidPrevFrame = true;
    }

    private void onLidClosed(){
        Debug.Log("CLOSED LID");
        closedLidPrevFrame = true;
    }


    void onGetAccountInfoSuccess(){
        waitingForNameValidationResponse = false;
        displayNameValidated = true;
        nameIsUnique = false;
    }

    void onGetAccountInfoFailure(){
        waitingForNameValidationResponse = false;
        displayNameValidated = true;
        nameIsUnique = true;
    }


    private void onPlayFabLoginSuccess(PlayFab.ClientModels.LoginResult result){
        Debug.Log("Login Success OBM");
        onPlayFabLoginSuccess();
    }

    private void onPlayFabLoginSuccess(){
        Debug.Log("Login Success OBM");
        loggedIn = true;
    }

    private void onPlayFabLoginFailure(){
        Debug.Log("Login Fail OBM");
        loginFailed = true;
    }

    private void onPlayFabSetDisplayNameSuccess(PlayFab.ClientModels.UpdateUserTitleDisplayNameResult result){
        Debug.Log("DisplayName Success OBM");
        displayNameSet = true;
    }

    private void onPlayFabSetDisplayNameFailure(){
        Debug.Log("DisplayName Failure OBM");
        setDisplayNameFailed = true;
    }


    private void disableAllMineAreaColliders(){
        if(robotCollider == null){
            robotCollider = GameObject.Find("Robot").GetComponent<Collider2D>();
        }
        if(minecartCollider == null){
            minecartCollider = GameObject.Find("Minecart").GetComponent<Collider2D>();
        }
        if(mineshaftCollider == null){
            mineshaftCollider = GameObject.Find("Mine_Shaft").GetComponent<Collider2D>();
        }

        robotCollider.enabled = false;
        minecartCollider.enabled = false;
        mineshaftCollider.enabled = false;
    }

    private void enableAllMineAreaColliders(){
        if(robotCollider == null){
            robotCollider = GameObject.Find("Robot").GetComponent<Collider2D>();
        }
        if(minecartCollider == null){
            minecartCollider = GameObject.Find("Minecart").GetComponent<Collider2D>();
        }
        if(mineshaftCollider == null){
            mineshaftCollider = GameObject.Find("Mine_Shaft").GetComponent<Collider2D>();
        }


        robotCollider.enabled = true;
        minecartCollider.enabled = true;
        mineshaftCollider.enabled = true;
    }


    private void disableAllRocketAreaColliders(){
        Debug.Log("DISABLE COLLIDERS!");
        // Rocket Button
        // Rocket Building
        if(rocketButtonCollider == null){
            rocketButtonCollider = GameObject.Find("Rocket_Button").GetComponent<Collider2D>();
        }
        if(rocketBuildingCollider == null){
            rocketBuildingCollider = GameObject.Find("Rocket_Building").GetComponent<Collider2D>();
        }
    
        rocketButtonCollider.enabled = false;
        rocketBuildingCollider.enabled = false;
    }

    private void enableAllRocketAreaColliders(){
        if(rocketButtonCollider == null){
            rocketButtonCollider = GameObject.Find("Rocket_Button").GetComponent<Collider2D>();
        }
        if(rocketBuildingCollider == null){
            rocketBuildingCollider = GameObject.Find("Rocket_Building").GetComponent<Collider2D>();
        }

        rocketButtonCollider.enabled = true;
        rocketBuildingCollider.enabled = true;
    }
}
