using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

public class Background_Controller : MonoBehaviour
{

    // [SerializeField]
    // private GameObject Background_Chunk_Prefab;
    public List<GameObject> ActiveChunks = new List<GameObject>();
    // [SerializeField]
    // private int numChunksToLoad = 50;
    // private List<GameObject> InactiveChunks = new List<GameObject>();

    bool initialized = false;

    private Object_Pool Chunks_Pool;

    // Size data about the background chunk
    Bounds bounds;
    Vector3 extents;

    [SerializeField]
    private Camera cam;
    private Vector3 cam_start_loc;
    [SerializeField]
    private float renderDist;

    [SerializeField]
    private double updateOn = 10.0;
    private double frameCount = 0.0;


    private Game_Scaler gameScaler;
    private Rocket_Game_Manager rocketGameManager;




    public List<float> BackgroundAlts;
    public List<Color> BackgroundColors;


    private GameObject rocket;

    public Material fogMaterial;
    public List<float> fogAlts;
    public List<Color> fogColors;
    public List<float> fogSizes;
    [HideInInspector]
    public bool fogActivated = true;



    private Dictionary<float, Color> fog_height_color_map;
    List<float> fog_height_color_size_map_sorted_keys;
    private Dictionary<float, float> fog_height_size_map;

    private Dictionary<float, Color> height_color_map;
    // private Dictionary<float, Color> height_color_map = new Dictionary<float, Color>()
    // {
    //     [0.0f] = new Color(53, 74, 178, 255),
    //     [100.0f] = new Color(30, 30, 30, 255),
    //     [200.0f] = new Color(255, 125, 246, 255),
    //     [250.0f] = new Color(61, 171, 44, 255),
    //     [500.0f] = new Color(61, 249, 255, 255)
    // };
    List<float> height_color_map_sorted_keys;


    void Awake(){
        // If we didn't supply the info in the editor... then we wait until we get it (See _LateStart())
        if(BackgroundAlts == null){
            BackgroundAlts = new List<float>();
        }
        // Debug.Assert(BackgroundAlts.Count == BackgroundColors.Count);
        // height_color_map = new Dictionary<float, Color>();
        // for (int i=0; i<BackgroundColors.Count; i++){
        //     height_color_map[BackgroundAlts[i]] = new Color(BackgroundColors[i].r* 255f, BackgroundColors[i].g* 255f, BackgroundColors[i].b * 255f, 255);
        //     //Debug.Log("HCM[" + BackgroundAlts[i] + "] = " + height_color_map[BackgroundAlts[i]]);
        // }

        // Debug.Assert(fogAlts.Count == fogColors.Count);
        // Debug.Assert(fogAlts.Count == fogSizes.Count);

        // fog_height_color_map = new Dictionary<float, Color>();
        // fog_height_size_map = new Dictionary<float, float>();
        // for (int i=0; i<fogAlts.Count; i++){
        //     fog_height_color_map[fogAlts[i]] = new Color(fogColors[i].r* 255f, fogColors[i].g* 255f, fogColors[i].b * 255f, fogColors[i].a * 255f);
        //     fog_height_size_map[fogAlts[i]] = fogSizes[i];
        // }

        
        // fogMaterial = GameObject.Find("Fog").GetComponent<SpriteRenderer>().material;
        // rocket = GameObject.Find("Rocket");
    }


