using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Dark_Matter : Space_Junk_Base
{

    // GameObject camObj;
    // Camera cam;


    // Rocket_Game_Manager rocketGameManager;

    public delegate void AlertDarkMatterEnabled(GameObject DarkMatter);
    public static event AlertDarkMatterEnabled AlertDarkMatterEnabledInfo;


    public delegate void AlertDarkMatterDisabled(GameObject DarkMatter);
    public static event AlertDarkMatterDisabled AlertDarkMatterDisabledInfo;

    // float altitudeLoc;

    float timeSinceLastRocketCollision = 0f;


    void OnEnable(){
        // camObj = GameObject.Find("Main Camera");
        // cam = camObj.GetComponent<Camera>();

        // rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        
        // altitudeLoc = rocketGameManager.calculateAltitude(gameObject.transform.position.y);
        base.OnEnable();

        // Debug.Log("DO WE HAVE THE SHIELD UNLOCKED? " + Upgrades_Manager.instance.upgradesUnlockedDict[Upgrade.Particle_Shield]);
        if(Upgrades_Manager.instance.upgradesUnlockedDict[Upgrade.Particle_Shield]){
            GetComponent<Collider2D>().enabled = false;
        }
        if(AlertDarkMatterEnabledInfo != null){
            AlertDarkMatterEnabledInfo(gameObject);
        }
    }

    void OnDisable(){
        base.OnDisable();

        //Rocket_Game_Manager.PauseLaunchSceneInfo -= onGamePause;
        if(AlertDarkMatterDisabledInfo != null){
            AlertDarkMatterDisabledInfo(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastRocketCollision += Time.deltaTime;
        // setLocation();
    }


    void OnCollisionEnter2D(Collision2D collision){
        GameObject other = collision.gameObject;
        if (other.name == "Rocket"){
            if(timeSinceLastRocketCollision <= 5f){
                other.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -150f, 0f));
            }
            timeSinceLastRocketCollision = 0f;
        }
    }

    // void setLocation(){
    //     Vector2 pos = gameObject.transform.position;
    //     pos.x = camObj.transform.position.x;
    //     if(altitudeLoc != null){
    //         pos.y = rocketGameManager.calculateGameYPos((float)altitudeLoc);
    //     }
    //     gameObject.transform.position = pos;
    // }
}
