using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Rocket_Game_Initialization_Scriptable_Object", menuName = "ScriptableObjects/Rocket_Game_Initialization_Object", order = 6)]
public class Rocket_Game_Initialization_Scriptable_Object : ScriptableObject
{

    // Rocket Game Manager Stuff
    [SerializeField]
    private float thrustAltMultiplier;
    [SerializeField]
    private float targetAltitude;


    // Background Stuff
    [SerializeField]
    public List<float> fogAlts;
    [SerializeField]
    public List<Color> fogColors;
    [SerializeField]
    public List<float> fogSizes;

    [SerializeField]
    public List<float> BackgroundAlts;
    [SerializeField]
    public List<Color> BackgroundColors;


    // Junk Spawner Stuff
    [SerializeField]
    public List<GameObject> poolObjList;
    [SerializeField]
    public List<int> numToSpawnList;
    [SerializeField]
    public List<bool> shouldExpandList;
    [SerializeField]
    private List<Vector2> spawnObjAltitudeRanges;
    [SerializeField]
    private float spawnRate;



    // Background Junk Spawner Stuff
    [SerializeField]
    public List<GameObject> poolObjListBackground;
    [SerializeField]
    public List<int> numToSpawnListBackground;
    [SerializeField]
    public List<bool> shouldExpandListBackground;
    [SerializeField]
    private List<Vector2> spawnObjAltitudeRangesBackground;
    [SerializeField]
    private float backgroundSpawnRate;


    // TODO: SET THIS UP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    [SerializeField]
    private Sprite leavingPlanetSprite;
    [SerializeField]
    private float leavingPlanetBotParallaxRocketAlt;
    [SerializeField]
    private float leavingPlanetTopParallaxRocketAlt;
    [SerializeField]
    private Sprite arrivingPlanetSprite;
    [SerializeField]
    private float arrivingPlanetBotParallaxRocketAlt;
    [SerializeField]
    private float arrivingPlanetTopParallaxRocketAlt;


    public void Validate(){
        Debug.Assert(fogAlts.Count == fogColors.Count && fogColors.Count == fogSizes.Count);
        Debug.Assert(BackgroundAlts.Count == BackgroundColors.Count);
        Debug.Assert(poolObjList.Count == numToSpawnList.Count && numToSpawnList.Count == shouldExpandList.Count && shouldExpandList.Count == spawnObjAltitudeRanges.Count);
        Debug.Assert(poolObjListBackground.Count == numToSpawnListBackground.Count && numToSpawnListBackground.Count == shouldExpandListBackground.Count && shouldExpandListBackground.Count == spawnObjAltitudeRangesBackground.Count);
    }

    public void initializeRocketGame(bool freePlayMode){
        //Debug.Log("INITIALIZING ROCKET GAME");

        Rocket_Game_Manager rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        rocketGameManager.targetAltitude = targetAltitude;
        rocketGameManager.freePlayMode = freePlayMode;
        rocketGameManager.thrustAltMultiplier = thrustAltMultiplier;
        // TODO: THRUST ALT MULTIPLIER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        Rocket_Control rocketControl =  GameObject.Find("Rocket").GetComponent<Rocket_Control>();
        rocketControl.thrustAltMultiplier = thrustAltMultiplier;

        Background_Controller backgroundController = GameObject.Find("Background_Manager").GetComponent<Background_Controller>();
        backgroundController.fogAlts = new List<float>(fogAlts);
        backgroundController.fogColors = new List<Color>(fogColors);
        backgroundController.fogSizes = new List<float>(fogSizes);
        backgroundController.BackgroundAlts = new List<float>(BackgroundAlts);
        backgroundController.BackgroundColors = new List<Color>(BackgroundColors);


        GameObject Space_Junk_Spawner_Obj = GameObject.Find("Junk_Spawner");
        Space_Junk_Spawner spaceJunkSpawner = Space_Junk_Spawner_Obj.GetComponent<Space_Junk_Spawner>();
        Object_Pool spaceJunkObjectPool = Space_Junk_Spawner_Obj.GetComponent<Object_Pool>();

        spaceJunkSpawner.spawnRate = spawnRate;
        spaceJunkSpawner.spawnObjAltitudeRanges = new List<Vector2>(spawnObjAltitudeRanges);
        spaceJunkObjectPool.poolObjList = new List<GameObject>(poolObjList);
        spaceJunkObjectPool.numToSpawnList = new List<int>(numToSpawnList);
        spaceJunkObjectPool.shouldExpandList = new List<bool>(shouldExpandList);




        GameObject Background_Space_Junk_Spawner_Obj = GameObject.Find("Background_Junk_Spawner");
        Space_Junk_Spawner backgroundSpaceJunkSpawner = Background_Space_Junk_Spawner_Obj.GetComponent<Space_Junk_Spawner>();
        Object_Pool backgroundSpaceJunkObjectPool = Background_Space_Junk_Spawner_Obj.GetComponent<Object_Pool>();

        backgroundSpaceJunkSpawner.spawnRate = backgroundSpawnRate;
        backgroundSpaceJunkSpawner.spawnObjAltitudeRanges = new List<Vector2>(spawnObjAltitudeRangesBackground);
        backgroundSpaceJunkObjectPool.poolObjList = new List<GameObject>(poolObjListBackground);
        backgroundSpaceJunkObjectPool.numToSpawnList = new List<int>(numToSpawnListBackground);
        backgroundSpaceJunkObjectPool.shouldExpandList = new List<bool>(shouldExpandListBackground);




        GameObject Leaving_Planet = GameObject.Find("Leaving_Planet");
        Background_Object_Parallax leavingObjectParallax = Leaving_Planet.GetComponent<Background_Object_Parallax>();
        Leaving_Planet.GetComponent<SpriteRenderer>().sprite = leavingPlanetSprite;
        leavingObjectParallax.rocketBotAlt = leavingPlanetBotParallaxRocketAlt;
        leavingObjectParallax.rocketTopAlt = leavingPlanetTopParallaxRocketAlt;
        leavingObjectParallax.firstEnabled = true;
        

        GameObject Arriving_Planet = GameObject.Find("Arriving_Planet");
        Background_Object_Parallax arrivingObjectParallax = Arriving_Planet.GetComponent<Background_Object_Parallax>();
        Arriving_Planet.GetComponent<SpriteRenderer>().sprite = arrivingPlanetSprite;
        arrivingObjectParallax.rocketBotAlt = arrivingPlanetBotParallaxRocketAlt;
        arrivingObjectParallax.rocketTopAlt = arrivingPlanetTopParallaxRocketAlt;

        // Make it so we pass the arriving planet
        if(freePlayMode){
            float rocketNewTopAltRatio = (arrivingObjectParallax.objectTopPos-(-arrivingObjectParallax.objectBotPos))/(arrivingObjectParallax.objectBotPos-arrivingObjectParallax.objectTopPos);
            arrivingObjectParallax.objectTopPos = -arrivingObjectParallax.objectBotPos;
            arrivingObjectParallax.rocketTopAlt = arrivingObjectParallax.rocketTopAlt + (rocketNewTopAltRatio*(arrivingObjectParallax.rocketTopAlt-arrivingObjectParallax.rocketBotAlt));
            //Debug.Log("NEW PARALLAX POS PLANET: " + arrivingObjectParallax.objectTopPos +  " --- " +  arrivingObjectParallax.objectBotPos);
            //Debug.Log("NEW PARALLAX ALT ROCKET: " + arrivingObjectParallax.rocketBotAlt + " --- " + arrivingObjectParallax.rocketTopAlt);
        }


        arrivingObjectParallax.firstEnabled = true;
    }
}
