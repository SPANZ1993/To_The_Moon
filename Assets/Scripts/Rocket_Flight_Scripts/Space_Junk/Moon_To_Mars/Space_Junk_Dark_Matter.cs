using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Dark_Matter : Space_Junk_Base
{

    GameObject camObj;
    Camera cam;


    Rocket_Game_Manager rocketGameManager;


    float altitudeLoc;

    void OnEnable(){
        camObj = GameObject.Find("Main Camera");
        cam = camObj.GetComponent<Camera>();

        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        
        altitudeLoc = rocketGameManager.calculateAltitude(gameObject.transform.position.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setLocation();
    }


    void setLocation(){
        Vector2 pos = gameObject.transform.position;
        pos.x = camObj.transform.position.x;
        if(altitudeLoc != null){
            pos.y = rocketGameManager.calculateGameYPos((float)altitudeLoc);
        }
        gameObject.transform.position = pos;
    }
}
