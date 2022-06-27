using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Experiments_Manager : MonoBehaviour
{

    private Game_Manager gameManager;
    private UI_Controller uiController;
    private Localization_Manager localizationManager;
    string ui_experiments_table = "UI_Experiments";

    public List<ExperimentId> unlockedExperimentIDs;// {get; private set;} // Which Experiments Should We Display in The Menu?
    public Dictionary<ExperimentId, Experiment> experimentId2Experiment;




    //#########################################################################################

    //1
    private ExperimentId experiment1Id = ExperimentId.Autopilot_System;
    private string experiment1Name;
    private string experiment1Description;
    private bool experiment1IsPersistent = true;
    private Denomination experiment1Denomination = Denomination.Gems;
    private double experiment1Price = 10.0;
    private Duration experiment1Duration = Duration.Permanent;
    [SerializeField]
    private Sprite  experiment1Sprite;
    private Experiment experiment1;

    
    //2
    private ExperimentId experiment2Id = ExperimentId.Lateral_Boosters;
    private string experiment2Name;
    private string experiment2Description;
    private bool experiment2IsPersistent = true;
    private Denomination experiment2Denomination = Denomination.Gems;
    private double experiment2Price = 50.0;
    private Duration experiment2Duration = Duration.Permanent;
    [SerializeField]
    private Sprite experiment2Sprite;
    private Experiment experiment2;



    //3
    private ExperimentId experiment3Id = ExperimentId.Particle_Shield;
    private string experiment3Name;
    private string experiment3Description;
    private bool experiment3IsPersistent = true;
    private Denomination experiment3Denomination = Denomination.Gems;
    private double experiment3Price = 500.0;
    private Duration experiment3Duration = Duration.Permanent;
    [SerializeField]
    private Sprite experiment3Sprite;
    private Experiment experiment3;


    
    //4
    private ExperimentId experiment4Id = ExperimentId.Cow_Catcher;
    private string experiment4Name;
    private string experiment4Description;
    private bool experiment4IsPersistent = false;
    private Denomination experiment4Denomination = Denomination.Gems;
    private double experiment4Price = 10.0;
    private Duration experiment4Duration = Duration.Three_Launches;
    [SerializeField]
    private Sprite experiment4Sprite;
    private Experiment experiment4;



    //5
    private ExperimentId experiment5Id = ExperimentId.Turbo_Boost;
    private string experiment5Name;
    private string experiment5Description;
    private bool experiment5IsPersistent = false;
    private Denomination experiment5Denomination = Denomination.Gems;
    private double experiment5Price = 25.0;
    private Duration experiment5Duration = Duration.Three_Launches;
    [SerializeField]
    private Sprite experiment5Sprite;
    private Experiment experiment5;


    
    //6
    private ExperimentId experiment6Id = ExperimentId.Da_Bomb;
    private string experiment6Name;
    private string experiment6Description;
    private bool  experiment6IsPersistent = false;
    private Denomination  experiment6Denomination = Denomination.Gems;
    private double  experiment6Price = 50.0;
    private Duration experiment6Duration = Duration.Three_Uses;
    [SerializeField]
    private Sprite experiment6Sprite;
    private Experiment experiment6;



    //7
    private ExperimentId experiment7Id = ExperimentId.Gem_Magnet;
    private string experiment7Name;
    private string experiment7Description;
    private bool experiment7IsPersistent = true;
    private Denomination experiment7Denomination = Denomination.Coins;
    private double experiment7Price = 1000.0;
    private Duration experiment7Duration = Duration.Permanent;
    [SerializeField]
    private Sprite experiment7Sprite;
    private Experiment experiment7;


    //#########################################################################################



    public List<Experiment> experimentsList {get; private set;}



    private GameObject Experiment_Panel1, Experiment_Panel2, Experiment_Panel3, Experiment_Panel4, Experiment_Panel5, Experiment_Panel6, Experiment_Panel7;
    private ObjectHolder Experiment_Panel1_Object_Holder, Experiment_Panel2_Object_Holder, Experiment_Panel3_Object_Holder, Experiment_Panel4_Object_Holder, Experiment_Panel5_Object_Holder, Experiment_Panel6_Object_Holder, Experiment_Panel7_Object_Holder;
    private List<GameObject> experimentPanelsList;






    public static Experiments_Manager instance;
    public static int instanceID;


    void OnEnable()
    {
        Localization_Manager.LocaleChangedInfo += onLocaleChanged;
        // UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo += onResearchersMenuConfirmationBoxYesPressed;
    }

    void OnDisable()
    {
        Localization_Manager.LocaleChangedInfo -= onLocaleChanged;
        // UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo -= onResearchersMenuConfirmationBoxYesPressed;
    }



    void Awake(){
        if (!instance){
            instance = this;
            instanceID = gameObject.GetInstanceID();
            DontDestroyOnLoad(this.gameObject);       
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

        experimentId2Experiment = new Dictionary<ExperimentId, Experiment>();
        experimentsList = new List<Experiment>();
        experimentPanelsList = new List<GameObject>();

        localizationManager = GameObject.Find("Localizer").GetComponent<Localization_Manager>();



        experiment1Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment1.Name");
        experiment1Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment1.Description");
        experiment1 = new Experiment(
                        ExperimentId: experiment1Id,
                        ExperimentName: experiment1Name,
                        ExperimentDescription: experiment1Description,
                        IsPersistent: experiment1IsPersistent,
                        Denomination: experiment1Denomination,
                        Price: experiment1Price,
                        Duration: experiment1Duration,
                        ExperimentSprite: experiment1Sprite
                        );
        experimentsList.Add(experiment1);
        Experiment_Panel1 = GameObject.Find("Experiment_Upgrade_Panel_1"); 
        Experiment_Panel1_Object_Holder = Experiment_Panel1.GetComponent<ObjectHolder>();
        Experiment_Panel1_Object_Holder.Obj = experiment1;
        experimentPanelsList.Add(Experiment_Panel1);
        experimentId2Experiment[experiment1Id] = experiment1;

        
        experiment2Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment2.Name");
        experiment2Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment2.Description");
        experiment2 = new Experiment(
                        ExperimentId: experiment2Id,
                        ExperimentName: experiment2Name,
                        ExperimentDescription: experiment2Description,
                        IsPersistent: experiment2IsPersistent,
                        Denomination: experiment2Denomination,
                        Price: experiment2Price,
                        Duration: experiment2Duration,
                        ExperimentSprite: experiment2Sprite
                        );
        experimentsList.Add(experiment2);
        Experiment_Panel2 = GameObject.Find("Experiment_Upgrade_Panel_2"); 
        Experiment_Panel2_Object_Holder = Experiment_Panel2.GetComponent<ObjectHolder>();
        Experiment_Panel2_Object_Holder.Obj = experiment2;
        experimentPanelsList.Add(Experiment_Panel2);
        experimentId2Experiment[experiment2Id] = experiment2;


        experiment3Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment3.Name");
        experiment3Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment3.Description");
        experiment3 = new Experiment(
                        ExperimentId: experiment3Id,
                        ExperimentName: experiment3Name,
                        ExperimentDescription: experiment3Description,
                        IsPersistent: experiment3IsPersistent,
                        Denomination: experiment3Denomination,
                        Price: experiment3Price,
                        Duration: experiment3Duration,
                        ExperimentSprite: experiment3Sprite
                        );
        experimentsList.Add(experiment3);
        Experiment_Panel3 = GameObject.Find("Experiment_Upgrade_Panel_3"); 
        Experiment_Panel3_Object_Holder = Experiment_Panel3.GetComponent<ObjectHolder>();
        Experiment_Panel3_Object_Holder.Obj = experiment3;
        experimentPanelsList.Add(Experiment_Panel3);
        experimentId2Experiment[experiment3Id] = experiment3;


        experiment4Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment4.Name");
        experiment4Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment4.Description");
        experiment4 = new Experiment(
                        ExperimentId: experiment4Id,
                        ExperimentName: experiment4Name,
                        ExperimentDescription: experiment4Description,
                        IsPersistent: experiment4IsPersistent,
                        Denomination: experiment4Denomination,
                        Price: experiment4Price,
                        Duration: experiment4Duration,
                        ExperimentSprite: experiment4Sprite
                        );
        experimentsList.Add(experiment4);
        Experiment_Panel4 = GameObject.Find("Experiment_Upgrade_Panel_4"); 
        Experiment_Panel4_Object_Holder = Experiment_Panel4.GetComponent<ObjectHolder>();
        Experiment_Panel4_Object_Holder.Obj = experiment4;
        experimentPanelsList.Add(Experiment_Panel4);
        experimentId2Experiment[experiment4Id] = experiment4;


        experiment5Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment5.Name");
        experiment5Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment5.Description");
        experiment5 = new Experiment(
                        ExperimentId: experiment5Id,
                        ExperimentName: experiment5Name,
                        ExperimentDescription: experiment5Description,
                        IsPersistent: experiment5IsPersistent,
                        Denomination: experiment5Denomination,
                        Price: experiment5Price,
                        Duration: experiment5Duration,
                        ExperimentSprite: experiment5Sprite
                        );
        experimentsList.Add(experiment5);
        Experiment_Panel5 = GameObject.Find("Experiment_Upgrade_Panel_5"); 
        Experiment_Panel5_Object_Holder = Experiment_Panel5.GetComponent<ObjectHolder>();
        Experiment_Panel5_Object_Holder.Obj = experiment5;
        experimentPanelsList.Add(Experiment_Panel5);
        experimentId2Experiment[experiment5Id] = experiment5;

        
        experiment6Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment6.Name");
        experiment6Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment6.Description");
        experiment6 = new Experiment(
                        ExperimentId: experiment6Id,
                        ExperimentName: experiment6Name,
                        ExperimentDescription: experiment6Description,
                        IsPersistent: experiment6IsPersistent,
                        Denomination: experiment6Denomination,
                        Price: experiment6Price,
                        Duration: experiment6Duration,
                        ExperimentSprite: experiment6Sprite
                        );
        experimentsList.Add(experiment6);
        Experiment_Panel6 = GameObject.Find("Experiment_Upgrade_Panel_6"); 
        Experiment_Panel6_Object_Holder = Experiment_Panel6.GetComponent<ObjectHolder>();
        Experiment_Panel6_Object_Holder.Obj = experiment6;
        experimentPanelsList.Add(Experiment_Panel6);
        experimentId2Experiment[experiment6Id] = experiment6;


        experiment7Name = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment7.Name");
        experiment7Description = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment7.Description");
        experiment7 = new Experiment(
                        ExperimentId: experiment7Id,
                        ExperimentName: experiment7Name,
                        ExperimentDescription: experiment7Description,
                        IsPersistent: experiment7IsPersistent,
                        Denomination: experiment7Denomination,
                        Price: experiment7Price,
                        Duration: experiment7Duration,
                        ExperimentSprite: experiment7Sprite
                        );
        experimentsList.Add(experiment7);
        Experiment_Panel7 = GameObject.Find("Experiment_Upgrade_Panel_7"); 
        Experiment_Panel7_Object_Holder = Experiment_Panel7.GetComponent<ObjectHolder>();
        Experiment_Panel7_Object_Holder.Obj = experiment7;
        experimentPanelsList.Add(Experiment_Panel7);
        experimentId2Experiment[experiment7Id] = experiment7;



        foreach (GameObject g in experimentPanelsList){
            //Debug.Log("Added Panel: " + g.name);
        }
    }




    void OnLevelWasLoaded(){
        if (instance == this){
            if (SceneManager.GetActiveScene().name == "Main_Area"){
                experimentPanelsList = new List<GameObject>();
 
                Experiment_Panel1 = GameObject.Find("Experiment_Upgrade_Panel_1");
                Experiment_Panel1_Object_Holder = Experiment_Panel1.GetComponent<ObjectHolder>();
                Experiment_Panel1_Object_Holder.Obj = experiment1;
                experimentPanelsList.Add(Experiment_Panel1);

                
                Experiment_Panel2 = GameObject.Find("Experiment_Upgrade_Panel_2");
                Experiment_Panel2_Object_Holder = Experiment_Panel2.GetComponent<ObjectHolder>();
                Experiment_Panel2_Object_Holder.Obj = experiment2;
                experimentPanelsList.Add(Experiment_Panel2);

                
                Experiment_Panel3 = GameObject.Find("Experiment_Upgrade_Panel_3");
                Experiment_Panel3_Object_Holder = Experiment_Panel3.GetComponent<ObjectHolder>();
                Experiment_Panel3_Object_Holder.Obj = experiment3;
                experimentPanelsList.Add(Experiment_Panel3);

                Experiment_Panel4 = GameObject.Find("Experiment_Upgrade_Panel_4");
                Experiment_Panel4_Object_Holder = Experiment_Panel4.GetComponent<ObjectHolder>();
                Experiment_Panel4_Object_Holder.Obj = experiment4;
                experimentPanelsList.Add(Experiment_Panel4);

                Experiment_Panel5 = GameObject.Find("Experiment_Upgrade_Panel_5");
                Experiment_Panel5_Object_Holder = Experiment_Panel5.GetComponent<ObjectHolder>();
                Experiment_Panel5_Object_Holder.Obj = experiment5;
                experimentPanelsList.Add(Experiment_Panel5);

                Experiment_Panel6 = GameObject.Find("Experiment_Upgrade_Panel_6");
                Experiment_Panel6_Object_Holder = Experiment_Panel6.GetComponent<ObjectHolder>();
                Experiment_Panel6_Object_Holder.Obj = experiment6;
                experimentPanelsList.Add(Experiment_Panel6);

                Experiment_Panel7 = GameObject.Find("Experiment_Upgrade_Panel_7");
                Experiment_Panel7_Object_Holder = Experiment_Panel7.GetComponent<ObjectHolder>();
                Experiment_Panel7_Object_Holder.Obj = experiment7;
                experimentPanelsList.Add(Experiment_Panel7);
            }
        }
    }










    // Update is called once per frame
    void Update()
    {
        
    }

    public void refreshAllExperiments(){
        foreach (Experiment experiment in experimentsList){
            refreshExperiment(experiment);
        }
    }

    public void refreshAllExperimentsPanels(){
      foreach (GameObject experimentPanel in experimentPanelsList){
            if (uiController.debugExperiment){
                Debug.Log("REFRESHING PANEL EXPERIMENT: " + experimentPanel.name);
            }
            refreshExperimentPanel(experimentPanel);
        }
    }





    public void refreshExperiment(Experiment experiment){
        // Don't actually need to do anything here, but in case we do later
    }



    public void refreshExperimentPanel(GameObject experimentPanel){ // GOT IT.... PROBLEM IS THAT WE ARE CALLING ENABLE UI ELEMENT ON PARENTS OF LOWER ELEMENTS
        // WE PRETTY MUCH JUST NEED TO SWITCH ON EVERY SINGLE CHILD GAMEOBJECT AND NOT USE ENABLEUIELEMENT ON IT
        // int researchPanelNum = int.Parse(researchPanel.name[researchPanel.name.Length-1].ToString());
        // Research research = researchList[researchPanelNum-1];
        Experiment experiment = (Experiment)experimentPanel.GetComponent<ObjectHolder>().Obj;
        int experimentIdNum = (int)experiment.experimentId;
        int durationIdNum = (int)experiment.duration;
        
        // Just make sure we havent messed this up
        // Want to make sure the correct experiment is assigned to the correct panel
        int experimentPanelNum = int.Parse(experimentPanel.name[experimentPanel.name.Length-1].ToString());
        char experimentPanelNumChar = experimentPanel.name[experimentPanel.name.Length-1];

        //Debug.Log("EXP NUM: " + experimentIdNum + " --- PANEL NUM: " + experimentPanelNumChar);
        Debug.Assert(experimentIdNum == experimentPanelNum);
        //refreshResearchPanelTime(researchPanel, callRefreshPanel: false);

        GameObject curGameObj;
        foreach (Transform child in experimentPanel.transform){
            curGameObj = child.gameObject;
            if (curGameObj.name == "Upgrade_Button"){
                foreach (Transform buttonchild in curGameObj.transform){
                    buttonchild.gameObject.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Buy");
                }
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Gem_Image"){
                if(experiment.denomination == Denomination.Gems){
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Upgrade_Price_Text_Gems"){
                //TODO: MAKE THIS NUMBER FORMATTED
                if(experiment.denomination == Denomination.Gems){
                    curGameObj.GetComponent<TextMeshProUGUI>().text = experiment.price.ToString(); // TODO: Make format func
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Upgrade_Price_Text_Coins"){
                //TODO: MAKE THIS NUMBER FORMATTED
                if(experiment.denomination == Denomination.Coins){
                    curGameObj.GetComponent<TextMeshProUGUI>().text = experiment.price.ToString(); // TODO: Make format func
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Upgrade_Image"){
                curGameObj.GetComponent<Image>().sprite = experiment.experimentSprite;
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Upgrade_Description_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment" + experimentIdNum.ToString() + ".Description");
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Upgrade_Duration_Label_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.DurationLabel") + ":";
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Upgrade_Duration_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Duration" + durationIdNum);
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Upgrade_Label_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_experiments_table, "UI.Experiment.Experiment" + experimentIdNum.ToString() + ".Name");
                uiController.EnableUIElement(curGameObj);
            }
            else{
                // Debug.Log("AYO: " + curGameObj.name);
                // Debug.Log("WAYO: " + uiController.mainAreaLocalScales[curGameObj]);
                uiController.EnableUIElement(curGameObj);
            }
        }
    // TODO:  If this research has a researcher assigned, then update the sprite to reflect that
    }

    public void onLocaleChanged(){
        refreshAllExperiments();
        refreshAllExperimentsPanels();
    }



    public void setUnlockedExperimentIds(List<int> UnlockedExperimentIds, bool alertExperimentsUpdated = false){
        List<ExperimentId> experimentIdList = new List<ExperimentId>();
        foreach(int xid in UnlockedExperimentIds){
            if (System.Enum.IsDefined(typeof(ExperimentId), xid))
            {
                experimentIdList.Add((ExperimentId)xid);
            }
            else
            {
                throw new System.ArgumentException("Cannot set unlocked experiments " + xid + " does not correspond to any ExperimentId");
            }
        }
        setUnlockedExperimentIds(experimentIdList, alertExperimentsUpdated);
    }

    public void setUnlockedExperimentIds(int[] unlockedExperimentIds, bool alertExperimentsUpdated = false){
        //uiController.setActiveResearch(activeResearchIds, alertResearchersUpdated);
        setUnlockedExperimentIds(new List<int>(unlockedExperimentIds), alertExperimentsUpdated);
    }



    
    public void setUnlockedExperimentIds(List<ExperimentId> UnlockedExperimentIds, bool alertExperimentsUpdated = false){
        unlockedExperimentIDs = UnlockedExperimentIds.Distinct().ToList();
    }

    public void setUnlockedExperimentIds(ExperimentId[] unlockedExperimentIds, bool alertExperimentsUpdated = false){
        //uiController.setActiveResearch(activeResearchIds, alertResearchersUpdated);
        setUnlockedExperimentIds(new List<ExperimentId>(unlockedExperimentIds), alertExperimentsUpdated);
    }

    public List<ExperimentId> getUnlockedExperimentIds(){
        //return uiController._getActiveResearchIds();
        return unlockedExperimentIDs;
    }



}
