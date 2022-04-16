using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListeningSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;


    public GameObject Spawn(){
        GameObject spawnedObject = Instantiate(obj, transform.position, Quaternion.identity);
        //Debug.Log("SPAWNED: " + spawnedObject);
        return spawnedObject;
    }
}
