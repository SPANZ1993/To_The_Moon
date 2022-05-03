using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System;
using System.IO;
using System.Linq;

using UnityEngine.SceneManagement;

using TMPro; // REMOVE


public class Game_Manager : MonoBehaviour
{


    // Stuff Relating to time in the game
    private DateTime localSessionStartTime; // The system time that this session started at
    private DateTime serverSessionStartTime; // The server time that this session started at

    private double localSessionStartTimeUnix;
    private double serverSessionStartTimeUnix;
    private DateTime localSessionCurrentTime;
    private DateTime localSessionPrevFrameTime;

    private double frameCount = 0.0;

    [SerializeField]
    private float getServerTimeWait = 1.0f; // How long do we wait before retrying the time server?
    [SerializeField]
    private int getServerTimeRetries = 3; // How many times do we retry the time server before giving up?
    [SerializeField]
    private string timeRequestUrl = "localhost:8000"; // Address to send time request
    private bool currentlyTryingServerTime = false;
    private bool doneTryingServerTime = false;
    private bool gotServerTime = false;

    public bool initializeGameOnReturnToMainArea = false; // If this is true, when we enter the main area scene, try to query playfab to reinitialize the game

    
    private Upgrades_Manager upgradesManager;


    // Stuff Related to Saved Game
    private bool doneCheckingForSave = false; // Only says we have checked if a save exists or not, not that we are done loading it completely
    private bool saveExists = false;
    //private bool saveLoaded = false;
    private bool currentlyLoadingSave = false;
    [SerializeField]
    private float loadSaveWait = 1.0f;
    [SerializeField]
    private int loadSaveRetries = 3;
    public SaveGameObject loadedGame;


    private bool initializedGame = false; // Is the game initiated?
    private bool initializeGameStarted = false; // Have we called the function that initializes the game?



    [SerializeField]
    public bool offLineMode { get; private set; } // Is the user playing in offline mode?
    private bool prevOfflineMode = false; // Was the last user session playing in offline mode?
    public double gameTimeUnix {set; get;} // The game time... may be set via server or local depending on offline mode
    public double gameStartTimeUnix {set; get;}

    // Game Info
    public double coins = 0.0;
    public double gems = 0.0;
    public int maxLaunches = 3; // How many launches would we have if they were full
    public int remainingLaunches = 3;
    public double thrust = 100.0;




    //MineCart Info
    public double mineCartLastEmptiedTimeUnix;
    public double mineCartCurCoins;
    public double mineCartCoinsCapacity;
    public double mineCartCoinsPerSecond;
    public double mineCartCoinsPerSecondUpgradePrice;
    public double mineCartCoinsCapacityUpgradePrice;


    //Mine Shaft Info
    public double mineGameLastPlayedUnix;
    public double mineGameRefreshTime { get; private set; }  // Seconds until mine game is updated

    //Mine Game Info
    public double mineGameHitCoins; // How many coins awarded per hit in the mine game
    public double mineGameSolveCoins; // How many coins awarded per block solve in the mine game
    public double mineGameHitCoinsUpgradePrice; // How much will the next upgrade cost to the mineGameHitCoins
    public double mineGameSolveCoinsUpgradePrice; // How much will the next upgrade cost to the minGameSolveCoins
        

    //Launch Info
    public double prevLaunchTimeUnix;
    [SerializeField]
    public double launchRefreshTime { get; private set; } // Seconds until launch is updated


    //Research & Researcher Info
    public List<int> unlockedResearchIds;
    public List<int> unlockedResearcherIds;
    public List<ResearchAssignmentObject> assignedResearchers;

    //Experiment Info
    public List<int> unlockedExperimentIds;


    // Serialization
    public bool doneLoading = false; // Are we done loading the player save, getting server time, and player preferences (This will eventually determine whether loading screen shows or not)    
    public bool currentlySerializing {get; private set;}
    private bool serializingPrevFrame = false;
    private GameObject Save_Indicator;

    // References to game element controllers
    private ISerialization_Manager serializationManager;
    private Minecart_Manager minecartManager;
    private Robot_Manager robotManager;
    private Mine_Shaft_Controller mineShaftController;
    private Research_Manager researchManager;
    private Researcher_Manager researcherManager;
    private Touch_Detection touchDetection;


    private Ads_Manager adsManager;
    private Scene_Manager sceneManager;


    public static Game_Manager instance;
    public static int instanceID;

