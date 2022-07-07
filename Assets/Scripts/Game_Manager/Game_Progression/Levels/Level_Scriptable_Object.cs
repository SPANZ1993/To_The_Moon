using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Rocket_Game_Initialization_Object{
    public float thrustAltitudeMultiplier;
}


[CreateAssetMenu(fileName = "Level_Scriptable_Object", menuName = "ScriptableObjects/Level_Scriptable_Object", order = 5)]
public class Level_Scriptable_Object :  ScriptableObject
{

    public int LevelId { get { return levelId; } private set { levelId = value; } }
    public string LevelName { get { return levelName; } private set { levelName = value; } }

    public int NextLevelId { get { return nextLevelId; } private set { nextLevelId = value; } }


    public Level_End_Event_Scriptable_Object OnLevelCompleteEvent { get { return onLevelCompleteEvent; } private set { onLevelCompleteEvent = value;} }

    public Rocket_Game_Initialization_Object RocketGameInitializationObject { get { return rocketGameInitializationObject; } private set { rocketGameInitializationObject = value;} }
    


    [SerializeField]
    private int levelId;
    [SerializeField]
    private string levelName;

    [SerializeField]
    private int nextLevelId;

    [SerializeField]
    private Level_End_Event_Scriptable_Object onLevelCompleteEvent; // The event that plays after we beat the level

    [SerializeField]
    private Rocket_Game_Initialization_Object rocketGameInitializationObject;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
