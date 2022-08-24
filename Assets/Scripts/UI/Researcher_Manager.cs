using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;


public class Researcher_Manager : MonoBehaviour
{


    private Localization_Manager localizationManager;
    private UI_Controller uiController;
    string ui_researchers_table = "UI_Researchers";

    private List<int> unlockedResearchersIds;
    //private Dictionary<Researcher, Research> researcher2AssignedResearch;


    [SerializeField]
    List<Sprite> r1HeadshotList;
    [SerializeField]
    private Sprite r1Barcode;
    private double r1GenerateTimeMultiplier(){
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private double r1GenerateThrustMultiplier(){
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private Researcher researcher1;


    [SerializeField]
    List<Sprite> r2HeadshotList;
    [SerializeField]
    private Sprite r2Barcode;
    private double r2GenerateTimeMultiplier(){
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private double r2GenerateThrustMultiplier(){
        return Researcher.GetRandomNumber(0.9, 1.5);
    }
    private Researcher researcher2;

    
    [SerializeField]
    List<Sprite> r3HeadshotList;
    [SerializeField]
    private Sprite r3Barcode;
    private double r3GenerateTimeMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private double r3GenerateThrustMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.5);
    }
    private Researcher researcher3;


    [SerializeField]
    List<Sprite> r4HeadshotList;
    [SerializeField]
    private Sprite r4Barcode;
    private double r4GenerateTimeMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private double r4GenerateThrustMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.5);
    }
    private Researcher researcher4;


    [SerializeField]
    List<Sprite> r5HeadshotList;
    [SerializeField]
    private Sprite r5Barcode;
    private double r5GenerateTimeMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.1);
    }
    private double r5GenerateThrustMultiplier(){ // UPDATE
        return Researcher.GetRandomNumber(0.9, 1.5);
    }
    private Researcher researcher5;







    public List<Researcher> researchersList;


    [SerializeField]
    private GameObject ID_Card_1, ID_Card_2, ID_Card_3, ID_Card_4, ID_Card_5;
    private ObjectHolder ID_Card_1_Object_Holder, ID_Card_2_Object_Holder, ID_Card_3_Object_Holder, ID_Card_4_Object_Holder, ID_Card_5_Object_Holder;
    private List<GameObject> idCardList;



    public static Researcher_Manager instance;
    public static int instanceID;


    System.Random random;




    public delegate void ResearchersRefreshed();
    public static event ResearchersRefreshed ResearchersRefreshedInfo;





    void OnEnable()
    {
        Localization_Manager.LocaleChangedInfo += onLocaleChanged;
    }

    void OnDisable()
    {
        Localization_Manager.LocaleChangedInfo -= onLocaleChanged;
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
        if (instanceID == gameObject.GetInstanceID()){
            if (SceneManager.GetActiveScene().name.StartsWith("Main_Area")){
                //Debug.Log("CALLING START RESEARCHER MANAGER.");
                Start();
            }
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();

        researchersList = new List<Researcher>();
        idCardList = new List<GameObject>();

        localizationManager = GameObject.Find("Localizer").GetComponent<Localization_Manager>();
        

        researcher1 = new Researcher(
                                     ResearcherId: 1,
                                     Headshots: r1HeadshotList,
                                     Barcode: r1Barcode,
                                     Name: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher1.Name"),
                                     FavoriteLabel: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher1.FavoriteLabel"),
                                     FavoriteText: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher1.FavoriteText"),
                                     Description: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher1.Description"),
                                     GenerateTimeMultiplier: r1GenerateTimeMultiplier,
                                     GenerateThrustMultiplier: r1GenerateThrustMultiplier);

        researchersList.Add(researcher1);
        ID_Card_1 = GameObject.Find("Researcher_ID_Card_1");
        ID_Card_1_Object_Holder = ID_Card_1.GetComponent<ObjectHolder>();
        ID_Card_1_Object_Holder.Obj = researcher1;
        idCardList.Add(ID_Card_1);


        researcher2 = new Researcher(
                                     ResearcherId: 2,
                                     Headshots: r2HeadshotList,
                                     Barcode: r2Barcode,
                                     Name: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher2.Name"),
                                     FavoriteLabel: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher2.FavoriteLabel"),
                                     FavoriteText: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher2.FavoriteText"),
                                     Description: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher2.Description"),
                                     GenerateTimeMultiplier: r2GenerateTimeMultiplier,
                                     GenerateThrustMultiplier: r2GenerateThrustMultiplier);

        researchersList.Add(researcher2);
        ID_Card_2 = GameObject.Find("Researcher_ID_Card_2");
        ID_Card_2_Object_Holder = ID_Card_2.GetComponent<ObjectHolder>();
        ID_Card_2_Object_Holder.Obj = researcher2;
        idCardList.Add(ID_Card_2);


        researcher3 = new Researcher(
                                     ResearcherId: 3,
                                     Headshots: r3HeadshotList,
                                     Barcode: r3Barcode,
                                     Name: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher3.Name"),
                                     FavoriteLabel: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher3.FavoriteLabel"),
                                     FavoriteText: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher3.FavoriteText"),
                                     Description: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher3.Description"),
                                     GenerateTimeMultiplier: r3GenerateTimeMultiplier,
                                     GenerateThrustMultiplier: r3GenerateThrustMultiplier);

        researchersList.Add(researcher3);
        ID_Card_3 = GameObject.Find("Researcher_ID_Card_3");
        ID_Card_3_Object_Holder = ID_Card_3.GetComponent<ObjectHolder>();
        ID_Card_3_Object_Holder.Obj = researcher3;
        idCardList.Add(ID_Card_3);



        researcher4 = new Researcher(
                                     ResearcherId: 4,
                                     Headshots: r4HeadshotList,
                                     Barcode: r4Barcode,
                                     Name: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher4.Name"),
                                     FavoriteLabel: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher4.FavoriteLabel"),
                                     FavoriteText: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher4.FavoriteText"),
                                     Description: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher4.Description"),
                                     GenerateTimeMultiplier: r4GenerateTimeMultiplier,
                                     GenerateThrustMultiplier: r4GenerateThrustMultiplier);

        researchersList.Add(researcher4);
        ID_Card_4 = GameObject.Find("Researcher_ID_Card_4");
        ID_Card_4_Object_Holder = ID_Card_4.GetComponent<ObjectHolder>();
        ID_Card_4_Object_Holder.Obj = researcher4;
        idCardList.Add(ID_Card_4);



        researcher5 = new Researcher(
                                     ResearcherId: 5,
                                     Headshots: r5HeadshotList,
                                     Barcode: r5Barcode,
                                     Name: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher5.Name"),
                                     FavoriteLabel: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher5.FavoriteLabel"),
                                     FavoriteText: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher5.FavoriteText"),
                                     Description: localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher5.Description"),
                                     GenerateTimeMultiplier: r5GenerateTimeMultiplier,
                                     GenerateThrustMultiplier: r5GenerateThrustMultiplier);

        researchersList.Add(researcher5);
        ID_Card_5 = GameObject.Find("Researcher_ID_Card_5");
        ID_Card_5_Object_Holder = ID_Card_5.GetComponent<ObjectHolder>();
        ID_Card_5_Object_Holder.Obj = researcher5;
        idCardList.Add(ID_Card_5);


        refreshAllResearchers();
        refreshAllIDSprites();
    }
    

    double frameNum = 0;
    // Update is called once per frame
    void Update()
    {
        if (frameNum%2500==0){
            foreach (Researcher researcher in researchersList){
                //print("RESEARCHER NAME: " + researcher.name);
                //print("RESEARCHER FAVORITE: " + researcher.favoriteText + ": " + researcher.favoriteLabel);
                //print("RESEARCHER DESCRIPTION: " + researcher.description);
            }
        }
        frameNum++;
    }

    public void onLocaleChanged(){
        refreshAllResearchers();
        refreshAllIDSprites();
    }

    public void refreshResearcher(Researcher researcher){
        researcher.name = localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher" + researcher.researcherId.ToString() + ".Name");
        researcher.favoriteLabel = localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher" + researcher.researcherId.ToString() + ".FavoriteLabel");
        researcher.favoriteText = localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher" + researcher.researcherId.ToString() + ".FavoriteText");
        researcher.description = localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Researcher" + researcher.researcherId.ToString() + ".Description");
    }

    public void refreshAllResearchers(){
        foreach (Researcher researcher in researchersList){
            refreshResearcher(researcher);
        }
    }


    public void refreshIDSprite(GameObject idCard){
        //int idCardIdNum = int.Parse(idCard.name[idCard.name.Length-1].ToString());
        //Researcher researcher = researchersList[idCardIdNum-1];
        Researcher researcher = (Researcher)idCard.GetComponent<ObjectHolder>().Obj;
        int idCardIdNum = researcher.researcherId;

        //Debug.Log("REFRESHING ID NUM: " + idCardIdNum);
        Debug.Assert(idCardIdNum == researcher.researcherId);
        //Debug.Log("REFRESHING GOT THIS RESEARCHER: " + researcher.name);

        GameObject curGameObj;
        foreach (Transform child in idCard.transform){
            curGameObj = child.gameObject;
            if (curGameObj.name == "Researcher_ID_Headshot"){
                int i = random.Next(researcher.headshots.Count);
                //Debug.Log("REFRESHING HEADSHOT NUM: " + i);
                curGameObj.GetComponent<Image>().sprite = researcher.headshots[i];
            }
            else if (curGameObj.name == "Researcher_ID_Barcode"){
                //Debug.Log("REFRESHING GOT BARCODE");
                curGameObj.GetComponent<Image>().sprite = researcher.barcode;
            }
            else if (curGameObj.name == "Researcher_ID_Name_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = researcher.name;
            }
            else if (curGameObj.name == "Researcher_ID_Info_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = "<u>" + researcher.favoriteLabel + "</u>: " +  researcher.favoriteText + 
                                                                  "\n<u>" + localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Card.Profile") + 
                                                                  " </u>: " +  researcher.description;
            }
            else if (curGameObj.name == "Researcher_ID_Header_Text"){
                curGameObj.GetComponent<TextMeshProUGUI>().text = "<u>" + localizationManager.GetLocalizedString(ui_researchers_table, "UI.Researcher.Card.Header") + "</u>";
            }
        }
    }


    public void refreshAllIDSprites(){
        foreach (GameObject idCard in idCardList){
                refreshIDSprite(idCard);
            }
    }


    private void addUnlockedResearcher(int researcherId){
        if (!unlockedResearchersIds.Contains(researcherId)){
            unlockedResearchersIds.Add(researcherId);
        }
    }

    private void removeUnlockedResearcher(int researcherId){
        if (unlockedResearchersIds.Contains(researcherId)){
            unlockedResearchersIds.Remove(researcherId);
        }
    }


    public void setUnlockedResearchersIds(List<int> unlockedResearchersIdsNew){
        unlockedResearchersIds = unlockedResearchersIdsNew.Distinct().ToList();
        //Debug.Log("UNLOCKED RESEARCHERS ARE: " + string.Join(", ", unlockedResearchersIds));
    }

    public void setUnlockedResearchersIds(int[] unlockedResearchersIdsNew){
        setUnlockedResearchersIds(new List<int>(unlockedResearchersIdsNew));
    }

    


    // public List<int> getActiveResearcherIds(){
    //     return uiController._getActiveResearcherIds();
    // }

    public List<int> getUnlockedResearchersIds(){
        return unlockedResearchersIds;
    }


    public Researcher getResearcherById(int researcherId){
        Researcher foundResearcher = null;
        // Researcher currentResearcher;
        foreach (Researcher currentResearcher in researchersList){
            if (foundResearcher == null){
                if (currentResearcher.researcherId == researcherId){
                    foundResearcher = currentResearcher;
                }
            }
        }
        return foundResearcher;
    }


}
