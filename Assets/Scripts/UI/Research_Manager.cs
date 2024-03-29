using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class Research_Manager : MonoBehaviour
{

    private Game_Manager gameManager;
    private UI_Controller uiController;
    private Localization_Manager localizationManager;
    string ui_research_table = "UI_Research";

    public List<int> unlockedResearchIDs {get; private set;}



    private Researcher_Manager researcherManager;


    [SerializeField]
    Sprite research1Sprite;
    string research1Name;
    double research1ExpectedTime = 600; //600.0 / 100.0;
    double research1ExpectedThrust = 100.0;
    double research1Price = 100.0;
    private Research research1;

    [SerializeField]
    Sprite research2Sprite;
    string research2Name;
    double research2ExpectedTime = 1800; // 1800.0 / 100.0;
    double research2ExpectedThrust = 500.0;
    double research2Price = 1000.0;
    private Research research2;

    [SerializeField]
    Sprite research3Sprite;
    string research3Name;
    double research3ExpectedTime = 5400.0; //5400.0 / 100.0;
    double research3ExpectedThrust = 1000.0;
    double research3Price = 5000.0;
    private Research research3;

    [SerializeField]
	Sprite research4Sprite;
    string research4Name;
    double research4ExpectedTime = 10800.0 / 1000.0; // 10800
    double research4ExpectedThrust = 5000.0;
    double research4Price = 25000.0;
    private Research research4;

    [SerializeField]
	Sprite research5Sprite;
    string research5Name;
    double research5ExpectedTime = 21600.0 / 1000.0; // 21600
    double research5ExpectedThrust = 10000.0;
    double research5Price = 100000.0;
    private Research research5;
    
    [SerializeField]
	Sprite research6Sprite;
    string research6Name;
    double research6ExpectedTime = 43200.0 / 1000.0; // 43200
    double research6ExpectedThrust = 25000.0;
    double research6Price = 1000000.0;
    private Research research6;

    [SerializeField]
	Sprite research7Sprite;
    string research7Name;
    double research7ExpectedTime = 86400.0 / 1000.0; // 86400
    double research7ExpectedThrust = 1000000.0;
    double research7Price = 100000000.0;
    private Research research7;




    public List<Research> researchList {get; private set;}

    private GameObject Research_Panel1, Research_Panel2, Research_Panel3, Research_Panel4, Research_Panel5, Research_Panel6, Research_Panel7;
    private ObjectHolder Research_Panel1_Object_Holder, Research_Panel2_Object_Holder, Research_Panel3_Object_Holder, Research_Panel4_Object_Holder, Research_Panel5_Object_Holder, Research_Panel6_Object_Holder, Research_Panel7_Object_Holder;
    private List<GameObject> researchPanelsList;



    public static Research_Manager instance;
    public static int instanceID;


    System.Random random;



    public delegate void ResearchersUpdated(); // Tells the game manager that a change has been made to the research or researchers so it can update its records
    public static event ResearchersUpdated ResearchersUpdatedInfo;




    void OnEnable()
    {
        Localization_Manager.LocaleChangedInfo += onLocaleChanged;
        UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo += onResearchersMenuConfirmationBoxYesPressed;
    }

    void OnDisable()
    {
        Localization_Manager.LocaleChangedInfo -= onLocaleChanged;
        UI_Controller.ResearchersMenuConfirmationBoxYesPressedInfo -= onResearchersMenuConfirmationBoxYesPressed;
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


    
    void OnLevelWasLoaded(){
        if (instance == this){
            if (SceneManager.GetActiveScene().name.StartsWith("Main_Area")){
                researchPanelsList = new List<GameObject>();
 
                Research_Panel1 = GameObject.Find("Research_Panel_1");
                Research_Panel1_Object_Holder = Research_Panel1.GetComponent<ObjectHolder>();
                Research_Panel1_Object_Holder.Obj = research1;
                researchPanelsList.Add(Research_Panel1);

                
                Research_Panel2 = GameObject.Find("Research_Panel_2");
                Research_Panel2_Object_Holder = Research_Panel2.GetComponent<ObjectHolder>();
                Research_Panel2_Object_Holder.Obj = research2;
                researchPanelsList.Add(Research_Panel2);

                
                Research_Panel3 = GameObject.Find("Research_Panel_3");
                Research_Panel3_Object_Holder = Research_Panel3.GetComponent<ObjectHolder>();
                Research_Panel3_Object_Holder.Obj = research3;
                researchPanelsList.Add(Research_Panel3);

                Research_Panel4 = GameObject.Find("Research_Panel_4");
                Research_Panel4_Object_Holder = Research_Panel4.GetComponent<ObjectHolder>();
                Research_Panel4_Object_Holder.Obj = research4;
                researchPanelsList.Add(Research_Panel4);

                Research_Panel5 = GameObject.Find("Research_Panel_5");
                Research_Panel5_Object_Holder = Research_Panel5.GetComponent<ObjectHolder>();
                Research_Panel5_Object_Holder.Obj = research5;
                researchPanelsList.Add(Research_Panel5);

                Research_Panel6 = GameObject.Find("Research_Panel_6");
                Research_Panel6_Object_Holder = Research_Panel6.GetComponent<ObjectHolder>();
                Research_Panel6_Object_Holder.Obj = research6;
                researchPanelsList.Add(Research_Panel6);

                Research_Panel7 = GameObject.Find("Research_Panel_7");
                Research_Panel7_Object_Holder = Research_Panel7.GetComponent<ObjectHolder>();
                Research_Panel7_Object_Holder.Obj = research7;
                researchPanelsList.Add(Research_Panel7);
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        

        researchList = new List<Research>();
        researchPanelsList = new List<GameObject>();

        localizationManager = GameObject.Find("Localizer").GetComponent<Localization_Manager>();


        researcherManager = GameObject.Find("Researcher_Manager").GetComponent<Researcher_Manager>();


        research1Name = "Research Name 1";
        research1 = new Research(
                        ResearchId: 1,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research1.Name"),
                        ExpectedTime: research1ExpectedTime,
                        ExpectedThrust: research1ExpectedThrust,
                        Price: research1Price,
                        ResearchSprite: research1Sprite
                        );
        researchList.Add(research1);
        Research_Panel1 = GameObject.Find("Research_Panel_1");
        Research_Panel1_Object_Holder = Research_Panel1.GetComponent<ObjectHolder>();
        Research_Panel1_Object_Holder.Obj = research1;
        researchPanelsList.Add(Research_Panel1);


        research2Name = "Research Name 2";
        research2 = new Research(
                        ResearchId: 2,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research2.Name"),
                        ExpectedTime: research2ExpectedTime,
                        ExpectedThrust: research2ExpectedThrust,
                        Price: research2Price,
                        ResearchSprite: research2Sprite
                        );
        researchList.Add(research2);
        Research_Panel2 = GameObject.Find("Research_Panel_2");
        Research_Panel2_Object_Holder = Research_Panel2.GetComponent<ObjectHolder>();
        Research_Panel2_Object_Holder.Obj = research2;
        researchPanelsList.Add(Research_Panel2);        

        research3Name = "Research Name 3";
        research3 = new Research(
                        ResearchId: 3,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research3.Name"),
                        ExpectedTime: research3ExpectedTime,
                        ExpectedThrust: research3ExpectedThrust,
                        Price: research3Price,
                        ResearchSprite: research3Sprite
                        );
        researchList.Add(research3);
        Research_Panel3 = GameObject.Find("Research_Panel_3");
        Research_Panel3_Object_Holder = Research_Panel3.GetComponent<ObjectHolder>();
        Research_Panel3_Object_Holder.Obj = research3;
        researchPanelsList.Add(Research_Panel3);


        research4Name = "Research Name 4";
        research4 = new Research(
                        ResearchId: 4,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research4.Name"),
                        ExpectedTime: research4ExpectedTime,
                        ExpectedThrust: research4ExpectedThrust,
                        Price: research4Price,
                        ResearchSprite: research4Sprite
                        );
        researchList.Add(research4);
        Research_Panel4 = GameObject.Find("Research_Panel_4");
        Research_Panel4_Object_Holder = Research_Panel4.GetComponent<ObjectHolder>();
        Research_Panel4_Object_Holder.Obj = research4;
        researchPanelsList.Add(Research_Panel4);


        research5Name = "Research Name 5";
        research5 = new Research(
                        ResearchId: 5,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research5.Name"),
                        ExpectedTime: research5ExpectedTime,
                        ExpectedThrust: research5ExpectedThrust,
                        Price: research5Price,
                        ResearchSprite: research5Sprite
                        );
        researchList.Add(research5);
        Research_Panel5 = GameObject.Find("Research_Panel_5");
        Research_Panel5_Object_Holder = Research_Panel5.GetComponent<ObjectHolder>();
        Research_Panel5_Object_Holder.Obj = research5;
        researchPanelsList.Add(Research_Panel5);


        research6Name = "Research Name 6";
        research6 = new Research(
                        ResearchId: 6,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research6.Name"),
                        ExpectedTime: research6ExpectedTime,
                        ExpectedThrust: research6ExpectedThrust,
                        Price: research6Price,
                        ResearchSprite: research6Sprite
                        );
        researchList.Add(research6);
        Research_Panel6 = GameObject.Find("Research_Panel_6");
        Research_Panel6_Object_Holder = Research_Panel6.GetComponent<ObjectHolder>();
        Research_Panel6_Object_Holder.Obj = research6;
        researchPanelsList.Add(Research_Panel6);


        research7Name = "Research Name 7";
        research7 = new Research(
                        ResearchId: 7,
                        ResearchName: localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research7.Name"),
                        ExpectedTime: research7ExpectedTime,
                        ExpectedThrust: research7ExpectedThrust,
                        Price: research7Price,
                        ResearchSprite: research7Sprite
                        );
        researchList.Add(research7);
        Research_Panel7 = GameObject.Find("Research_Panel_7");
        Research_Panel7_Object_Holder = Research_Panel7.GetComponent<ObjectHolder>();
        Research_Panel7_Object_Holder.Obj = research7;
        researchPanelsList.Add(Research_Panel7);


        refreshAllResearch();
        refreshAllResearchPanels();
    }


    // Update is called once per frame
    void Update()
    {
        if (uiController.researchMenuDisplayed){
            refreshAllResearchPanelsTime();
        }
    }


    public void refreshResearch(Research research){
        research.researchName = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Research" + research.researchId.ToString() + ".Name");
    }

    public void refreshAllResearch(){
        foreach (Research research in researchList){
            refreshResearch(research);
        }
    }

    public void refreshResearchPanel(GameObject researchPanel){ // GOT IT.... PROBLEM IS THAT WE ARE CALLING ENABLE UI ELEMENT ON PARENTS OF LOWER ELEMENTS
        // WE PRETTY MUCH JUST NEED TO SWITCH ON EVERY SINGLE CHILD GAMEOBJECT AND NOT USE ENABLEUIELEMENT ON IT
        // int researchPanelNum = int.Parse(researchPanel.name[researchPanel.name.Length-1].ToString());
        // Research research = researchList[researchPanelNum-1];
        Research research = (Research)researchPanel.GetComponent<ObjectHolder>().Obj;
        //Debug.Log(research);
        //Debug.Log(research.researchId);
        int researchPanelNum = research.researchId;

        //Debug.Log("REFRESHING RESEARCH PANEL NUM: " + researchPanelNum);
        Debug.Assert(researchPanelNum == research.researchId);
        //Debug.Log("REFRESHING GOT THIS RESEARCH: " + research.researchName);


        //researchPanel.GetComponent<Image>().enabled = true;
        //uiController.EnableUIElement(researchPanel);

        refreshResearchPanelTime(researchPanel, callRefreshPanel: false);

        GameObject curGameObj;
        foreach (Transform child in researchPanel.transform){
            curGameObj = child.gameObject;
            if (curGameObj.name == "Research_Label_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = research.researchName;
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Research_Duration_Label_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.DurationLabel") + ":";
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Research_Expected_Thrust_Label_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ThrustLabel") + ":";
                uiController.EnableUIElement(curGameObj);
            }
            else if (curGameObj.name == "Research_Price_Label_Text"){
                if (!research.isResearcherAssigned()){
                    curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.PriceLabel") + ":";
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Price_Text"){
                if (!research.isResearcherAssigned()){
                    curGameObj.GetComponent<TextMeshProUGUI>().text = Number_String_Formatter.formatResearchPriceText(research.price);
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Time_Left_Label_Text"){
                if (research.isResearcherAssigned() && !research.researchComplete){ //research.timeLeft > 0.0){
                    curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.TimeLeft") + ":";
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Assign_Button"){
                if (!research.isResearcherAssigned()){
                    uiController.EnableUIElement(curGameObj);
                    foreach (Transform buttonchild in curGameObj.transform){
                        buttonchild.gameObject.GetComponent<TextMeshProUGUI>().text = System.Text.RegularExpressions.Regex.Unescape(localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ButtonLabel"));
                    }
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Assigned_Researcher_Image"){
                //Debug.Log("AYO THE SIZE IS: " + curGameObj.transform.localScale + gameManager.gameTimeUnix);
                if (research.isResearcherAssigned()){
                    if(UI_Controller.UIElementIsEnabled(curGameObj)){
                        int headshot_i = random.Next(research.assignedResearcher.headshots.Count);
                        //Debug.Log("PICKING RANDOM HEADSHOT: " + headshot_i + " OUT OF " + research.assignedResearcher.headshots.Count);
                        uiController.EnableUIElement(curGameObj);
                        curGameObj.GetComponent<Image>().sprite = research.assignedResearcher.headshots[headshot_i];
                    }
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Assigned_Researcher_Image_Border"){
                if (research.isResearcherAssigned()){
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Assigned_Researcher_Text"){
                if (research.isResearcherAssigned()){
                    uiController.EnableUIElement(curGameObj);
                    curGameObj.GetComponent<TextMeshProUGUI>().text = research.assignedResearcher.name;
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Assigned_Researcher_Label_Text"){
                if (research.isResearcherAssigned()){
                    uiController.EnableUIElement(curGameObj);
                    curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.ResearcherLabel") + ":";
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Bottom_Separator_Panel_1"){
                if (!research.isResearcherAssigned()){
                    uiController.EnableUIElement(curGameObj);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Time_Left_Text"){
                if (research.isResearcherAssigned() && !research.researchComplete){
                    uiController.EnableUIElement(curGameObj);
                    curGameObj.GetComponent<TextMeshProUGUI>().text = Number_String_Formatter.formatTimeForResearchTimeLeftText(research.timeLeft);
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Time_Left_Label_Text"){
                if (research.isResearcherAssigned() && !research.researchComplete){// research.timeLeft > 0.0){
                    uiController.EnableUIElement(curGameObj);
                    curGameObj.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.TimeLeft") + ":";
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Collect_Button"){
                if (research.isResearcherAssigned() && research.researchComplete){// research.timeLeft <= 0.0){
                    uiController.EnableUIElement(curGameObj);
                    foreach (Transform buttonchild in curGameObj.transform){
                        buttonchild.gameObject.GetComponent<TextMeshProUGUI>().text = localizationManager.GetLocalizedString(ui_research_table, "UI.Research.Finish");
                    }
                }
                else{
                    uiController.DisableUIElement(curGameObj);
                }
            }
            else if (curGameObj.name == "Research_Image"){
                curGameObj.GetComponent<Image>().sprite = research.researchSprite;
            }
            else if (curGameObj.name == "Research_Duration_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = Number_String_Formatter.formatTimeForResearchDurationText(research.expectedTime);
            }
            else if (curGameObj.name == "Research_Expected_Thrust_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = Number_String_Formatter.formatResearchExpectedThrustText(research.expectedThrust); 
            }
            else{
                // Debug.Log("AYO: " + curGameObj.name);
                // Debug.Log("WAYO: " + uiController.mainAreaLocalScales[curGameObj]);
                uiController.EnableUIElement(curGameObj);
            }
        }
    // TODO:  If this research has a researcher assigned, then update the sprite to reflect that
    }

    public void refreshAllResearchPanels(){
      foreach (GameObject researchPanel in researchPanelsList){
            if (uiController.debugResearch){
                Debug.Log("REFRESHING PANEL RESEARCH: " + researchPanel.name);
            }
            refreshResearchPanel(researchPanel);
        }
    }

    public void refreshResearchPanelTime(GameObject researchPanel, bool callRefreshPanel = true){
        Research research = (Research)researchPanel.GetComponent<ObjectHolder>().Obj;
        GameObject curGameObj = researchPanel.transform.Find("Research_Time_Left_Text").gameObject;
        double curTimeLeft = research.calculateTimeRemaining();
        if (research.isResearcherAssigned()){
            uiController.EnableUIElement(curGameObj);
            //research.timeLeft -= Time.deltaTime;
            if(!research.researchComplete){
                curGameObj.GetComponent<TextMeshProUGUI>().text = Number_String_Formatter.formatTimeForResearchTimeLeftText(curTimeLeft);
            }
            else if (callRefreshPanel){
                //uiController.DisableUIElement(curGameObj);
                refreshResearchPanel(researchPanel); // If we end on this frame we want to display the button so we need a full refresh
            }
        }
        else{
            uiController.DisableUIElement(curGameObj);
        }
    }

    public void refreshAllResearchPanelsTime(){
        foreach (GameObject research in researchPanelsList){
            //Debug.Log("REFRESHING PANEL: " + research.name);
            refreshResearchPanelTime(research);
        }
    }

    public void onLocaleChanged(){
        refreshAllResearch();
        refreshAllResearchPanels();
    }

    public void onResearchersMenuConfirmationBoxYesPressed(int researchID, int researcherID){
        Research research = null;
        Researcher researcher = null;
        foreach (Research cur_research in researchList){
            //Debug.Log("CHECKING RESEARCH: " + cur_research.researchName + " WITH ID: " + cur_research.researchId + " =?= " + researchID);
            if(cur_research.researchId == researchID){
                research = cur_research;
                //Debug.Log("CHECKING RESEARCH GOT RESEARCH: " + research.researchName);
            }
        }
        foreach (Researcher cur_researcher in researcherManager.researchersList){
            //Debug.Log("CHECKING RESEARCHER: " + cur_researcher.name + " WITH ID: " + cur_researcher.researcherId +  " =?= " + researcherID);
            if (cur_researcher.researcherId == researcherID){
                researcher = cur_researcher;
                //Debug.Log("CHECKING RESEARCHER GOT RESEARCHER: " + researcher.name);
            }
        }
        if (research != null && researcher != null){
            //Debug.Log("FRICK YEA WE GOT RESEARCH " + research.researchName + "   AND RESEARCHER: " + researcher.name);
            research.assignResearcher(researcher);
            if (ResearchersUpdatedInfo != null){
                ResearchersUpdatedInfo();
            }
        }
        else if (research == null){
            throw new ArgumentException("Couldn't find research to assign researcher to.");
        }
        else if (researcher == null){
            throw new ArgumentException("Couldn't find researcher to assign to research.");
        }
        refreshAllResearchPanels(); // TODO: Only update the research panel we need
    }


    // public void setActiveResearch(int[] activeResearchIds, bool alertResearchersUpdated = true){
    //     uiController.setActiveResearch(activeResearchIds, alertResearchersUpdated);
    // }


    public void setUnlockedResearchIds(List<int> unlockedResearchIds, bool alertResearchersUpdated = true){
        unlockedResearchIDs = unlockedResearchIds.Distinct().ToList();
    }

    public void setUnlockedResearchIds(int[] unlockedResearchIds, bool alertResearchersUpdated = true){
        //uiController.setActiveResearch(activeResearchIds, alertResearchersUpdated);
        setUnlockedResearchIds(new List<int>(unlockedResearchIds), alertResearchersUpdated);
    }

    public List<int> getUnlockedResearchIds(){
        //return uiController._getActiveResearchIds();
        return unlockedResearchIDs;
    }

    

    // public List<int> getResearchIds(){
    //     return uiController.getUnlockedResearchIds();
    // }

    public List<ResearchAssignmentObject> generateResearchAssignmentObjects(){
        List<ResearchAssignmentObject> researchAssignmentObjects = new List<ResearchAssignmentObject>();
        
        //Debug.Log("GENERATING RESEARCH ASSIGNMENT OBJECTS");

        //Research currentResearch;
        Researcher currentAssignedResearcher;
        foreach (Research currentResearch in researchList){
            //currentResearch = (Research)researchPanel.GetComponent<ObjectHolder>().Obj;
            //Debug.Log("GENERATING RESEARCH ASSIGNMENT OBJECTS" + " RESEARCH ID : " + currentResearch.researchId + " HAS RESEARCHER? " + currentResearch.isResearcherAssigned());
            if (currentResearch.isResearcherAssigned()){
                currentAssignedResearcher = currentResearch.assignedResearcher;
                //Debug.Log("GENERATING RESEARCH ASSIGNMENT OBJECTS" + " RESEARCH ID: " + currentResearch.researchId + " HAS RESEARCHER: " + currentAssignedResearcher.researcherId + " ASSIGNED");
                researchAssignmentObjects.Add(new ResearchAssignmentObject( researchId: currentResearch.researchId, 
                                                                            researcherId: currentAssignedResearcher.researcherId, 
                                                                            assignedTime: currentResearch.assignedTime, 
                                                                            timeLeft: currentResearch.timeLeft, 
                                                                            thrustReward: currentResearch.thrustReward
                                                                            ));
            }
        }
        return researchAssignmentObjects;
    }


    // public void initializeResearchWithResearchAssignmentObjects(List<ResearchAssignmentObject> researchAssignmentObjects){
    //     uiController.initializeResearchWithResearchAssignmentObjects(researchAssignmentObjects);
    // }


    public void initializeResearchWithResearchAssignmentObjects(List<ResearchAssignmentObject> researchAssignmentObjects){
        //Research currentResearch;
        int currentResearchId;
        //foreach(ResearchAssignmentObject researchAssignmentObject in researchAssignmentObjects){
            //Debug.Log("GOT RESEARCH ASSIGNMENT OBJECT ASSIGNING RESEARCHER: " + researchAssignmentObject.ResearcherId + " TO RESEARCH: " + researchAssignmentObject.ResearchId);
        //}

        Researcher currentResearcherToAssign = null;
        foreach (Research currentResearch in researchList){
            currentResearchId = currentResearch.researchId;
            foreach(ResearchAssignmentObject researchAssignmentObject in researchAssignmentObjects){
                if(researchAssignmentObject.ResearchId == currentResearchId){
                    currentResearcherToAssign = researcherManager.getResearcherById(researchAssignmentObject.ResearcherId);
                    if (currentResearcherToAssign == null){
                        throw new ArgumentException("Couldn't find researcher with Id" + researchAssignmentObject.ResearchId);
                    }
                    currentResearch.assignResearcher(currentResearcherToAssign, researchAssignmentObject.AssignedTime, researchAssignmentObject.TimeLeft, researchAssignmentObject.ThrustReward);
                }
            }
        }
    }







}
