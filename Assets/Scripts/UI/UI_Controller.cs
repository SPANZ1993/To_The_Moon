//819.50454346

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.Linq;
using System.Reflection;

using UI_Characters;

using UnityEngine.SceneManagement;


using System.Text;
using System.Linq;




public enum TextSpeed{
    Slow = 0,
    Medium = 1,
    Fast = 2
}





public class UI_Controller : MonoBehaviour
{

    // REMOVE
    public bool debugResearch = false;
    public bool debugExperiment = false;

    // UI Original LocalScale Dictionaries
    public Dictionary<GameObject, Vector3> currentLocalScales; // Just used as a reference inside EnableUIElement
    public Dictionary<GameObject, Vector3> landingPageLocalScales;
    public Dictionary<GameObject, Vector3> mainAreaLocalScales;
    public Dictionary<GameObject, Vector3> mineGameLocalScales;
    public Dictionary<GameObject, Vector3> rocketGameLocalScales;
    //

    //Landing Page
    private GameObject Retry_Connect_Box;
    private float minTimeBetweenDisplayRetryConnectBox = 1f;
    private float timeSinceLastDisplayedRetryConnectBox = 1f;
    //End Landing Page

    // Banner
    private GameObject Thrust_Label_Text;
    private GameObject Launch_Label_Text;
    private GameObject Coins_Label_Text;
    private GameObject Gems_Label_Text;
    private GameObject LaunchIndicator1;
    private GameObject LaunchIndicator2;
    private GameObject LaunchIndicator3;

    private GameObject ThrustTextObj;
    private GameObject CoinsTextObj;
    private GameObject GemsTextObj;
    // End Banner

    // Speech Banner
    public bool speechIsDisplayed {get; private set;}
    bool speechBannerButtonPressed = false;
    private GameObject SpeechBanner;

    private GameObject SpeechText;
    private TextMeshProUGUI SpeechText_TMP;

    private GameObject NameText;
    private TextMeshProUGUI NameText_TMP;

    private GameObject SpeechImage;
    private Animator SpeechImageAnimator;

    private Characters2Emotions characters2Emotions = new Characters2Emotions();
    private Characters2Names characters2Names = new Characters2Names();
    private Characters2DisplayNames characters2DisplayNames = new Characters2DisplayNames();

    Characters curCharacter;
    Emotions curEmotion;
    // End Speech Banner


    // Name Input Box
    private GameObject nameInputBox;
    private GameObject nameInputBoxSubmitButton;
    private TextMeshProUGUI nameInputBoxHeaderText;
    private TextMeshProUGUI nameInputBoxPromptText;
    private TextMeshProUGUI nameInputBoxHintText;
    private TMP_InputField nameEnterField;
    private TextMeshProUGUI nameEnterFieldPlaceholder;
    
    public delegate void NameInputBoxSubmitButtonPressed(string nameText);
    public static event NameInputBoxSubmitButtonPressed NameInputBoxSubmitButtonPressedInfo;


    private GameObject nameInputConfirmationBox;
    private TextMeshProUGUI nameInputConfirmationBoxText;
    private TextMeshProUGUI nameInputConfirmationBoxPromptText;


    public delegate void NameSubmitConfirmationButtonPressed(bool selectedYes);
    public static event NameSubmitConfirmationButtonPressed NameSubmitConfirmationButtonPressedInfo;

    // End Name Input Box

    // Coin Name Input Box
    private GameObject coinNameInputBox;
    private GameObject coinNameInputBoxSubmitButton;
    private TextMeshProUGUI coinNameInputBoxText;
    private TextMeshProUGUI coinNameInputBoxHintText;
    private TextMeshProUGUI coinNameInputBoxCoinText;
    private TMP_InputField coinNameEnterField;
    private TextMeshProUGUI coinnameEnterFieldPlaceholder;
    
    public delegate void CoinNameInputBoxSubmitButtonPressed(string nameText);
    public static event CoinNameInputBoxSubmitButtonPressed CoinNameInputBoxSubmitButtonPressedInfo;

    private GameObject coinNameInputConfirmationBox;
    private TextMeshProUGUI coinNameInputConfirmationBoxText;
    private TextMeshProUGUI coinNameInputConfirmationBoxPromptText;



    public delegate void CoinNameSubmitConfirmationButtonPressed(bool selectedYes);
    public static event CoinNameSubmitConfirmationButtonPressed CoinNameSubmitConfirmationButtonPressedInfo;
    // End Coin Name Input Box

    // UI Swipe Arrows
    private GameObject leftSwipeArrow;
    private GameObject rightSwipeArrow;
    // End UI Swipe Arrows


    // Robot Menu
    bool robotMenuFirstDisplayed = false; // Has the robot menu been shown yet
    public bool robotMenuDisplayed = false;
    private bool mineUpgradeSelected = true;
    private GameObject mineSelectionPanel;
    private GameObject mineSelectionPanelText;
    private GameObject cartSelectionPanel;
    private GameObject cartSelectionPanelText;
    private GameObject hashingUpgradePanel;
    private TextMeshProUGUI hashingUpgradePriceText;
    private GameObject blockchainNetworkUpgradePanel;
    private TextMeshProUGUI blockchainNetworkUpgradePriceText;
    private GameObject graphicsCardUpgradePanel;
    private TextMeshProUGUI graphicsCardUpgradePriceText;
    private GameObject coldStorageUpgradePanel;
    private TextMeshProUGUI coldStorageUpgradePriceText;
    private GameObject RobotMenuObj;
    // End Robot Menu

    // Rocket Building Menu
    bool rocketBuildingMenuFirstDisplayed = false;
    public bool rocketBuildingMenuDisplayed = false;
    public bool researchMenuDisplayed = false;
    private GameObject RocketBuildingMenuObj;
    private GameObject researchSelectionPanel;
    private GameObject researchSelectionPanelText;
    private GameObject experimentsSelectionPanel;
    private GameObject experimentsSelectionPanelText;

    private GameObject labMenuMidPanel;
    private UnityEngine.UI.ScrollRect labMenuMidPanelScrollRect;

    private GameObject researchContainerPanel;
    private GameObject experimentsContainerPanel;

    private List<Vector2> experimentPanelsPossibleAnchoredPositions;
    private List<Vector2> experimentPanelsPossiblePositions;
    private List<GameObject> experimentPanels;
    private int numActiveExperiments;

    private Vector2 experimentContainerPanelsOrigAnchoredPosition;
    private float experimentContainerPanelsBottomBuffer; // How far away should the bottom of the experiment container be from the bottom of the lowest selection panel?
    private float experimentContainerPanelsTopBuffer;



    // Dictionary<string, int> experimentname2experimentid = new Dictionary<string, int>()
    //     {
    //         {"Experiment 1", 0},
    //         {"Experiment 2", 1},
    //         {"Experiment 3", 2},
    //         {"Experiment 4", 3}
    //     };


    private List<Vector2> researchPanelsPossibleAnchoredPositions;
    private List<Vector2> researchPanelsPossiblePositions;
    private List<GameObject> researchPanels;
    private int numActiveResearch;

    private Vector2 researchContainerPanelsOrigAnchoredPosition;
    private float researchContainerPanelsBottomBuffer; // How far away should the bottom of the research container be from the bottom of the lowest selection panel?
    private float researchContainerPanelsTopBuffer;




    Dictionary<string, int> researchname2researchid = new Dictionary<string, int>()
        {
            {"Research Name 1", 0},
            {"Research Name 2", 1},
            {"Research Name 3", 2}
        };



    GameObject selectedResearchPanel; // Holds the research panel that we selected when we hit the "Assign Researcher" Button
    Research_Manager researchManager;

    Researcher_Manager researchersManager;

    Experiments_Manager experimentsManager;


    public delegate void ExperimentsUpdated(); // Tells the game manager that a change has been made to the research or researchers so it can update its records
    public static event ExperimentsUpdated ExperimentsUpdatedInfo;

    public delegate void ResearchFinished(double thrustReward);
    public static event ResearchFinished ResearchFinishedInfo;
    // End Rocket Building Menu


    // Researcher Menu
    private GameObject ResearchersMenu;
    private GameObject ResearchersConfirmationBox;
    private GameObject researchersMenuMidPanel;
    private UnityEngine.UI.ScrollRect researchersMenuMidPanelScrollRect;

    private GameObject researchersMenuContainerPanel;
    private Vector2 researchersMenuContainerPanelsOrigAnchoredPosition;
    private float researchersMenuContainerPanelsBottomBuffer; // How far away should the bottom of the research menu container be from the bottom of the lowest selection panel?
    private float researchersMenuContainerPanelsTopBuffer;


    private List<Vector2> researchersPanelsPossibleAnchoredPositions;
    private List<Vector2> researchersPanelsPossiblePositions;
    private List<GameObject> researchersPanels;
    private int numActiveResearchers; // WHAT DO WE NEED AS AN ANALOG HERE?
    

    private bool researchersMenuDisplayed;
    private bool researchersConfirmationBoxDisplayed;

    GameObject selectedResearcherPanel;


    public delegate void ResearchersMenuConfirmationBoxYesPressed(int researchID, int researcherID); // Tells the Researcher Manager & Research Manager That We Are Assigning a Researcher To The Research
    public static event ResearchersMenuConfirmationBoxYesPressed ResearchersMenuConfirmationBoxYesPressedInfo;


    public delegate void ResearchersUpdated(); // Tells the game manager that a change has been made to the research or researchers so it can update its records
    public static event ResearchersUpdated ResearchersUpdatedInfo;
    // End Researcher Menu

    // Not Enough Coins Box
    public GameObject NotEnoughCoinsBox;
    private bool NotEnoughCoinsBoxDisplayed = false;
    // End Not Enough Coins Box


    // autopilot Confirmation Box
    public GameObject autopilotConfirmationBox;
    public TextMeshProUGUI autopilotConfirmationBoxText;
    public TextMeshProUGUI autopilotConfirmationBoxYesText;
    public TextMeshProUGUI autopilotConfirmationBoxNoText;
    
    public delegate void AutopilotSelected(bool selected);
    public static event AutopilotSelected AutopilotSelectedInfo;
    // End autopilot Confirmation Box


    // Bookshelf Menu
    private GameObject bookshelfMenu;
    private bool bookshelfMenuDisplayed = false;

    private GameObject Bookshelf_Container_Panel;
    private ScrollRect bookshelfScrollRect;

    private GameObject Options_Selection_Panel;
    private TextMeshProUGUI Options_Selection_Panel_Text;
    //private ScrollRect optionsScrollRect;
    private GameObject Records_Selection_Panel;
    private TextMeshProUGUI Records_Selection_Panel_Text;

    private TextMeshProUGUI Highest_Altitude_Value_Text;
    private TextMeshProUGUI Most_Coins_Value_Text;

    private TextMeshProUGUI Minecart_Capacity_Value_Text;
    private TextMeshProUGUI Minecart_Rate_Value_Text;
    private TextMeshProUGUI Mineshaft_Per_Swing_Value_Text;
    private TextMeshProUGUI Mineshaft_Per_Block_Value_Text;


    private Slider musicLevelSlider;
    private Slider soundFxLevelSlider;


    private Checkbox_Group textSpeedCheckboxGroup;
    private Checkbox_Group languageCheckboxGroup;

    // End Bookshelf Menu

    // Computer Menu
    private GameObject computerMenu;
    private bool computerMenuDisplayed = false;

    private GameObject Computer_Container_Panel;
    private ScrollRect computerScrollRect;

    private GameObject shopScrollPanel;
    private GameObject exchangeScrollPanel;
    // End Computer Menu






    bool screenTintFirstDisplayed = false; // Has the screen tint object been shown yet?
    private GameObject ScreenTintObj;




    // Rocket Flight Dashboard
    private Game_Scaler gameScaler;

    private Rocket_Game_Manager rocketGameManager;
    private Rocket_Control rocketControl;
    private Camera rocketGameCamera;
    
    private GameObject fuelNeedleFull;
    private GameObject fuelNeedle;
    private GameObject fuelNeedleNut;
    private float fuelNeedleDistance; // How far is the fuel needle from the nut? Use this to rotate the needle.
    private TextMeshProUGUI fuelText;

    private GameObject altitudeBarTopImage;
    private GameObject altitudeBarProgress;
    private Vector3 altitudeBarProgressOrigPos;
    private UnityEngine.Rendering.Universal.Light2D altitudeBarProgressLight;
    private Color altitudeBarProgressLightColor;
    private FieldInfo _altitudeBarProgressLightShapePath;
    private TextMeshProUGUI altitudeText;


    private GameObject RocketFlightRewardedAdConfirmationBox;
    private TextMeshProUGUI RocketFlightRewardedAdConfirmationBoxText;
    // End Rocket Flight Dashboard

    // Mine Game UI
    private Mine_Game_Manager mineGameManager;

    private TextMeshProUGUI MineGameScoreText, MineGameTimerText, MineGameScoreLabelText, MineGameTimerLabelText;

    private GameObject MineGameRewardedAdConfirmationBox;
    private TextMeshProUGUI MineGameRewardedAdConfirmationBoxText;
    // End Mine Game UI


    private Upgrades_Manager upgradesManager;
    private Speech_Object_Generator speechObjectGenerator;
    private Localization_Manager localizationManager;


    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite unselectedSprite;
    [SerializeField]
    private TMP_FontAsset selectedFont;
    [SerializeField]
    private TMP_FontAsset unselectedFont;


    public static UI_Controller instance;
    public static int instanceID;


    private Scene_Manager sceneManager;
    private Game_Manager gameManager;



    public delegate void UIDisplayStarted(Vector3[] boundingBox); // Mostly to alert the game manager to prevent non-UI taps
    public static event UIDisplayStarted UIDisplayStartedInfo;

    public delegate void UIDisplayEnded(Vector3[] boundingBox);
    public static event UIDisplayEnded UIDisplayEndedInfo;


   // Robot Menu Upgrades
    public delegate void HashingUpgradeButtonPressed();
    public static event HashingUpgradeButtonPressed HashingUpgradeButtonPressedInfo;


    public delegate void BlockChainNetworkUpgradeButtonPressed();
    public static event BlockChainNetworkUpgradeButtonPressed BlockChainNetworkUpgradeButtonPressedInfo;


    public delegate void GraphicsCardUpgradeButtonPressed();
    public static event GraphicsCardUpgradeButtonPressed GraphicsCardUpgradeButtonPressedInfo;

    public delegate void ColdStorageUpgradeButtonPressed();
    public static event ColdStorageUpgradeButtonPressed ColdStorageUpgradeButtonPressedInfo;
    // Robot Menu Upgrades End

    // String table names
    string main_area_ui_table = "UI_Banner";
    string ui_research_table = "UI_Research";
    string ui_rocket_flight_table = "Rocket_Flight_UI";
    string ui_mine_game_table = "Mine_Game_UI";
    // End string table names


    // Ads Manager Alerts
    public delegate void AlertRewardedAdAccepted();
    public static event AlertRewardedAdAccepted AlertRewardedAdAcceptedInfo;

    public delegate void AlertRewardedAdRejected();
    public static event AlertRewardedAdRejected AlertRewardedAdRejectedInfo;
    // End Ads Manager Alerts


 



    void OnEnable()
    {
        Localization_Manager.LocaleChangedInfo += onLocaleChanged;
        Robot_Manager.RobotTappedInfo += onRobotTapped;
        Rocket_Building_Manager.RocketBuildingTappedInfo += onRocketBuildingTapped;
        Researcher_Manager.ResearchersRefreshedInfo += onResearchersRefreshed;
    }

    void OnDisable()
    {
        Localization_Manager.LocaleChangedInfo -= onLocaleChanged;
        Robot_Manager.RobotTappedInfo -= onRobotTapped;
        Rocket_Building_Manager.RocketBuildingTappedInfo -= onRocketBuildingTapped;
        Researcher_Manager.ResearchersRefreshedInfo -= onResearchersRefreshed;
    }

