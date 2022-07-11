using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;


[CreateAssetMenu(fileName = "Level_Scriptable_Object", menuName = "ScriptableObjects/Level_Scriptable_Object", order = 5)]
public class Level_Scriptable_Object :  ScriptableObject
{

    public int LevelId { get { return levelId; } private set { levelId = value; } }
    public string LevelName { get { return levelName; } private set { levelName = value; } }

    public int NextLevelId { get { return nextLevelId; } private set { nextLevelId = value; } }


    // What Experiments/Research/Researchers are Unlocked when this level is completed
    public ExperimentId[] UnlockedExperimentIds { get { return unlockedExperimentIds; } private set { unlockedExperimentIds = value; } }
    public int[] UnlockedResearchIds { get { return unlockedResearchIds; } private set { unlockedResearchIds = value; } }
    public int[] UnlockedResearcherIds { get { return unlockedResearcherIds; } private set { unlockedResearcherIds = value; } }

    public Level_End_Event_Scriptable_Object OnLevelCompleteEvent { get { return onLevelCompleteEvent; } private set { onLevelCompleteEvent = value;} }

    public Rocket_Game_Initialization_Scriptable_Object RocketGameInitializationObject { get { return rocketGameInitializationObject; } private set { rocketGameInitializationObject = value;} }
    


    [SerializeField]
    private int levelId;
    [SerializeField]
    private string levelName;

    [SerializeField]
    private int nextLevelId;


    [SerializeField]
    private ExperimentId[] unlockedExperimentIds;

    [SerializeField]
    private int[] unlockedResearchIds;

    [SerializeField]
    private int[] unlockedResearcherIds;

    [SerializeField]
    private Level_End_Event_Scriptable_Object onLevelCompleteEvent; // The event that plays after we beat the level

    [SerializeField]
    private Rocket_Game_Initialization_Scriptable_Object rocketGameInitializationObject;


    
    public void initializeRocketGame(bool freePlayMode){
        //Debug.Log(LevelName + " IS INITIALIZING ROCKET GAME");
        rocketGameInitializationObject.initializeRocketGame(freePlayMode);
    }


    public void Validate(){
        Debug.Assert(UnlockedExperimentIds.Select(id => Experiments_Manager.instance.experimentsList.Select(e => e.experimentId).Contains(id)).All(v => v));
        Debug.Assert(UnlockedResearchIds.Select(id => Research_Manager.instance.researchList.Select(r => r.researchId).Contains(id)).All(v => v));
        Debug.Assert(UnlockedResearcherIds.Select(id => Researcher_Manager.instance.researchersList.Select(r => r.researcherId).Contains(id)).All(v => v));
        rocketGameInitializationObject.Validate();
        Debug.Log("VALIDATED LEVEL: " + LevelName);
    }

}
