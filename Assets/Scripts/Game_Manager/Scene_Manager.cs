 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{

    private Game_Manager gameManager;
    private Upgrades_Manager upgradesManager;
    private UI_Controller uiController;

    // Tiny Scientists
    private Tiny_Scientists_Manager tinyScientistsManager;

    // Launch Tower
    private Rocket_Tower_Manager rocketTowerManager;

    // Rocket
    private Rocket_Controller_Main_Area rocketController;
    public bool startedRocketSceneTransition {get; private set;}
    public delegate void InitiateLaunch();
    public static event InitiateLaunch InitiateLaunchInfo;

    //Mine Shaft
    public bool startedMineSceneTransition {get; private set;}
    private Mine_Shaft_Controller mineShaftController;



    public static Scene_Manager instance;


    public string scene_name;
    public string prev_scene_name;

    void OnEnable()
    {
        Landing_Page_Manager.EndLandingPageSceneInfo += onEndLandingPageScene;
        Launch_Button_Controller.InitiateLaunchInfo += onLaunchInitiated;
        Rocket_Game_Manager.EndLaunchSceneInfo += onEndLaunchScene;
        Mine_Game_Manager.EndMineSceneInfo += onEndMineScene;
        Mine_Shaft_Controller.MineShaftTappedInfo += onMineShaftTapped;
        Scene_Transition.LeavingSceneCompleteInfo += onLeavingSceneTransitionComplete;
        UI_Controller.AutopilotSelectedInfo += onAutopilotSelected;
    }

    void OnDisable()
    {
        Landing_Page_Manager.EndLandingPageSceneInfo -= onEndLandingPageScene;
        Launch_Button_Controller.InitiateLaunchInfo -= onLaunchInitiated;
        Rocket_Game_Manager.EndLaunchSceneInfo -= onEndLaunchScene;
        Mine_Game_Manager.EndMineSceneInfo -= onEndMineScene;
        Mine_Shaft_Controller.MineShaftTappedInfo -= onMineShaftTapped;
        Scene_Transition.LeavingSceneCompleteInfo -= onLeavingSceneTransitionComplete;
        UI_Controller.AutopilotSelectedInfo -= onAutopilotSelected;
    }


    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            scene_name = SceneManager.GetActiveScene().name;
            prev_scene_name = scene_name;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();

        startedRocketSceneTransition = false;
        startedMineSceneTransition = false;

        scene_name = SceneManager.GetActiveScene().name;
        if(scene_name == "Main_Area"){
            tinyScientistsManager = GameObject.Find("Tiny_Scientists_Manager").GetComponent<Tiny_Scientists_Manager>();
            rocketTowerManager = GameObject.Find("Rocket_Tower").GetComponent<Rocket_Tower_Manager>();
            rocketController = GameObject.Find("Rocket").GetComponent<Rocket_Controller_Main_Area>();
            mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();   
        }
    }

    void OnLevelWasLoaded(){
        IEnumerator setPrevScene(){
            yield return new WaitForSeconds(0);
            if (SceneManager.GetActiveScene().name != prev_scene_name){
                prev_scene_name = scene_name;
            }
        }
        if (this == instance){
            startedRocketSceneTransition = false;
            startedMineSceneTransition = false;
            scene_name = SceneManager.GetActiveScene().name;
            StartCoroutine(setPrevScene());
            if (scene_name == "Main_Area"){
                upgradesManager = GameObject.Find("Upgrades_Manager").GetComponent<Upgrades_Manager>();
                tinyScientistsManager = GameObject.Find("Tiny_Scientists_Manager").GetComponent<Tiny_Scientists_Manager>();
                rocketTowerManager = GameObject.Find("Rocket_Tower").GetComponent<Rocket_Tower_Manager>();
                rocketController = GameObject.Find("Rocket").GetComponent<Rocket_Controller_Main_Area>();
                mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();
            }
            else if(scene_name == "Rocket_Flight"){
                if(upgradesManager.autopilotFlag){
                    IEnumerator waitDuringAutopilot(Wipe OldWipe){
                        double timer = 0.0;
                        int oldWipeTweenIdEnter = OldWipe.enteringWipeTweenId;
                        int oldWipeTweenIdLeave = OldWipe.leavingWipeTweenId;
                        if (LeanTween.isTweening(oldWipeTweenIdEnter)){
                            //Debug.Log("CANCELING");
                            LeanTween.cancel(oldWipeTweenIdEnter);
                            Destroy(OldWipe);
                        }
                        if (LeanTween.isTweening(oldWipeTweenIdLeave)){
                            //Debug.Log("CANCELING");
                            LeanTween.cancel(oldWipeTweenIdLeave);
                            Destroy(OldWipe);
                        }
                        while(timer < .25){ // How long we display the black screen for the rocket game
                            timer += Time.deltaTime;
                            oldWipeTweenIdEnter = OldWipe.enteringWipeTweenId;
                            oldWipeTweenIdLeave = OldWipe.leavingWipeTweenId;
                            if (LeanTween.isTweening(oldWipeTweenIdEnter)){
                                //Debug.Log("CANCELING");
                                LeanTween.cancel(oldWipeTweenIdEnter);
                                Destroy(OldWipe);
                            }
                            if (LeanTween.isTweening(oldWipeTweenIdLeave)){
                                //Debug.Log("CANCELING");
                                LeanTween.cancel(oldWipeTweenIdLeave);
                                Destroy(OldWipe);
                            }
                            yield return new WaitForSeconds(0);
                        }
                        Scene_Transition wipe = gameObject.AddComponent<Wipe>();
                        //wipe.BeginLeavingScene(nextScene: "Main_Area");
                        SceneManager.LoadScene(sceneName: "Main_Area");
                    }

                    Wipe oldWipe = gameObject.GetComponent<Wipe>();
                    GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>().RunAutopilotSimulation();
                    StartCoroutine(waitDuringAutopilot(oldWipe));
                }
            }
        }
    }




    // Update is called once per frame
    float timer = 5.0f;
    [SerializeField]
    bool playMineGame = true;
    bool d = false;
    void Update()
    {
        if (!d){
            if (timer < 0.0f && playMineGame){
                scene_name = "Mine_Game";
                SceneManager.LoadScene(sceneName: "Mine_Game");
                d = true;
            }
            else{
                timer -= Time.deltaTime;
            }
        }
    }


    private void onAutopilotSelected(bool autopilot){
        upgradesManager.autopilotFlag = autopilot;

        if(InitiateLaunchInfo != null){
            startedRocketSceneTransition = true;
            InitiateLaunchInfo();
        }

        StartCoroutine(_onLaunchInitiated(autopilot));
    }

    IEnumerator _onLaunchInitiated(bool autopilot=false)
    {
    


        if(tinyScientistsManager.launchComplete && rocketTowerManager.launchComplete && rocketController.launchComplete){
            //SceneManager.LoadScene(sceneName: "Rocket_Flight");
            //scene_name = "Rocket_Flight";
            if (!autopilot || autopilot){ // We con do some other stuff below if we want to do something different depending on autopilot selection
                if (gameObject.GetComponent<Wipe>() == null){
                    gameObject.AddComponent<Wipe>();
                    Scene_Transition wipe = gameObject.GetComponent<Wipe>();
                //Scene_Transition wipe = new Wipe();
                    wipe.BeginLeavingScene(nextScene: "Rocket_Flight");
                }
            }
            else{
                // What do we do if they select autopilot?
            }
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onLaunchInitiated(autopilot));
        }
    }


    void onLaunchInitiated(){
        //Debug.Log("UGM: " + upgradesManager + upgradesManager.upgradesUnlockedDict);
        if(upgradesManager == null || !upgradesManager.upgradesUnlockedDict[Upgrade.Autopilot_System]){ // If we don't have the autopilot perk
            if(InitiateLaunchInfo != null){
                startedRocketSceneTransition = true;
                InitiateLaunchInfo();
            }
            StartCoroutine(_onLaunchInitiated(false));
        }
        else{
            uiController.displayAutoPilotConfirmationBox(true);
        }
    }



    IEnumerator _onEndLaunchScene()
    {
        if (gameObject.GetComponent<Wipe>() == null){
            gameObject.AddComponent<Wipe>();
        }
        if(!gameManager.currentlySerializing){ // If we wanna wait for some stuff put it here
            //SceneManager.LoadScene("Main_Area");
            Scene_Transition wipe = gameObject.GetComponent<Wipe>();
            if (wipe != null){
                wipe.BeginLeavingScene(nextScene: "Main_Area");
                scene_name = "Main_Area";
            }
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onEndLaunchScene());
        }
    }



    void onEndLaunchScene(){
        StartCoroutine(_onEndLaunchScene());
    }



    IEnumerator _onEndMineScene()
    {

        // if (gameObject.GetComponent<Circle_Wipe>() == null){
        //     gameObject.AddComponent<Circle_Wipe>();
        // }

        if (gameObject.GetComponent<Wipe>() == null){
            gameObject.AddComponent<Wipe>();
        }

        if(!gameManager.currentlySerializing){ // If we wanna wait for some stuff put it here
            Scene_Transition wipe = gameObject.GetComponent<Wipe>();
            //Scene_Transition wipe = gameObject.GetComponent<Circle_Wipe>();
            if (wipe != null){
                wipe.BeginLeavingScene(nextScene: "Main_Area");
                scene_name = "Main_Area";
            }
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onEndMineScene());
        }
    }



    void onEndMineScene(){
        StartCoroutine(_onEndMineScene());
    }



    IEnumerator _onStartMineScene()
    {
        if(true){ // If we wanna wait for some stuff put it here
            //SceneManager.LoadScene("Mine_Game");
            //scene_name = "Mine_Game";
            //gameObject.AddComponent<Circle_Wipe>();
            gameObject.AddComponent<Wipe>();
            // CIRCLE WIPE IS OFF THE TABLE RIGHT NOW... GONNA REQUIRE SOME SERIOUS RENDERING FUCKERY
            //Scene_Transition wipe = gameObject.GetComponent<Circle_Wipe>();
            //Scene_Transition wipe = new Wipe();
            Scene_Transition wipe = gameObject.GetComponent<Wipe>();
            // Move to the next Mine Game Scene That Corresponds to the Current Main Area Scene
            wipe.BeginLeavingScene(nextScene: "Mine_Game" + SceneManager.GetActiveScene().name.Substring("Main_Area".Length));
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onStartMineScene());
        }
    }



    void onStartMineScene(){
        startedMineSceneTransition = true;
        StartCoroutine(_onStartMineScene());
    }


    void onMineShaftTapped(){
        if(mineShaftController.mineGameIsReady){
            Touch_Detection.instance.disableReticle(disableswipes:true);
            onStartMineScene();
        }
    }
    
    





    private void onLeavingSceneTransitionComplete(string nextScene, Scene_Transition scene_transition){
        // IEnumerator StartSceneTransitionOnSceneChange(){

        // }
        SceneManager.LoadScene(sceneName: nextScene);
        scene_name = nextScene;
        // scene_transition.BeginEnteringScene();
    }




    void onEndLandingPageScene(){
        StartCoroutine(_onEndingLandingPageScene());
    }


    IEnumerator _onEndingLandingPageScene()
    {
    
        if(true){ // If we want to wait for something put it here
            if (gameObject.GetComponent<Wipe>() == null){
                gameObject.AddComponent<Wipe>();
                Scene_Transition wipe = gameObject.GetComponent<Wipe>();
            //Scene_Transition wipe = new Wipe();
                wipe.BeginLeavingScene(nextScene: "Main_Area");
            }
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onEndingLandingPageScene());
        }
    }






}