    void Awake()
    {

        if (!instance || this.GetInstanceID()==instanceID){
            
            // if (!instance){
            //     indexUIElementSizes();
            // }

            instance = this;
            instanceID = gameObject.GetInstanceID();
            DontDestroyOnLoad(this.gameObject);
            
            //Debug.Log("NOT DESTROYING " + instance + " " + this.GetInstanceID());
            
        }
        else{
            //Debug.Log("DESTROYING");
            Destroy(this.gameObject);
        }

        
    }


    void OnLevelWasLoaded(){
        if (this != instance){
            return;
        }
        sceneManager = Scene_Manager.instance; // This is not the Unity SceneManager... this is our custom class
        //Scene_Manager sceneManager = GameObject.Find("Scene_Manager").GetComponent<Scene_Manager>();
        speechIsDisplayed = false;
        Retry_Connect_Box = null;
        if (SceneManager.GetActiveScene().name == "Landing_Page"){
            indexUIElementSizes();

            Retry_Connect_Box = GameObject.Find("Retry_Connect_Box");
            DisableUIElement(Retry_Connect_Box);
        }
        else if (SceneManager.GetActiveScene().name == "Main_Area"){
            ThrustTextObj = null;
            //Debug.Log("INDEXING FROM LEVEL LOADED");
            indexUIElementSizes();
            
            upgradesManager = Upgrades_Manager.instance;
            gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
            localizationManager = GameObject.Find("Localizer").GetComponent<Localization_Manager>();
            speechObjectGenerator = GameObject.Find("Speech_Object_Generator").GetComponent<Speech_Object_Generator>();
            

            Retry_Connect_Box = GameObject.Find("Retry_Connect_Box");
            DisableUIElement(Retry_Connect_Box);


            // Banner
            LaunchIndicator1 = GameObject.Find("Launch_Indicator1");
            LaunchIndicator2 = GameObject.Find("Launch_Indicator2");
            LaunchIndicator3 = GameObject.Find("Launch_Indicator3");

            Thrust_Label_Text = GameObject.Find("Thrust_Label_Text");
            Launch_Label_Text = GameObject.Find("Launch_Label_Text");
            Coins_Label_Text = GameObject.Find("Coins_Label_Text");
            Gems_Label_Text = GameObject.Find("Gems_Label_Text");
            // End Banner

            // Speech Banner
            SpeechBanner = GameObject.Find("Speech_Banner");
            SpeechText = GameObject.Find("Speech_Text");
            SpeechText_TMP = SpeechText.GetComponent<TextMeshProUGUI>();

            NameText = GameObject.Find("Speech_Name_Text");
            NameText_TMP = NameText.GetComponent<TextMeshProUGUI>();

            SpeechImage = GameObject.Find("Speech_Image");
            SpeechImageAnimator = SpeechImage.GetComponent<Animator>();

            // End Speech Banner

            ThrustTextObj = GameObject.Find("Thrust_Text");
            CoinsTextObj = GameObject.Find("Coins_Text");
            GemsTextObj = GameObject.Find("Gems_Text");



            // Name Enter Box
            nameInputBox = GameObject.Find("Name_Input_Box");
            nameEnterField = GameObject.Find("Name_Enter_Input_Field").GetComponent<TMP_InputField>();
            nameEnterFieldPlaceholder = GameObject.Find("Name_Enter_Input_Field_Placeholder").GetComponent<TextMeshProUGUI>();

            nameInputBoxHeaderText = GameObject.Find("Name_Enter_Box_Text").GetComponent<TextMeshProUGUI>();
            nameInputBoxPromptText = GameObject.Find("Name_Enter_Box_Prompt_Text").GetComponent<TextMeshProUGUI>();
            nameInputBoxHintText = GameObject.Find("Name_Enter_Box_Hint_Text").GetComponent<TextMeshProUGUI>();

            nameInputConfirmationBox = GameObject.Find("Name_Input_Confirmation_Box");
            nameInputConfirmationBoxText = GameObject.Find("Name_Input_Confirmation_Text").GetComponent<TextMeshProUGUI>();
            nameInputConfirmationBoxPromptText = GameObject.Find("Name_Input_Confirmation_Prompt_Text").GetComponent<TextMeshProUGUI>();
            // End Name Enter Box
            
            // Coin Name Enter Box
            coinNameInputBox = GameObject.Find("Coin_Name_Input_Box");
            coinNameEnterField = GameObject.Find("Coin_Name_Enter_Input_Field").GetComponent<TMP_InputField>();
            coinnameEnterFieldPlaceholder = GameObject.Find("Coin_Name_Enter_Input_Field_Placeholder").GetComponent<TextMeshProUGUI>();

            coinNameInputBoxText = GameObject.Find("Coin_Name_Enter_Box_Text").GetComponent<TextMeshProUGUI>();
            coinNameInputBoxHintText = GameObject.Find("Coin_Name_Enter_Box_Hint_Text").GetComponent<TextMeshProUGUI>();
            coinNameInputBoxCoinText = GameObject.Find("Coin_Name_Enter_Box_Coin_Text").GetComponent<TextMeshProUGUI>();

            coinNameInputConfirmationBox = GameObject.Find("Coin_Name_Input_Confirmation_Box");
            coinNameInputConfirmationBoxText = GameObject.Find("Coin_Name_Input_Confirmation_Text").GetComponent<TextMeshProUGUI>();
            coinNameInputConfirmationBoxPromptText = GameObject.Find("Coin_Name_Input_Confirmation_Prompt_Text").GetComponent<TextMeshProUGUI>();
            // End Coin Name Enter Box

            // UI Swipe Arrows
            leftSwipeArrow = GameObject.Find("Left_Swipe_Arrow");
            rightSwipeArrow = GameObject.Find("Right_Swipe_Arrow");
            // End UI Swipe Arrows

            // Robot Menu
            RobotMenuObj = GameObject.Find("Robot_Menu");

            mineSelectionPanel = GameObject.Find("Mines_Selection_Panel");
            cartSelectionPanel = GameObject.Find("Cart_Selection_Panel");
            mineSelectionPanelText = GameObject.Find("Mines_Selection_Panel_Text");
            cartSelectionPanelText = GameObject.Find("Cart_Selection_Panel_Text");

            
            hashingUpgradePanel = GameObject.Find("Hashing_Upgrade_Panel");
            blockchainNetworkUpgradePanel = GameObject.Find("Blockchain_Network_Upgrade_Panel");
            graphicsCardUpgradePanel = GameObject.Find("Graphics_Card_Upgrade_Panel");
            coldStorageUpgradePanel = GameObject.Find("Cold_Storage_Upgrade_Panel");

            hashingUpgradePriceText = hashingUpgradePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            blockchainNetworkUpgradePriceText = blockchainNetworkUpgradePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            graphicsCardUpgradePriceText = graphicsCardUpgradePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            coldStorageUpgradePriceText = coldStorageUpgradePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            // End Robot Menu

            // Rocket Building Menu
            RocketBuildingMenuObj = GameObject.Find("Lab_Menu");

            researchSelectionPanel = GameObject.Find("Research_Selection_Panel");
            experimentsSelectionPanel = GameObject.Find("Experiments_Selection_Panel");
            experimentsSelectionPanelText = GameObject.Find("Experiments_Selection_Panel_Text");
            researchSelectionPanelText = GameObject.Find("Research_Selection_Panel_Text");

            labMenuMidPanel = GameObject.Find("Lab_Menu_Mid_Panel");
            labMenuMidPanelScrollRect = labMenuMidPanel.GetComponent<UnityEngine.UI.ScrollRect>();

            experimentsContainerPanel = GameObject.Find("Experiments_Container_Panel");
            researchContainerPanel = GameObject.Find("Research_Container_Panel");

            experimentPanels = new List<GameObject>();
            experimentPanelsPossibleAnchoredPositions = new List<Vector2>();
            experimentPanelsPossiblePositions = new List<Vector2>();
            foreach(Transform t in experimentsContainerPanel.transform){
                experimentPanels.Add(t.gameObject);
                experimentPanelsPossibleAnchoredPositions.Add(t.GetComponent<RectTransform>().anchoredPosition);
                experimentPanelsPossiblePositions.Add(t.GetComponent<RectTransform>().position);
            }
            numActiveExperiments = experimentPanels.Count;

            experimentContainerPanelsTopBuffer =  experimentsContainerPanel.GetComponent<RectTransform>().anchoredPosition[0] - experimentPanelsPossibleAnchoredPositions[0][0];
            experimentContainerPanelsBottomBuffer = experimentsContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] - experimentPanelsPossibleAnchoredPositions[1][1];
            //Debug.Log("EXPERIMENTS BUFFER: " + experimentContainerPanelsBottomBuffer);
            //Debug.Log("EXPERIMENTS BUFFER TOP: " + experimentContainerPanelsTopBuffer);
            experimentContainerPanelsOrigAnchoredPosition = experimentsContainerPanel.GetComponent<RectTransform>().anchoredPosition;



            researchPanels = new List<GameObject>();
            researchPanelsPossibleAnchoredPositions = new List<Vector2>();
            researchPanelsPossiblePositions = new List<Vector2>();
            foreach(Transform t in researchContainerPanel.transform){
                researchPanels.Add(t.gameObject);
                researchPanelsPossibleAnchoredPositions.Add(t.GetComponent<RectTransform>().anchoredPosition);
                researchPanelsPossiblePositions.Add(t.GetComponent<RectTransform>().position);
            }
            numActiveResearch = researchPanels.Count;
            

            researchContainerPanelsTopBuffer =  researchContainerPanel.GetComponent<RectTransform>().anchoredPosition[0] - researchPanelsPossibleAnchoredPositions[0][0];
            researchContainerPanelsBottomBuffer = researchContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] - researchPanelsPossibleAnchoredPositions[1][1];
            //Debug.Log("RESEARCH BUFFER CONTAINER ANCHORED POS: " + researchContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] + " ... " + researchPanelsPossibleAnchoredPositions[1][1] + " AND ITS " + (researchContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] - researchPanelsPossibleAnchoredPositions[1][1]).ToString());
            //Debug.Log("RESEARCH BUFFER: " + researchContainerPanelsBottomBuffer);
            //Debug.Log("RESEARCH BUFFER TOP: " + researchContainerPanelsTopBuffer);
            researchContainerPanelsOrigAnchoredPosition = researchContainerPanel.GetComponent<RectTransform>().anchoredPosition;

            researchManager = GameObject.Find("Research_Manager").GetComponent<Research_Manager>();
            researchersManager = GameObject.Find("Researcher_Manager").GetComponent<Researcher_Manager>();
            experimentsManager = GameObject.Find("Experiments_Manager").GetComponent<Experiments_Manager>();
            //Debug.Log("EXPERIMENTS MANAGER: " + experimentsManager);
            // End Rocket Building Menu


            // Researchers Menu
            ResearchersMenu = GameObject.Find("Researcher_Menu");
            ResearchersConfirmationBox = GameObject.Find("Research_Confirmation_Box");
            researchersMenuMidPanel = GameObject.Find("Researcher_Menu_Mid_Panel");
            researchersMenuMidPanelScrollRect = researchersMenuMidPanel.GetComponent<UnityEngine.UI.ScrollRect>();
            researchersMenuContainerPanel = GameObject.Find("Researcher_Menu_Container_Panel");


            //Debug.Log("RMCP: " + researchersMenuContainerPanel.name);

            researchersPanels = new List<GameObject>();
            researchersPanelsPossibleAnchoredPositions = new List<Vector2>();
            researchersPanelsPossiblePositions = new List<Vector2>();
            foreach (Transform t in researchersMenuContainerPanel.transform){
                researchersPanels.Add(t.gameObject);
                researchersPanelsPossibleAnchoredPositions.Add(t.GetComponent<RectTransform>().anchoredPosition);
                researchersPanelsPossiblePositions.Add(t.GetComponent<RectTransform>().position);
            }