    // Start is called before the first frame update
    void Start()
    {
        // Chunks_Pool = GetComponent<Object_Pool>();
        // //Debug.Log("CHUNKS POOL: " + Chunks_Pool);
        // gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        // rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        // // Debug.Log("RGMO: " + GameObject.Find("Rocket_Game_Manager"));
        // // Debug.Log("RGM: " + GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>());
        // // Object_Info_Getter oig = new Object_Info_Getter();
        // // oig.ListComponents(GameObject.Find("Rocket_Game_Manager"), "RGM");


        // // REMOVED
        // // for (int i = 0; i < numChunksToLoad; i++){
        // //     GameObject cur_inactive_chunk = (GameObject)Instantiate(Background_Chunk_Prefab, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());
        // //     cur_inactive_chunk.transform.localScale = new Vector3(gameScaler.Scale_Value_To_Screen_Width(cur_inactive_chunk.transform.localScale.x), gameScaler.Scale_Value_To_Screen_Height(cur_inactive_chunk.transform.localScale.y), cur_inactive_chunk.transform.localScale.z);
        // //     cur_inactive_chunk.SetActive(false);
        // //     InactiveChunks.Add(cur_inactive_chunk);
        // // }


        

        // scale_height_color_map();
        

        // height_color_map_sorted_keys = height_color_map.Keys.ToList();
        // height_color_map_sorted_keys = height_color_map_sorted_keys.OrderBy(x => x).ToList();


        // fog_height_color_size_map_sorted_keys = fog_height_color_map.Keys.ToList();
        // fog_height_color_size_map_sorted_keys = fog_height_color_size_map_sorted_keys.OrderBy(x => x).ToList();

        // //Debug.Log("HCMSK: " + height_color_map_sorted_keys[2] + " --- " + height_color_map[height_color_map_sorted_keys[2]]);

        // normalizeColors();
        // normalizeFogColors();

        // cam_start_loc = cam.transform.position;
        // //GameObject seed_chunk = (GameObject)Instantiate(Background_Chunk_Prefab, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());
        Chunks_Pool = GetComponent<Object_Pool>();
        StartCoroutine(_LateStart());
    }

