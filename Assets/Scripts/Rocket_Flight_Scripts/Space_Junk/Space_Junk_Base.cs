using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Base : MonoBehaviour
{

    protected Rigidbody2D rb;
    [SerializeField]
    private string bumpSound;

    [SerializeField]
    private GameObject TeamRocketObject;


    protected void OnEnable(){

        Debug.Assert(TeamRocketObject != null);

        rb = GetComponent<Rigidbody2D>();

        //Debug.Log("GOT RB: " + gameObject.name);

        Rocket_Game_Manager.PauseLaunchSceneInfo += onGamePause;
    }

    protected void OnDisable(){
        Rocket_Game_Manager.PauseLaunchSceneInfo -= onGamePause;
        SpawnTeamRocket(); // TODO: ONLY DO THIS IF WE AREN'T DONE WITH THE ROCKET GAME
    }

    void onGamePause(bool paused){
        
        if (paused){
            //Debug.Log("HEY TRYING TO FREEZE POSITION: " + gameObject.name);
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else{
            //Debug.Log("HEY TRYING TO UNFREEZE POSITION: " + gameObject.name);
            rb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        }
    }

    public string getBumpSound(){
        return bumpSound;
    }

    
    private void SpawnTeamRocket(){
        Instantiate(TeamRocketObject, transform.position, transform.rotation);
    }
}