            researchersMenuContainerPanelsTopBuffer =  researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition[0] - researchersPanelsPossiblePositions[0][0];
            researchersMenuContainerPanelsBottomBuffer = researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] - researchersPanelsPossibleAnchoredPositions[1][1];
            //Debug.Log("RESEARCHER BUFFER CONTAINER ANCHORED POS: " + researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] + " ... " + researchersPanelsPossibleAnchoredPositions[1][1] + " AND ITS " + (researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition[1] - researchersPanelsPossibleAnchoredPositions[1][1]).ToString());
            //Debug.Log("RESEARCHER BUFFER: " + researchersMenuContainerPanelsBottomBuffer);
            //Debug.Log("RESEARCHER BUFFER TOP: " + researchersMenuContainerPanelsTopBuffer);
            researchersMenuContainerPanelsOrigAnchoredPosition = researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition;
            //numActiveResearchers = researchersPanels.Count;

            researchersMenuDisplayed = false;
            researchersConfirmationBoxDisplayed = false;
            // End Researchers Menu

            // Not Enough Coins Box
            NotEnoughCoinsBox = GameObject.Find("Not_Enough_Coins_Box");
            // End Not Enough Coins Box

            // autopilot Confirmation Box
            autopilotConfirmationBox = GameObject.Find("Autopilot_Confirmation_Box");
            autopilotConfirmationBoxText = GameObject.Find("Autopilot_Confirmation_Text").GetComponent<TextMeshProUGUI>();
            autopilotConfirmationBoxYesText = GameObject.Find("Autopilot_Confirmation_Yes_Button_Text").GetComponent<TextMeshProUGUI>();
            autopilotConfirmationBoxNoText = GameObject.Find("Autopilot_Confirmation_No_Button_Text").GetComponent<TextMeshProUGUI>();
            displayAutoPilotConfirmationBox(false);
            // End autopilot Confirmation Box
            
            // Bookshelf Menu
            bookshelfMenu = GameObject.Find("Bookshelf_Menu");

            Options_Selection_Panel = GameObject.Find("Options_Selection_Panel");
            Options_Selection_Panel_Text = GameObject.Find("Options_Selection_Panel_Text").GetComponent<TextMeshProUGUI>();
            Bookshelf_Container_Panel =  GameObject.Find("Bookshelf_Container_Panel");
            bookshelfScrollRect = Bookshelf_Container_Panel.GetComponent<ScrollRect>();

            Records_Selection_Panel = GameObject.Find("Records_Selection_Panel");
            Records_Selection_Panel_Text = GameObject.Find("Records_Selection_Panel_Text").GetComponent<TextMeshProUGUI>();
            //Records_Container_Panel = GameObject.Find("Records_Container_Panel");
            //recordsScrollRect = GameObject.Find("Records_Container_Panel").GetComponent<ScrollRect>();

            musicLevelSlider = GameObject.Find("Music_Slider").GetComponent<Slider>();
            soundFxLevelSlider = GameObject.Find("Sound_FX_Slider").GetComponent<Slider>();

            textSpeedCheckboxGroup = GameObject.Find("Text_Speed_Separator_Panel").GetComponent<Checkbox_Group>();
            languageCheckboxGroup = GameObject.Find("Language_Separator_Panel").GetComponent<Checkbox_Group>();


            Most_Coins_Value_Text = GameObject.Find("Most_Coins_Value_Text").GetComponent<TextMeshProUGUI>();
            Highest_Altitude_Value_Text = GameObject.Find("Highest_Altitude_Value_Text").GetComponent<TextMeshProUGUI>();


            Minecart_Capacity_Value_Text = GameObject.Find("Mines_Info_Minecart_Info_Capacity_Value_Text").GetComponent<TextMeshProUGUI>();
            Minecart_Rate_Value_Text = GameObject.Find("Mines_Info_Minecart_Info_Rate_Value_Text").GetComponent<TextMeshProUGUI>();
            Mineshaft_Per_Swing_Value_Text = GameObject.Find("Mines_Info_Mineshaft_Info_Per_Swing_Value_Text").GetComponent<TextMeshProUGUI>();
            Mineshaft_Per_Block_Value_Text = GameObject.Find("Mines_Info_Mineshaft_Info_Per_Block_Value_Text").GetComponent<TextMeshProUGUI>();


            // End Bookshelf Menu

            // Computer Menu
            computerMenu = GameObject.Find("Computer_Menu");

            Computer_Container_Panel = GameObject.Find("Computer_Container_Panel");
            computerScrollRect = Computer_Container_Panel.GetComponent<ScrollRect>();

            shopScrollPanel = GameObject.Find("Shop_Scroll_Panel");
            exchangeScrollPanel = GameObject.Find("Exchange_Scroll_Panel");
            // End Computer Menu


            //setActiveExperiments(new int[] {0, 1, 3});


            ScreenTintObj = GameObject.Find("Screen_Tint");

            //indexUIElementSizes();



            DisableUIElement(SpeechBanner);
            DisableUIElement(nameInputBox);
            DisableUIElement(nameInputConfirmationBox);
            DisableUIElement(coinNameInputBox);
            DisableUIElement(coinNameInputConfirmationBox);
            DisableUIElement(bookshelfMenu);
            DisableUIElement(computerMenu);
            DisableUIElement(leftSwipeArrow);
            DisableUIElement(rightSwipeArrow);
            DisableUIElement(RobotMenuObj);
            DisableUIElement(RocketBuildingMenuObj);
            DisableUIElement(ResearchersMenu);
            DisableUIElement(ResearchersConfirmationBox);
            DisableUIElement(NotEnoughCoinsBox);
            DisableUIElement(ScreenTintObj);

            //_getActiveResearchIds();
            //_getActiveResearcherIds();


        }
        else if (SceneManager.GetActiveScene().name == "Rocket_Flight"){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
            rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
            rocketControl = GameObject.Find("Rocket").GetComponent<Rocket_Control>();
            rocketGameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();


            fuelNeedleFull = GameObject.Find("Fuel_Needle_Full");
            //fuelNeedle = GameObject.Find("Fuel_Needle");
            //fuelNeedleNut = GameObject.Find("Fuel_Needle_Nut");
            //fuelNeedleDistance = Vector3.Distance(fuelNeedle.GetComponent<RectTransform>().anchoredPosition, fuelNeedleNut.GetComponent<RectTransform>().anchoredPosition);
            fuelText = GameObject.Find("Fuel_Text").GetComponent<TextMeshProUGUI>();

            altitudeBarTopImage = GameObject.Find("Altitude_Bar_Top_Image");
            altitudeBarProgress = GameObject.Find("Altitude_Bar_Progress");
            altitudeBarProgressOrigPos = altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition;
            //altitudeBarProgressOrigWorldPos = cam.
            altitudeBarProgressLight = altitudeBarProgress.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            altitudeBarProgressLightColor = altitudeBarProgressLight.color;
            _altitudeBarProgressLightShapePath = altitudeBarProgressLight.GetType().GetField("m_ShapePath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //_LightCookieSprite =  typeof( Light2D ).GetField( "m_LightCookieSprite", BindingFlags.NonPublic | BindingFlags.Instance );
            altitudeText = GameObject.Find("Altitude_Text").GetComponent<TextMeshProUGUI>();
        

            RocketFlightRewardedAdConfirmationBox = GameObject.Find("Rewarded_Ad_Confirmation_Box");
            RocketFlightRewardedAdConfirmationBoxText = GameObject.Find("Rewarded_Ad_Confirmation_Text").GetComponent<TextMeshProUGUI>();

            indexUIElementSizes();
    
            //Debug.Log("DISABLING REWARDED AD CONFIRMATION BOX: " + RocketFlightRewardedAdConfirmationBox);
            DisableUIElement(RocketFlightRewardedAdConfirmationBox);

        }
        else if (SceneManager.GetActiveScene().name == "Mine_Game"){
            mineGameManager = GameObject.Find("Mine_Game_Manager").GetComponent<Mine_Game_Manager>();

            MineGameScoreText = GameObject.Find("Score_Text").GetComponent<TextMeshProUGUI>();
            MineGameTimerText = GameObject.Find("Timer_Text").GetComponent<TextMeshProUGUI>();
            MineGameScoreLabelText = GameObject.Find("Score_Label_Text").GetComponent<TextMeshProUGUI>();
            MineGameTimerLabelText = GameObject.Find("Timer_Label_Text").GetComponent<TextMeshProUGUI>();


            MineGameRewardedAdConfirmationBox = GameObject.Find("Rewarded_Ad_Confirmation_Box");
            MineGameRewardedAdConfirmationBoxText = GameObject.Find("Rewarded_Ad_Confirmation_Text").GetComponent<TextMeshProUGUI>();

            indexUIElementSizes();
    
            //Debug.Log("DISABLING REWARDED AD CONFIRMATION BOX: " + MineGameRewardedAdConfirmationBox);
            DisableUIElement(MineGameRewardedAdConfirmationBox);
        }
        // else if (sceneManager.scene_name == "Landing_Page"){
        //     StartCoroutine(LateStart());
        // }
    }

    //IEnumerator LateStart(){
        //yield return new WaitForSeconds(0);
        //_LateStart();
    //}

    //void _LateStart(){
        //gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
    //}


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(_LateStart());
        //gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        //Debug.Log("STARTING UI CONTROLLER");
        OnLevelWasLoaded();
        //Invoke("displayExampleSpeech", 15);
    }


    public void onButtonTmp(){
        Researcher_Manager researcherManager = GameObject.Find("Researcher_Manager").GetComponent<Researcher_Manager>(); 
        List<int> unlockedResearchersIds = researcherManager.getUnlockedResearchersIds();
        if (unlockedResearchersIds.Max() < 6){
            //Debug.Log("ADDING RESEARCHER: " + unlockedResearchersIds.Max() + 1);
            unlockedResearchersIds.Add(unlockedResearchersIds.Max() + 1);
            researcherManager.setUnlockedResearchersIds(unlockedResearchersIds);
        }
    }

    public void onButtonTmp2(){
        Research_Manager researchManager = GameObject.Find("Research_Manager").GetComponent<Research_Manager>(); 
        List<int> unlockedResearchIds = researchManager.getUnlockedResearchIds();
        if (unlockedResearchIds.Max() < 7){
            //Debug.Log("ADDING RESEARCH: " + unlockedResearchIds.Max() + 1);
            unlockedResearchIds.Add(unlockedResearchIds.Max() + 1);
            researchManager.setUnlockedResearchIds(unlockedResearchIds);
        }
    }

    public void onButtonTmp3(){
        gameManager.coins += 500.0;
        gameManager.gems += 5;
    }

    public void onButtonTmp4(){
        ((ISerialization_Manager)GameObject.Find("Serialization_Manager").GetComponent<Serializer>()).deleteSave();
    }

    public void onButtonTmp5(){
        displayExampleSpeech();
    }

    public void onButtonTmp6(){
        Experiments_Manager experimentsManager = GameObject.Find("Experiments_Manager").GetComponent<Experiments_Manager>(); 
        List<ExperimentId> unlockedExperimentIdsId = experimentsManager.getUnlockedExperimentIds();
        List<int> unlockedExperimentIds = new List<int>();
        foreach(ExperimentId exp in unlockedExperimentIdsId){
            unlockedExperimentIds.Add((int)exp);
        }
        int addedNum = -1;
        int attempts = 0;
        bool found = false;
        System.Random rnd = new System.Random();
        while(attempts < 1000 && (unlockedExperimentIds.Contains(addedNum) || addedNum == -1)){
            addedNum = rnd.Next(1, 8);
            attempts++;
        }
        if (attempts != 1000){
            found = true;
        }
        if (found){
            Debug.Log("ADDING EXPERIMENT: " + (ExperimentId)addedNum);
            unlockedExperimentIds.Add(addedNum);
            experimentsManager.setUnlockedExperimentIds(unlockedExperimentIds);
        }
        else{
            Debug.Log("COULDN'T ADD ANY MORE EXPERIMENTS");
        }
    }


    private int[] randomListTmp(int num){
        System.Random rand = new System.Random();
        List<int> a = new List<int>();
        for(int i=0; i<num; i++){
            if (rand.Next(0, 2) != 0)
            {
                a.Add(i);
            }
        }
        //return a.ToArray();
        return new int[] {0,1,2};
    }


    void indexUIElementSizes(){
        string curScene = SceneManager.GetActiveScene().name;
        if (curScene == "Landing_Page"){
            landingPageLocalScales = new Dictionary<GameObject, Vector3>();
            _indexUIElementSizes(landingPageLocalScales, GameObject.Find("Canvas"));
        }
        else if (curScene == "Main_Area"){ //&& mainAreaLocalScales is null){
            //Debug.Log("INDEXING MAIN AREA");
            mainAreaLocalScales = new Dictionary<GameObject, Vector3>();
            _indexUIElementSizes(mainAreaLocalScales, GameObject.Find("Canvas"));
            //Debug.Log("INDEXING SIZES");
            //_testUIElementSizes(mainAreaLocalScales, GameObject.Find("UI_Controller"));
        }
        else if (curScene == "Mine_Game"){ //&& mainAreaLocalScales is null){
            mineGameLocalScales = new Dictionary<GameObject, Vector3>();
            _indexUIElementSizes(mineGameLocalScales , GameObject.Find("Canvas"));
            //Debug.Log("INDEXING SIZES");
            //_testUIElementSizes(mainAreaLocalScales, GameObject.Find("UI_Controller"));
        }
        else if (curScene == "Rocket_Flight"){ //&& mainAreaLocalScales is null){
            rocketGameLocalScales = new Dictionary<GameObject, Vector3>();
            _indexUIElementSizes(rocketGameLocalScales, GameObject.Find("Canvas"));
            //Debug.Log("INDEXING SIZES");
            //_testUIElementSizes(mainAreaLocalScales, GameObject.Find("UI_Controller"));
        }

    }

    void _indexUIElementSizes(Dictionary<GameObject, Vector3> localScaleDict, GameObject curObj){
        localScaleDict[curObj] = curObj.transform.localScale;
        if(curObj.name == "Retry_Connect_Box"){
            //Debug.Log("RETRY CONNECT BOX DURING INDEX: " + curObj.transform.localScale + " ____ " + localScaleDict[curObj] +  " --- " + DateTime.Now);
        }
        foreach(Transform child in curObj.transform){
            _indexUIElementSizes(localScaleDict, child.gameObject);
        }
        _testUIElementSizes(localScaleDict, curObj);
    }

    void _testUIElementSizes(Dictionary<GameObject, Vector3> localScaleDict, GameObject curObj){
        //Debug.Log(gameObject.GetInstanceID() + " LSCALE --- " + curObj.name + ": " + localScaleDict[curObj] + " " + curObj.GetInstanceID());
        foreach(Transform child in curObj.transform){
            _testUIElementSizes(localScaleDict, child.gameObject);
        }
    }




    int frame_count = 0;
    // Update is called once per frame
    void Update()
    {
        
        //setActiveResearch(new int[] {1, 2, 3});
        //setActiveResearchers(new int[] {1, 2, 4, 5});
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            if (ThrustTextObj != null){ // Just Making Sure We Have A Reference To The Banner
                updateBannerUI();
                frame_count++;
            }
        }
        
        else if (SceneManager.GetActiveScene().name == "Rocket_Flight"){
            // DO SOME STUFF
            if ((fuelNeedle != null && fuelNeedle != null) || fuelNeedleFull != null){
                updateFuelNeedle();
                updateAltitudeBar();
                updateFuelText();
                updateAltitudeText();
            }
        }
        
        else if (SceneManager.GetActiveScene().name == "Mine_Game"){
            if (!mineGameManager.gameOver){
                updateMineGameScoreText();
            }
            updateMineGameTimerText();
        }

        timeSinceLastDisplayedRetryConnectBox = Mathf.Min(timeSinceLastDisplayedRetryConnectBox + Time.deltaTime, minTimeBetweenDisplayRetryConnectBox);
    }

    void OnApplicationFocus(){
        timeSinceLastDisplayedRetryConnectBox = minTimeBetweenDisplayRetryConnectBox;
    }

    void OnApplicationPause(){
        timeSinceLastDisplayedRetryConnectBox = minTimeBetweenDisplayRetryConnectBox;
    }


    // Name Input Box Stuff
    public void displayNameInputBox(string hint = null, string potentialName = null){
        
        EnableUIElement(ScreenTintObj);
        EnableUIElement(nameInputBox);
        DisableUIElement(ScreenTintObj, touchOnly:true);
        string UIStringTable = "UI_Banner";
        
        nameInputBoxHeaderText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Input_Box.Header");
        nameInputBoxPromptText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Input_Box.Prompt");
        nameEnterFieldPlaceholder.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Input_Box.Default_Name");
        
        if (hint == null){
            nameInputBoxHintText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Input_Box.Hint.Default");
        }
        else{
            nameInputBoxHintText.text = hint;
        }

        if(potentialName == null){
            nameEnterField.text = "";
        }
        else{
            nameEnterField.text = potentialName;
        }
    }


    public void displayNameConfirmationBox(string potentialName){
        UI_Controller.instance.EnableUIElement(ScreenTintObj);
        DisableUIElement(ScreenTintObj, touchOnly:true);
        EnableUIElement(nameInputConfirmationBox);
        string UIStringTable = "UI_Banner";
        nameInputConfirmationBoxText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Confirmation_Box.Text").Replace("{potentialPlayerName}", potentialName);
        nameInputConfirmationBoxPromptText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Name_Confirmation_Box.Prompt").Replace("{potentialPlayerName}", potentialName);

    }

    // End Name Input Box Stuff



    // Coin Name Input Box Stuff
    public void displayCoinNameInputBox(string hint = null, string potentialCoinName = null){
        foreach (Transform child in coinNameInputBoxHintText.gameObject.transform){
            Destroy(child.gameObject);
        }
        UI_Controller.instance.EnableUIElement(coinNameInputBox);
        UI_Controller.instance.EnableUIElement(ScreenTintObj);
        DisableUIElement(ScreenTintObj, touchOnly:true);
        string UIStringTable = "UI_Banner";
        coinNameInputBoxText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Coin_Name_Input_Box.Prompt");
        coinNameInputBoxCoinText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Coin_Name_Input_Box.Coin");
        coinnameEnterFieldPlaceholder.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Coin_Name_Input_Box.Default_Name");
        if (hint == null){
            coinNameInputBoxHintText.text = "";
        }
        else{
            coinNameInputBoxHintText.text = hint;
        }
        if(potentialCoinName == null){
            coinNameEnterField.text = "";
        }
        else{
            coinNameEnterField.text = potentialCoinName;
        }
    }

    public void displayCoinNameConfirmationBox(string potentialName){
        UI_Controller.instance.EnableUIElement(ScreenTintObj);
        DisableUIElement(ScreenTintObj, touchOnly:true);
        EnableUIElement(coinNameInputConfirmationBox);
        string UIStringTable = "UI_Banner";
        coinNameInputConfirmationBoxText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Coin_Name_Confirmation_Box.Text").Replace("{potentialCoinName}", potentialName);
        coinNameInputConfirmationBoxPromptText.text = Localization_Manager.instance.GetLocalizedString(UIStringTable, "UI.Onboarding.Coin_Name_Confirmation_Box.Prompt").Replace("{potentialCoinName}", potentialName);
    }
    // End Coin Name Input Box Stuff




    public void displayExampleSpeech(){
            List<string> example_speech_strings = new List<string>();
            List<Characters> example_speech_characters = new List<Characters>();
            List<Emotions> example_speech_emotions = new List<Emotions>();
            List<Emotions> example_speech_post_emotions = new List<Emotions>();
            // example_speech_strings.Add("I've got the hook-up on black market Michelle Obama feet pics. HMU.");
            // example_speech_strings.Add("Hey have you seen that dank new meme called the 'BIG CHUNGUS'?");
            // example_speech_strings.Add("I sure do love that fat bunny.");
            example_speech_strings.Add("OH NO! I have fallen victim to the woke cancel culture narrative!");
            example_speech_characters.Add(Characters.Gorilla);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Sad);

            example_speech_strings.Add("I will trigger the libs with a hilarious inside joke...");
            example_speech_characters.Add(Characters.Gorilla);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Idle);

            example_speech_strings.Add("LET'S GO BRANDON!");
            example_speech_characters.Add(Characters.Gorilla);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Happy);

            example_speech_strings.Add("Lol the libs don't know we are mocking Sleepy Joe!!!");
            example_speech_characters.Add(Characters.Guy);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Happy);

            example_speech_strings.Add("Those blue haired new wave feminists are so upset.");
            example_speech_characters.Add(Characters.Guy);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Idle);

            
            example_speech_strings.Add("ACK!! HERE COMES THE WOKE MOB!!!");
            example_speech_characters.Add(Characters.Guy);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Sad);


            example_speech_strings.Add("They want us to eat soy based tofurkey and quinoa!");
            example_speech_characters.Add(Characters.Dog);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Sad);

            example_speech_strings.Add("Those libs are always trying to bring about the downfall of the Western Man.");
            example_speech_characters.Add(Characters.Dog);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Idle);

            example_speech_strings.Add("Don't worry! Our technoking Elong Musk will save us with a cool new robot or something!");
            example_speech_characters.Add(Characters.Dog);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Happy);

            example_speech_strings.Add("Beep Boop. Am Robot. I bring you manly beef jerky snack.");
            example_speech_characters.Add(Characters.Robot);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Idle);

            example_speech_strings.Add("The greasy meats lubricate my circuits nicely.");
            example_speech_characters.Add(Characters.Robot);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Happy);

            example_speech_strings.Add("My circuits yearn for the meta-human experience.");
            example_speech_characters.Add(Characters.Robot);
            example_speech_emotions.Add(Emotions.Talking);
            example_speech_post_emotions.Add(Emotions.Sad);


            Speech_Object example_so = new Speech_Object(false, // IsBlocker
                                            example_speech_strings, 
                                            example_speech_characters, 
                                            example_speech_emotions, 
                                             example_speech_post_emotions);
            Display_Speech(example_so);
    }


    void updateBannerUI(){
        //Debug.Log("1: " + LaunchIndicator1);
        //Debug.Log("2: " + LaunchIndicator2);
        //Debug.Log("3: " + LaunchIndicator3);

        if (gameManager.remainingLaunches == 0){
            updateRemainingLaunchIndicator(LaunchIndicator1, false);
            updateRemainingLaunchIndicator(LaunchIndicator2, false);
            updateRemainingLaunchIndicator(LaunchIndicator3, false);
        }
        else if (gameManager.remainingLaunches == 1){
            updateRemainingLaunchIndicator(LaunchIndicator1, true);
            updateRemainingLaunchIndicator(LaunchIndicator2, false);
            updateRemainingLaunchIndicator(LaunchIndicator3, false);
        }
        else if (gameManager.remainingLaunches == 2){
            updateRemainingLaunchIndicator(LaunchIndicator1, true);
            updateRemainingLaunchIndicator(LaunchIndicator2, true);
            updateRemainingLaunchIndicator(LaunchIndicator3, false);
        }
        else if (gameManager.remainingLaunches == 3){
            updateRemainingLaunchIndicator(LaunchIndicator1, true);
            updateRemainingLaunchIndicator(LaunchIndicator2, true);
            updateRemainingLaunchIndicator(LaunchIndicator3, true);
        }


        updateThrustText(ThrustTextObj, gameManager.thrust);
        updateCoinsText(CoinsTextObj, gameManager.coins);
        updateGemsText(GemsTextObj, gameManager.gems);
    }

    void updateRemainingLaunchIndicator(GameObject launchIndicator, bool enable){
        // Update this once we get a better sprite for these
        Image img = launchIndicator.GetComponent<Image>();
        Color onColor = new Color(0, 255, 0, 255);
        Color offColor = new Color(255, 0, 0, 255);
        if (enable){
            img.color = onColor;
        }
        else{
            img.color = offColor;
        }
    }

    void updateThrustText(GameObject ThrustTextObj, double thrust){
        TextMeshProUGUI ThrustText = ThrustTextObj.GetComponent<TextMeshProUGUI>();
        ThrustText.text = Number_String_Formatter.bannerUIFormatThrustNumberText(thrust, decimals:3);
    }

    void updateCoinsText(GameObject CoinsTextObj, double coins){
        TextMeshProUGUI CoinsText = CoinsTextObj.GetComponent<TextMeshProUGUI>();
        CoinsText.text = Number_String_Formatter.bannerUIFormatCoinsNumberText(coins, decimals:3);
    }

    void updateGemsText(GameObject GemsTextObj, double gems){
        TextMeshProUGUI GemsText = GemsTextObj.GetComponent<TextMeshProUGUI>();
        GemsText.text = Number_String_Formatter.bannerUIFormatGemsNumberText(gems, decimals:3);
    }



    void onRobotTapped(){
        if (!robotMenuDisplayed && !rocketBuildingMenuDisplayed && !sceneManager.startedMineSceneTransition){

            EnableUIElement(RobotMenuObj);
            EnableUIElement(ScreenTintObj);
            selectMineUpgrade();
            robotMenuDisplayed = true;
            if (UIDisplayStartedInfo != null){
                UIDisplayStartedInfo(new Vector3[0]);
            }
        }
    }


    void onRocketBuildingTapped(){
        if (!robotMenuDisplayed && !rocketBuildingMenuDisplayed && !sceneManager.startedRocketSceneTransition){

            EnableUIElement(RocketBuildingMenuObj);
            //IEnumerator tmpIE(){
                //yield return new WaitForSeconds(0);
                //researchManager.refreshAllResearchPanels();
            //}
            //StartCoroutine(tmpIE());
            //researchManager.refreshAllResearchPanels(); // THIS DOESN'T WORK BUT THE ABOVE DOES.... PROBABLY GOING TO HAVE TO CHANGE ENABLEUIELEMENT() CODE A BIT
            EnableUIElement(ScreenTintObj);
            //AUDIO
            selectResearch();
            rocketBuildingMenuDisplayed = true;
            if(!Audio_Manager.instance.IsPlaying("Rocket_Building_Tapped")){
                Audio_Manager.instance.Play("Rocket_Building_Tapped");
            }
            if (UIDisplayStartedInfo != null){
                UIDisplayStartedInfo(new Vector3[0]);
            }
        }
    }

    public void closeMenus(){
        bool sendUIEndAlert = false;

        if (robotMenuDisplayed){
            DisableUIElement(RobotMenuObj);
            robotMenuDisplayed = false;
            DisableUIElement(ScreenTintObj);

            sendUIEndAlert = true;
        }
        else if (!researchersMenuDisplayed){
            DisableUIElement(RocketBuildingMenuObj);

            rocketBuildingMenuDisplayed = false;

            DisableUIElement(ScreenTintObj);

            sendUIEndAlert = true;
        }
        else if (researchersMenuDisplayed && !researchersConfirmationBoxDisplayed){
            DisableUIElement(ResearchersMenu);
            DisableUIElement(RocketBuildingMenuObj);

            rocketBuildingMenuDisplayed = false;
            researchersMenuDisplayed = false;

            DisableUIElement(ScreenTintObj);

            sendUIEndAlert = true;
        }
        else if (researchersMenuDisplayed && researchersConfirmationBoxDisplayed){
            DisableUIElement(RocketBuildingMenuObj);
            DisableUIElement(ResearchersMenu);
            DisableUIElement(ResearchersConfirmationBox);

            rocketBuildingMenuDisplayed = false;
            researchersMenuDisplayed = false;
            researchersConfirmationBoxDisplayed = false;
            
            DisableUIElement(ScreenTintObj);

            EnableUIElement(RocketBuildingMenuObj, touchOnly: true);
            EnableUIElement(ResearchersMenu, touchOnly: true);

            sendUIEndAlert = true;
        }

        if(bookshelfMenuDisplayed){
            DisableUIElement(bookshelfMenu);
            DisableUIElement(ScreenTintObj);
            bookshelfMenuDisplayed = false;
        }

        if(computerMenuDisplayed){
            DisableUIElement(computerMenu);
            DisableUIElement(ScreenTintObj);
            computerMenuDisplayed = false;
        }

        

        if (NotEnoughCoinsBoxDisplayed){
            DisableUIElement(NotEnoughCoinsBox);
        }


        
        if (sendUIEndAlert && UIDisplayEndedInfo != null){
            UIDisplayEndedInfo(new Vector3[0]);
        }

        researchMenuDisplayed = false;

        selectedResearchPanel = null;
        selectedResearcherPanel = null;
    }





    public void EnableUIElement(GameObject UI, bool touchOnly=false, Dictionary<GameObject,Vector3> localScalesDict=null){
        for(int i = 0; i < UI.transform.childCount; i++)
        {
            GameObject child = UI.transform.GetChild(i).gameObject;
            EnableUIElement(child, touchOnly: touchOnly);
        }

        if (touchOnly){     

            Button button = UI.GetComponent<Button>();
            UnityEngine.UI.ScrollRect scrollRect = UI.GetComponent<UnityEngine.UI.ScrollRect>();

            if (button != null){
                button.enabled = true;
            }

            if (scrollRect != null){
                scrollRect.enabled = true;
            }

        }
        else{
            
            // TextMeshProUGUI textMesh = UI.GetComponent<TextMeshProUGUI>();
            // Image image = UI.GetComponent<Image>();
            // Button button = UI.GetComponent<Button>();

            // if (textMesh != null){
            //     textMesh.enabled = true;
            // }

            // if (image != null){
            //     image.enabled = true;
            // }

            // if (button != null){
            //     button.enabled = true;
            // }

            currentLocalScales = null;
            if (localScalesDict == null){
                if (SceneManager.GetActiveScene().name == "Landing_Page"){
                    currentLocalScales = landingPageLocalScales;
                }
                else if(SceneManager.GetActiveScene().name == "Main_Area"){
                    //Debug.Log("TRYING TO ENABLE: " + UI.name);
                    //Debug.Log("TRYING TO ENABLE: " + UI.name + " WITH SCALE " + mainAreaLocalScales[UI]);
                    currentLocalScales = mainAreaLocalScales;
                }
                else if (SceneManager.GetActiveScene().name == "Mine_Game"){
                    currentLocalScales = mineGameLocalScales;
                }
                else if (SceneManager.GetActiveScene().name == "Rocket_Flight"){
                    currentLocalScales = rocketGameLocalScales;
                }
            }
            else{
                currentLocalScales = localScalesDict;
            }

            try{
                UI.transform.localScale = currentLocalScales[UI];
                // if(UI.GetComponent<VerticalLayoutGroup>() != null){
                //     UI.GetComponent<VerticalLayoutGroup>().enabled = true;
                // }
            }
            catch(Exception e){
                Debug.LogWarning(gameObject.GetInstanceID() + " MESSED UP LSCALE ON " + UI + " " + UI.GetInstanceID());
                //Debug.Log(mainAreaLocalScales[UI] + "...?");
                if(UI.name.StartsWith("TMP SubMeshUI")){
                    Destroy(UI);
                }
                else{
                    throw e;
                }
            }

        }
    }


    public void DisableUIElement(GameObject UI, bool touchOnly = false){
        for(int i = 0; i < UI.transform.childCount; i++)
        {
            GameObject child = UI.transform.GetChild(i).gameObject;
            DisableUIElement(child, touchOnly: touchOnly);
        }

        if (touchOnly){

            Button button = UI.GetComponent<Button>();
            UnityEngine.UI.ScrollRect scrollRect = UI.GetComponent<UnityEngine.UI.ScrollRect>();

            if (button != null){
                button.enabled = false;
            }

            if (scrollRect != null){
                scrollRect.enabled = false;
            }
        }
        else{

        //     TextMeshProUGUI textMesh = UI.GetComponent<TextMeshProUGUI>();
        //     Image image = UI.GetComponent<Image>();
        //     Button button = UI.GetComponent<Button>();

        //     if (textMesh != null){
        //         textMesh.enabled = false;
        //     }

        //     if (image != null){
        //         image.enabled = false;
        //     }

        //     if (button != null){
        //         button.enabled = false;
        //     }
            if(SceneManager.GetActiveScene().name == "Main_Area" || SceneManager.GetActiveScene().name == "Mine_Game" || SceneManager.GetActiveScene().name == "Rocket_Flight" || SceneManager.GetActiveScene().name == "Landing_Page"){
                UI.transform.localScale = new Vector3(0f, 0f, 0f);
                // if(UI.GetComponent<VerticalLayoutGroup>() != null){
                //     UI.GetComponent<VerticalLayoutGroup>().enabled = true;
                // }
            }

        }
    }


    public static bool UIElementIsEnabled(GameObject UI, bool touchOnly = false){
        bool isEnabled = false;
        if (touchOnly){
            Button button = UI.GetComponent<Button>();
            UnityEngine.UI.ScrollRect scrollRect = UI.GetComponent<UnityEngine.UI.ScrollRect>();

            if (button != null){
                button.enabled = false;
                if(button.enabled){
                    isEnabled = true;
                }
            }

            if (scrollRect != null){
                if(scrollRect.enabled){
                    isEnabled = true;
                }
            }
        }
        else{
            if(SceneManager.GetActiveScene().name == "Main_Area" || SceneManager.GetActiveScene().name == "Mine_Game" || SceneManager.GetActiveScene().name == "Rocket_Flight" || SceneManager.GetActiveScene().name == "Landing_Page"){
                if(!UI.transform.localScale.Equals(new Vector3(0f, 0f, 0f))){
                    isEnabled = true;
                }
            }
        }
        return isEnabled;
    }


    // Speech Banner


    // This code is very good and makes sense
    public void Display_Speech(Speech_Object speech_object, System.Action callBack=null){
        int curSpeechIndex = 0;
        int curCharIndex = 1;
        //SpeechText_TMP
        bool doneDisplayingSpeech = false;
        bool started_next_speech_element = false;

        bool doneDisplayingCurSpeech = false;
        string curSpeechText = "";

        Dictionary<Characters, Dictionary<Emotions, Sound[]>> charsEmotions2Sounds = new Dictionary<Characters, Dictionary<Emotions, Sound[]>>();
        Sound[] allSpeechSounds = new Sound[0];


        void initializeChars2Emotions2Sounds(){
            Characters curChar;
            Emotions curEmot;
            for(int i=0; i<speech_object.speech_strings.Count; i++){
                curChar = speech_object.characters[i];
                curEmot = speech_object.postEmotions[i];
                if(!charsEmotions2Sounds.Keys.Contains(curChar)){
                    charsEmotions2Sounds[curChar] = new Dictionary<Emotions, Sound[]>();
                }
                if(!charsEmotions2Sounds[curChar].Keys.Contains(curEmot)){
                    charsEmotions2Sounds[curChar][curEmot] = UI_Characters.CharacterEmotions2SpeechSounds.getSounds(curChar, curEmot);
                    allSpeechSounds = allSpeechSounds.Concat(charsEmotions2Sounds[curChar][curEmot]).ToArray();
                }
            }
        }

        void adjustBeepPitch(Sound s, Emotions e){
            Audio_Manager.instance.ResetPitch(s);
            // Randomly Adjust Pitch In Some Range Depending on What The Emotion Is
            // Just to give the speech a little more pizazz
            switch(e){
                case Emotions.Idle:
                    Audio_Manager.instance.SetPitch(s, s.origPitch * UnityEngine.Random.Range(0.95f, 1.05f));
                    break;
                case Emotions.Happy:
                    Audio_Manager.instance.SetPitch(s, s.origPitch * s.origPitch * UnityEngine.Random.Range(0.95f, 1.05f));
                    break;
                case Emotions.Sad:
                    Audio_Manager.instance.SetPitch(s, s.origPitch * s.origPitch * UnityEngine.Random.Range(0.95f, 1.05f));
                    break;
                default:
                    break;
            }
        }


        IEnumerator _playSpeechSound(){

            yield return new WaitForSeconds(0);
            playSpeechSound();
        }




        void playSpeechSound(){
            //yield return new WaitForSeconds(0);
            if(curSpeechIndex < speech_object.speech_strings.Count && curCharIndex < speech_object.speech_strings[curSpeechIndex].Length){
                //Debug.Log("SBBP: " + speechBannerButtonPressed + " --- SNSE: " + started_next_speech_element);

                
                if(curCharIndex < curSpeechText.Length && !(allSpeechSounds.Select(s => Audio_Manager.instance.IsPlaying(s)).ToArray().Contains(true))){
                    //yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.025f)); // Wait for a random interval in between doots
                    if(charsEmotions2Sounds[speech_object.characters[curSpeechIndex]][speech_object.postEmotions[curSpeechIndex]].Length > 0){
                        int noiseI = UnityEngine.Random.Range(0, charsEmotions2Sounds[speech_object.characters[curSpeechIndex]][speech_object.postEmotions[curSpeechIndex]].Length);
                        
                        adjustBeepPitch(charsEmotions2Sounds[speech_object.characters[curSpeechIndex]][speech_object.postEmotions[curSpeechIndex]][noiseI], speech_object.postEmotions[curSpeechIndex]);

                        Audio_Manager.instance.Play(charsEmotions2Sounds[speech_object.characters[curSpeechIndex]][speech_object.postEmotions[curSpeechIndex]][noiseI]);
                        //Debug.Log("Playing: " + charsEmotions2Sounds[speech_object.characters[curSpeechIndex]][speech_object.postEmotions[curSpeechIndex]][noiseI].name);
                    }
                }
                else if(curCharIndex == curSpeechText.Length){
                    //Debug.Log("STOPPING DOOTS");
                    foreach(Sound s in allSpeechSounds){
                        Audio_Manager.instance.Stop(s);
                    }
                }
            }
            if(!doneDisplayingSpeech){
                StartCoroutine(_playSpeechSound());
            }
        }





        //<color=#00000000>.</color>
        string generate_speech_string(){
            string genstr = curSpeechText.Substring(0, curCharIndex);
            // if (genstr != curSpeechText){
            //     genstr += '_';
            // }
            for (int i = curCharIndex; i < curSpeechText.Length; i++){
                if (curSpeechText[i] == ' '){
                    genstr += curSpeechText[i];
                }
                else{
                    genstr += "<color=#00000000>" + curSpeechText[i] + "</color>";
                }
            }
            return genstr;
        }


        
        IEnumerator _display_speech_element()
        {    
            started_next_speech_element = false; // New
            //Debug.Log("CSI: " + curSpeechIndex);
            yield return new WaitForSeconds(0);
            if (curSpeechIndex < speech_object.speech_strings.Count){
                SpeechImageAnimator.SetInteger("Character", 0);
                SpeechImageAnimator.SetInteger("Emotion", 0);
                SpeechImageAnimator.SetInteger("Character", (int)speech_object.characters[curSpeechIndex]);
                SpeechImageAnimator.SetInteger("Emotion", (int)speech_object.emotions[curSpeechIndex]);
                curSpeechText = speech_object.speech_strings[curSpeechIndex];
                NameText_TMP.text = characters2DisplayNames[speech_object.characters[curSpeechIndex]];
                //SpeechText_TMP.text = curSpeechText[0].ToString() + "_" + new string("\u00A0"[0], curSpeechText.Length-2);
                curCharIndex = 0;
                SpeechText_TMP.text = generate_speech_string();
                curCharIndex = 1;
                StartCoroutine(_update_displayed_text(curSpeechIndex));
            }
            if(doneDisplayingCurSpeech){
                if (curSpeechIndex >= speech_object.speech_strings.Count){
                    doneDisplayingSpeech = true;
                    DisableUIElement(SpeechBanner);
                    speechIsDisplayed = false;
                    SpeechText_TMP.text = "";
                    // For some reason, a child object gets added to our speech text gameobject and it messes up the UI manager. Just find that and delete it real quick when we're done displaying
                    foreach(Transform textChildTransform in SpeechText.transform){
                        Destroy(textChildTransform.gameObject);
                    }
                    if(callBack != null){
                        callBack();
                    }
                    
                    Vector3[] speechBannerBb = new Vector3[4];
                    SpeechBanner.GetComponent<RectTransform>().GetWorldCorners(speechBannerBb);
                    if (UIDisplayEndedInfo != null){
                        if (speech_object.is_blocker){
                            UIDisplayEndedInfo(new Vector3[0]);
                        }
                        else{
                            UIDisplayEndedInfo(speechBannerBb);
                        }
                    }
                }
                else{
                    doneDisplayingCurSpeech = false;
                    //StartCoroutine(_display_speech_element());
                }
            }  
            //}
        }

        IEnumerator _update_displayed_text(int _cur_speech_index){
            if(Game_Manager.instance.textSpeed == TextSpeed.Fast){
                yield return new WaitForSeconds(0.0075f);
            }
            else if(Game_Manager.instance.textSpeed == TextSpeed.Medium){
                yield return new WaitForSeconds(0.03f);
            }
            else if (Game_Manager.instance.textSpeed == TextSpeed.Slow){
                yield return new WaitForSeconds(0.065f);
            }
            else{
                yield return new WaitForSeconds(0.03f);
            }
            //yield return new WaitForSeconds(0.2f);
            // if(speechBannerButtonPressed){
            //     Debug.Log("SPEECH BANNER PRESSED OUT HERE!!");
            // }
            if (curSpeechIndex == _cur_speech_index){
                if (SpeechText_TMP.text == curSpeechText){
                    SpeechImageAnimator.SetInteger("Character", 0);
                    SpeechImageAnimator.SetInteger("Emotion", 0);
                    SpeechImageAnimator.SetInteger("Character", (int)speech_object.characters[curSpeechIndex]);
                    SpeechImageAnimator.SetInteger("Emotion", (int)speech_object.postEmotions[curSpeechIndex]);
                    if (speechBannerButtonPressed && !started_next_speech_element){
                        curSpeechIndex++;
                        doneDisplayingCurSpeech = true;
                        started_next_speech_element = true;
                        //Debug.Log("BUTTON PRESSED! " + curSpeechIndex);
                        speechBannerButtonPressed = false;
                        StartCoroutine(_display_speech_element());
                    }
                    else{
                        // if(speechBannerButtonPressed){
                        //     Debug.Log("1");
                        // }
                        StartCoroutine(_update_displayed_text(_cur_speech_index));
                    }
                }
                else{
                    if (curCharIndex < curSpeechText.Length && !speechBannerButtonPressed){
                        //SpeechText_TMP.text = curSpeechText.Substring(0, curCharIndex) + "_" + new string("\u00A0"[0], curSpeechText.Length - curCharIndex - 1);
                        SpeechText_TMP.text = generate_speech_string();
                        // string speechS = "Speech_Beep_Gorilla_Happy_" + UnityEngine.Random.Range(0,10).ToString();
                        // Debug.Log(speechS);
                        // Audio_Manager.instance.Play(speechS);
                        //Debug.Log(SpeechText_TMP.text);
                        playSpeechSound();
                        curCharIndex++;
                    }
                    else{
                        SpeechText_TMP.text = curSpeechText;
                        curCharIndex = curSpeechText.Length; // New
                        started_next_speech_element = false;
                        if (speechBannerButtonPressed){
                            speechBannerButtonPressed = false;
                        }
                    }
                    // if(speechBannerButtonPressed){
                    //     Debug.Log("2");
                    // }
                    StartCoroutine(_update_displayed_text(_cur_speech_index));
                }
            }
        }

        if(!speechIsDisplayed){
            Debug.Log("Displaying Speech");
            speechIsDisplayed = true;
            Vector3[] speechBannerBb = new Vector3[4];
            SpeechBanner.GetComponent<RectTransform>().GetWorldCorners(speechBannerBb);
            if(UIDisplayStartedInfo != null){
                if (speech_object.is_blocker){
                    UIDisplayStartedInfo(new Vector3[0]);
                }
                else{
                    UIDisplayStartedInfo(speechBannerBb);
                }
            }

            EnableUIElement(SpeechBanner);
            //touch_detection
            initializeChars2Emotions2Sounds();
            StartCoroutine(_display_speech_element());
            //StartCoroutine(_playSpeechSound());
        }
    }
    // Speech Banner End



    // Localization Update Handlers
    public void onLocaleChanged(){

        

        Thrust_Label_Text.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Banner.Thrust_Text");
        Launch_Label_Text.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Banner.Launches_Text");
        Coins_Label_Text.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Banner.Coins_Text");
        Gems_Label_Text.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Banner.Gems_Text");
    }
    // End Localization Update Handlers


    // Button Handlers Robot Menu
    public void selectMineUpgrade(){
        if(robotMenuDisplayed && !Audio_Manager.instance.IsPlaying("UI_Select_Pane_1")){
            Audio_Manager.instance.Play("UI_Select_Pane_1");
        }
        cartSelectionPanel.GetComponent<Image>().sprite = unselectedSprite;
        mineSelectionPanel.GetComponent<Image>().sprite = selectedSprite;
        cartSelectionPanelText.GetComponent<TextMeshProUGUI>().font = unselectedFont;
        mineSelectionPanelText.GetComponent<TextMeshProUGUI>().font = selectedFont;


        hashingUpgradePriceText.text = Number_String_Formatter.robotMenuFormatPriceText(gameManager.mineGameHitCoinsUpgradePrice);
        blockchainNetworkUpgradePriceText.text = Number_String_Formatter.robotMenuFormatPriceText(gameManager.mineGameSolveCoinsUpgradePrice);
        EnableUIElement(hashingUpgradePanel);
        EnableUIElement(blockchainNetworkUpgradePanel);
        DisableUIElement(graphicsCardUpgradePanel);
        DisableUIElement(coldStorageUpgradePanel);
        mineUpgradeSelected = true;
    }

    public void selectCartUpgrade(){
        if(robotMenuDisplayed && !Audio_Manager.instance.IsPlaying("UI_Select_Pane_2")){
            Audio_Manager.instance.Play("UI_Select_Pane_2");
        }
        cartSelectionPanel.GetComponent<Image>().sprite = selectedSprite;
        mineSelectionPanel.GetComponent<Image>().sprite = unselectedSprite;
        cartSelectionPanelText.GetComponent<TextMeshProUGUI>().font = selectedFont;
        mineSelectionPanelText.GetComponent<TextMeshProUGUI>().font = unselectedFont;


        
        graphicsCardUpgradePriceText.text = Number_String_Formatter.robotMenuFormatPriceText(gameManager.mineCartCoinsPerSecondUpgradePrice);
        coldStorageUpgradePriceText.text = Number_String_Formatter.robotMenuFormatPriceText(gameManager.mineCartCoinsCapacityUpgradePrice);
        EnableUIElement(graphicsCardUpgradePanel);
        EnableUIElement(coldStorageUpgradePanel);
        //DisableUIElement(hashingUpgradePanel);
        //DisableUIElement(blockchainNetworkUpgradePanel);
        mineUpgradeSelected = false;
    }


    IEnumerator RefreshRobotMenuNextFrame(){
        yield return new WaitForSeconds(0);
        if (mineUpgradeSelected){
            selectMineUpgrade();
        }
        else{
            selectCartUpgrade();
        }
    }

    public void onRobotMenuButtonFailed(){
        DisableUIElement(RobotMenuObj, touchOnly: true);
        EnableUIElement(NotEnoughCoinsBox);
        NotEnoughCoinsBoxDisplayed = true;
    }


    public void onHashingUpgradeButtonPressed(){
        if(!speechIsDisplayed && HashingUpgradeButtonPressedInfo != null){
            HashingUpgradeButtonPressedInfo();
        }
        //selectMineUpgrade();
        StartCoroutine(RefreshRobotMenuNextFrame());
    }

    public void onBlockChainNetworkUpgradeButtonPressed(){
        if(!speechIsDisplayed && BlockChainNetworkUpgradeButtonPressedInfo != null){
            BlockChainNetworkUpgradeButtonPressedInfo();
        }
        //selectMineUpgrade();
        StartCoroutine(RefreshRobotMenuNextFrame());
    }

    public void onGraphicsCardUpgradeButtonPressed(){
        if(!speechIsDisplayed && GraphicsCardUpgradeButtonPressedInfo != null){
            GraphicsCardUpgradeButtonPressedInfo();
        }
        //selectCartUpgrade();
        StartCoroutine(RefreshRobotMenuNextFrame());
    }

    public void onColdStorageUpgradeButtonPressed(){
        if(!speechIsDisplayed && ColdStorageUpgradeButtonPressedInfo != null){
            ColdStorageUpgradeButtonPressedInfo();
        }
        //selectCartUpgrade();
        StartCoroutine(RefreshRobotMenuNextFrame());
    }

    public void onSpeechBannerButtonPressed(){
        IEnumerator _setButtonPressedFalse(){
            yield return new WaitForSeconds(0.1f);
            speechBannerButtonPressed = false;
        }
        speechBannerButtonPressed = true;
        //StartCoroutine(_setButtonPressedFalse());
        //Debug.Log("SPEECH BANNER PRESSED " + speechBannerButtonPressed);
    }
    // End Button Handlers Robot Menu








    // Rocket Building Menu

    private void _scaleExperimentsContainerPanel(){
        GameObject tmpUIParentExp = new GameObject();
        foreach(GameObject expPanel in experimentPanels){
            expPanel.transform.SetParent(tmpUIParentExp.transform);
        }

        if(numActiveExperiments > 0){
            experimentsContainerPanel.GetComponent<RectTransform>().offsetMin = new Vector2(experimentsContainerPanel.GetComponent<RectTransform>().offsetMin.x, experimentPanelsPossibleAnchoredPositions[numActiveExperiments-1][1] + experimentContainerPanelsBottomBuffer);
            experimentsContainerPanel.GetComponent<RectTransform>().offsetMax = new Vector2(experimentsContainerPanel.GetComponent<RectTransform>().offsetMin.x, experimentPanelsPossibleAnchoredPositions[0][0] + experimentContainerPanelsTopBuffer);
        }

        foreach(GameObject expPanel in experimentPanels){
            expPanel.transform.SetParent(experimentsContainerPanel.transform);
        }
        Destroy(tmpUIParentExp);
    }

    private void setActiveExperiments(int[] activeExperiments, bool alertExperimentsUpdated = true){
        experimentsContainerPanel.GetComponent<RectTransform>().anchoredPosition = experimentContainerPanelsOrigAnchoredPosition;
        if (activeExperiments.Length < 3){
            labMenuMidPanelScrollRect.enabled = false;
        }
        else{
            labMenuMidPanelScrollRect.enabled = true;
        }
        int curPossibleLoc = 0;
        for(int i=0; i<experimentPanels.Count; i++){
                if (activeExperiments.Contains(i)){
                    //experimentPanels[i].SetActive(true);
                    EnableUIElement(experimentPanels[i]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = experimentPanelsPossibleAnchoredPositions[curPossibleLoc];
                    experimentPanels[i].GetComponent<RectTransform>().position = new Vector2(experimentPanels[i].GetComponent<RectTransform>().position[0], experimentPanelsPossiblePositions[curPossibleLoc][1]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(experimentPanels[i].GetComponent<RectTransform>().anchoredPosition[0], experimentPanelsPossibleAnchoredPositions[curPossibleLoc]);
                    curPossibleLoc++;
                }
                else{
                    //experimentPanels[i].SetActive(false);
                    DisableUIElement(experimentPanels[i]);
                }
        }
        numActiveExperiments = activeExperiments.Length;
        _scaleExperimentsContainerPanel();
        if (ExperimentsUpdatedInfo != null && alertExperimentsUpdated){
            ExperimentsUpdatedInfo();
        }
    }

    public List<int> getActiveExperimentIds(){
        List<int> activeExperimentIds = new List<int>();
        for(int i=0; i<experimentPanels.Count; i++){
            if (experimentPanels[i].activeSelf){
                activeExperimentIds.Add(i);
            }
        }
        return activeExperimentIds;
    }




    private void _scaleResearchContainerPanel(){
        GameObject tmpUIParentRes = new GameObject();
        foreach(GameObject resPanel in researchPanels){
            resPanel.transform.SetParent(tmpUIParentRes.transform);
        }

        researchContainerPanel.GetComponent<RectTransform>().offsetMin = new Vector2(researchContainerPanel.GetComponent<RectTransform>().offsetMin.x, researchPanelsPossibleAnchoredPositions[numActiveResearch-1][1] + researchContainerPanelsBottomBuffer);
        researchContainerPanel.GetComponent<RectTransform>().offsetMax = new Vector2(researchContainerPanel.GetComponent<RectTransform>().offsetMin.x, researchPanelsPossibleAnchoredPositions[0][0] + researchContainerPanelsTopBuffer);

        foreach(GameObject resPanel in researchPanels){
            resPanel.transform.SetParent(researchContainerPanel.transform);
        }

        Destroy(tmpUIParentRes);
    }


    // TODO: DO THIS FOR EXPERIMENTS
    public void enableActiveResearchPanels(){

        List<int> activeResearchIds = researchManager.getUnlockedResearchIds();
        researchContainerPanel.GetComponent<RectTransform>().anchoredPosition = researchContainerPanelsOrigAnchoredPosition;

        int curPossibleLoc = 0;
        for(int i=0; i<researchPanels.Count; i++){
                UnityEngine.Object o1 = researchPanels[i];
                UnityEngine.Object o2 = researchPanels[i].GetComponent<ObjectHolder>();
                Research o3 = (Research)researchPanels[i].GetComponent<ObjectHolder>().Obj;
                int o4 = ((Research)researchPanels[i].GetComponent<ObjectHolder>().Obj).researchId;
                if (activeResearchIds.Contains(((Research)researchPanels[i].GetComponent<ObjectHolder>().Obj).researchId)){
                    if (debugResearch){
                        //Debug.Log("SETTING ACTIVE RESEARCH: " + i + " TO HEIGHT " + researchPanelsPossiblePositions[curPossibleLoc][1] + " --- " + researchPanels[i].transform.Find("Research_Label_Text").gameObject.GetComponent<TextMeshProUGUI>().text);
                    }
                    //researchPanels[i].SetActive(true);
                    EnableUIElement(researchPanels[i]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = experimentPanelsPossibleAnchoredPositions[curPossibleLoc];
                    researchPanels[i].GetComponent<RectTransform>().position = new Vector2(researchPanels[i].GetComponent<RectTransform>().position[0], researchPanelsPossiblePositions[curPossibleLoc][1]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(experimentPanels[i].GetComponent<RectTransform>().anchoredPosition[0], experimentPanelsPossibleAnchoredPositions[curPossibleLoc]);
                    curPossibleLoc++;
                }
                else{
                    //researchPanels[i].SetActive(false);
                    if (debugResearch){
                        //Debug.Log("DISABLING RESEARCH: " + i);
                    }
                    DisableUIElement(researchPanels[i]);
                }
        }


        if (curPossibleLoc < 3){
            labMenuMidPanelScrollRect.enabled = false;
        }
        else{
            labMenuMidPanelScrollRect.enabled = true;
        }
        numActiveResearch = curPossibleLoc;
        _scaleResearchContainerPanel();
        // if (ResearchersUpdatedInfo != null && alertResearchersUpdated){
        //     ResearchersUpdatedInfo();
        // }
    }

    // public List<int> _getActiveResearchIds(){
    //     List<int> currentActiveResearchIds = new List<int>();
    //     foreach (GameObject researchPanel in researchPanels){
    //         //if (researcherPanel.GetComponent<Image>().enabled == true){
    //         if (researchPanel.activeSelf){                                                              /// PROBLEM.. THIS IS ALWAYS GOING TO BE TRUE NOW SINCE WE AREN'T DISABLING ELEMENTS ANYMORE
    //             currentActiveResearchIds.Add(((Research)researchPanel.GetComponent<ObjectHolder>().Obj).researchId);
    //         }
    //     }
    //     Debug.Log("ACTIVE RESEARCH IDS: " + string.Join(", ", currentActiveResearchIds));
    //     return currentActiveResearchIds;
    // }

    public List<int> getUnlockedResearchIds(){
        return _getActiveResearchIds();
    }

    public List<int> _getActiveResearchIds(){
        //Debug.Log("GETTING UNLOCKED RESEARCH IDS: ");
        //Debug.Log("UNLOCKED RESEARCH IDS ARE: " + researchManager.unlockedResearchIDs);
        return researchManager.unlockedResearchIDs;
    }
    



    private string getResearchPanelName(GameObject researchPanel){
        if (researchPanel.transform.parent.gameObject.name != "Research_Container_Panel"){
            throw new ArgumentException("Must pass a Research UI Panel to getResearchPanelName()... got " + researchPanel);
        }
        foreach (Transform child in researchPanel.transform){
            //Debug.Log("CHILD NAME: " + child.gameObject.name);
            if (child.gameObject.name == "Research_Label_Text"){
                return child.gameObject.GetComponent<TextMeshProUGUI>().text;
            }
        }
        return "";
    }






    // private void _scaleExperimentsContainerPanel(){
    //     GameObject tmpUIParentRes = new GameObject();
    //     foreach(GameObject expPanel in experimentPanels){
    //         expPanel.transform.SetParent(tmpUIParentRes.transform);
    //     }

    //     experimentContainerPanel.GetComponent<RectTransform>().offsetMin = new Vector2(experimentContainerPanel.GetComponent<RectTransform>().offsetMin.x, experimentPanelsPossibleAnchoredPositions[numActiveResearch-1][1] + experimentContainerPanelsBottomBuffer);
    //     experimentContainerPanel.GetComponent<RectTransform>().offsetMax = new Vector2(experimentContainerPanel.GetComponent<RectTransform>().offsetMin.x, experimentPanelsPossibleAnchoredPositions[0][0] + experimentContainerPanelsTopBuffer);

    //     foreach(GameObject expPanel in experimentPanels){
    //         expPanel.transform.SetParent(experimentContainerPanel.transform);
    //     }

    //     Destroy(tmpUIParentRes);
    // }








    public void enableActiveExperimentPanels(){

        List<ExperimentId> activeExperimentIdsExp = experimentsManager.getUnlockedExperimentIds();
        List<int> activeExperimentIds = new List<int>();
        foreach(ExperimentId exp in activeExperimentIdsExp){
            // If we have already purchased a persistent experiment, then don't display its panel
            if(!(upgradesManager.upgradesUnlockedDict[(Upgrade)exp] && experimentsManager.experimentId2Experiment[exp].isPersistent)){
                activeExperimentIds.Add((int)exp);
            }
            else{
                //Debug.Log("NOT DISPLAYING EXPERIMENT " + experimentsManager.experimentId2Experiment[exp].experimentName + " WITH ID " + exp + "/"  + (Upgrade)exp + " BECAUSE WE ALREADY BOUGHT IT");
            }
        }
        //Debug.Log("SETTING THESE IDS: " + string.Join(", ", activeExperimentIds));
        experimentsContainerPanel.GetComponent<RectTransform>().anchoredPosition = experimentContainerPanelsOrigAnchoredPosition;

        int curPossibleLoc = 0;
        for(int i=0; i<experimentPanels.Count; i++){
                UnityEngine.Object o1 = experimentPanels[i];
                UnityEngine.Object o2 = experimentPanels[i].GetComponent<ObjectHolder>();
                Experiment o3 = (Experiment)experimentPanels[i].GetComponent<ObjectHolder>().Obj;
                int o4 = (int)(((Experiment)experimentPanels[i].GetComponent<ObjectHolder>().Obj).experimentId);
                if (activeExperimentIds.Contains((int)((Experiment)experimentPanels[i].GetComponent<ObjectHolder>().Obj).experimentId)){
                    if (debugExperiment){
                        //Debug.Log("SETTING ACTIVE EXPERIMENT: " + i + " TO HEIGHT " + experimentPanelsPossiblePositions[curPossibleLoc][1] + " --- " + experimentPanels[i].transform.Find("Upgrade_Label_Text").gameObject.GetComponent<TextMeshProUGUI>().text);
                    }
                    //researchPanels[i].SetActive(true);
                    EnableUIElement(experimentPanels[i]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = experimentPanelsPossibleAnchoredPositions[curPossibleLoc];
                    experimentPanels[i].GetComponent<RectTransform>().position = new Vector2(experimentPanels[i].GetComponent<RectTransform>().position[0], experimentPanelsPossiblePositions[curPossibleLoc][1]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(experimentPanels[i].GetComponent<RectTransform>().anchoredPosition[0], experimentPanelsPossibleAnchoredPositions[curPossibleLoc]);
                    curPossibleLoc++;
                }
                else{
                    //researchPanels[i].SetActive(false);
                    if (debugExperiment){
                        Debug.Log("DISABLING EXPERIMENT: " + i);
                    }
                    DisableUIElement(experimentPanels[i]);
                }
        }


        if (curPossibleLoc < 3){
            labMenuMidPanelScrollRect.enabled = false;
        }
        else{
            labMenuMidPanelScrollRect.enabled = true;
        }
        numActiveExperiments = curPossibleLoc;
        _scaleExperimentsContainerPanel();
        experimentsManager.refreshAllExperimentsPanels();
        // if (ResearchersUpdatedInfo != null && alertResearchersUpdated){
        //     ResearchersUpdatedInfo();
        // }
    }










    // End Rocket Building Menu


    // Button Handlers Rocket Building Menu

   public void selectResearch(){
        //Debug.Log("SELECTING RESEARCH");
        if(rocketBuildingMenuDisplayed && !Audio_Manager.instance.IsPlaying("UI_Select_Pane_1")){
            Audio_Manager.instance.Play("UI_Select_Pane_1");
        }
        researchSelectionPanel.GetComponent<Image>().sprite = selectedSprite;
        experimentsSelectionPanel.GetComponent<Image>().sprite = unselectedSprite;
        researchSelectionPanelText.GetComponent<TextMeshProUGUI>().font = selectedFont;
        experimentsSelectionPanelText.GetComponent<TextMeshProUGUI>().font = unselectedFont;

        labMenuMidPanelScrollRect.content = researchContainerPanel.GetComponent<RectTransform>();

        EnableUIElement(researchContainerPanel);
        researchManager.refreshAllResearchPanels();

        DisableUIElement(experimentsContainerPanel);
        researchMenuDisplayed = true;

        // New
        //setActiveResearch(_getActiveResearchIds().ToArray(), false);
        enableActiveResearchPanels();
        researchManager.refreshAllResearchPanels();
        //mineUpgradeSelected = true;

        

    }

    public void selectExperiments(){
        if(rocketBuildingMenuDisplayed && !Audio_Manager.instance.IsPlaying("UI_Select_Pane_2")){
            Audio_Manager.instance.Play("UI_Select_Pane_2");
        }
        researchSelectionPanel.GetComponent<Image>().sprite = unselectedSprite;
        experimentsSelectionPanel.GetComponent<Image>().sprite = selectedSprite;
        researchSelectionPanelText.GetComponent<TextMeshProUGUI>().font = unselectedFont;
        experimentsSelectionPanelText.GetComponent<TextMeshProUGUI>().font = selectedFont;

        labMenuMidPanelScrollRect.content = experimentsContainerPanel.GetComponent<RectTransform>();

        EnableUIElement(experimentsContainerPanel);
        experimentsManager.refreshAllExperimentsPanels();

        DisableUIElement(researchContainerPanel);
        researchMenuDisplayed = false;
        
        //mineUpgradeSelected = true;
        enableActiveExperimentPanels();
        experimentsManager.refreshAllExperimentsPanels();
    }


    public void experimentPanelButtonHandler(GameObject ExperimentPanel){
        Experiment experiment = (Experiment)ExperimentPanel.GetComponent<ObjectHolder>().Obj;
        ExperimentId experimentId = experiment.experimentId;
        Denomination experimentDenomination = experiment.denomination;
        double price = experiment.price;


        bool transactionExecuted = false; // Did pressing the button result in the Experiment being purchased
        
        //Debug.Log("YOYOYO WE CLICKED EXPERIMENT: " + experimentId);
        // If our experiment has a corresponding upgrade
        if(Enum.GetValues(typeof(Upgrade)).Cast<int>().ToList().IndexOf((int)experimentId) != -1){
            Upgrade upgradeId = upgradesManager.experimentId2Upgrade[experimentId];
            if(upgradesManager.upgradesUnlockedDict[upgradeId] == false || (upgradesManager.upgradesNumberDict[upgradeId] < upgradesManager.upgradesMaxNumberDict[upgradeId])){
                if(experimentDenomination == Denomination.Gems && gameManager.gems >= price){
                    upgradesManager.upgradesUnlockedDict[upgradeId] = true;
                    upgradesManager.upgradesNumberDict[upgradeId] += 1;
                    gameManager.gems -= price;
                    transactionExecuted = true;
                }
                else if(experimentDenomination == Denomination.Coins && gameManager.coins >= price){
                    upgradesManager.upgradesUnlockedDict[upgradeId] = true;
                    upgradesManager.upgradesNumberDict[upgradeId] += 1;
                    gameManager.coins -= price;
                    gameManager.playInterstitialAdOnMenuClose = true;
                    transactionExecuted = true;
                }
            }
        }
        else{
            
        }

        if(transactionExecuted){
            if(!Audio_Manager.instance.IsPlaying("UI_Button_Confirm")){
                Audio_Manager.instance.Play("UI_Button_Confirm");
            }
            // TODO: Do something here (Sound effect?)
            gameManager.playInterstitialAdOnMenuClose = true;
            enableActiveExperimentPanels();
        }
        else{
            Debug.Log("WE DIDN'T DO ANYTHING WITH EXPERIMENT : " + experimentId);
            // TODO: Do something here
            if(!Audio_Manager.instance.IsPlaying("UI_Button_Deny")){
                Audio_Manager.instance.Play("UI_Button_Deny");
            }
        }
        
    }



    public void researchPanelButtonHandler(GameObject ResearchPanel){
        //Debug.Log("BUTTON PRESSED " + getResearchPanelName(ResearchPanel));
        if (gameManager.coins >= ((Research)ResearchPanel.GetComponent<ObjectHolder>().Obj).price){
            if(!Audio_Manager.instance.IsPlaying("UI_Button_Confirm")){
                Audio_Manager.instance.Play("UI_Button_Confirm");
            }
            EnableUIElement(ResearchersMenu);
            enableActiveResearchersPanels();
            //setActiveResearchers(_getActiveResearcherIds().ToArray()); // NEW
            researchersMenuDisplayed = true;
            selectedResearchPanel = ResearchPanel;
        }
        else{
            if(!Audio_Manager.instance.IsPlaying("UI_Button_Deny")){
                Audio_Manager.instance.Play("UI_Button_Deny");
            }
            DisableUIElement(RocketBuildingMenuObj, touchOnly: true);
            EnableUIElement(NotEnoughCoinsBox);
            NotEnoughCoinsBoxDisplayed = true;
        }
    }

    public void finishResearchButtonHandler(GameObject researchPanel){
        //Debug.Log("RESEARCH PANEL " + researchPanel.name + " BUTTON PRESSED");
        Research research = (Research)researchPanel.GetComponent<ObjectHolder>().Obj;
        Researcher researcher = research.assignedResearcher;
        double reward = research.thrustReward;
        research.deassignResearcher();
        if(!Audio_Manager.instance.IsPlaying("UI_Button_Process_Complete")){
            Audio_Manager.instance.Play("UI_Button_Process_Complete");
        }
        if (ResearchFinishedInfo != null){
            ResearchFinishedInfo(reward);
        }
        researchManager.refreshResearchPanel(researchPanel);
        //addActiveResearcher(researcher.researcherId, alertResearchersUpdated: true);
    }
    // End Button Handlers Rocket Building Menu
    

    // Researchers Menu
    public void onResearchersRefreshed(){
        //Debug.Log("Researcher Refreshed From UI Controller");
    }

    public void researchersIdCardButtonHandler(GameObject IDCard){
        //Debug.Log("PRESSED ID CARD " + IDCard.name);

        GameObject curGameObj;
        foreach (Transform child in ResearchersConfirmationBox.transform){
            curGameObj = child.gameObject;
            
            if (curGameObj.name == "Research_Confirmation_Text"){
                string t = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.Text");
                t = t.Replace("{researcher}", IDCard.transform.Find("Researcher_ID_Name_Text").GetComponent<TextMeshProUGUI>().text);
                t = t.Replace("{research}", selectedResearchPanel.transform.Find("Research_Label_Text").GetComponent<TextMeshProUGUI>().text);
                curGameObj.GetComponent<TextMeshProUGUI>().text = t;

                // curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.Text_0") + " " + 
                //                                                 IDCard.transform.Find("Researcher_ID_Name_Text").GetComponent<TextMeshProUGUI>().text + " " + 
                //                                                 localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.Text_1") + " " + 
                //                                                 selectedResearchPanel.transform.Find("Research_Label_Text").GetComponent<TextMeshProUGUI>().text + 
                //                                                 localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.Text_2");

            }
            else if (curGameObj.name == "Research_Confirmation_Yes_Button"){
                foreach (Transform bchild in curGameObj.transform){
                    if (bchild.gameObject.name == "Research_Confirmation_Yes_Button_Text"){
                        bchild.gameObject.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.Yes");
                    }
                }
            }
            else if (curGameObj.name == "Research_Confirmation_No_Button"){
                foreach (Transform bchild in curGameObj.transform){
                    if (bchild.gameObject.name == "Research_Confirmation_No_Button_Text"){
                        bchild.gameObject.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ConfirmationBox.No");
                    }
                }
            }
        }

        EnableUIElement(ResearchersConfirmationBox);
        selectedResearcherPanel = IDCard;
        researchersConfirmationBoxDisplayed = true;
        DisableUIElement(RocketBuildingMenuObj, touchOnly: true);
        DisableUIElement(ResearchersMenu, touchOnly: true);
        if(!Audio_Manager.instance.IsPlaying("UI_Button_No_Effect")){
            Audio_Manager.instance.Play("UI_Button_No_Effect");
        }
    }
    // End Researchers Menu

    // Researchers Menu Button Handlers
    private void _scaleresearchersMenuContainerPanel(){
        GameObject tmpUIParentResearch = new GameObject();
        foreach(GameObject researcherPanel in researchersPanels){
            researcherPanel.transform.SetParent(tmpUIParentResearch.transform);
        }
        
        if(numActiveResearchers > 0){
            researchersMenuContainerPanel.GetComponent<RectTransform>().offsetMin = new Vector2(researchersMenuContainerPanel.GetComponent<RectTransform>().offsetMin.x, researchersPanelsPossibleAnchoredPositions[numActiveResearchers-1][1] + researchersMenuContainerPanelsBottomBuffer);
            researchersMenuContainerPanel.GetComponent<RectTransform>().offsetMax = new Vector2(researchersMenuContainerPanel.GetComponent<RectTransform>().offsetMin.x, researchersPanelsPossibleAnchoredPositions[0][0] + researchersMenuContainerPanelsTopBuffer);
        }
        else{
            // TODO: WHAT GOES HERE.. IF WE ARE OUT OF RESEARCHERS
        }

        foreach(GameObject researcherPanel in researchersPanels){
            researcherPanel.transform.SetParent(researchersMenuContainerPanel.transform);
        }

        Destroy(tmpUIParentResearch);
    }



    public void enableActiveResearchersPanels(){
        //Debug.Log("SETTING ACTIVE RESEARCHERS: " + string.Join(", ", activeresearchersids.ToList()));
        List<int> assignedResearcherIds = new List<int>();
        List<int> currentUnlockedIds = researchManager.getUnlockedResearchIds();
        foreach(Research currentResearch in researchManager.researchList){
            if (currentUnlockedIds.Contains(currentResearch.researchId) && currentResearch.isResearcherAssigned()){
                assignedResearcherIds.Add(currentResearch.assignedResearcher.researcherId);
            }
        }

        //Debug.Log("ASSIGNED RESEARCHERS IDS: " + string.Join(", ", assignedResearcherIds));

        List<int> activeresearchersids = researchersManager.getUnlockedResearchersIds();
        researchersMenuContainerPanel.GetComponent<RectTransform>().anchoredPosition = researchersMenuContainerPanelsOrigAnchoredPosition;
        int curPossibleLoc = 0;
        int curResearcherId = -1;
        for(int i=0; i<researchersPanels.Count; i++){
                curResearcherId = ((Researcher)researchersPanels[i].GetComponent<ObjectHolder>().Obj).researcherId;
                if (activeresearchersids.Contains(curResearcherId) && !assignedResearcherIds.Contains(curResearcherId)){
                    //researchersPanels[i].SetActive(true);
                    EnableUIElement(researchersPanels[i]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = experimentPanelsPossibleAnchoredPositions[curPossibleLoc];
                    researchersPanels[i].GetComponent<RectTransform>().position = new Vector2(researchersPanels[i].GetComponent<RectTransform>().position[0], researchersPanelsPossiblePositions[curPossibleLoc][1]);
                    //experimentPanels[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(experimentPanels[i].GetComponent<RectTransform>().anchoredPosition[0], experimentPanelsPossibleAnchoredPositions[curPossibleLoc]);
                    curPossibleLoc++;
                }
                else{
                    //researchersPanels[i].SetActive(false);
                    DisableUIElement(researchersPanels[i]);
                }
        }

        if (curPossibleLoc < 3){
            researchersMenuMidPanelScrollRect.enabled = false;
        }
        else{
            researchersMenuMidPanelScrollRect.enabled = true;
        }

        numActiveResearchers = curPossibleLoc;
        _scaleresearchersMenuContainerPanel();
        // if (ResearchersUpdatedInfo != null && alertResearchersUpdated){
        //     ResearchersUpdatedInfo();
        // }
    }
    
    public List<int> _getActiveResearcherIds(){
        List<int> currentActiveResearcherIds = new List<int>();
        foreach (GameObject researcherPanel in researchersPanels){
            //if (researcherPanel.GetComponent<Image>().enabled == true){
            if (researcherPanel.activeSelf){
                currentActiveResearcherIds.Add(((Researcher)researcherPanel.GetComponent<ObjectHolder>().Obj).researcherId);
            }
        }
        //Debug.Log("ACTIVE RESEARCHER IDS: " + string.Join(", ", currentActiveResearcherIds));
        return currentActiveResearcherIds;
    }



    
    // public List<int> getUnlockedResearcherIds(){
    //     List<int> unlockedResearcherIds =  _getActiveResearcherIds();

    //     Research currentResearch;
    //     foreach (GameObject researchPanel in researchPanels){
    //         if (researchPanel.activeSelf){
    //             currentResearch = (Research)researchPanel.GetComponent<ObjectHolder>().Obj;
    //             if (currentResearch.isResearcherAssigned()){
    //                 unlockedResearcherIds.Add(currentResearch.assignedResearcher.researcherId);
    //             }
    //         }
    //     }
    //     return unlockedResearcherIds;
    // }





    public void researchersConfirmationBoxYesButtonHandler(){
        if(!Audio_Manager.instance.IsPlaying("UI_Button_Confirm")){
            Audio_Manager.instance.Play("UI_Button_Confirm");
        }
        //Debug.Log("PRESSED YES! " + selectedResearchPanel.transform.Find("Researcher_Label_Text").GetComponent<TextMeshProUGUI>().text + " " + selectedResearcherPanel.transform.Find("Researcher_ID_Name_Text").GetComponent<TextMeshProUGUI>().text);
        Research selectedResearch = (Research)selectedResearchPanel.GetComponent<ObjectHolder>().Obj;
        Researcher selectedResearcher = (Researcher)selectedResearcherPanel.GetComponent<ObjectHolder>().Obj;
        if(gameManager.coins >= selectedResearch.price){
            if (ResearchersMenuConfirmationBoxYesPressedInfo != null){
                gameManager.coins -= selectedResearch.price;
                int selectedResearchID = selectedResearch.researchId;
                int selectedResearcherID = selectedResearcher.researcherId;
                //Debug.Log("PRESSED YES! RESEARCH ID: " + selectedResearchID  + " RESEARCHER ID: " + selectedResearcherID);
                if (ResearchersMenuConfirmationBoxYesPressedInfo != null){
                    //removeActiveResearcher(selectedResearcherID);
                    ResearchersMenuConfirmationBoxYesPressedInfo(selectedResearchID, selectedResearcherID);
                }
            }
            // DisableUIElement(RocketBuildingMenuObj);
            // DisableUIElement(ResearchersMenu);
            // DisableUIElement(ResearchersConfirmationBox);
            // DisableUIElement(ScreenTintObj);
            EnableUIElement(RocketBuildingMenuObj, touchOnly: true);
            EnableUIElement(ResearchersMenu, touchOnly: true);
            gameManager.playInterstitialAdOnMenuClose = true;
            closeMenus();
        }
        else{
            DisableUIElement(ResearchersConfirmationBox);
            EnableUIElement(NotEnoughCoinsBox);
        }
    }

    public void researchersConfirmationBoxNoButtonHandler(){
        if(!Audio_Manager.instance.IsPlaying("UI_Button_Deny")){
            Audio_Manager.instance.Play("UI_Button_Deny");
        }
        DisableUIElement(ResearchersConfirmationBox);
        //Debug.Log("PRESSED NO! " + selectedResearchPanel + " " + selectedResearcherPanel);
        EnableUIElement(RocketBuildingMenuObj, touchOnly: true);
        EnableUIElement(ResearchersMenu, touchOnly: true);
        selectedResearcherPanel = null;
        enableActiveResearchersPanels();
    }
    // End Researchers Menu Button Handlers

    // Not Enough Coins Box Button Handlers
    public void onNotEnoughCoinsButtonPressed(){
        if(!Audio_Manager.instance.IsPlaying("UI_Button_No_Effect")){
            Audio_Manager.instance.Play("UI_Button_No_Effect");
        }
        EnableUIElement(RocketBuildingMenuObj, touchOnly: true);
        EnableUIElement(ResearchersMenu, touchOnly: true);
        EnableUIElement(RobotMenuObj, touchOnly: true);
        DisableUIElement(ResearchersMenu);
        DisableUIElement(NotEnoughCoinsBox);
    }
    // End Not Enough Coins Box Button Handlers

    // autopilot Confirmation Button Handlers
    public void displayAutoPilotConfirmationBox(bool enable){
        if (enable){
            EnableUIElement(autopilotConfirmationBox);
            autopilotConfirmationBoxText.text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Autopilot.Confirmation_Box_Text");
            autopilotConfirmationBoxYesText.text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Autopilot.Confirmation_Box_Text.Yes");
            autopilotConfirmationBoxNoText.text = localizationManager.GetLocalizedString(main_area_ui_table, "UI.Autopilot.Confirmation_Box_Text.No");
        }
        else{
            DisableUIElement(autopilotConfirmationBox);
        }
    }

    public void autopilotConfirmationBoxYesButtonHandler(){
        displayAutoPilotConfirmationBox(false);
        if(AutopilotSelectedInfo != null){
            AutopilotSelectedInfo(true);
        }
    }

    public void autopilotConfirmationBoxNoButtonHandler(){
        displayAutoPilotConfirmationBox(false);
        if(AutopilotSelectedInfo != null){
            AutopilotSelectedInfo(false);
        }
    }
    // End autopilot Confirmation Button Handlers

    // For use with scroll rects that aren't shitty, i.e. ones based off the bookshelf menu prefab
    // Not sure why... but if we only do this set on one frame, Unity overrides it and sets it to .5 immediately after
    public IEnumerator setScrollRectToTop(ScrollRect sr){
        float t = 0;
        while(t < 0.05){
            yield return new WaitForSeconds(0);
            sr.verticalNormalizedPosition = 1f;
            t += Time.deltaTime;
        }
        //Canvas.ForceUpdateCanvases();
    }



    // Bookshelf Menu Button Handlers
    public void onBookshelfTapped(){
        if (!bookshelfMenuDisplayed && !computerMenuDisplayed){
            EnableUIElement(bookshelfMenu);
            EnableUIElement(ScreenTintObj);
            bookshelfMenuDisplayed = true;

            Touch_Detection.instance.disableReticle(disableswipes:true);

            selectOptions();
        } 
    }

    bool monitoring = false;
    public void selectOptions(){


        Options_Selection_Panel.GetComponent<Image>().sprite = selectedSprite;
        Records_Selection_Panel.GetComponent<Image>().sprite = unselectedSprite;
        Options_Selection_Panel_Text.GetComponent<TextMeshProUGUI>().font = selectedFont;
        Records_Selection_Panel_Text.GetComponent<TextMeshProUGUI>().font = unselectedFont;

        soundFxLevelSlider.value = Game_Manager.instance.soundFxSoundLevel;
        musicLevelSlider.value = Game_Manager.instance.musicSoundLevel;

        //foreach(Toggle t in textSpeedCheckboxGroup.registeredToggles){
            //Debug.Log(t.gameObject.name + " --- " + t.gameObject.GetComponent<TextSpeedHolder>().textSpeed + " --- " + (t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed));
            //Debug.Log("AY: " + textSpeedCheckboxGroup.registeredToggles.Where(t => (t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed)).ToList());
            //Debug.Log("AY: " + textSpeedCheckboxGroup.registeredToggles.Where(t => (t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed)).ToList().Count());
        //}
        //Debug.Log(string.Join(",", textSpeedCheckboxGroup.registeredToggles.Where(t => t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed).ToList()));
        //Debug.Log("WOO: " + textSpeedCheckboxGroup.registeredToggles.Where(t => t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed).ToList()[0]);
        textSpeedCheckboxGroup.SelectCheckbox(textSpeedCheckboxGroup.registeredToggles.Where(t => t.gameObject.GetComponent<TextSpeedHolder>().textSpeed == Game_Manager.instance.textSpeed).ToList()[0]);
        // optionsScrollRect.content = null;
        
        
        //DisableUIElement(Records_Container_Panel);
        //EnableUIElement(Bookshelf_Container_Panel);
        bookshelfScrollRect.content = GameObject.Find("Options_Scroll_Panel").GetComponent<RectTransform>();
        // optionsScrollRect.content = GameObject.Find("Bookshelf_Container_Panel").GetComponent<RectTransform>();
        
        
        DisableUIElement(GameObject.Find("Records_Scroll_Panel"));
        EnableUIElement(GameObject.Find("Options_Scroll_Panel"));


        //optionsScrollRect.verticalNormalizedPosition = 1f;
        //Debug.Log(optionsScrollRect.verticalNormalizedPosition);
        // IEnumerator _setVertPos(){
        //     yield return new WaitForEndOfFrame();
        //     optionsScrollRect.enabled = false;
        //     optionsScrollRect.enabled = true;
        //     optionsScrollRect.verticalNormalizedPosition = 1f;

        // }
        //optionsScrollRect.SetNormalizedPosition(1f, 1);
        //Canvas.ForceUpdateCanvases();
        //StartCoroutine(setScrollRectToTop(optionsScrollRect));
        StartCoroutine(setScrollRectToTop(bookshelfScrollRect));
        //StartCoroutine(_selectOptionsNextFrame());
        // IEnumerator monitorOptions(){
        //     monitoring = true;
        //     while(monitoring){
        //         yield return new WaitForSeconds(0);
        //         Debug.Log("OSRP: " + optionsScrollRect.verticalNormalizedPosition);
        //     }
        // }

        // optionsScrollRect.verticalNormalizedPosition = 1f;
        // StartCoroutine(_setVertPos());

    }


    public void selectRecords(){

        Options_Selection_Panel.GetComponent<Image>().sprite = unselectedSprite;
        Records_Selection_Panel.GetComponent<Image>().sprite = selectedSprite;
        Options_Selection_Panel_Text.GetComponent<TextMeshProUGUI>().font = unselectedFont;
        Records_Selection_Panel_Text.GetComponent<TextMeshProUGUI>().font = selectedFont;

        Highest_Altitude_Value_Text.text = Number_String_Formatter.formatHighestAltitudeText(Game_Manager.instance.metrics.maxAltAllTime); 
        Most_Coins_Value_Text.text = Number_String_Formatter.formatMostCoinsText(Game_Manager.instance.metrics.maxCoinsAlltime);


        Minecart_Capacity_Value_Text.text = Number_String_Formatter.formatMinecartCapacityText(Game_Manager.instance.mineCartCoinsCapacity);
        Minecart_Rate_Value_Text.text = Number_String_Formatter.formatMinecartRateText(Game_Manager.instance.mineCartCoinsPerSecond * 60.0);
        Mineshaft_Per_Swing_Value_Text.text = Number_String_Formatter.formatMineshaftPerSwingText(Game_Manager.instance.mineGameHitCoins);
        Mineshaft_Per_Block_Value_Text.text = Number_String_Formatter.formatMineshaftPerBlockText(Game_Manager.instance.mineGameSolveCoins);



        bookshelfScrollRect.content = GameObject.Find("Records_Scroll_Panel").GetComponent<RectTransform>();

        DisableUIElement(GameObject.Find("Options_Scroll_Panel"));
        EnableUIElement(GameObject.Find("Records_Scroll_Panel"));


        
        //optionsScrollRect.verticalNormalizedPosition = 1f;
        //Debug.Log(optionsScrollRect.verticalNormalizedPosition);
        //DisableUIElement(Bookshelf_Container_Panel);
        //EnableUIElement(Records_Container_Panel);

        //recordsScrollRect.content = GameObject.Find("Records_Container_Panel").GetComponent<RectTransform>();
        // IEnumerator _setVertPos(){
        //     yield return new WaitForEndOfFrame();
        //     recordsScrollRect.enabled = false;
        //     recordsScrollRect.enabled = true;
        //     recordsScrollRect.verticalNormalizedPosition = 1f;

        // }

        //recordsScrollRect.Move
        // recordsScrollRect.verticalNormalizedPosition = 1f;
        // StartCoroutine(_setVertPos());
        //Canvas.ForceUpdateCanvases();
        //StartCoroutine(setScrollRectToTop(recordsScrollRect));
        StartCoroutine(setScrollRectToTop(bookshelfScrollRect));
        
    }

    // End Bookshelf Button Handlers

    // Computer Button Handlers
    public void onComputerTapped(){
        if (!bookshelfMenuDisplayed && !computerMenuDisplayed){ // And computer menu not displayed
            EnableUIElement(computerMenu);
            EnableUIElement(ScreenTintObj);
            computerMenuDisplayed = true;

            Touch_Detection.instance.disableReticle(disableswipes:true);

            //selectOptions();
            selectShop();

            Crypto_Manager.instance.getPricesActiveCryptos();
        }
    }

    public void selectShop(){
        computerScrollRect.content = shopScrollPanel.GetComponent<RectTransform>();

        DisableUIElement(exchangeScrollPanel);
        EnableUIElement(shopScrollPanel);
    }

    public void selectExchange(){
        computerScrollRect.content = exchangeScrollPanel.GetComponent<RectTransform>();

        DisableUIElement(shopScrollPanel);
        EnableUIElement(exchangeScrollPanel);
        if(Crypto_Manager.instance.activeCryptosToPrice != null){

        }
        else{

        }
    }
    // End Computer Button Handlers


    // Rocket Flight UI
    private void updateFuelNeedle(){
        Vector3 calculateFuelNeedleLoc(float curRotation){
            if (curRotation < 0f){
                curRotation = curRotation * -1f;
            }


            Vector3 fuelNeedleNutPos = fuelNeedleNut.GetComponent<RectTransform>().anchoredPosition;
            if (curRotation == 0){
                return fuelNeedle.GetComponent<RectTransform>().anchoredPosition;
            }
            else{
                Vector3 newPos;
                float XLoc = Mathf.Sin(Mathf.Deg2Rad * curRotation) * fuelNeedleDistance;
                float YLoc = Mathf.Cos(Mathf.Deg2Rad * curRotation) * fuelNeedleDistance;
                //float YLoc = Mathf.Pow(Mathf.Pow(XLoc, 2.0f) + Mathf.Pow(fuelNeedleDistance, 2.0f), 0.5f);
                XLoc += fuelNeedleNutPos.x;
                YLoc += fuelNeedleNutPos.y;
                return new Vector3(XLoc, YLoc, fuelNeedleNutPos.z);
            }
        }


        //RectTransform fuelNeedleRectTransform = fuelNeedle.GetComponent<RectTransform>();
        //RectTransform fuelNeedleNutRectTransform = fuelNeedleNut.GetComponent<RectTransform>();
        double startRotationZ =  0.0f;
        double endRotationZ = -90.0f;
        double curRotation = MathUtils.Lerp(startRotationZ, endRotationZ, (1.0 - rocketControl.thrust/gameManager.thrust));
        //Debug.Log("CUR ROTATION: " + curRotation);
        //Vector3 curRot = fuelNeedle
        //Debug.Log("ROCKET CONTROL THRUST RATIO: " + rocketControl.thrust/gameManager.thrust + " AND ROTATION " + curRotation);
        //fuelNeedleRectTransform.rotation = Quaternion.Euler(0f, 0f, (float)curRotation);
        
        RectTransform fuelNeedleFullRectTransform = fuelNeedleFull.GetComponent<RectTransform>();
        fuelNeedleFullRectTransform.eulerAngles = new Vector3(fuelNeedleFullRectTransform.eulerAngles.x, fuelNeedleFullRectTransform.eulerAngles.y, (float)curRotation);
        //fuelNeedleNutRectTransform.rotation = fuelNeedleRectTransform.rotation; // ...? SHOULD WORK??
        //fuelNeedleNutRectTransform.eulerAngles = new Vector3(fuelNeedleNutRectTransform.eulerAngles.x, fuelNeedleNutRectTransform.eulerAngles.y, (float)curRotation);
        

        //Vector3 fuelNeedleLoc = calculateFuelNeedleLoc((float)curRotation);
        //fuelNeedle.GetComponent<RectTransform>().anchoredPosition = fuelNeedleLoc;
    }


    private void updateFuelText(){
        //fuelText.text = localizationManager.GetLocalizedString(ui_rocket_flight_table, "UI.Rocket_Flight.Thrust") + "\n" + Number_String_Formatter.rocketFlightFormatThrustNumberText(rocketControl.thrust, decimals:3);
        fuelText.text = Number_String_Formatter.rocketFlightFormatThrustNumberText(rocketControl.thrust, decimals:3);
    }


    public static string SerializeVector3Array(Vector3[] aVectors)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Vector3 v in aVectors)
        {
            sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append("|");
        }
        if (sb.Length > 0) // remove last "|"
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }





    private void updateAltitudeBar(){
        float altitudePercent = Mathf.Lerp(0, 1, rocketGameManager.rocketAltitude/rocketGameManager.targetAltitude);
        if (altitudePercent < .025f){
            altitudePercent = .025f;
        }
        //Debug.Log("ALTITUDE PERCENT: " + altitudePercent);

        float altitudeProgressBarPosY = altitudeBarProgressOrigPos.y + (altitudePercent * (((-altitudeBarProgressOrigPos.y) + altitudeBarTopImage.GetComponent<RectTransform>().anchoredPosition.y))/2f);
        //Debug.Log("TOP ANCHORED POSITION: " + altitudeBarTopImage.GetComponent<RectTransform>().anchoredPosition);
        //Vector2 sv2 = new Vector2(altitudeBarProgress.GetComponent<RectTransform>().sizeDelta.x, altitudePercent * (altitudeBarTopImage.GetComponent<RectTransform>().anchoredPosition.y + (-altitudeBarProgressOrigPos.y)));
        //Vector3 sv3 = new Vector3(altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition.x, altitudeProgressBarPosY, altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition.x);

        //altitudeBarProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(altitudeBarProgress.GetComponent<RectTransform>().sizeDelta.x, altitudePercent * (altitudeBarTopImage.GetComponent<RectTransform>().anchoredPosition.y + (-altitudeBarProgressOrigPos.y)));
        //altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition = new Vector3(altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition.x, altitudeProgressBarPosY, altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition.x);
    
        float heightNum = Mathf.Pow(gameScaler.Scale_Value_To_Screen_Width(1.0f), -1f) * 1947.3f;

        altitudeBarProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(altitudeBarProgress.GetComponent<RectTransform>().sizeDelta.x, altitudePercent * heightNum);
        altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition = new Vector2(altitudeBarProgress.GetComponent<RectTransform>().anchoredPosition.x, altitudePercent * heightNum / 2f);

        Vector3[] v4 = new Vector3[4];
        //altitudeBarProgress.GetComponent<RectTransform>().GetWorldCorners(v4);
        altitudeBarProgress.GetComponent<RectTransform>().GetLocalCorners(v4);
        //Debug.Log("Local Corners: " + SerializeVector3Array(v4) + " SIZE VECTOR: " + sv2 + " POS VECTOR: " + sv3);
        altitudeBarProgressLight.color = altitudeBarProgressLightColor; // Maybe explicitly setting the color to the original each frame will fix the wrong color issue?
        _altitudeBarProgressLightShapePath.SetValue(altitudeBarProgressLight, v4);
    }
    
    private void updateAltitudeText(){
        // string s = localizationManager.GetLocalizedString(ui_rocket_flight_table, "UI.Rocket_Flight.Altitude") + "\n" + Number_String_Formatter.rocketFlightFormatAltitudeNumberText(rocketGameManager.rocketAltitude);
        // Debug.Log("HELLO: " + s);
        // Debug.Log("YELLO: " + altitudeText);
        altitudeText.text = localizationManager.GetLocalizedString(ui_rocket_flight_table, "UI.Rocket_Flight.Altitude") + "\n" + Number_String_Formatter.rocketFlightFormatAltitudeNumberText(rocketGameManager.rocketAltitude, decimals:3) + " " + localizationManager.GetLocalizedString(ui_rocket_flight_table, "UI.Rocket_Flight.Altitude_Abbreviation");
    }



    // public void startRewardedAdRocketFlight(){
    //     Debug.Log("SHOULD BE ALERTING AD");
    //     if (AlertRewardedAdAcceptedInfo != null){
    //         Debug.Log("ALERTING AD");
    //         AlertRewardedAdAcceptedInfo();
    //     }
    // }

    // public void passRewardedAdRocketFlight(){
    //     if (AlertRewardedAdRejectedInfo != null){
    //         AlertRewardedAdRejectedInfo();
    //     }
    // }

    public void rocketFlightEnableRewardedAdConfirmationBox(){
        gameManager.metrics.numRewardedAdsOfferedRocketFlight++;
        RocketFlightRewardedAdConfirmationBoxText.text = localizationManager.GetLocalizedString(ui_rocket_flight_table, "UI.Rocket_Flight.Rewarded_Ad_Text");
        EnableUIElement(RocketFlightRewardedAdConfirmationBox);
    }
    
    public void rocketFlightDisableRewardedAdConfirmationBox(){
        DisableUIElement(RocketFlightRewardedAdConfirmationBox);
    }





    // End Rocket Flight UI


    // Mine Game UI
    private void updateMineGameScoreText(){
        MineGameScoreLabelText.text = localizationManager.GetLocalizedString(ui_mine_game_table, "UI.Mine_Game.Coins");
        MineGameScoreText.text = Number_String_Formatter.mineGameFormatScoreNumberText(mineGameManager.score, decimals:3); 
        //ScoreText.text = Math.Round(score, 2).ToString();
    }

    private void updateMineGameTimerText(){
        MineGameTimerLabelText.text = localizationManager.GetLocalizedString(ui_mine_game_table, "UI.Mine_Game.Time");
        MineGameTimerText.text =  Number_String_Formatter.mineGameFormatTimeNumberText(mineGameManager.timer, decimals:0); 
        //TimerText.text = Math.Round(timer).ToString();
    }



    public void startRewardedAd(){
        //Debug.Log("SHOULD BE ALERTING AD");
        if (AlertRewardedAdAcceptedInfo != null){
            //Debug.Log("ALERTING AD");
            AlertRewardedAdAcceptedInfo();
        }
    }

    public void passRewardedAd(){
        if (AlertRewardedAdRejectedInfo != null){
            AlertRewardedAdRejectedInfo();
        }
    }

    public void mineGameEnableRewardedAdConfirmationBox(){
        gameManager.metrics.numRewardedAdsOfferedMineGame++;
        MineGameRewardedAdConfirmationBoxText.text = localizationManager.GetLocalizedString(ui_mine_game_table, "UI.Mine_Game.Rewarded_Ad_Text");
        EnableUIElement(MineGameRewardedAdConfirmationBox);
    }
    
    public void mineGameDisableRewardedAdConfirmationBox(){
        DisableUIElement(MineGameRewardedAdConfirmationBox);
    }

    // End Mine Game UI


    //Landing Page UI
    public void enableRetryConnectBox(){
        StartCoroutine(_enableRetryConnectBox());
    }

    IEnumerator _enableRetryConnectBox(){
        // Debug.Log("WAITING TO DISPLAY --- " + System.DateTime.Now);
        if (!UIElementIsEnabled(Retry_Connect_Box)){ // If for some reason the box gets displayed while we are waiting then abort
            if(minTimeBetweenDisplayRetryConnectBox != timeSinceLastDisplayedRetryConnectBox){
                //Debug.Log("TRYING TO ENABLE CONNECTION BOX UI WAITING");
                yield return new WaitForSeconds(0);
                StartCoroutine(_enableRetryConnectBox());
            }
            else{
                // Debug.Log("ENABLING BOX -- " + System.DateTime.Now);
                //Debug.Log("TRYING TO ENABLE CONNECTION BOX UI DOING IT! " + Retry_Connect_Box);
                EnableUIElement(Retry_Connect_Box);
            }
        }
    }

    public void disableRetryConnectBox(){
        // Debug.Log("DISABLING BOX -- " + System.DateTime.Now);
        DisableUIElement(Retry_Connect_Box);
        timeSinceLastDisplayedRetryConnectBox = 0f;
    }
    //End Landing Page UI



    // Speech Handlers
    public void Display_Autopilot_Result_Speech(AutopilotReturnState autopilotReturnState, double autopilotHeight, int autopilotGems){
        //Speech_Object speechObj = new Speech_Object();
        Speech_Object speechObj = speechObjectGenerator.GenerateAutopilotResultSpeech(autopilotReturnState, autopilotHeight, autopilotGems);
        Display_Speech(speechObj);
    }
    // End Speech Handlers


    // Name Input Handlers
    public void onNameSubmitButtonPressed(){
        
        // If this button is displayed then there better be an onboarding event manager attached to the game manager
        // Onboarding_Manager onboardingManager = gameManager.gameObject.GetComponent<Onboarding_Manager>();
        // if(onboardingManager == null){
        //     return;
        // }
        // onboardingManager.displayName = nameEnterField.text;
        // onboardingManager.displayName
        Debug.Log("YO YO");
        if(NameInputBoxSubmitButtonPressedInfo != null){
            Debug.Log("YO YO YO");
            NameInputBoxSubmitButtonPressedInfo(nameEnterField.text);
        }
    }

    public void onNameSubmitConfirmationButtonPressed(bool selectedYes){
        if(NameSubmitConfirmationButtonPressedInfo != null){
            NameSubmitConfirmationButtonPressedInfo(selectedYes);
        }
    }
    // End Name Input Handlers

    // Coin Name Input Handlers
    public void onCoinNameSubmitButtonPressed(){
        
        Debug.Log("Coin Name Submit Button Pressed From UI Manager");
        // Debug.Log("YO YO");
        if(CoinNameInputBoxSubmitButtonPressedInfo != null){
            Debug.Log("YO YO YO");
            CoinNameInputBoxSubmitButtonPressedInfo(coinNameEnterField.text);
        }
    }

    public void onCoinNameSubmitConfirmationButtonPressed(bool selectedYes){
        if(CoinNameSubmitConfirmationButtonPressedInfo != null){
            CoinNameSubmitConfirmationButtonPressedInfo(selectedYes);
        }
    }
    // Coin Name Input Handlers End

}
