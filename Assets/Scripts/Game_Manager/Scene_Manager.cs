 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{

    // Tiny Scientists
    private Tiny_Scientists_Manager tinyScientistsManager;

    // Launch Tower
    private Rocket_Tower_Manager rocketTowerManager;

    // Rocket
    private Rocket_Controller_Main_Area rocketController;

    //Mine Shaft
    private Mine_Shaft_Controller mineShaftController;


    public static Scene_Manager instance;


    public string scene_name;

    void OnEnable()
    {
        Landing_Page_Manager.EndLandingPageSceneInfo += onEndLandingPageScene;
        Launch_Button_Controller.InitiateLaunchInfo += onLaunchInitiated;
        Rocket_Game_Manager.EndLaunchSceneInfo += onEndLaunchScene;
        Mine_Game_Manager.EndMineSceneInfo += onEndMineScene;
        Mine_Shaft_Controller.MineShaftTappedInfo += onMineShaftTapped;
        Scene_Transition.LeavingSceneCompleteInfo += onLeavingSceneTransitionComplete;
    }

    void OnDisable()
    {
        Landing_Page_Manager.EndLandingPageSceneInfo -= onEndLandingPageScene;
        Launch_Button_Controller.InitiateLaunchInfo -= onLaunchInitiated;
        Rocket_Game_Manager.EndLaunchSceneInfo -= onEndLaunchScene;
        Mine_Game_Manager.EndMineSceneInfo -= onEndMineScene;
        Mine_Shaft_Controller.MineShaftTappedInfo -= onMineShaftTapped;
        Scene_Transition.LeavingSceneCompleteInfo -= onLeavingSceneTransitionComplete;
    }


    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            scene_name = SceneManager.GetActiveScene().name;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scene_name = SceneManager.GetActiveScene().name;
        if(scene_name == "Main_Area"){
            tinyScientistsManager = GameObject.Find("Tiny_Scientists_Manager").GetComponent<Tiny_Scientists_Manager>();
            rocketTowerManager = GameObject.Find("Rocket_Tower").GetComponent<Rocket_Tower_Manager>();
            rocketController = GameObject.Find("Rocket").GetComponent<Rocket_Controller_Main_Area>();
            mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>();
            
        }
    }

    void OnLevelWasLoaded(){
        scene_name = SceneManager.GetActiveScene().name;
        if (scene_name == "Main_Area"){
            tinyScientistsManager = GameObject.Find("Tiny_Scientists_Manager").GetComponent<Tiny_Scientists_Manager>();
            rocketTowerManager = GameObject.Find("Rocket_Tower").GetComponent<Rocket_Tower_Manager>();
            rocketController = GameObject.Find("Rocket").GetComponent<Rocket_Controller_Main_Area>();
            mineShaftController = GameObject.Find("Mine_Shaft").GetComponent<Mine_Shaft_Controller>(); 
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


    IEnumerator _onLaunchInitiated()
    {
    
        if(tinyScientistsManager.launchComplete && rocketTowerManager.launchComplete && rocketController.launchComplete){
            //SceneManager.LoadScene(sceneName: "Rocket_Flight");
            //scene_name = "Rocket_Flight";
            if (gameObject.GetComponent<Wipe>() == null){
                gameObject.AddComponent<Wipe>();
                Scene_Transition wipe = gameObject.GetComponent<Wipe>();
            //Scene_Transition wipe = new Wipe();
                wipe.BeginLeavingScene(nextScene: "Rocket_Flight");
            }
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onLaunchInitiated());
        }
    }

    void onLaunchInitiated(){
        StartCoroutine(_onLaunchInitiated());
    }



    IEnumerator _onEndLaunchScene()
    {
        if (gameObject.GetComponent<Wipe>() == null){
            gameObject.AddComponent<Wipe>();
        }
        if(true){ // If we wanna wait for some stuff put it here
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

        if (gameObject.GetComponent<Circle_Wipe>() == null){
            gameObject.AddComponent<Circle_Wipe>();
        }

        if(true){ // If we wanna wait for some stuff put it here
            Scene_Transition wipe = gameObject.GetComponent<Circle_Wipe>();
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
            gameObject.AddComponent<Circle_Wipe>();
            Scene_Transition wipe = gameObject.GetComponent<Circle_Wipe>();
            //Scene_Transition wipe = new Wipe();
            wipe.BeginLeavingScene(nextScene: "Mine_Game");
        }
        else{
            yield return new WaitForSeconds(0.0f);
            StartCoroutine(_onStartMineScene());
        }
    }



    void onStartMineScene(){
        StartCoroutine(_onStartMineScene());
    }


    void onMineShaftTapped(){
        if(mineShaftController.mineGameIsReady){
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
