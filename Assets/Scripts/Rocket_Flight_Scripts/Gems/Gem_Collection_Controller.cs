using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_Collection_Controller : MonoBehaviour
{


    public delegate void GemCollected();
    public static event GemCollected GemCollectedInfo;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Rocket"){
            if (GemCollectedInfo != null){
                if(!Audio_Manager.instance.IsPlaying("Space_Gem_Collect")){
                    Audio_Manager.instance.Play("Space_Gem_Collect");
                }
                GemCollectedInfo();
                Destroy(gameObject);
            }
        }

    }
}
