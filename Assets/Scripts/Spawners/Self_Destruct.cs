using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Self_Destruct : MonoBehaviour
{
    private GameObject cam;
    private Game_Scaler gameScaler;
    [SerializeField]
    private float maxDrawDistance = 1.0f;
    [SerializeField]
    private string spaceJunkSpawnerName;
    private Space_Junk_Spawner spaceJunkSpawner;

    // Start is called before the first frame update
    void Start()
    {
        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        cam = GameObject.Find("Main Camera");
    
        spaceJunkSpawner = GameObject.Find(spaceJunkSpawnerName).GetComponent<Space_Junk_Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFar()){
            //Destroy(gameObject);
            try{
                spaceJunkSpawner.despawnPoolObj(gameObject);
            }
            catch(System.Exception e){
                Destroy(gameObject);
            }
        }
    }

    bool isFar(){
        float scaledMaxDrawDistance = maxDrawDistance * gameScaler.ScaleX;
    
        if (Vector2.Distance(new Vector2(cam.transform.position.x, cam.transform.position.y), transform.position) >= scaledMaxDrawDistance){
            return true;
        }
        else{
            return false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.gameObject.tag == "Kill Plane"){
            try{
                spaceJunkSpawner.despawnPoolObj(gameObject);
            }
            catch(System.Exception){
                Destroy(gameObject);
            }
        }
    
    }
}
