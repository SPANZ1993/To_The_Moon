using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


using TMPro;

public class Rocket_Game_Manager : MonoBehaviour
{


    private GameObject rocket;
    private GameObject rocketParticles;

    [SerializeField]
    private int maxAutopilotGems;

    private float rocketOrigGameHeight; // Measured in game coordinates
    public float rocketAltitude; // Measured in "Altitude"
    public float rocketMaxAltitude;

    public int numGemsCollected = 0;
    private GameObject arrivingPlanet;
    //public double rocketAltitude;
    [SerializeField]
    public float targetAltitude;// The altitude at which the destination is reached
    public bool reachedTargetAltitude = false;

    public bool gameStarted {get; private set;}
    private bool firstFrameGameStarted = false; // Is it the first frame since the game started?


    public bool freePlayMode = false;
    private float gameTimer;


    bool rewardedAdLoaded = false;
    bool rewardedAdAttempted = false; // Have we either shown the rewarded ad, have it rejected by user, or failed to load/show the ad?


    private Space_Junk_Spawner spaceJunkSpawner;
    private Rocket_Control rocketControl;
    private bool startedRocketControlSpiral = false;

    Upgrades_Manager upgradesManager;
    Game_Scaler gameScaler;
    UI_Controller uiController;
    Game_Manager gameManager;

    // UI STUFF
    //private TextMeshProUGUI Altitude_Text;
    private TextMeshProUGUI Time_Text;


    private Background_Controller backgroundController;


    [SerializeField]
    private Camera cam;
    // [SerializeField]
    // private float camOrthOgraphicSize;



    public delegate void EndLaunchScene();
    public static event EndLaunchScene EndLaunchSceneInfo;


    public delegate void PauseLaunchScene(bool pause);
    public static event PauseLaunchScene PauseLaunchSceneInfo;




    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;


        rocket = GameObject.Find("Rocket");
        rocketParticles = GameObject.Find("Rocket_Particles");
        rocketControl = rocket.GetComponent<Rocket_Control>();
        arrivingPlanet = GameObject.Find("Arriving_Planet");

        upgradesManager = Upgrades_Manager.instance;
        //upgradesManager = GameObject.Find("Upgrades_Manager").GetComponent<Upgrades_Manager>();
        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();    
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        gameManager = Game_Manager.instance;
        //gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        // UI STUFF
        //Altitude_Text = GameObject.Find("Altitude_Text").GetComponent<TextMeshProUGUI>();
        //Time_Text = GameObject.Find("Time_Text").GetComponent<TextMeshProUGUI>();
        spaceJunkSpawner = GameObject.Find("Junk_Spawner").GetComponent<Space_Junk_Spawner>();

        backgroundController = GameObject.Find("Background_Manager").GetComponent<Background_Controller>();

