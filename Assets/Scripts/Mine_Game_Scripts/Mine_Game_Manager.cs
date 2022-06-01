using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

using System;
using System.Linq;
using TMPro;

//using LimitedQueue;

public class Mine_Game_Manager : MonoBehaviour
{

    public double score {get; private set;}
    public double timer {get; private set;}

    //[SerializeField]
    //private GameObject ScoreTextObj, StatusTextObj, TimerTextObj;
    //private TextMeshProUGUI ScoreText, TimerText;
    //private TextMeshProUGUI StatusText;

    [SerializeField]
    private GameObject Left_Tap_Button, Right_Tap_Button, Tap_Button;
    private Animator Left_Tap_Button_Anim, Right_Tap_Button_Anim, Tap_Button_Anim;

    [SerializeField]
    private GameObject CartSpawner, BlockSpawner;

    [SerializeField]
    private GameObject BlockCollector, CartCollector;

    [SerializeField]
    private GameObject curBlock;
    private GameObject prevBlock;
    [SerializeField]
    private GameObject curCart;
    private GameObject prevCart;
    [SerializeField]
    private double minTimeBetweenRobotSounds;
    [SerializeField]
    private float robotSoundLikelihood;
    [SerializeField]
    private string[] robotSoundStrings;
    private Sound[] robotSounds;
    private double lastRobotSoundTime;
    
    private Block_Controller curBlockController, prevBlockController;

    [SerializeField]
    private GameObject HitIndicator, RobotBase, RobotArms, RobotClothes, RobotScreen, Pickax, RobotRings;
    private SpriteRenderer RobotArmsRenderer, PickaxRenderer;
    private Animator HitIndicatorAnim, RobotArmsAnim, PickaxAnim, RobotScreenAnim;
    private Vector3 HitIndicatorWaypointLoc_0, HitIndicatorWaypointLoc_1, HitIndicatorWaypointLoc_2, HitIndicatorWaypointLoc_3;
    private Vector3[] HitIndicatorWaypointLocs;
    private Vector3[] RobotWaypointLocs;
    private int HitIndicatorSelection;


    private int curBlockTweenId, prevBlockTweenId, curCartTweenId, prevCartTweenId = -1;

    [SerializeField]
    private float cartMoveSeconds, sparkDisplaySeconds, robotHitDisplaySeconds, robotDisplayChangeSeconds;
    private float sparkDisplaySecondsRemaining = 0.0f, robotHitDisplaySecondsRemaining = 0.0f, robotDisplayChangeSecondsRemaining = 0.0f;

    private LimitedQueue<int> previousMathScreenIndices = new LimitedQueue<int>(8); //// THIS IS WHAT YOU ARE WORKING ON!!! WANT TO SAVE THE LAST 8 SCREENS SO WE
    // CAN CYCLE THROUGH THE RANDOM MATH SCREENS SOMEWHAT EVENLY


    private Vector3 activeCartLoc, activeBlockLoc, destroyCartLoc, destroyBlockLoc;
    private Vector3 RobotArmsOffset, RobotClothesOffset, RobotScreenOffset, RobotPickaxOffset, RobotRingsOffset;


    private bool currentlyHitting = false;
    private bool armsUp = true;

    private bool swingButtonHeld = false;


    enum PickaxState {Down = 0, Swing = 1, Up = 2};

    private bool gameActive = false;
    public bool gameOver {get; private set;}
    private bool rewardedAdComplete = false;
    private bool beganSceneTransition = false;


    public delegate void AddMineGameCoins(double coins);
    public static event AddMineGameCoins AddMineGameCoinsInfo;

    public delegate void EndMineScene();
    public static event EndMineScene EndMineSceneInfo;


    [SerializeField]
    private GameObject floatingText;
    private GameObject curFloatingText;


    [SerializeField]
    float floatingTextLifespan;
    [SerializeField]
    int floatingTextTurnabouts;


    // Floating Text Stuff
    private GameObject floatingTextExample;
    List<Color> shaderFacesColors = new List<Color>();
    string[] shaderFaces = new string[]{};


    //Ads stuff
    private bool rewardedAdLoaded = false;

    private Game_Manager gameManager;
    private Game_Scaler gameScaler;
    private UI_Info_Holder uiInfoHolder;
    private LeanTween LeanTween;

    private UI_Controller uiController;




    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();

        //ScoreText = ScoreTextObj.GetComponent<TextMeshProUGUI>();
        //StatusText = StatusTextObj.GetComponent<TextMeshProUGUI>();
        //TimerText = TimerTextObj.GetComponent<TextMeshProUGUI>();

