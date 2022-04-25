using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Base : MonoBehaviour
{

    protected Rigidbody2D rb;


    protected void OnEnable(){
        rb = GetComponent<Rigidbody2D>();

        //Debug.Log("GOT RB: " + gameObject.name);

        Rocket_Game_Manager.PauseLaunchSceneInfo += onGamePause;
    }

    protected void OnDisable(){
        Rocket_Game_Manager.PauseLaunchSceneInfo -= onGamePause;
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
}