        // cam.orthographicSize = camOrthographicSize;
        firstFrameGameStarted = true;

    }

    //private float curSize = -1f;
    // Update is called once per frame
    void Update()
    {
        gameStarted = true; // Remove.. set this to true once you gain control of the rocket

        if (firstFrameGameStarted && gameStarted){
            rocketOrigGameHeight = rocket.transform.position.y;
            firstFrameGameStarted = false;
            gameTimer = 0f;
        }
        else if (!firstFrameGameStarted && gameStarted){
            gameTimer += Time.deltaTime;
            //updateUI();
        }
        if (gameStarted){
            rocketAltitude = calculateAltitude(rocket);
            if ((rocketMaxAltitude == -1 || rocketAltitude > rocketMaxAltitude) && !startedRocketControlSpiral){
                rocketMaxAltitude = rocketAltitude;
            }
            //Debug.Log("REACHED TARGET ALTITUDE: " + reachedTargetAltitude);
            if(rocketAltitude >= targetAltitude && !freePlayMode){
                Progression_Manager.instance.setRecentlyCompletedLevelIdToCurLevel();
                reachedTargetAltitude = true;
                rocketControl.reachedTargetAltitude = true;
                if(!startedRocketControlSpiral){
                    endGameOnSuccess(arrivingPlanet);
                }
            }
        }
        
        if(spaceJunkSpawner.frameRate <= 20){
            backgroundController.disableFog();
        }


        // if(curSize < 0){
        //     curSize = cam.orthographicSize;
        // }
        // else{
        //     cam.orthographicSize = curSize;
        // }
        // curSize *= 1.0001f;
        // Debug.Log("CUR CAM SIZE: " + curSize);

    }



    void OnEnable()
    {
        Rocket_Control.AlertFuelEmptyInfo += onRocketFuelEmpty;
        Ads_Manager.RewardedAdCompletedInfo += onRewardedAdComplete;
        Ads_Manager.AdLoadingErrorInfo += onAdLoadingError;
        Ads_Manager.AdLoadingSuccessInfo += onAdLoadingSuccess;
        Ads_Manager.AdShowErrorInfo += onAdShowError;
        Gem_Collection_Controller.GemCollectedInfo += onGemCollected;
        UI_Controller.AlertRewardedAdRejectedInfo += onRewardedAdRejected;
    }

    void OnDisable()
    {
        Rocket_Control.AlertFuelEmptyInfo -= onRocketFuelEmpty;
        Ads_Manager.RewardedAdCompletedInfo -= onRewardedAdComplete;
        Ads_Manager.AdLoadingErrorInfo -= onAdLoadingError;
        Ads_Manager.AdLoadingSuccessInfo -= onAdLoadingSuccess;
        Ads_Manager.AdShowErrorInfo -= onAdShowError;
        Gem_Collection_Controller.GemCollectedInfo -= onGemCollected;
        UI_Controller.AlertRewardedAdRejectedInfo -= onRewardedAdRejected;
    }





    // void updateUI(){
    //     //Altitude_Text.text = calculateAltitude(rocket).ToString();
    //     Time_Text.text = gameTimer.ToString();
    // }

    public float calculateGameYPos(float height){ // Calculate Game Y Position From Altitude (Inverse of CalculateAltitude)
        return (height/gameScaler.ScaleY) - rocketOrigGameHeight;
    }

    public float calcAlt(float ypos){
        float height = (ypos - rocketOrigGameHeight) * gameScaler.ScaleY;
        return height;
    }


    public float calculateAltitude(float ypos){
        
        float calculateAltitudeFromGameDistance(float gameDistanceFromOrigPos){
            return gameDistanceFromOrigPos * gameScaler.ScaleY;
        }

        float gameDistanceFromOrigPos = ypos - rocketOrigGameHeight;
        float height = calculateAltitudeFromGameDistance(gameDistanceFromOrigPos);
        return height;
    }

    public float calculateAltitude(GameObject g){
        return calculateAltitude(g.transform.position.y);
    }

    private void onGemCollected(){
        numGemsCollected++;
    }


    private void resumeAfterRewardedAd(){

    }

    private void endGameOnFailure(){
        // End this scene as if the rocket did not reach the destination
        uiController.rocketFlightDisableRewardedAdConfirmationBox();
        gameManager.metrics.updateFlights(numGemsCollected, false, rocketMaxAltitude);
        SendAlertEndScene();
    }

    private void endGameOnSuccess(GameObject destinationObject){

        void StartShipSpiral(){

            float targetDistance = .0000001f;
            float shipOrigDistanceFromDestination = Vector2.Distance(rocket.transform.position, destinationObject.transform.position);

            float rocketScaleX = rocket.transform.localScale.x;
            float rocketScaleY = rocket.transform.localScale.y;
            Vector2 rocketOrigScale = rocket.transform.localScale;
            Vector2 rocketParticlesOrigScale = rocketParticles.transform.localScale;


            IEnumerator ScaleRocketDuringSpiral(){
                yield return new WaitForSeconds(0);
                //Debug.Log("SCALING ROCKET T: " + (Vector2.Distance(rocket.transform.position, destinationObject.transform.position)-targetDistance)/(shipOrigDistanceFromDestination-targetDistance));
                //Debug.Log("SCALING ROCKET: " + Vector2.Lerp(, Vector2.zero, (Vector2.Distance(rocket.transform.position, destinationObject.transform.position)-targetDistance)/(shipOrigDistanceFromDestination-targetDistance)));
                if (!rocket.transform.localScale.Equals(Vector2.zero)){
                    rocket.transform.localScale = Vector2.Lerp(Vector2.zero, rocketOrigScale, (Vector2.Distance(rocket.transform.position, destinationObject.transform.position)-targetDistance)/(shipOrigDistanceFromDestination-targetDistance));
                }
                if (!rocketParticles.transform.localScale.Equals(Vector2.zero)){
                    rocketParticles.transform.localScale = Vector2.Lerp(Vector2.zero, rocketParticlesOrigScale, (Vector2.Distance(rocket.transform.position, destinationObject.transform.position)-targetDistance)/(shipOrigDistanceFromDestination-targetDistance));
                }


                if(!rocket.transform.localScale.Equals(Vector2.zero)){
                    StartCoroutine(ScaleRocketDuringSpiral());
                }
                else if (rocket.transform.localScale.Equals(Vector2.zero) || rocketParticles.transform.localScale.Equals(Vector2.zero)){
                    rocket.transform.localScale = Vector2.zero;
                    rocketParticles.transform.localScale = Vector2.zero;
                }
            }

            Spiral_Around.startSpiralAround(rocket, destinationObject, targetDistance, 180f, 0.1f, 1.5f, endGameOnFailure);
            StartCoroutine(ScaleRocketDuringSpiral());
        }


        // End this scene as if the rocket did reach the destination
        cam.GetComponent<Follow>().enabled = false;
        rocket.GetComponent<CapsuleCollider2D>().enabled = false;
        startedRocketControlSpiral = true;
        destinationObject.GetComponent<Background_Object_Parallax>().enabled = false;
        int rocketMoveId = LeanTween.move(rocket, Vector2.Lerp(rocket.transform.position, destinationObject.transform.position, .75f), 2f).setEase(LeanTweenType.easeInOutSine).id;
        LeanTween.descr(rocketMoveId).setOnComplete(StartShipSpiral);
    }


    private void onRewardedAdComplete(UnityAdsShowCompletionState showCompletionState){
        uiController.rocketFlightDisableRewardedAdConfirmationBox();
        rewardedAdAttempted = true;
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED){
            // Do Something A
            ContinueSceneAfterRewardedAd();
        }
        //else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED || showCompletionState == UnityAdsShowCompletionState.UNKNOWN){
        else{
            // Do Something B
            EndSceneAfterRewardedAd();
        }
    }

    private void ContinueSceneAfterRewardedAd(){
        // If we watched a rewarded ad, continue the game and refill the ship's fuel
        rocketControl.thrust = gameManager.thrust * .25;
        rocketControl.alertedFuelEmpty = false; // This will make sure the rocket alerts again when it is out of fuel
        
        // For some reason the listeners don't get this message if we don't delay it for a sec
        IEnumerator unpause(){
            yield return new WaitForSeconds(0.1f);
            SendAlertPause(false);
        }
        StartCoroutine(unpause());
    }

    private void EndSceneAfterRewardedAd(){
        IEnumerator dropRocket(){
            yield return new WaitForSeconds(0f);
            rocketControl.dropRocket();
        }

        SendAlertPause(false);
        StartCoroutine(dropRocket()); // Not sure why we need to do it like this but we do
        Invoke("endGameOnFailure", 3f);
    }

    
    private void onRewardedAdRejected(){
        uiController.rocketFlightDisableRewardedAdConfirmationBox();
        EndSceneAfterRewardedAd();
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
            EndSceneAfterRewardedAd(); // Just proceed as if the user said no to the ad
        }
    }


    private void onRocketFuelEmpty(){
        //Debug.Log("FUEL EMPTY FROM RGM");
        if(rewardedAdAttempted){
            rocketControl.dropRocket();
            Invoke("endGameOnFailure", 5f);
        }
        else{
            uiController.rocketFlightEnableRewardedAdConfirmationBox();
            SendAlertPause(true);
        }
    }



    private void SendAlertEndScene(){
        if (EndLaunchSceneInfo != null){
            EndLaunchSceneInfo();
        }
    }

    public void SendAlertPause(bool paused){
        // Have to do it in this order so we enable the spawners before
        // We unpause them and disable them after we pause them
        if(!paused){
            LeanTween.resumeAll();
            spaceJunkSpawner.enabled = true;
        }
        if (PauseLaunchSceneInfo != null){
            PauseLaunchSceneInfo(paused);
        }
        if(paused){
            LeanTween.pauseAll();
            spaceJunkSpawner.enabled = false;
        }

    }

    public void RunAutopilotSimulation(){

        if (upgradesManager == null){
            //upgradesManager = GameObject.Find("Upgrades_Manager").GetComponent<Upgrades_Manager>().instance;
            upgradesManager = Upgrades_Manager.instance;
        }



        if(Game_Manager.instance.metrics.numNonAutopilotFlights >= 10){
            upgradesManager.autopilotHeight = 100.0;
            try{
                upgradesManager.autopilotHeight = Game_Manager.instance.metrics.maxAltAllTime - MathUtils.RandRange(Game_Manager.instance.metrics.maxAltAllTime*0.25, Game_Manager.instance.metrics.maxAltAllTime*0.75);
            }
            catch(System.Exception e){
                Debug.Log("Error while setting autopilot height " + e);
                upgradesManager.autopilotHeight = 100.0;
            }
            if(upgradesManager.autopilotHeight >= Game_Manager.instance.metrics.maxAltAllTime*0.75){
                upgradesManager.autopilotHeight = Game_Manager.instance.metrics.maxAltAllTime*0.75;
            }
            else if(upgradesManager.autopilotHeight <= Game_Manager.instance.metrics.maxAltAllTime*0.25){
                upgradesManager.autopilotHeight = Game_Manager.instance.metrics.maxAltAllTime*0.25;
            }
            // int gemSeed = UnityEngine.Random.Range(0,10);
            // if (gemSeed < 5){
            //     upgradesManager.autopilotGems = 0;
            // }
            // else if (gemSeed >= 5 && gemSeed < 8){
            //     upgradesManager.autopilotGems = 1;
            // }
            // else if (gemSeed >= 8){
            //     upgradesManager.autopilotGems = 10;
            // }


            double n_gems = 1; 
            // for(int i=0; i<10; i++){
            //     Debug.Log("MEAN GEMS COLLECTED :" + (float)Game_Manager.instance.metrics.getMeanRocketGameNonAutopilotGemsCollected());
            //     Debug.Log("STD GEMS COLLECTED: " + (float)Game_Manager.instance.metrics.getStdRocketGameNonAutopilotGemsCollected());
            //     n_gems = System.Math.Abs(RandomFromDistribution.RandomNormalDistribution((float)Game_Manager.instance.metrics.getMeanRocketGameNonAutopilotGemsCollected(), (float)Game_Manager.instance.metrics.getStdRocketGameNonAutopilotGemsCollected()));
            //     Debug.Log("TESTING n_gems: " + n_gems);
            // }

            // Use this to scale the mean of our probability distribution... If we go higher, then we expect to find more gems, and vice, versa
            double ratioOfMaxHeightAllTime = (double)upgradesManager.autopilotHeight / Game_Manager.instance.metrics.maxAltAllTime;
            if(Game_Manager.instance.metrics.rocketGameMaxNonAutopilotGemsCollected >= 1){
                if((System.Convert.ToDouble(System.Single.MaxValue) >= Game_Manager.instance.metrics.getMeanRocketGameNonAutopilotGemsCollected()) && (System.Convert.ToDouble(System.Single.MaxValue) >= Game_Manager.instance.metrics.getStdRocketGameNonAutopilotGemsCollected())){
                    // We shouldn't encounter any overflows, proceed as normal
                    try{
                        // Don't want to award more gems than we ever have before
                        int tries = 0;
                        do{
                            n_gems = (int)System.Math.Abs(RandomFromDistribution.RandomNormalDistribution((float)Game_Manager.instance.metrics.getMeanRocketGameNonAutopilotGemsCollected(), (float)Game_Manager.instance.metrics.getStdRocketGameNonAutopilotGemsCollected()));
                            tries++;
                        }while(n_gems > Game_Manager.instance.metrics.rocketGameMaxNonAutopilotGemsCollected && tries < 50);
                        if(tries >= 50){
                            n_gems = Game_Manager.instance.metrics.rocketGameMaxNonAutopilotGemsCollected;
                        }

                        //Debug.Log("FINAL n_gems: " + n_gems + " rounds to --> " + (int)n_gems);
                    }
                    catch(System.Exception e){
                        // But just in case something goes screwy
                        Debug.Log("Error in Calculating Autopilot gems" + e);
                        n_gems = 0;
                    }
                }
                else{
                    // If Our values are too big then just award the max
                    n_gems = maxAutopilotGems;
                }
            }
            else{
                n_gems = 0;
                //Debug.Log("SETTING N GEMS TO ZERO  " + Game_Manager.instance.metrics.rocketGameMaxNonAutopilotGemsCollected);
            }



            if((int)n_gems >= maxAutopilotGems || (int)n_gems < 0){
                n_gems = maxAutopilotGems;
            }

            //Debug.Log("FINAL N GEMS: " + (int)n_gems);
            upgradesManager.autopilotGems = (int)n_gems;
        }
        // If we haven't had enough non-auto flights yet
        else{
            upgradesManager.autopilotHeight = MathUtils.RandRange(80, 120);
            upgradesManager.autopilotGems = 0;
        }






        upgradesManager.autopilotReturnState = AutopilotReturnState.Normal; // Could switch this up if we need to
        //Debug.Log("RUNNING AUTOPILOT SIMULATION " + upgradesManager.autopilotHeight + " " + upgradesManager.autopilotGems + " " + upgradesManager.autopilotReturnState);
        if (gameManager == null){
            gameManager = Game_Manager.instance;
        }

        gameManager.gems += (double)upgradesManager.autopilotGems;
        //gameManager.Handle_Autopilot_Return();
    }


}