        score = 0.0;
        timer = 15.0;
        gameOver = false;

        LeanTween = GetComponent<LeanTween>();
        
        activeCartLoc = curCart.transform.position;
        activeBlockLoc = curBlock.transform.position;
        destroyCartLoc = CartCollector.transform.position;
        destroyBlockLoc = BlockCollector.transform.position;

        curBlockController = curBlock.GetComponent<Block_Controller>();


        RobotArmsRenderer = RobotArms.GetComponent<SpriteRenderer>();
        PickaxRenderer = Pickax.GetComponent<SpriteRenderer>();

        lastRobotSoundTime = gameManager.gameTimeUnix;
        robotSounds = new Sound[robotSoundStrings.Length];
        for(int i=0; i<robotSoundStrings.Length; i++){
            robotSounds[i] = Audio_Manager.instance.GetSound(robotSoundStrings[i]);
        }

        // Floating Text Stuff
        uiInfoHolder = new UI_Info_Holder();
    
        floatingTextExample = GameObject.Find("Floating_Text_Example");

        MeshRenderer rend = floatingText.GetComponent<MeshRenderer>();
        Material textMaterial = rend.sharedMaterial;
        Shader textShader = textMaterial.shader;

        shaderFaces = uiInfoHolder.GetMaterialFaces(textShader);
        
        foreach (string shaderFace in shaderFaces){
            shaderFacesColors.Add(textMaterial.GetColor(shaderFace));
        }
    
        GameObject.Destroy(floatingTextExample);



        // HitIndicatorWaypointLocs = new Vector3[] {HitIndicatorWaypointLoc_0, HitIndicatorWaypointLoc_1, HitIndicatorWaypointLoc_2, HitIndicatorWaypointLoc_3};
        // RobotWaypointLocs = new Vector3[] {new Vector3(HitIndicatorWaypointLoc_0[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_0[2]), 
        //                                    new Vector3(HitIndicatorWaypointLoc_1[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_1[2]),
        //                                    new Vector3(HitIndicatorWaypointLoc_2[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_2[2]),
        //                                    new Vector3(HitIndicatorWaypointLoc_3[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_3[2])};

        // HitIndicatorSelection = (int)UnityEngine.Random.Range(0, 4);

        // HitIndicatorAnim = HitIndicator.GetComponent<Animator>();
        // RobotArmsAnim = RobotArms.GetComponent<Animator>();
        // PickaxAnim = Pickax.GetComponent<Animator>();
        // RobotScreenAnim = RobotScreen.GetComponent<Animator>();
        // Left_Tap_Button_Anim = Left_Tap_Button.GetComponent<Animator>();
        // Right_Tap_Button_Anim = Right_Tap_Button.GetComponent<Animator>();
        // Tap_Button_Anim = Tap_Button.GetComponent<Animator>();

        // RobotArmsOffset = RobotBase.transform.position - RobotArms.transform.position;
        // RobotClothesOffset = RobotBase.transform.position - RobotClothes.transform.position;
        // RobotScreenOffset = RobotBase.transform.position - RobotScreen.transform.position;
        // RobotPickaxOffset = RobotBase.transform.position - Pickax.transform.position;
        // RobotRingsOffset = RobotBase.transform.position - RobotRings.transform.position;


