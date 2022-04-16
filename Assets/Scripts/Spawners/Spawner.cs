using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private float minSecondsBetweenSpawn;
    [SerializeField]
    private float maxSecondsBetweenSpawn;
    [SerializeField]
    private GameObject obj;

    private float timeToNextSpawn;
    
    // Should we instantiate one of the things at the start?
    [SerializeField]
    private bool spawnAtStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnAtStart){
            Instantiate(obj, transform.position, Quaternion.identity);
        }
        updateTimeToSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToNextSpawn <= 0){
            Instantiate(obj, transform.position, Quaternion.identity);
            updateTimeToSpawn();
        }
        timeToNextSpawn -= Time.deltaTime;   
    }

    void updateTimeToSpawn(){
        if (minSecondsBetweenSpawn == maxSecondsBetweenSpawn){
            timeToNextSpawn = minSecondsBetweenSpawn;
        }
        else{
            timeToNextSpawn = Random.Range(minSecondsBetweenSpawn, maxSecondsBetweenSpawn);
        }
    }
}
