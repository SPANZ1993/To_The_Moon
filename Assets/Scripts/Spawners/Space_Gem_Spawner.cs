using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Space_Gem_Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject GemPrefab;
    // This class is going to be a leech on the spaceJunkSpawner, using it's spawn area to spawn gems
    // Not super elegant but meh
    private Space_Junk_Spawner spaceJunkSpawner;
    private Rocket_Game_Manager rocketGameManager;
    private Game_Manager gameManager;
    private Game_Scaler gameScaler;

    private List<float> spawningAltitudes; // The altitudes that we are planning to spawn the gems at
    private List<bool> hasSpawned; // Have we spawned the gems?


    private GameObject spawnReticle_Bot_Left, spawnReticle_Bot_Right, spawnReticle_Top_Left, spawnReticle_Top_Right;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Game_Manager.instance;
        gameScaler = Game_Scaler.instance;
        spaceJunkSpawner = GameObject.Find("Junk_Spawner").GetComponent<Space_Junk_Spawner>();

        spawnReticle_Bot_Left = spaceJunkSpawner.spawnReticle_Bot_Left;
        spawnReticle_Bot_Right = spaceJunkSpawner.spawnReticle_Bot_Right;
        spawnReticle_Top_Left = spaceJunkSpawner.spawnReticle_Top_Left;
        spawnReticle_Top_Right = spaceJunkSpawner.spawnReticle_Top_Right;

        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();

        chooseSpawningAltitudes();
    }

    // Update is called once per frame
    void Update()
    {
        if(checkForSpawn()){
            spawn();
        }
        
    }


    private bool checkForSpawn(){
        return getSpawnId() != -1;
    }

    private int getSpawnId(){
        float[] spawnRetAlts = new float[] {rocketGameManager.calculateAltitude(spawnReticle_Bot_Left), rocketGameManager.calculateAltitude(spawnReticle_Bot_Right), rocketGameManager.calculateAltitude(spawnReticle_Top_Left), rocketGameManager.calculateAltitude(spawnReticle_Top_Right)};
        float minSpawnAlt = spawnRetAlts.Min();
        float maxSpawnAlt = spawnRetAlts.Max();
        float alt;
        for(int i=0; i<spawningAltitudes.Count; i++){
            alt = spawningAltitudes[i];
            if(!hasSpawned[i] && minSpawnAlt < alt && maxSpawnAlt > alt){
                return i;
            }
        }
        return -1;
    }

    private void spawn(){
        float[] spawnRetXLocs = new float[] {spawnReticle_Bot_Left.transform.position.x, spawnReticle_Bot_Right.transform.position.x, spawnReticle_Top_Left.transform.position.x, spawnReticle_Top_Right.transform.position.x};
        float minSpawnX = spawnRetXLocs.Min();
        float maxSpawnX = spawnRetXLocs.Max();
        float xLoc = UnityEngine.Random.Range(minSpawnX, maxSpawnX);
        int spawnId = getSpawnId();
        if (spawnId != -1){
            gameScaler.ScaleObject(Instantiate(GemPrefab, new Vector3(xLoc, rocketGameManager.calculateGameYPos(spawningAltitudes[spawnId]), 0f), Quaternion.identity));
            hasSpawned[spawnId] = true;
            //Debug.Log("Spawned a gem at altitude: " + rocketGameManager.calculateGameYPos(spawningAltitudes[spawnId]));
        }
    }

    private void chooseSpawningAltitudes(){
        spawningAltitudes = new List<float>();
        hasSpawned = new List<bool>();
        int numGems;
        System.Random rnd = new System.Random();
        if(true){ // If we don't have a history
            numGems = rnd.Next(1, 5);
            for(int i=0; i<numGems; i++){
                spawningAltitudes.Add(UnityEngine.Random.Range(rocketGameManager.targetAltitude*0.01f, rocketGameManager.targetAltitude*0.9f)); // 10% to target through 90% to target
                hasSpawned.Add(false);
            }
        }
        else{

        }
    
        //Debug.Log("Will be spawning " + spawningAltitudes.Count + " gems at these altitudes: " + string.Join(", ", spawningAltitudes));
    }
}
