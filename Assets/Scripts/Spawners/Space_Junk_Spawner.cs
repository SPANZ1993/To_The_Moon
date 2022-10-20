using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Space_Junk_Spawner : MonoBehaviour
{
    public GameObject cam;
    public Game_Scaler gameScaler;
    private Rocket_Game_Manager rocketGameManager;
    private Object_Pool spawnObjectPool;
    public GameObject spawnReticle_Bot_Left, spawnReticle_Bot_Right, spawnReticle_Top_Left, spawnReticle_Top_Right;

    [SerializeField]
    private bool displaySpawnReticles = true;

    private GameObject rocketObj;
    private Rocket_Control rocket;
    [SerializeField]
    public float spawnRate = 2.0f; // Just a scalar for how likely we are to spawn each second

    [SerializeField]
    private float minSpawnX, minSpawnY, maxSpawnX, maxSpawnY;
    //[SerializeField]
    //private float minSpawnX_Left, minSpawnY_Bot, maxSpawnX_Left, maxSpawnY_Bot;
    //[SerializeField]
    //private float minSpawnX_Right, minSpawnY_Top, maxSpawnX_Right, maxSpawnY_Top;

    private Queue<float> prevFrameDeltas = new Queue<float>();
    [SerializeField]
    private int prevFramesToExamine = 10;
    [SerializeField]
    public float frameRate = 1000.0f;
    [SerializeField]
    public List<Vector2> spawnObjAltitudeRanges;
    private List<int> possibleSpawnObjIndices = new List<int>();
    [SerializeField]
    private float timeToRefreshSpawnIndices = 0.25f;
    private float timeToRefreshSpawnIndicesLeft = 0.0f;

    [SerializeField]
    public List<GameObject> oneOffSpawnObjects;
    [SerializeField]
    public List<float> oneOffSpawnAltitudes;
    public List<bool> oneOffDidSpawnObjects;


    private bool paused = false; // Is the game paused currently, if so we don't want to spawn


    void OnEnable(){
        Rocket_Game_Manager.PauseLaunchSceneInfo += onPause;
    }

    void OnDisable(){
        Debug.Log("DISABLING SPAWNER");
        Rocket_Game_Manager.PauseLaunchSceneInfo -= onPause;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnObjectPool = gameObject.GetComponent<Object_Pool>();
        Debug.Assert(spawnObjectPool.poolObjList.Count == spawnObjAltitudeRanges.Count);

        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        rocketObj = GameObject.Find("Rocket");
        Debug.Assert(rocketObj != null);
        Debug.Assert(rocketGameManager != null);
        rocket = rocketObj.GetComponent<Rocket_Control>();
        spawnReticle_Bot_Left = GameObject.Find("Spawn_Reticle_Bot_Left"); // Make this so it finds this specific spawner's reticles
        spawnReticle_Bot_Right = GameObject.Find("Spawn_Reticle_Bot_Right");
        spawnReticle_Top_Left = GameObject.Find("Spawn_Reticle_Top_Left");
        spawnReticle_Top_Right = GameObject.Find("Spawn_Reticle_Top_Right");

        spawnReticle_Bot_Left.GetComponent<SpriteRenderer>().enabled = displaySpawnReticles;
        spawnReticle_Bot_Right.GetComponent<SpriteRenderer>().enabled = displaySpawnReticles;
        spawnReticle_Top_Left.GetComponent<SpriteRenderer>().enabled = displaySpawnReticles;
        spawnReticle_Top_Right.GetComponent<SpriteRenderer>().enabled = displaySpawnReticles;

        CalculatePotentialSpawnArea();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFrameRate();
        CalculatePotentialSpawnArea();
        if(spawnObjectPool.initialized){
            if(rocketGameManager.gameStarted){
                timeToRefreshSpawnIndicesLeft -= Time.deltaTime;
                if (timeToRefreshSpawnIndicesLeft <= 0.0f){
                    CalculatePotentialSpawnObjIndices();
                    timeToRefreshSpawnIndicesLeft = timeToRefreshSpawnIndices;
                }
                bool shouldSpawn = RollForSpawn();
                if (shouldSpawn && !paused){
                    Spawn();
                }
                else if(shouldSpawn){
                    //Debug.Log("STILL NOT SPAWNING????");
                }
            }
            else{
                //Debug.Log("NOT SPAWNING BC GAME NOT STARTED");
            }


            // Check to see if we need to spawn any one off objects
            spawnOneOffObjects();

        }
        else{
            //Debug.Log("NOT SPAWNING BC POOL NOT INITIALIZED");
        }
    }

    void spawnOneOffObjects(){
        float rocketAltitude = rocketGameManager.calculateAltitude(rocketObj);
        //Debug.Log("CHECKING SPAWN ... ROCKET ALT: " + rocketAltitude);
        for (int i = 0; i < oneOffSpawnObjects.Count; i++){
            if(!oneOffDidSpawnObjects[i] && oneOffSpawnAltitudes[i] <= rocketAltitude+30.0f){
                //Debug.Log("SPAWNING!!!");
                Instantiate(oneOffSpawnObjects[i], new Vector3(rocketObj.transform.position.x, oneOffSpawnAltitudes[i], 0.0f), new Quaternion());
                oneOffDidSpawnObjects[i] = true;
            }
        }
    }
    
    // void preInstantiateSpawnObjs(){
    //     foreach (GameObject g in spawnObjList){
    //         Queue();
    //     }
    // }


    
    void CalculateFrameRate(){
        prevFrameDeltas.Enqueue(Time.deltaTime);
        frameRate = 1.0f/prevFrameDeltas.Average();
    }

    // void CalculatePotentialSpawnArea(){
    //     minSpawnX_Left = cam.transform.position.x - (gameScaler.ScreenWidth*2.0f);
    //     maxSpawnX_Left = cam.transform.position.x - (gameScaler.ScreenWidth*1.5f);
    //     minSpawnX_Right = cam.transform.position.x + (gameScaler.ScreenWidth*1.5f);
    //     maxSpawnX_Right = cam.transform.position.x + (gameScaler.ScreenWidth*2.0f);
    //     minSpawnY_Bot = cam.transform.position.y - (gameScaler.ScreenHeight*2.0f);
    //     maxSpawnY_Bot = cam.transform.position.y - (gameScaler.ScreenHeight*1.5f);
    //     minSpawnY_Top = cam.transform.position.y + (gameScaler.ScreenHeight*1.5f);
    //     maxSpawnY_Top = cam.transform.position.y + (gameScaler.ScreenHeight*2.0f);
    // }

    bool RollForSpawn(){
        // If true, we will spawn a GameObject, otherwise not
        if (frameRate == 0.0f){
            return false;
        }

        float roll = Random.Range(0.0f, frameRate);
        //Debug.Log("ROLLIN: " + Time.time);
        return roll <= spawnRate;
    }

    // void Spawn(){
    //     float spawnPosX;
    //     float spawnPosY;
        
    //     float choice = Random.Range(0.0f, 1.0f);
    //     if (choice >= 0.0f  && choice <= 0.25f){
    //         spawnPosX = Random.Range(minSpawnX_Left, maxSpawnX_Left);
    //         //spawnPosY = Random.Range(minSpawnY_Bot, maxSpawnY_Top);
    //         spawnPosY = Random.Range(minSpawnY_Top, maxSpawnY_Top);
    //     }
    //     else if (choice >= 0.25f && choice <= 0.50f){
    //         spawnPosX = Random.Range(minSpawnX_Right, maxSpawnX_Right);
    //         //spawnPosY = Random.Range(minSpawnY_Bot, minSpawnY_Top);
    //         spawnPosY = Random.Range(minSpawnY_Top, maxSpawnY_Top);
    //     }
    //     else if (choice >= 0.50f && choice <= 0.75f){
    //         spawnPosX = Random.Range(minSpawnX_Left, maxSpawnX_Right);
    //         //spawnPosY = Random.Range(minSpawnY_Bot, maxSpawnY_Bot);
    //         spawnPosY = Random.Range(minSpawnY_Top, maxSpawnY_Top);
    //     }
    //     else{
    //         spawnPosX = Random.Range(minSpawnX_Left, maxSpawnX_Right);
    //         spawnPosY = Random.Range(minSpawnY_Top, maxSpawnY_Top);
    //     }


    //     Instantiate(spawnObj, new Vector3(spawnPosX, spawnPosY, 0.0f), new Quaternion());
    // }




    void CalculatePotentialSpawnArea(){
        Vector2 NormedDir = rocket.dir.normalized;
        Vector2 SpawnBoxMid = (Vector2)cam.transform.position + (new Vector2 (NormedDir.x * gameScaler.ScreenWidth * 3.0f, NormedDir.y * gameScaler.ScreenHeight * 3.0f));        
        
        float minSpawnYFloor = cam.transform.position.y + (gameScaler.ScreenHeight*1.25f);
        float maxSpawnYFloor = cam.transform.position.y + (gameScaler.ScreenHeight*1.50f);

        minSpawnX = SpawnBoxMid.x - (gameScaler.ScreenWidth * 6.0f);
        maxSpawnX = SpawnBoxMid.x + (gameScaler.ScreenWidth * 6.0f);
        minSpawnY = Mathf.Max(minSpawnYFloor, SpawnBoxMid.y - (gameScaler.ScreenHeight * 0.5f));
        maxSpawnY = Mathf.Max(maxSpawnYFloor, SpawnBoxMid.y + (gameScaler.ScreenHeight * 0.5f));
    
        if (displaySpawnReticles){
            spawnReticle_Bot_Left.transform.position = new Vector2(minSpawnX, minSpawnY);
            spawnReticle_Bot_Right.transform.position = new Vector2(maxSpawnX, minSpawnY);
            spawnReticle_Top_Left.transform.position = new Vector2(minSpawnX, maxSpawnY);
            spawnReticle_Top_Right.transform.position = new Vector2(maxSpawnX, maxSpawnY);
        }
    }



    void Spawn(){
        float spawnPosX = Random.Range(minSpawnX, maxSpawnX);
        float spawnPosY = Random.Range(minSpawnY, maxSpawnY);

        //Instantiate(spawnObjList[Random.Range(0, spawnObjList.Count)], new Vector3(spawnPosX, spawnPosY, 0.0f), new Quaternion());
        //spawnObjectPool.spawnPoolObj(Random.Range(0, spawnObjectPool.poolObjList.Count), new Vector3(spawnPosX, spawnPosY, 0.0f), new Quaternion());


        if (possibleSpawnObjIndices.Count > 0){
            spawnPoolObj(possibleSpawnObjIndices[Random.Range(0, possibleSpawnObjIndices.Count)], new Vector3(spawnPosX, spawnPosY, 0.0f), new Quaternion());
        }
        else{
            //Debug.Log("NO SPAWN INDICES");
        }
    }


    public void spawnPoolObj(int i, Vector3 loc, Quaternion rot){
        spawnObjectPool.spawnPoolObj(i, loc, rot);
    }

    public void spawnPoolObj(GameObject g, Vector3 loc, Quaternion rot){
        spawnObjectPool.spawnPoolObj(g, loc, rot);
    }


    public void spawnPoolObj(string tag, Vector3 loc, Quaternion rot){
        spawnObjectPool.spawnPoolObj(tag, loc, rot);
    }

    public void despawnPoolObj(GameObject g){
        spawnObjectPool.despawnPoolObj(g);
    }


    private void CalculatePotentialSpawnObjIndices(){
        float rocketAltitude = rocketGameManager.calculateAltitude(rocketObj);
        possibleSpawnObjIndices.Clear();
        float bot;
        float top;
        bool passedBot;
        bool passedTop;
        for (int i = 0; i < spawnObjAltitudeRanges.Count; i++){
            bot = spawnObjAltitudeRanges[i].x;
            top = spawnObjAltitudeRanges[i].y;
            passedBot = false;
            passedTop = false;

            if(bot <= 0.0f || rocketAltitude >= bot){
                passedBot = true;
            }
            if(top <= 0.0f || rocketAltitude <= top){
                passedTop = true;
            }
            if (passedBot && passedTop){
                possibleSpawnObjIndices.Add(i);
            }
        }
        
        //Debug.Log("POSSIBLE INDICES: " + string.Join(", ", possibleSpawnObjIndices.ToArray().Select(i => spawnObjectPool.poolObjList[i])));

        //Debug.Log("Calculating Potential Spawn Indices Altitude is " + rocketAltitude +  " Indices: " + System.String.Join(" ", possibleSpawnObjIndices));
    }

    void onPause(bool Paused){
        paused = Paused;
    }

}