        // moveHitIndicator();
        // updateRobotArms();
        // moveRobot();
        StartCoroutine(_LateStart());
    }


    IEnumerator _LateStart(){
        yield return new WaitForSeconds(0); // Need to wait for the first block to be fully scaled before we initialize some things

        // HitIndicatorWaypointLoc_0 = curBlock.transform.GetChild(10).transform.position;
        // HitIndicatorWaypointLoc_1 =  curBlock.transform.GetChild(11).transform.position;
        // HitIndicatorWaypointLoc_2 = curBlock.transform.GetChild(12).transform.position;
        // HitIndicatorWaypointLoc_3 = curBlock.transform.GetChild(13).transform.position;

        HitIndicatorWaypointLoc_0 = curBlock.transform.GetChild(0).transform.position;
        HitIndicatorWaypointLoc_1 =  curBlock.transform.GetChild(1).transform.position;
        HitIndicatorWaypointLoc_2 = curBlock.transform.GetChild(2).transform.position;
        HitIndicatorWaypointLoc_3 = curBlock.transform.GetChild(3).transform.position;


        HitIndicatorWaypointLocs = new Vector3[] {HitIndicatorWaypointLoc_0, HitIndicatorWaypointLoc_1, HitIndicatorWaypointLoc_2, HitIndicatorWaypointLoc_3};
        RobotWaypointLocs = new Vector3[] {new Vector3(HitIndicatorWaypointLoc_0[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_0[2]), 
                                           new Vector3(HitIndicatorWaypointLoc_1[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_1[2]),
                                           new Vector3(HitIndicatorWaypointLoc_2[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_2[2]),
                                           new Vector3(HitIndicatorWaypointLoc_3[0], RobotBase.transform.position[1], HitIndicatorWaypointLoc_3[2])};

        HitIndicatorSelection = (int)UnityEngine.Random.Range(0, 4);

        HitIndicatorAnim = HitIndicator.GetComponent<Animator>();
        RobotArmsAnim = RobotArms.GetComponent<Animator>();
        PickaxAnim = Pickax.GetComponent<Animator>();
        RobotScreenAnim = RobotScreen.GetComponent<Animator>();
        Left_Tap_Button_Anim = Left_Tap_Button.GetComponent<Animator>();
        Right_Tap_Button_Anim = Right_Tap_Button.GetComponent<Animator>();
        Tap_Button_Anim = Tap_Button.GetComponent<Animator>();

        RobotArmsOffset = RobotBase.transform.position - RobotArms.transform.position;
        RobotClothesOffset = RobotBase.transform.position - RobotClothes.transform.position;
        RobotScreenOffset = RobotBase.transform.position - RobotScreen.transform.position;
        RobotPickaxOffset = RobotBase.transform.position - Pickax.transform.position;
        RobotRingsOffset = RobotBase.transform.position - RobotRings.transform.position;


        moveHitIndicator();
        updateRobotArms();
        moveRobot();
        gameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive){
            updateRobotArms();
            if (sparkDisplaySecondsRemaining > 0.0f){
                sparkDisplaySecondsRemaining -= Time.deltaTime;
            }
            else{
                HitIndicatorAnim.SetBool("Swing", false);
                if(swingButtonHeld){
                    updatePickax(PickaxState.Down);
                }
                else{
                    updatePickax(PickaxState.Up);
                }
            }

            // if (robotHitDisplaySecondsRemaining > 0.0f){
            //     robotHitDisplaySecondsRemaining -= Time.deltaTime;
            // }
            // else{
            //     updateRobotScreen(false);
            // }
            
            if (robotDisplayChangeSecondsRemaining > 0.0f){
                robotDisplayChangeSecondsRemaining -= Time.deltaTime;
            }
            else{
                updateRobotScreen(false);
            }



            if (timer > 0.0){
                timer -= Time.deltaTime;
                if (timer <= 0.0){
                    timer = 0.0;
                }
                //updateTimerText();
            }
            else{
                //updateStatusText("NICE JOB!\nYOU GOT " + Math.Round(score).ToString() + " POINTS!");
                gameActive = false;
                gameOver = true;
            }
        }
        else if (gameOver){
            if (!beganSceneTransition){

                armsUp = true;
                updateRobotArms();
                updateRobotScreen(false);
                updatePickax(PickaxState.Up);
                Left_Tap_Button.GetComponent<BoxCollider2D>().enabled = false;
                Right_Tap_Button.GetComponent<BoxCollider2D>().enabled = false;
                Tap_Button.GetComponent<CircleCollider2D>().enabled = false;
                
                if(rewardedAdLoaded){
                    waitForRewardedAdComplete();
                }
                else{
                    onRewardedAdRejected(); // Just operate as if the user said no but don't show the text box
                }
                // if(AddMineGameCoinsInfo != null){
                //     AddMineGameCoinsInfo(score);
                // }
                // // if (EndMineSceneInfo != null){ // REPLACE!!!
                // //     EndMineSceneInfo();
                // // }
                beganSceneTransition = true;
            }
        }
    }


    void waitForRewardedAdComplete(){
        uiController.mineGameEnableRewardedAdConfirmationBox(); // Display box
        //adsManager.showRewardedAd();
        StartCoroutine(_waitForRewardedAdComplete()); // Either wait for the user to decline, for the ad to be over, or an error showing the ad

    }

    IEnumerator _waitForRewardedAdComplete(){
        while(rewardedAdComplete == false){
            yield return new WaitForSeconds(0);
        }
    }


    void OnEnable()
    {
        Tap_Object_Controller.ObjectTappedStartInfo += onObjectTappedStart;
        Tap_Object_Controller.ObjectTappedInfo += onObjectTapped;
        Block_Controller.BlockSolvedInfo += onBlockSolved;
        Ads_Manager.RewardedAdCompletedInfo += onRewardedAdComplete;
        Ads_Manager.AdLoadingErrorInfo += onAdLoadingError;
        Ads_Manager.AdLoadingSuccessInfo += onAdLoadingSuccess;
        Ads_Manager.AdShowErrorInfo += onAdShowError;
        UI_Controller.AlertRewardedAdRejectedInfo += onRewardedAdRejected;
    }

    void OnDisable()
    {
        Tap_Object_Controller.ObjectTappedStartInfo -= onObjectTappedStart;
        Tap_Object_Controller.ObjectTappedInfo -= onObjectTapped;
        Block_Controller.BlockSolvedInfo -= onBlockSolved;
        Ads_Manager.RewardedAdCompletedInfo -= onRewardedAdComplete;
        Ads_Manager.AdLoadingErrorInfo -= onAdLoadingError;
        Ads_Manager.AdLoadingSuccessInfo -= onAdLoadingSuccess;
        Ads_Manager.AdShowErrorInfo -= onAdShowError;
        UI_Controller.AlertRewardedAdRejectedInfo -= onRewardedAdRejected;
    }



    //private void updateScoreText(){
        //ScoreText.text = Math.Round(score, 2).ToString();
    //}

    // private void updateStatusText(string updateString){
    //     StatusText.text = updateString;
    // }

    //private void updateTimerText(){
        //TimerText.text = Math.Round(timer).ToString();
    //}

    // private void onObjectTapped(string gameObjectName){
    //     if (gameObjectName == "Tap_Button" && timer > 0.0){
    //         score++;
    //         updateScoreText();
    //         updateStatusText("Tap!!! --- +1 Points!");
    //     }
    // }

    private void onBlockSolved(){
        if (timer > 0.0){
            //score += gameManager.mineGameSolveCoins;
            //updateScoreText();
            //updateStatusText("Block Solved!!! --- +" + gameManager.mineGameSolveCoins +  " Points!");
        }

        prevCart = curCart;
        prevBlock = curBlock;
        prevBlockController = prevBlock.GetComponent<Block_Controller>();
        curCart = CartSpawner.GetComponent<ListeningSpawner>().Spawn();
        curBlock = BlockSpawner.GetComponent<ListeningSpawner>().Spawn();
        curBlockController = curBlock.GetComponent<Block_Controller>();

        
        

        curBlockTweenId = LeanTween.move(curBlock, activeBlockLoc, cartMoveSeconds).setEase(LeanTweenType.easeInOutSine).id;
        prevBlockTweenId = LeanTween.move(prevBlock, destroyBlockLoc, cartMoveSeconds).setEase(LeanTweenType.easeInOutSine).id;
        curCartTweenId = LeanTween.move(curCart, activeCartLoc, cartMoveSeconds).setEase(LeanTweenType.easeInOutSine).id;
        prevCartTweenId = LeanTween.move(prevCart, destroyCartLoc, cartMoveSeconds).setEase(LeanTweenType.easeInOutSine).id;
        if(!Audio_Manager.instance.IsPlaying("Minecart_Earth")){ // TODO: Switch this depending on what level we are on
            Audio_Manager.instance.Play("Minecart_Earth");
        }
    }


    private void moveHitIndicator(){
        if (HitIndicatorSelection > 3){
            HitIndicatorSelection = 3;
        }
        HitIndicator.transform.position = HitIndicatorWaypointLocs[HitIndicatorSelection];
    }


    private void moveRobot(){
        if (HitIndicatorSelection > 3){
            HitIndicatorSelection = 3;
        }
        RobotBase.transform.position = RobotWaypointLocs[HitIndicatorSelection];
        RobotArms.transform.position = RobotWaypointLocs[HitIndicatorSelection] - RobotArmsOffset;
        RobotClothes.transform.position = RobotWaypointLocs[HitIndicatorSelection] - RobotClothesOffset;
        RobotScreen.transform.position = RobotWaypointLocs[HitIndicatorSelection] - RobotScreenOffset;
        Pickax.transform.position = RobotWaypointLocs[HitIndicatorSelection] - RobotPickaxOffset;
        RobotRings.transform.position = RobotWaypointLocs[HitIndicatorSelection] - RobotRingsOffset;
    }

    private void spinForRobotSound(){
        float spinNum = UnityEngine.Random.Range(0, 10);
        if (spinNum <= (robotSoundLikelihood/100f) && gameManager.gameTimeUnix-lastRobotSoundTime >= minTimeBetweenRobotSounds){
            if(!(robotSounds.Select(s => Audio_Manager.instance.IsPlaying(s)).ToArray().Contains(true))){
                int choice = UnityEngine.Random.Range((int)0, (int)robotSounds.Length);
                Audio_Manager.instance.Play(robotSounds[choice]);
            }
        }
    }


    private void updateRobotArms(){
        RobotArmsAnim.SetBool("ArmsUp", armsUp);
        // Make sure the hands render on the correct side of the pickax
        if(armsUp){
            RobotArmsRenderer.sortingOrder = 3;
            PickaxRenderer.sortingOrder = 4;
        }
        else{
            RobotArmsRenderer.sortingOrder = 4;
            PickaxRenderer.sortingOrder = 3;
        }
    }

    private void updateRobotScreen(bool hitting){
        if (hitting){
            RobotScreenAnim.SetBool("Hitting", true);
            robotDisplayChangeSecondsRemaining = robotHitDisplaySeconds;
        }
        else{
            RobotScreenAnim.SetBool("Hitting", false);
            int newScreeni = (int)UnityEngine.Random.Range(0, 13);
            while(previousMathScreenIndices.Contains(newScreeni)){
                newScreeni = (int)UnityEngine.Random.Range(0, 13);
            }
            previousMathScreenIndices.Enqueue(newScreeni);
            RobotScreenAnim.SetInteger("Math_Screen_Num", newScreeni);
            robotDisplayChangeSecondsRemaining = robotDisplayChangeSeconds;
        }
    }



    private void updatePickax(PickaxState state)
    {
        switch(state)
        {
            case PickaxState.Down:
            {
                PickaxAnim.SetBool("PickaxUp", false);
                PickaxAnim.SetBool("PickaxSwing", false);
                break;
            }
            case PickaxState.Swing:
            {
                PickaxAnim.SetBool("PickaxUp", false);
                PickaxAnim.SetBool("PickaxSwing", true);
                break;
            }
            case PickaxState.Up:
            {
                PickaxAnim.SetBool("PickaxUp", true);
                PickaxAnim.SetBool("PickaxSwing", false);
                break;
            }
            default: 
                break;
        }
    }

    private void onObjectTappedStart(string gameObjectName){
        if (gameObjectName == "Tap_Button"){
            swingButtonHeld = true;
            armsUp = false;
            updateRobotArms();
            updateRobotScreen(true);
            Tap_Button_Anim.SetBool("Tapped", true);
            if (!currentlyHitting){
                currentlyHitting = true;
                HitIndicatorAnim.SetBool("Swing", true);
                updatePickax(PickaxState.Swing);
                sparkDisplaySecondsRemaining = sparkDisplaySeconds;
                bool solved = curBlockController.Hit(HitIndicatorSelection);
                if(solved){
                    Audio_Manager.instance.Play("Block_Solved");
                }
                else{
                    Audio_Manager.instance.Play("Switch_Lights");
                }
                double curTapScore = solved ? gameManager.mineGameSolveCoins : gameManager.mineGameHitCoins;
                score += curTapScore;
                //updateScoreText();
                //updateStatusText("Tap!!! --- + " + curTapScore + " Points!");
                
                curFloatingText = Instantiate(floatingText, HitIndicator.transform.position, Quaternion.identity);
                initializeFloatingText(curFloatingText, curTapScore);
                Audio_Manager.instance.Play("Pickax_Hit");

                currentlyHitting = false;
            }
        }
        else if(gameObjectName == "Left_Tap_Button"){
            Left_Tap_Button_Anim.SetBool("Tapped", true);
            if (HitIndicatorSelection != 0){
                HitIndicatorSelection--;
                moveHitIndicator();
                moveRobot();
            }
        }
        else if(gameObjectName == "Right_Tap_Button"){
            Right_Tap_Button_Anim.SetBool("Tapped", true);
            if (HitIndicatorSelection <= 3){
                HitIndicatorSelection++;
                moveHitIndicator();
                moveRobot();
            }
        }
    }


    private void initializeFloatingText(GameObject floatingText, double points){

        Material textMaterial;
        Shader textShader;
        
        

        void tweenAlphaUpdate(GameObject floatingText, float alphaValue){
            MeshRenderer rend = floatingText.GetComponent<MeshRenderer>();
            textMaterial = rend.material;
            textShader = textMaterial.shader;

            
            LeanTween.value(floatingText, 1.0f, 0.0f, floatingTextLifespan).setEase(LeanTweenType.easeInOutSine).setOnUpdate(
                (value) =>
                {
                    Color curColor;
                    string curShaderFace;
                    for (int i=0; i<shaderFacesColors.Count; i++){
                        curColor = shaderFacesColors[i];
                        curShaderFace = shaderFaces[i];
                        textMaterial.SetColor(shaderFaces[i], new Color(shaderFacesColors[i].r, shaderFacesColors[i].b, shaderFacesColors[i].g, value));
                        //Debug.Log("TWEENED SOMETHING: " + shaderFaces[i] + " " + value);
                    }
                }
            );
        }




        float XDir = 0.2f * gameScaler.ScaleX;
        Vector3 startLoc = floatingText.transform.position;
        int x_tween_id = LeanTween.moveX(floatingText, startLoc.x + XDir, floatingTextLifespan/(float)floatingTextTurnabouts).setEase(LeanTweenType.easeInOutSine).id;
        int y_tween_id =  LeanTween.moveY(floatingText, startLoc.y + (2.0f * gameScaler.ScaleY), floatingTextLifespan).setEase(LeanTweenType.easeInOutSine).id;
        // int fade_tween_id = LeanTween.alpha(floatingText.GetComponent<RectTransform>(), 0.0f, floatingTextLifespan).setEase(LeanTweenType.easeInOutSine).id; Not working

        floatingText.GetComponent<TMPro.TextMeshPro>().text = "+" + Number_String_Formatter.mineGameFormatFloatingNumberText(points);

        tweenAlphaUpdate(floatingText, 0.5f);

        Destroy(floatingText, floatingTextLifespan);



        IEnumerator _initializeFloatingText(){
            yield return new WaitForSeconds(0);
            if(floatingText){
                if(LeanTween.isTweening(x_tween_id)){
                    StartCoroutine(_initializeFloatingText());
                }
                else{
                    //Debug.Log("TURNING");
                    XDir = -XDir;
                    x_tween_id = LeanTween.moveX(floatingText, floatingText.transform.position.x + XDir, floatingTextLifespan/(float)floatingTextTurnabouts).setEase(LeanTweenType.easeInOutSine).id;
                    StartCoroutine(_initializeFloatingText());
                }
            }
        }
        StartCoroutine(_initializeFloatingText());
    }






    private void onObjectTapped(string gameObjectName){
        if (gameActive && !gameOver){
            //Debug.Log("OBJECT TAPPED");
            if (gameObjectName == "Tap_Button"){
                swingButtonHeld = false;
                armsUp = true;
                spinForRobotSound();
                updateRobotArms();
                updatePickax(PickaxState.Up);
                Tap_Button_Anim.SetBool("Tapped", false);
            }
            else if(gameObjectName == "Left_Tap_Button"){
                Left_Tap_Button_Anim.SetBool("Tapped", false);
            }
            else if(gameObjectName == "Right_Tap_Button"){
                Right_Tap_Button_Anim.SetBool("Tapped", false);
            }
        }
    }


    private void onRewardedAdComplete(UnityAdsShowCompletionState showCompletionState){
        uiController.mineGameDisableRewardedAdConfirmationBox();
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED){
            if(AddMineGameCoinsInfo != null){
                AddMineGameCoinsInfo(score*2.0);
            }
        }
        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED || showCompletionState == UnityAdsShowCompletionState.UNKNOWN){
            if(AddMineGameCoinsInfo != null){
                AddMineGameCoinsInfo(score);
            }
        }
        if (EndMineSceneInfo != null){
            EndMineSceneInfo();
        }
    }

    
    private void onRewardedAdRejected(){
        uiController.mineGameDisableRewardedAdConfirmationBox();
        if(AddMineGameCoinsInfo != null){
            AddMineGameCoinsInfo(score);
        }
        if (EndMineSceneInfo != null){
            EndMineSceneInfo();
        }
    }


    private void onAdLoadingError(string adUnitId){

    }

    private void onAdLoadingSuccess(string adUnitId){
        //Debug.Log("AD LOADED SUCCESS: " + adUnitId);
        if(adUnitId.StartsWith("Rewarded")){
            rewardedAdLoaded = true;
        }
    }

    private void onAdShowError(string adUnitId){
        if(adUnitId.StartsWith("Rewarded")){
            onRewardedAdRejected(); // Just treat it as if they said no to rewarded ad
        }
    }

}