    void Awake()
    {

        if (!instance){

            Application.targetFrameRate = 120;

            mineGameRefreshTime = 3600.0;
            launchRefreshTime = 1800.0;
            offLineMode = false;

            instance = this;
            instanceID = gameObject.GetInstanceID();
            DontDestroyOnLoad(this.gameObject);
            
            currentlySerializing = false;
            localSessionStartTime = DateTime.Now;
            localSessionStartTimeUnix = (double)((DateTimeOffset)localSessionStartTime).ToUnixTimeSeconds();
            localSessionCurrentTime = localSessionStartTime;
            localSessionPrevFrameTime = localSessionStartTime;
            StartCoroutine(GetServerTime(timeRequestUrl, getServerTimeRetries));
        }
        else{
            Destroy(this.gameObject);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnLevelWasLoaded(){
        if (instanceID == gameObject.GetInstanceID()){
            sceneManager = GameObject.Find("Scene_Manager").GetComponent<Scene_Manager>();
            if (sceneManager.scene_name == "Main_Area"){
                touchDetection = GameObject.Find("Input_Detector").GetComponent<Touch_Detection>();
                serializationManager = GameObject.Find("Serialization_Manager").GetComponent<ISerialization_Manager>();
                upgradesManager = GameObject.Find("Upgrades_Manager").GetComponent<Upgrades_Manager>();
                minecartManager = GameObject.Find("Minecart_Manager").GetComponent<Minecart_Manager>();
                robotManager = GameObject.Find("Robot_Manager").GetComponent<Robot_Manager>();
                mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();
                sceneManager = GameObject.Find("Scene_Manager").GetComponent<Scene_Manager>();
                adsManager = GameObject.Find("Ads_Manager").GetComponent<Ads_Manager>();
                researchManager = GameObject.Find("Research_Manager").GetComponent<Research_Manager>();
                researcherManager = GameObject.Find("Researcher_Manager").GetComponent<Researcher_Manager>();
                Save_Indicator = GameObject.Find("Save_Indicator");
                Save_Indicator.GetComponent<SpriteRenderer>().enabled = false;
                //Debug.Log("HELLO FROM START: " + serverSessionStartTime);

                minecartManager = GameObject.Find("Minecart_Manager").GetComponent<Minecart_Manager>();
                robotManager = GameObject.Find("Robot_Manager").GetComponent<Robot_Manager>();
                mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();
                //Debug.Log("PREV SCENE: " + sceneManager.prev_scene_name);


                
                if(initializeGameOnReturnToMainArea){
                    PlayFab_Initializer playFabInitializer = gameObject.AddComponent<PlayFab_Initializer>();
                    playFabInitializer.callBack = initializeGame;
                    initializeGameOnReturnToMainArea = false;
                }
                else{
                    initializeMineShaft(mineGameLastPlayedUnix, mineGameRefreshTime);
                }
                // if (sceneManager.prev_scene_name != "Landing_Page"){
                //     StartCoroutine(_initializeMineCartLevelStart(mineCartCoinsCapacity, mineCartCoinsPerSecond, mineCartLastEmptiedTimeUnix, mineCartCurCoins));
                //     initializeMineShaft(mineGameLastPlayedUnix, mineGameRefreshTime);
                // }
                //initializeResearch(loadedGame.UnlockedResearchIds, loadedGame.UnlockedResearcherIds, loadedGame.AssignedResearchers, offLineMode: true);
                //getResearcherInfo();
                //initializeResearch(unlockedResearchIds , unlockedResearcherIds, assignedResearchers, offLineMode: false);

                

                // If we just finished an autopilot flight
                if(upgradesManager.autopilotHeight != null && upgradesManager.autopilotGems != null && upgradesManager.autopilotReturnState != null){
                    Debug.Log("Just finished an autopilot with " + upgradesManager.autopilotHeight + " height and found " + upgradesManager.autopilotGems + " gems and state " + upgradesManager.autopilotReturnState);
                    upgradesManager.autopilotHeight = null;
                    upgradesManager.autopilotGems = null;
                    upgradesManager.autopilotReturnState = null;
                }

            }
            else if (sceneManager.scene_name == "Rocket_Flight"){
                if (remainingLaunches == maxLaunches){
                    prevLaunchTimeUnix = gameTimeUnix;
                }
                remainingLaunches--;
                

                Rocket_Control rocketControl = GameObject.Find("Rocket").GetComponent<Rocket_Control>();
                rocketControl.thrust = thrust;
                rocketControl.thrustInitialized = true;
            }
            else if (sceneManager.scene_name == "Mine_Game"){
                
            }
        }
    }




    private bool poo = true; //REMOVE

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SERIALIZING?: " + Serializing);
        //Debug.Log("MINEGAME: " + mineGameHitCoins + " --- " + mineGameSolveCoins);
        
        gameTimeUnix += (double)Time.deltaTime;


        // If we don't already have the save data loaded, then do that.
        if (!doneLoading){
            //loadData();
        }
        // After we've loaded the save data
        else{
            if (!poo){
                Debug.Log("LOCAL START TIME: " + localSessionStartTime);
                Debug.Log("SERVER START TIME: " + serverSessionStartTime);
                Debug.Log("Server Start Time: " + serverSessionStartTimeUnix);
                Debug.Log("Local Start Time Unix: " + localSessionStartTimeUnix);
                poo = true;
            }
            if (!initializedGame && !initializeGameStarted && SceneManager.GetActiveScene().name != "Landing_Page"){
                // Debug.Log("TRYING TO INITIALIZE GAME");
                initializeGame();
            }
            // This is where the game is
            else{
                if (currentlySerializing && !serializingPrevFrame){
                    // Debug.Log("CS & SPF: " + currentlySerializing + " " + serializingPrevFrame);
                    //startSaveSequence();
                    serializingPrevFrame = true;
                }
                else if (!currentlySerializing && serializingPrevFrame){
                    endSaveSequence();
                    serializingPrevFrame = false; // We do this so we don't call endsavesequence() every frame
                }
                else{
                    updateRemainingLaunches();
                }
            }
        }
        //Debug.Log(gameTimeUnix);
        frameCount++;
        localSessionPrevFrameTime = localSessionCurrentTime;
        localSessionCurrentTime = DateTime.Now;
    }



    void OnEnable()
    {
        Scene_Manager.InitiateLaunchInfo += onLaunchInitiated;
        Minecart_Manager.MinecartTappedInfo += onMinecartTapped;
        UI_Controller.UIDisplayStartedInfo += onUIDisplayStarted;
        UI_Controller.UIDisplayEndedInfo += onUIDisplayEnded;
        
        UI_Controller.HashingUpgradeButtonPressedInfo += onHashingUpgradeButtonPressed;
        UI_Controller.BlockChainNetworkUpgradeButtonPressedInfo += onBlockChainNetworkUpgradeButtonPressed;
        UI_Controller.GraphicsCardUpgradeButtonPressedInfo += onGraphicsCardUpgradeButtonPressed;
        UI_Controller.ColdStorageUpgradeButtonPressedInfo += onColdStorageUpgradeButtonPressed;
        UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo += onResearchersMenuConfirmationBoxYesPressed;
        UI_Controller.ResearchersUpdatedInfo += onResearchersUpdated;
        UI_Controller.ExperimentsUpdatedInfo += onExperimentsUpdated;
        UI_Controller.ResearchFinishedInfo += onResearchFinished;

        Gem_Collection_Controller.GemCollectedInfo += onGemCollected;

        Mine_Game_Manager.AddMineGameCoinsInfo += onMineGameCoinsAdd;

        Mine_Game_Manager.EndMineSceneInfo += onEndMineScene;


        Research_Manager.ResearchersUpdatedInfo += onResearchersUpdated;


        PlayFab_Serializer.SerializationStartedInfo += onSerializationStarted;
        PlayFab_Serializer.SerializationEndedInfo += onSerializationEnded;

        PlayFab_Initializer.StartingPlayFabInitiationInfo += onStartingPlayFabInitiation;
        PlayFab_Initializer.EndingPlayFabInitiationInfo += onEndingPlayFabInitiation;
    }

    void OnDisable()
    {
        Scene_Manager.InitiateLaunchInfo -= onLaunchInitiated;
        Minecart_Manager.MinecartTappedInfo -= onMinecartTapped;
        UI_Controller.UIDisplayStartedInfo -= onUIDisplayStarted;
        UI_Controller.UIDisplayEndedInfo -= onUIDisplayEnded;

        UI_Controller.HashingUpgradeButtonPressedInfo -= onHashingUpgradeButtonPressed;
        UI_Controller.BlockChainNetworkUpgradeButtonPressedInfo -= onBlockChainNetworkUpgradeButtonPressed;
        UI_Controller.GraphicsCardUpgradeButtonPressedInfo -= onGraphicsCardUpgradeButtonPressed;
        UI_Controller.ColdStorageUpgradeButtonPressedInfo -= onColdStorageUpgradeButtonPressed;
        UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo -= onResearchersMenuConfirmationBoxYesPressed;
        UI_Controller.ResearchersUpdatedInfo -= onResearchersUpdated;
        UI_Controller.ExperimentsUpdatedInfo -= onExperimentsUpdated;
        UI_Controller.ResearchFinishedInfo -= onResearchFinished;

        Gem_Collection_Controller.GemCollectedInfo -= onGemCollected;

        Mine_Game_Manager.AddMineGameCoinsInfo -= onMineGameCoinsAdd;
        
        Mine_Game_Manager.EndMineSceneInfo -= onEndMineScene;


        Research_Manager.ResearchersUpdatedInfo -= onResearchersUpdated;


        PlayFab_Serializer.SerializationStartedInfo -= onSerializationStarted;
        PlayFab_Serializer.SerializationEndedInfo -= onSerializationEnded;

        PlayFab_Initializer.StartingPlayFabInitiationInfo -= onStartingPlayFabInitiation;
        PlayFab_Initializer.EndingPlayFabInitiationInfo -= onEndingPlayFabInitiation;
    }




    public void onApplicationFocus(){ // REMOVE
        OnApplicationFocus();
    }


    public void onApplicationPause(){ // REMOVE
        OnApplicationPause();
    }

    void OnApplicationFocus(){

        // Debug.Log("APP FOCUS");
        if(SceneManager.GetActiveScene().name == "Mine_Game" || SceneManager.GetActiveScene().name == "Rocket_Flight"){
            initializeGameOnReturnToMainArea = true;
        }
        if (frameCount != 0){
            DateTime curTime = DateTime.Now;
            double timeSinceLastFrame0 = (double)((DateTimeOffset)localSessionCurrentTime).ToUnixTimeSeconds() - (double)((DateTimeOffset)localSessionPrevFrameTime).ToUnixTimeSeconds();
            double timeSinceLastFrame1 = (double)((DateTimeOffset)curTime).ToUnixTimeSeconds() - (double)((DateTimeOffset)localSessionCurrentTime).ToUnixTimeSeconds();
            double timeSinceLastFrame2 = (double)((DateTimeOffset)curTime).ToUnixTimeSeconds() - (double)((DateTimeOffset)localSessionPrevFrameTime).ToUnixTimeSeconds();
            List<double> ltmp = new List<double>(new double[3] {timeSinceLastFrame0, timeSinceLastFrame1, timeSinceLastFrame2});
            double timeSinceLastFrameI = ltmp.IndexOf(ltmp.Max());
            double timeSinceLastFrame = ltmp.Max();
            if (timeSinceLastFrame >= 2.0){
                gameTimeUnix += timeSinceLastFrame;
            }
            //initializeGame(); //?
            PlayFab_Initializer playFabInitializer = gameObject.AddComponent<PlayFab_Initializer>();
            playFabInitializer.callBack = initializeGame;
            GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text + "\nFocused Added " + timeSinceLastFrame + " Seconds " + timeSinceLastFrameI + " --- " + DateTime.Now;
        }
        else if(SceneManager.GetActiveScene().name == "Main_Area"){
            GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text + "\nFocused FC0 --- " + DateTime.Now;
        }
    }

    void OnApplicationPause(){
        if (frameCount != 0){
            // Debug.Log("WOOP: Application Paused --- " + DateTime.Now);
            //GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text + "\nPaused --- " + DateTime.Now;
            saveData(disableTouch: false, displayIndicator: false, serially: true);
        }
        else if(SceneManager.GetActiveScene().name == "Main_Area"){
                GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text + "\nPaused FC0 --- " + DateTime.Now;
        }
    }

    void OnApplicationQuit(){
        // Debug.Log("WOOP: Application Quit Beginning --- " + DateTime.Now);
        saveData(disableTouch: false, displayIndicator: false, serially: true);
        try{
            GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text + "\nQuit --- " + DateTime.Now;
        }
        catch(Exception e){

        }
        //StartCoroutine(_OnApplicationQuit());
    }

    //IEnumerator _OnApplicationQuit(){
        //Debug.Log("WOOP: Application Quit Ending 1");
        //yield return new WaitForSeconds(4);
        //Debug.Log("WOOP: Application Quit Ending 2");
    //}






    private void initializeGame(){
        initializeGameStarted = true;
        prevOfflineMode = loadedGame.OffLineMode;
        
        // Debug.Log("SETTING COINS TO: " + coins);
        
        coins = loadedGame.Coins;
        gems = loadedGame.Gems;
        //Debug.Log("SETTING LAUNCHES REMAINING TO: " + loadedGame.LaunchesRemaining);
        remainingLaunches = loadedGame.LaunchesRemaining + Convert.ToInt32(Math.Max(Math.Floor((gameTimeUnix - loadedGame.LastLaunchTimeUnix)/launchRefreshTime), 0.0));
        if(remainingLaunches > maxLaunches){ // NOT SURE WHY IT'S DOING THIS BUT...
            remainingLaunches = maxLaunches;
        }

        mineGameHitCoins = loadedGame.MineGameHitCoins;
        mineGameSolveCoins = loadedGame.MineGameSolveCoins;

        if (prevOfflineMode || offLineMode){

            //Debug.Log("OFFLINE DAWG!");
            //Minecart
            initializeMineCart(loadedGame.CartCapacity, loadedGame.CartCoinsPerSecond, minecartManager.spoofLastEmptiedTime(loadedGame.CartCurCoins), loadedGame.CartCurCoins);
            //
            //Mine Shaft
            mineGameLastPlayedUnix = mineShaftController.spoofLastMineGamePlayedTime();
            initializeMineShaft(mineGameLastPlayedUnix, mineGameRefreshTime);
            //
            // Rocket
            prevLaunchTimeUnix = gameTimeUnix;
            //
            // Research
            initializeResearch(loadedGame.UnlockedResearchIds, loadedGame.UnlockedResearcherIds, loadedGame.AssignedResearchers, offLineMode: true);
            //
        }
        else{
            //Debug.Log("SETTING CART EMPTY TIME TO: " +  loadedGame.CartLastEmptiedTimeUnix);
            //Minecart
            initializeMineCart(loadedGame.CartCapacity, loadedGame.CartCoinsPerSecond, loadedGame.CartLastEmptiedTimeUnix, loadedGame.CartCurCoins);
            // 
            //Mine Shaft
            //Debug.Log("MINEGAMELASTPLAYED: " + loadedGame.MineGameLastPlayedUnix);
            //Debug.Log("REFRESH TIME: " + mineGameRefreshTime);
            mineGameLastPlayedUnix = loadedGame.MineGameLastPlayedUnix;
            initializeMineShaft(mineGameLastPlayedUnix, mineGameRefreshTime);
            //
            // Rocket
            prevLaunchTimeUnix = loadedGame.LastLaunchTimeUnix;
            //
            // Research
            initializeResearch(loadedGame.UnlockedResearchIds, loadedGame.UnlockedResearcherIds, loadedGame.AssignedResearchers, offLineMode: false);
            //
        }

        mineGameHitCoinsUpgradePrice = loadedGame.MineGameHitCoinsUpgradePrice;
        mineGameSolveCoinsUpgradePrice = loadedGame.MineGameSolveCoinsUpgradePrice;
        mineCartCoinsPerSecondUpgradePrice = loadedGame.CartCoinsPerSecondUpgradePrice;
        mineCartCoinsCapacityUpgradePrice = loadedGame.CartCoinsCapacityUpgradePrice;

        getMineCartInfo();

        initializedGame = true;
    }



    // //#######################################################################################################################################
    // public void initializeGameMidgame(){

    // }
    // //#######################################################################################################################################


    private void loadData(){

        // IEnumerator GetServerTime(){
        //     playFabManager.Ge
        // }
        // if (doneTryingServerTime){
        //     if (gotServerTime){
        //         gameTimeUnix = serverSessionStartTimeUnix;
        //     }
        //     else{
        //         // TODO: Prompt the user if they want to play in offline mode
        //         Debug.Log("PLAYING IN OFFLINE MODE");
        //         gameTimeUnix = localSessionStartTimeUnix;
        //         offLineMode = true;
        //     }
        //     gameStartTimeUnix = gameTimeUnix;
        //     //Debug.Log("HELLO FROM UPDATE: " + serverSessionStartTime);
        // }



        // If we aren't sure that we have a save yet then check for it
        if (!doneCheckingForSave){
            StartCoroutine(checkForSave());
        }
        else{
            if (saveExists && !currentlyLoadingSave){
                currentlyLoadingSave = true;
                StartCoroutine(loadSave(loadSaveRetries));
            }
            else if (loadedGame == null){
                loadedGame = new SaveGameObject(); // TODO: Say that this is a new game
                //Debug.Log("NO SAVED GAME FOUND... GENERATING NEW GAME");
            }

        }
        if (doneTryingServerTime && loadedGame != null){
            doneLoading = true;
            //Debug.Log("EVERYTHING IS LOADED!");
        }
    }




    public void saveData(bool disableTouch = true, bool displayIndicator = true, bool serially = false){ // If Serially, then use no 
        //Debug.Log("SAVING?");
        startSaveSequence(disableTouch, displayIndicator);
        getMineCartInfo();
        getResearcherInfo();
        getExperimentInfo();
        if (!serially){
            StartCoroutine(_saveGame());
        }
        else{
            serializationManager.saveGameDataSerially();
        }
    }

    IEnumerator _saveGame(){
        yield return new WaitForSeconds(0);
        if (!currentlySerializing){
            currentlySerializing = true;
            serializationManager.saveGameData();
        }
        else{
            StartCoroutine(_saveGame());
        }
    }


    private void startSaveSequence(bool disableTouch, bool displayIndicator){
        //Debug.Log("STARTING SAVING?");
        if(disableTouch){
            disableNonUITouch();
        }
        //Save_Indicator.active = true;
        //Save_Indicator.SetActive(true);
        if(displayIndicator){
            Save_Indicator.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


    private void endSaveSequence(){
        //Debug.Log("ENDING SAVING?");
        enableNonUITouch();
        //Save_Indicator.active = false;
        //Save_Indicator.SetActive(false);
        Save_Indicator.GetComponent<SpriteRenderer>().enabled = false;
    }


    IEnumerator checkForSave(){
        yield return new WaitForSeconds(0.0f);
        saveExists = serializationManager.checkForSavedData();
        doneCheckingForSave = true;
    }


    IEnumerator loadSave(int n_attempts){
        if (n_attempts > 0 && loadedGame==null){

             // If this isn't our first attempt then wait
            if (n_attempts != loadSaveRetries){
                //Debug.Log("Start");
                yield return new WaitForSeconds(loadSaveWait);
                //Debug.Log("End");
            }
            try {
                loadedGame = serializationManager.loadSavedData();
                //Debug.Log("LOADED SAVE");
            }
            catch (FileNotFoundException e){
                Debug.LogError("FAILED TO LOAD SAVE... " + (n_attempts-1) + " MORE ATTEMPTS");
            }
            // SYSTEM'S FUCKED
            if (n_attempts == 1 && loadedGame == null){
                Debug.LogError("A SAVE FILE EXISTS BUT WE CAN'T LOAD IT");
                // TODO: This would be really bad if we made it here... what do? .. Just start a new game?
            }
        }
    }




    IEnumerator GetServerTime(string uri, int n_attempts)
    {
        currentlyTryingServerTime = true;
        if (n_attempts > 0 && !doneTryingServerTime){
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                    // If this isn't our first attempt then wait
                    if (n_attempts != getServerTimeRetries){
                        yield return new WaitForSeconds(getServerTimeWait);
                    }
                    
                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();

                    string[] pages = uri.Split('/');
                    int page = pages.Length - 1;

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                            Debug.LogError(pages[page] + ": Connection Error: " + webRequest.error);
                            StartCoroutine(GetServerTime(timeRequestUrl, n_attempts-1));
                            break;
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                            StartCoroutine(GetServerTime(timeRequestUrl, n_attempts-1));
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                            StartCoroutine(GetServerTime(timeRequestUrl, n_attempts-1));
                            break;
                        case UnityWebRequest.Result.Success:
                            // Debug.Log("GOT IT");
                            // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                            serverSessionStartTime = DateTime.Parse(webRequest.downloadHandler.text);
                            serverSessionStartTimeUnix = (double)((DateTimeOffset)serverSessionStartTime).ToUnixTimeSeconds();
                            currentlyTryingServerTime = false;
                            doneTryingServerTime = true;
                            gotServerTime = true;
                            break;
                    }
                // If we've tried as many times as possible, then we're done
                if (n_attempts == 1){
                    currentlyTryingServerTime = false;
                    doneTryingServerTime = true;

                    //TODO: Ask the user if they want to play in offline mode
                }
            }
        }
    }


    private void initializeMineShaft(double MineGameLastPlayedUnix, double MineGameRefreshTime){
        mineShaftController.mineGameLastPlayedUnix = MineGameLastPlayedUnix;
        mineShaftController.mineGameRefreshTime = MineGameRefreshTime;
        //Debug.Log("INITIALIZING MINESHAFT");
        mineShaftController.calculateNextGameTime();
        mineShaftController.initialized = true;
    }


    private void initializeMineCart(double coinsCapacity, double coinsPerSecond, double lastEmptiedTimeUnix, double prevCurCoins){
        //Debug.Log("MCT: " + minecartManager + ": " + gameTimeUnix);
        minecartManager.coinsCapacity = coinsCapacity;
        minecartManager.coinsPerSecond = coinsPerSecond;
        minecartManager.lastEmptiedTimeUnix = lastEmptiedTimeUnix;
        minecartManager.initializeCurCoins(prevCurCoins);
        minecartManager.calculateNextFullTime();
    }


    IEnumerator _initializeMineCartLevelStart(double coinsCapacity, double coinsPerSecond, double lastEmptiedTimeUnix, double prevCurCoins){
        // Does this a little too quick when loading into the main scene so wait for the cart to run its start method
        yield return new WaitForSeconds(0.0f);
        if (minecartManager.startComplete){
            initializeMineCart(coinsCapacity, coinsPerSecond, lastEmptiedTimeUnix, prevCurCoins);
        }
        else{
            StartCoroutine(_initializeMineCartLevelStart(coinsCapacity, coinsPerSecond, lastEmptiedTimeUnix, prevCurCoins));
        }
    }


    private void getMineCartInfo(){
        mineCartLastEmptiedTimeUnix = minecartManager.lastEmptiedTimeUnix;
        mineCartCurCoins = minecartManager.curCoins;
        mineCartCoinsCapacity =  minecartManager.coinsCapacity;
        mineCartCoinsPerSecond = minecartManager.coinsPerSecond;
    }
    

    private void updateRemainingLaunches(){
        //Debug.Log("HELLO WE'RE HERE ALSO: " + remainingLaunches);
        if (gameTimeUnix > prevLaunchTimeUnix + launchRefreshTime){
            remainingLaunches++;
            //prevLaunchTimeUnix += launchRefreshTime;
            prevLaunchTimeUnix = gameTimeUnix;
            //Debug.Log("REFRESHED!!!");
        }
    }






    private void initializeResearch(List<int> unlockedResearchIds, List<int> unlockedResearcherIds, List<ResearchAssignmentObject> researchAssignmentObjects = null, bool offLineMode = false){
        
        List<ResearchAssignmentObject> spoofResearchAssignmentObjects(List<ResearchAssignmentObject> researchAssignmentObjects, bool offLineMode){
            List<ResearchAssignmentObject> newResearchAssignmentObjects = new List<ResearchAssignmentObject>();

            foreach (ResearchAssignmentObject researchAssignmentObject in researchAssignmentObjects){
                if(!offLineMode){
                    researchAssignmentObject.TimeLeft = researchAssignmentObject.AssignedTime + researchAssignmentObject.TimeLeft - gameTimeUnix;
                }
                researchAssignmentObject.AssignedTime = gameTimeUnix;
                if(researchAssignmentObject.TimeLeft < 0.0){
                    researchAssignmentObject.TimeLeft = 0.0;
                }

                newResearchAssignmentObjects.Add(researchAssignmentObject);
            }

            return newResearchAssignmentObjects;
        }


        researchManager.setUnlockedResearchIds(unlockedResearchIds.ToArray());
        researcherManager.setUnlockedResearchersIds(unlockedResearcherIds.ToArray());
        if (researchAssignmentObjects != null && researchAssignmentObjects.Count > 0){
            researchAssignmentObjects = spoofResearchAssignmentObjects(researchAssignmentObjects, offLineMode);
            researchManager.initializeResearchWithResearchAssignmentObjects(researchAssignmentObjects);
        }
    }

    public void getResearcherInfo(){
        unlockedResearchIds = researchManager.getUnlockedResearchIds();
        unlockedResearcherIds = researcherManager.getUnlockedResearchersIds();
        assignedResearchers = researchManager.generateResearchAssignmentObjects();
    }


    private void getExperimentInfo(){
        //unlockedExperimentIds
    }













    private void onLaunchInitiated(){
        //remainingLaunches--;
        if (remainingLaunches > maxLaunches){
            remainingLaunches = maxLaunches;
        }
        Debug.Log("HELLO WE'RE HERE: " + remainingLaunches);
    }

    
    private void onMinecartTapped(double cartCoins){
        //Debug.Log("CART COINS: " + cartCoins);
        coins += cartCoins;
        getMineCartInfo();
    }



    private void onMineGameCoinsAdd(double mineGameCoins){
        coins += mineGameCoins;
        saveData(disableTouch: false, displayIndicator: false, serially: false);
    }

    
    private void onEndMineScene(){
        mineGameLastPlayedUnix = gameTimeUnix;
    }



    private void onUIDisplayStarted(Vector3[] boundingBox){
        if (boundingBox.Length == 0){
            disableNonUITouch();
        }
        else{
            disableNonUITouchBoundingBox(boundingBox);
        }
    }
    
    private void onUIDisplayEnded(Vector3[] boundingBox){
        if (boundingBox.Length == 0){
            enableNonUITouch();
        }
        else{
            enableNonUITouchBoundingBox(boundingBox);
        }
    }
    
    
    private void onResearchersUpdated(){
        getResearcherInfo();
    }

    private void onExperimentsUpdated(){
        getExperimentInfo();
    }


    private void onResearchFinished(double thrustReward){
        thrust += thrustReward;
    }

    //Turns off/on whether elements other than UI elements can be tapped on
    private void disableNonUITouch(){
        //Debug.Log("DisableRet Game Manager " + DateTime.Now);
        touchDetection.disableReticle();
    }

    private void enableNonUITouch(){
        //Debug.Log("EnableRet Game Manager " + DateTime.Now);
        touchDetection.enableReticle();
    }

    private void disableNonUITouchBoundingBox(Vector3[] boundingBox){
        touchDetection.disableReticleInBoundingBox(boundingBox);
    }

    private void enableNonUITouchBoundingBox(Vector3[] boundingBox){
        touchDetection.enableReticleInBoundingBox(boundingBox);
    }




    // UI Button Handlers
    private void onHashingUpgradeButtonPressed(){
        if (coins >= mineGameHitCoinsUpgradePrice){
            coins -= mineGameHitCoinsUpgradePrice;
            mineGameHitCoins = Progression_Multiplier_Generator.generateMineGameHitCoinsUpgradeValue(mineGameHitCoins);
            //Debug.Log("HASHING UPGRADE BUTTON PRESSED --- MINE GAME HIT COINS: " + mineGameHitCoins);
            mineGameHitCoinsUpgradePrice = Progression_Multiplier_Generator.generateMineGameHitCoinsUpgradePriceValue(mineGameHitCoinsUpgradePrice);
            //uiController.selectMineUpgrade();
            adsManager.showInterstitialAd();
        }
        else {
            // TODO: DISPLAY NOT ENOUGH COINS THINGY
            //Debug.Log("NOT ENOUGH COINS FOR THAT");
        }
    }

    private void onBlockChainNetworkUpgradeButtonPressed(){
        if (coins >= mineGameSolveCoinsUpgradePrice){
            coins -= mineGameSolveCoinsUpgradePrice;
            mineGameSolveCoins = Progression_Multiplier_Generator.generateMineGameSolveCoinsUpgradeValue(mineGameSolveCoins);
            mineGameSolveCoinsUpgradePrice = Progression_Multiplier_Generator.generateMineGameSolveCoinsUpgradePriceValue(mineGameSolveCoinsUpgradePrice);
            //Debug.Log("BLOCKCHAIN NETWORK UPGRADE BUTTON PRESSED --- MINE GAME SOLVE COINS: " + mineGameSolveCoins);
            //uiController.selectMineUpgrade();
            adsManager.showInterstitialAd();
        }
        else {
            // TODO: DISPLAY NOT ENOUGH COINS THINGY
            //Debug.Log("NOT ENOUGH COINS FOR THAT");
        }
    }

    private void onGraphicsCardUpgradeButtonPressed(){
        if (coins >= mineCartCoinsPerSecondUpgradePrice){
            coins -= mineCartCoinsPerSecondUpgradePrice;
            mineCartCoinsPerSecond = Progression_Multiplier_Generator.generateMineCartCoinsPerSecondUpgradeValue(mineCartCoinsPerSecond);
            //Debug.Log("GRAPHICS CARD UPGRADE BUTTON PRESSED --- MINE CART COINS PER SECOND: " + mineCartCoinsPerSecond);
            mineCartCoinsPerSecondUpgradePrice = Progression_Multiplier_Generator.generateMineCartCoinsPerSecondUpgradePriceValue(mineCartCoinsPerSecondUpgradePrice);
            minecartManager.coinsPerSecond = mineCartCoinsPerSecond;
            minecartManager.calculateNextFullTime();
            //uiController.selectCartUpgrade();
            adsManager.showInterstitialAd();
        }
        else{
            // TODO: DISPLAY NOT ENOUGH COINS THINGY
            //Debug.Log("NOT ENOUGH COINS FOR THAT");
        }
    }

    private void onColdStorageUpgradeButtonPressed(){
        if (coins >= mineCartCoinsCapacityUpgradePrice){
            coins -= mineCartCoinsCapacityUpgradePrice;
            mineCartCoinsCapacity = Progression_Multiplier_Generator.generateMineCartCoinsCapacityUpgradeValue(mineCartCoinsCapacity);
            //Debug.Log("COLD STORAGE UPGRADE BUTTON PRESSED --- MINE CART COINS CAPACITY: " + mineCartCoinsCapacity);
            mineCartCoinsCapacityUpgradePrice = Progression_Multiplier_Generator.generateMineCartCoinsCapacityUpgradePriceValue(mineCartCoinsCapacityUpgradePrice);
            minecartManager.coinsCapacity = mineCartCoinsCapacity;
            minecartManager.calculateNextFullTime();
            //uiController.selectCartUpgrade();
            adsManager.showInterstitialAd();
        }
        else{
            // TODO: DISPLAY NOT ENOUGH COINS THINGY
            //Debug.Log("NOT ENOUGH COINS FOR THAT");
        }
    }


    private void onResearchersMenuConfirmationBoxYesPressed(int researchID, int researcherID){
        // TODO: Deduct coins and do all that stuff
    }


    // End UI Button Handlers


    // Rocket Flight Event Handlers
    private void onGemCollected(){
        //Debug.Log("Gem Collected");
        gems++;
    }
    // End Rocket Flight Event Handlers


    // Serialization Event Handlers
    private void onSerializationStarted(){
        currentlySerializing = true;
    }
    private void onSerializationEnded(){
        currentlySerializing = false;
    }
    // End Serialization Event Handlers


    // Start PlayFab Game Initialization Event Handlers
    private void onStartingPlayFabInitiation(){
        string sceneName = SceneManager.GetActiveScene().name;
        switch(sceneName) 
        {
            case "Main_Area":
                //Debug.Log("DISABLING TOUCH");
                disableNonUITouch();
                break;
            case "Mine_Game":
                break;
            case "Rocket_Flight":
                break;
            default:
                break;
        }
    }

    private void onEndingPlayFabInitiation(){
        string sceneName = SceneManager.GetActiveScene().name;
        switch(sceneName) 
        {
            case "Main_Area":
                //Debug.Log("ENABLING TOUCH");
                enableNonUITouch();
                break;
            case "Mine_Game":
                break;
            case "Rocket_Flight":
                break;
            default:
                break;
        }
             
    }
    // End PlayFab Game Initializatin Event Handlers

}
