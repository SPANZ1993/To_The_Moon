using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Object_Pool : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> poolObjList;
    [SerializeField]
    public List<int> numToSpawnList;
    [SerializeField]
    public List<bool> shouldExpandList;

    private List<Queue<GameObject>> poolObjQueues;
    private Dictionary<string, int> poolTagToId; 

    [SerializeField]
    private bool ScaleObjects;
    private Game_Scaler gameScaler;

    // void Awake(){
    //     if (ScaleObjects){
    //         gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
    //         for (int i=0; i < poolObjList.Count; i++){
    //             GameObject cur_obj = poolObjList[i];
    //             cur_obj.transform.localScale = new Vector3(gameScaler.Scale_Value_To_Screen_Width(cur_obj.transform.localScale.x), gameScaler.Scale_Value_To_Screen_Height(cur_obj.transform.localScale.y), cur_obj.transform.localScale.z);
    //             poolObjList[i] = cur_obj;
    //         }
    //     }
    // }

    void Awake(){
        if (ScaleObjects){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
            for (int i=0; i < poolObjList.Count; i++){
                GameObject cur_obj = Instantiate(poolObjList[i], new Vector3(), new Quaternion());
                cur_obj.transform.localScale = new Vector3(gameScaler.Scale_Value_To_Screen_Width(cur_obj.transform.localScale.x), gameScaler.Scale_Value_To_Screen_Height(cur_obj.transform.localScale.y), cur_obj.transform.localScale.z);
                poolObjList[i] = cur_obj;
                cur_obj.SetActive(false);
            }
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        VerifyAllPoolObjects();
        InstantiateAllPoolObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void VerifyAllPoolObjects(){
        List<string> allTags = new List<string>();
        foreach (GameObject obj in poolObjList){
            if (obj.tag == "Untagged"){
                throw new ArgumentException("Every prefab in object pool must have an associated tag... " + obj.name + " does not have a tag associated.");
            }
            else{
                if (allTags.Contains(obj.tag)){
                    throw new ArgumentException("Every prefab in pool must have a unique tag.... Tag " + obj.tag + " Is Repeated.");
                }
                else{
                    allTags.Add(obj.tag);
                }
            }
        }
        if (!(poolObjList.Count == numToSpawnList.Count && numToSpawnList.Count == shouldExpandList.Count)){
            throw new ArgumentException("PoolObjList, NumToSpawnList, and ShouldExpandList should all be same length in pool.");
        }
    }

    public void InstantiateAllPoolObjects(){

        poolObjQueues = new List<Queue<GameObject>>();
        poolTagToId = new Dictionary<string, int>();


        GameObject curPrefab;
        GameObject curInstance;
        Queue<GameObject> curGameObjectQueue; 
        for (int i = 0; i < poolObjList.Count; i++){
            curPrefab = poolObjList[i];
            //Debug.Log("SETTING TAG " + curPrefab.tag);
            //Debug.Log("SETTING TAG " + curPrefab.tag + " TO " + i);
            poolTagToId[curPrefab.tag] = i;
            curGameObjectQueue = new Queue<GameObject>();
            for (int j = 0; j < numToSpawnList[i]; j++){
                curInstance = Instantiate(curPrefab, new Vector3(), new Quaternion());
                curGameObjectQueue.Enqueue(curInstance);
                curInstance.SetActive(false);                
            }
            poolObjQueues.Add(curGameObjectQueue);
        }
    }






    public GameObject getGameObj(int i){
        return poolObjList[i];
    }
    
    public GameObject getGameObj(string tag){
        return poolObjList[poolTagToId[tag]];
    }




    public GameObject spawnPoolObj(int i, Vector3 loc){
        return spawnPoolObj(i, loc, new Quaternion());
    }
    
    public GameObject spawnPoolObj(GameObject g, Vector3 loc){
        return spawnPoolObj(g, loc, new Quaternion());
    }

    public GameObject spawnPoolObj(string tag, Vector3 loc){
        return spawnPoolObj(tag, loc, new Quaternion());
    }



    public GameObject spawnPoolObj(int i, Vector3 loc, Quaternion rot){
        return spawnPoolObj(poolObjList[i], loc, rot);
    }

    public GameObject spawnPoolObj(GameObject g, Vector3 loc, Quaternion rot){
        return spawnPoolObj(g.tag, loc, rot);
    }

    public GameObject spawnPoolObj(string tag, Vector3 loc, Quaternion rot){
        if (poolObjQueues[poolTagToId[tag]].Count > 0){
            try{
                GameObject obj = poolObjQueues[poolTagToId[tag]].Dequeue();
                obj.transform.position = loc;
                obj.transform.rotation = rot;
                obj.SetActive(true);
                return obj;
            }
            catch(Exception e){
                if(shouldExpandList[poolTagToId[tag]]){
                    GameObject obj = Instantiate(poolObjList[poolTagToId[tag]], loc, rot);
                    return obj;
                }
                // Otherwise pass... This probably means the queue emptied after the peek. No problem.
                else{
                    return null;
                }
            }
        }
        else if(shouldExpandList[poolTagToId[tag]]){
            return Instantiate(poolObjList[poolTagToId[tag]], loc, rot);
        }
        else{
            return null;
        }
    }



    public void despawnPoolObj(GameObject obj){
        string tag = obj.tag;
        if (obj.activeSelf){
            obj.SetActive(false);
            poolObjQueues[poolTagToId[tag]].Enqueue(obj);
        }
    }
}
