using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_Page_Manager : MonoBehaviour
{

    bool contactedPlayfabServer = false;
    bool failedContactingPlayfabServer = false;
    bool startedSceneTransition = false;


    void OnEnable(){

    }

    void OnDisable(){

    }


    public delegate void EndLandingPageScene();
    public static event EndLandingPageScene EndLandingPageSceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("_EndLandingPageScene", 10f);
    }

    void _EndLandingPageScene(){
        if (EndLandingPageSceneInfo != null){
            EndLandingPageSceneInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