    IEnumerator _LateStart(){

        // Still waiting to be initialized
        while(BackgroundAlts.Count == 0 || !Chunks_Pool.initialized){
            yield return new WaitForSeconds(0);
        }


        Debug.Assert(BackgroundAlts.Count == BackgroundColors.Count);
        height_color_map = new Dictionary<float, Color>();
        for (int i=0; i<BackgroundColors.Count; i++){
            height_color_map[BackgroundAlts[i]] = new Color(BackgroundColors[i].r* 255f, BackgroundColors[i].g* 255f, BackgroundColors[i].b * 255f, 255);
            //Debug.Log("HCM[" + BackgroundAlts[i] + "] = " + height_color_map[BackgroundAlts[i]]);
        }

        Debug.Assert(fogAlts.Count == fogColors.Count);
        Debug.Assert(fogAlts.Count == fogSizes.Count);

        fog_height_color_map = new Dictionary<float, Color>();
        fog_height_size_map = new Dictionary<float, float>();
        for (int i=0; i<fogAlts.Count; i++){
            fog_height_color_map[fogAlts[i]] = new Color(fogColors[i].r* 255f, fogColors[i].g* 255f, fogColors[i].b * 255f, fogColors[i].a * 255f);
            fog_height_size_map[fogAlts[i]] = fogSizes[i];
        }

        
        fogMaterial = GameObject.Find("Fog").GetComponent<SpriteRenderer>().material;
        rocket = GameObject.Find("Rocket");


        /////////////////////////////////



        
        //Debug.Log("CHUNKS POOL: " + Chunks_Pool);
        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        // Debug.Log("RGMO: " + GameObject.Find("Rocket_Game_Manager"));
        // Debug.Log("RGM: " + GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>());
        // Object_Info_Getter oig = new Object_Info_Getter();
        // oig.ListComponents(GameObject.Find("Rocket_Game_Manager"), "RGM");


        // REMOVED
        // for (int i = 0; i < numChunksToLoad; i++){
        //     GameObject cur_inactive_chunk = (GameObject)Instantiate(Background_Chunk_Prefab, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());
        //     cur_inactive_chunk.transform.localScale = new Vector3(gameScaler.Scale_Value_To_Screen_Width(cur_inactive_chunk.transform.localScale.x), gameScaler.Scale_Value_To_Screen_Height(cur_inactive_chunk.transform.localScale.y), cur_inactive_chunk.transform.localScale.z);
        //     cur_inactive_chunk.SetActive(false);
        //     InactiveChunks.Add(cur_inactive_chunk);
        // }


        

        scale_height_color_map();
        

        height_color_map_sorted_keys = height_color_map.Keys.ToList();
        height_color_map_sorted_keys = height_color_map_sorted_keys.OrderBy(x => x).ToList();


        fog_height_color_size_map_sorted_keys = fog_height_color_map.Keys.ToList();
        fog_height_color_size_map_sorted_keys = fog_height_color_size_map_sorted_keys.OrderBy(x => x).ToList();

        //Debug.Log("HCMSK: " + height_color_map_sorted_keys[2] + " --- " + height_color_map[height_color_map_sorted_keys[2]]);

        normalizeColors();
        normalizeFogColors();

        cam_start_loc = cam.transform.position;
        //GameObject seed_chunk = (GameObject)Instantiate(Background_Chunk_Prefab, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());




        //////////////////////////////////////////



        Get_Background_Chunk_Prefab_Size();
        //Debug.Log("BC PREFAB SIZE BOUNDS: " + bounds);
        //Debug.Log("BC PREFAB SIZE EXTENTS: " + extents);
        GameObject seed_chunk = Chunks_Pool.spawnPoolObj("Background_Chunk", new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());
        //seed_chunk.transform.localScale = new Vector3(gameScaler.Scale_Value_To_Screen_Width(seed_chunk.transform.localScale.x), gameScaler.Scale_Value_To_Screen_Height(seed_chunk.transform.localScale.y), seed_chunk.transform.localScale.z);
        assignColor(seed_chunk);
        ActiveChunks.Add(seed_chunk);

        int prevActive = ActiveChunks.Count;
        int newActive = -1;
        while (prevActive != newActive){
            prevActive = ActiveChunks.Count;
            updateBackgroundChunks();
            newActive = ActiveChunks.Count;
        }
        //throw new ArgumentException("YO");
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount%updateOn==0.0 && initialized){
            //Debug.Log("CHECK " + (int)(frameCount/updateOn) + ": " + ActiveChunks.Count() + " ACTIVE CHUNKS");
            updateBackgroundChunks();
            updateFog();
        }
        frameCount++;

    }



    void scale_height_color_map(){
        Dictionary<float, Color> d = new Dictionary<float, Color>();

        foreach (float k in height_color_map.Keys){
            d[gameScaler.Scale_Value_To_Screen_Height(k)] = height_color_map[k];
        }

        height_color_map = d;
    }




    private void normalizeColors(){
        // Simply divide all of our original color values by 255.0f
        // This just makes it easier so we can transcribe exact color values from the editor
        foreach (float k in height_color_map_sorted_keys){
            float r = height_color_map[k].r/255.0f;
            float g = height_color_map[k].g/255.0f;
            float b = height_color_map[k].b/255.0f;
            float a = height_color_map[k].a/255.0f;
            height_color_map[k] = new Color(r, g, b, a);
        }
    }

    private void normalizeFogColors(){
        // Simply divide all of our original color values by 255.0f
        // This just makes it easier so we can transcribe exact color values from the editor
        foreach (float k in fog_height_color_size_map_sorted_keys){
            float r = fog_height_color_map[k].r/255.0f;
            float g = fog_height_color_map[k].g/255.0f;
            float b = fog_height_color_map[k].b/255.0f;
            float a = fog_height_color_map[k].a/255.0f;
            fog_height_color_map[k] = new Color(r, g, b, a);
        }
    }


    // Examine the size of the prefab chunk we supplied and determine its size
    private void Get_Background_Chunk_Prefab_Size(){
        GameObject nonDithering = null;
       
        for(int i = 0; i < Chunks_Pool.getGameObj(0).gameObject.transform.childCount; i++)
        {
            if (nonDithering is null){
                if (Chunks_Pool.getGameObj(0).gameObject.transform.GetChild(i).name == "Non_Dithering"){
                    nonDithering = Chunks_Pool.getGameObj(0).gameObject.transform.GetChild(i).gameObject;
                }
            }
        }

        //Debug.Log(nonDithering);

        // Component[] components = nonDithering.GetComponents(typeof(Component));
        // foreach(Component component in components) {
        //     Debug.Log(component.ToString());
        // }

        SpriteRenderer rend = nonDithering.GetComponent<SpriteRenderer>();

        //Debug.Log(rend.bounds);
        bounds = rend.bounds;
        extents = bounds.extents;
        //extents = extents * 0.999f; // Do this so we don't get jittering on the edges
        extents = extents * new Vector2(0.9999999f, 0.9975f); // * new Vector2(gameScaler.ScaleX, gameScaler.ScaleY); // TODO: Fix the sprite so that the borders line up
    }
    


    void assignColor(GameObject chunk){
        GameObject curChild;
        SpriteRenderer curRenderer;
        List<Color> colors = getColor(chunk.transform.position);

        //Debug.Log("COLORS LIST: " + colors[0] + "  " + colors[1] + "  " + colors[2]);

        for(int i = 0; i < chunk.gameObject.transform.childCount; i++)
        {              
            curChild = chunk.gameObject.transform.GetChild(i).gameObject;
            curRenderer = curChild.GetComponent<SpriteRenderer>();
            if (curChild.name == "Dithering_Bot"){
                curRenderer.color = colors[0];
            }
            else if (curChild.name == "Non_Dithering"){
                curRenderer.color = colors[1];
            }
            else if (curChild.name == "Dithering_Top"){
                curRenderer.color = colors[2];
            }
        }
    }

    public List<Color> getColor(Vector3 spriteLoc, bool first=true){
        // Given a vector defining the location of a background chunk sprite, return the three colors
        // for the sprite

        Color _chooseColor(float height, float height_lower, float height_higher){
            // When we know the two keys in our color dict that encase our current height,
            // interpolate a color based on the color values

            if (height == height_lower || height == height_higher){
                return height_color_map[height];
            }
            else{
                //Debug.Log("Height: " + height + "LOWER: " + height_lower + " -- " + height_color_map[height_lower] + " HIGHER: " +  height_higher + " -- " + height_color_map[height_higher]);
                float lr = height_color_map[height_lower].r;
                float lg = height_color_map[height_lower].g;
                float lb = height_color_map[height_lower].b;
                float la = height_color_map[height_lower].a;
                float hr = height_color_map[height_higher].r;
                float hg = height_color_map[height_higher].g;
                float hb = height_color_map[height_higher].b;
                float ha = height_color_map[height_higher].a;

                float percent_between = 1.0f-((height_higher-height)/(height_higher-height_lower));

                float nr = ((hr-lr)*percent_between)+lr;
                float ng = ((hg-lg)*percent_between)+lg;
                float nb = ((hb-lb)*percent_between)+lb;
                float na = ((ha-la)*percent_between)+la;

                //Debug.Log("Height: " + height + " LOWER: " + height_lower + " -- " + height_color_map[height_lower] + " HIGHER: " +  height_higher + " -- " + height_color_map[height_higher] + " NEW: " + new Color(nr, ng, nb, na));
                return new Color(nr, ng, nb, na);
            }

        }


        //float height = gameScaler.Scale_Value_To_Screen_Height(spriteLoc.y);
        float height;
        try{
            if(rocketGameManager == null){
                rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
            }
            height = rocketGameManager.calculateAltitude(spriteLoc.y);
            //Debug.Log("HEIGHT IS: " + height);
        }
        catch(Exception e){
            //Debug.Log("PROBLEM CALCULATING HEIGHT :(    :" + e + ")");
            height = 0.0f;
        }

        //Debug.Log("YEET " + height_color_map_sorted_keys[0]);
        float closest_lower = height_color_map_sorted_keys[0];
        float closest_higher = height_color_map_sorted_keys[height_color_map_sorted_keys.Count-1];

        //Debug.Log("HEIGHT: " + height);
        //Debug.Log("LOWER: " + closest_lower);
        //Debug.Log("HIGHER: " + closest_higher);

        Color chosen_color = height_color_map[closest_lower];
        bool color_found = false;


        if (height <= closest_lower){
            chosen_color = height_color_map[closest_lower];
            color_found = true;
            // Debug.Log("POO -- CHOSE LOWER -> " + closest_lower + " --- " + height + " -- " + chosen_color);
        }
        else if (height >= closest_higher){
            chosen_color = height_color_map[closest_higher];
            color_found = true;
            // Debug.Log("POO -- CHOSE HIGHER");
        }


        if (!color_found){
            foreach (float heightk in height_color_map_sorted_keys){
                if (heightk <= height){
                    closest_lower = heightk;
                }
                if (heightk >= height && closest_higher == height_color_map_sorted_keys[height_color_map_sorted_keys.Count-1]){
                    closest_higher = heightk;
                }
                //Debug.Log("POO --" + height + " LOW: " + closest_lower + " HIGH: " + closest_higher);
            }


            chosen_color = _chooseColor(height, closest_lower, closest_higher);
            // Debug.Log("POO -- COLOR: " + chosen_color);

        }

        if (first){
            return new List<Color>{
                                    getColor(new Vector3(spriteLoc.x, spriteLoc.y - (extents.y * 2.0f), spriteLoc.z), first:false)[0],
                                    chosen_color,
                                    getColor(new Vector3(spriteLoc.z, spriteLoc.y + (extents.y * 2.0f), spriteLoc.z), first:false)[0]
                                }; //List with color below and above
        }
        else{
            return new List<Color>{chosen_color};
        }
    }



    void updateBackgroundChunks(){

        void _destroyFarChunks(){
            for (int i = ActiveChunks.Count-1; i >= 0; i--){
                GameObject chunk = ActiveChunks[i];
                Vector3 camPos = cam.transform.position;
                if (Vector3.Distance(chunk.transform.position, new Vector3(camPos.x, camPos.y, 0.0f)) > renderDist){
                    //Debug.Log("DESTROYING: " + Vector3.Distance(chunk.transform.position, new Vector3(camPos.x, camPos.y, 0.0f)));
                    // REMOVED 2
                    //InactiveChunks.Add(ActiveChunks[i]);
                    //ActiveChunks[i].SetActive(false);
                    //ActiveChunks.RemoveAt(i);
                    Chunks_Pool.despawnPoolObj(ActiveChunks[i]);
                    ActiveChunks.RemoveAt(i);
                }
            }


        }

        bool _checkLocForChunk(Vector3 loc){
        // Given a location, does a chunk already exist there?
            foreach (GameObject chunk in ActiveChunks){
                if (chunk.transform.position.Equals(loc)){
                    //Debug.Log(chunk.transform.position + " == " + loc);
                    return true;
                }
            }
            return false;
        }

        void _instantiateNeighbors(){
            //List<GameObject> chunksToAdd = new List<GameObject>();
            int numChunks = ActiveChunks.Count;
            Vector3 cur_chunk_pos = new Vector3();
            Vector3 cur_test_pos = new Vector3();
            Vector3 camPos = cam.transform.position;
            for (int i=0; i < numChunks; i++){
                cur_chunk_pos = ActiveChunks[i].transform.position;
                for (int j=0; j < 4; j++){
                    switch (j){
                        case 0:
                            cur_test_pos = new Vector3(cur_chunk_pos.x, cur_chunk_pos.y - (extents.y * 2.0f), cur_chunk_pos.z);
                            break;
                        case 1:
                            cur_test_pos = new Vector3(cur_chunk_pos.x, cur_chunk_pos.y + (extents.y * 2.0f), cur_chunk_pos.z);
                            break;
                        case 2:
                            cur_test_pos = new Vector3(cur_chunk_pos.x - (extents.x * 2.0f), cur_chunk_pos.y, cur_chunk_pos.z);
                            break;
                        case 3:
                            cur_test_pos = new Vector3(cur_chunk_pos.x + (extents.x * 2.0f), cur_chunk_pos.y, cur_chunk_pos.z);
                            break;
                    }

                    //Debug.Log("CTP DIST: " + Vector3.Distance(cur_test_pos, new Vector3(camPos.x, camPos.y, 0.0f)) + " CHUNK? " + _checkLocForChunk(cur_test_pos));
                    if (Vector3.Distance(cur_test_pos, new Vector3(camPos.x, camPos.y, 0.0f)) <= renderDist && !_checkLocForChunk(cur_test_pos)){
                    //     //Debug.Log("DISTANCE: " + Vector3.Distance(cur_test_pos, cam.transform.position));
                        // REMOVED 2
                        // GameObject cur_chunk = InactiveChunks[InactiveChunks.Count-1];
                        // cur_chunk.SetActive(true);
                        GameObject cur_chunk = Chunks_Pool.spawnPoolObj("Background_Chunk", cur_test_pos);
                        // REMOVED
                        //cur_chunk.transform.position = cur_test_pos;
                        assignColor(cur_chunk);
                        ActiveChunks.Add(cur_chunk);
                        // REMOVED
                        // InactiveChunks.RemoveAt(InactiveChunks.Count-1);
                    //     //chunksToAdd.Add(cur_chunk);
                    }
                }
            }
            //return chunksToAdd;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        void _cleanUpActiveChunks(){
            // Sometimes inactive chunks remain in our active chunks list for some reason..
            // Just gonna remove those if any are there
            int origNumActiveChunks = ActiveChunks.Count();
            for (int i=origNumActiveChunks-1; i>=0; i--){
                if(!ActiveChunks[i].activeSelf){
                    ActiveChunks.RemoveAt(i);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////




        _destroyFarChunks();
        _instantiateNeighbors();
        ////////////////////////////////////////////////////////////////////////////////////////////
        _cleanUpActiveChunks();
        ////////////////////////////////////////////////////////////////////////////////////////////
        //List <GameObject> chunksToAdd = new List<GameObject>(); // Need to do it like this bc we can't update the instantiated chunks list during this loop

        // REMOVED 3
        // int origCount = ActiveChunks.Count;
        // for (int i=0; i < origCount; i++){
        //    _instantiateNeighbors();
        // }

        // foreach (GameObject cur_chunk in chunksToAdd){
        //     Debug.Log("ADDING CHUNKS");
        //     ActiveChunks.Add(cur_chunk);
        // }
    }






    public Dictionary<string, float> getFogColorAndSize(Vector3 spriteLoc){
        // Given a vector defining the location of a background chunk sprite, return the three colors
        // for the sprite

        Dictionary<string, float> _chooseColorandSize(float height, float height_lower, float height_higher){
            // When we know the two keys in our color dict that encase our current height,
            // interpolate a color based on the color values

            if (height == height_lower || height == height_higher){
                return new Dictionary<string, float>(){
                    {"r", fog_height_color_map[height].r},
                    {"g", fog_height_color_map[height].g},
                    {"b", fog_height_color_map[height].b},
                    {"a", fog_height_color_map[height].a},
                    {"s", fog_height_size_map[height]}
                    };
            }
            else{
                //Debug.Log("Height: " + height + "LOWER: " + height_lower + " -- " + height_color_map[height_lower] + " HIGHER: " +  height_higher + " -- " + height_color_map[height_higher]);
                float lr = fog_height_color_map[height_lower].r;
                float lg = fog_height_color_map[height_lower].g;
                float lb = fog_height_color_map[height_lower].b;
                float la = fog_height_color_map[height_lower].a;
                float hr = fog_height_color_map[height_higher].r;
                float hg = fog_height_color_map[height_higher].g;
                float hb = fog_height_color_map[height_higher].b;
                float ha = fog_height_color_map[height_higher].a;

                float ls = fog_height_size_map[height_lower];
                float hs = fog_height_size_map[height_higher];

                float percent_between = 1.0f-((height_higher-height)/(height_higher-height_lower));

                float nr = ((hr-lr)*percent_between)+lr;
                float ng = ((hg-lg)*percent_between)+lg;
                float nb = ((hb-lb)*percent_between)+lb;
                float na = ((ha-la)*percent_between)+la;

                float ns = ((hs-ls)*percent_between)+ls;

                //Debug.Log("Height: " + height + " LOWER: " + height_lower + " -- " + height_color_map[height_lower] + " HIGHER: " +  height_higher + " -- " + height_color_map[height_higher] + " NEW: " + new Color(nr, ng, nb, na));
                return new Dictionary<string, float>(){
                    {"r", nr},
                    {"g", ng},
                    {"b", nb},
                    {"a", na},
                    {"s", ns}
                };
            }

        }


        //float height = gameScaler.Scale_Value_To_Screen_Height(spriteLoc.y);
        float height;
        try{
            if(rocketGameManager == null){
                rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
            }
            height = rocketGameManager.calculateAltitude(spriteLoc.y);
            //Debug.Log("HEIGHT IS: " + height);
        }
        catch(Exception e){
            //Debug.Log("PROBLEM CALCULATING HEIGHT :(    :" + e + ")");
            height = 0.0f;
        }

        //Debug.Log("YEET " + height_color_map_sorted_keys[0]);
        float closest_lower = fog_height_color_size_map_sorted_keys[0];
        float closest_higher = fog_height_color_size_map_sorted_keys[fog_height_color_size_map_sorted_keys.Count-1];

        //Debug.Log("HEIGHT: " + height);
        //Debug.Log("LOWER: " + closest_lower);
        //Debug.Log("HIGHER: " + closest_higher);

        Dictionary<string, float> chosenColorSize = new Dictionary<string,float>(){
            {"r", fog_height_color_map[closest_lower].r},
            {"g", fog_height_color_map[closest_lower].g},
            {"b", fog_height_color_map[closest_lower].b},
            {"a", fog_height_color_map[closest_lower].a},
            {"s", fog_height_size_map[closest_lower]}
        };
        bool color_size_found = false;


        if (height <= closest_lower){
            color_size_found = true;
            // Debug.Log("POO -- CHOSE LOWER -> " + closest_lower + " --- " + height + " -- " + chosen_color);
        }
        else if (height >= closest_higher){
            chosenColorSize = new Dictionary<string,float>(){
                {"r", fog_height_color_map[closest_higher].r},
                {"g", fog_height_color_map[closest_higher].g},
                {"b", fog_height_color_map[closest_higher].b},
                {"a", fog_height_color_map[closest_higher].a},
                {"s", fog_height_size_map[closest_higher]}
            };
            color_size_found = true;
            // Debug.Log("POO -- CHOSE HIGHER");
        }


        if (!color_size_found){
            foreach (float heightk in fog_height_color_size_map_sorted_keys){
                if (heightk <= height){
                    closest_lower = heightk;
                }
                if (heightk >= height && closest_higher == fog_height_color_size_map_sorted_keys[fog_height_color_size_map_sorted_keys.Count-1]){
                    closest_higher = heightk;
                }
                //Debug.Log("POO --" + height + " LOW: " + closest_lower + " HIGH: " + closest_higher);
            }


            chosenColorSize = _chooseColorandSize(height, closest_lower, closest_higher);
            // Debug.Log("POO -- COLOR: " + chosen_color);

        }

        return chosenColorSize;
    }





    private Dictionary<string, float> currentColorSize = new Dictionary<string, float>(); // Just saving on initialization costs
    void updateFog(){



        if (fogMaterial == null){
            fogMaterial = GameObject.Find("Fog").GetComponent<SpriteRenderer>().material;
            Debug.LogError("CANT FIND FOG MATERIAL??");
        }


        if(fogActivated && fogMaterial != null){
            currentColorSize = getFogColorAndSize(rocket.transform.position);
            fogMaterial.SetFloat("_Fog_Size", currentColorSize["s"]);    
            fogMaterial.SetColor("_Fog_Color", new Color(currentColorSize["r"], currentColorSize["g"], currentColorSize["b"], currentColorSize["a"]));
        
        }
    }

    public void disableFog(){

        if (fogMaterial == null){
            fogMaterial = GameObject.Find("Fog").GetComponent<SpriteRenderer>().material;
            Debug.LogError("CANT FIND FOG MATERIAL??");
        }


        if (fogMaterial != null){
            fogActivated = false;
            fogMaterial.SetFloat("_Fog_Size", 0);
            fogMaterial.SetColor("_Fog_Color", Color.white);
        }
    }

}
