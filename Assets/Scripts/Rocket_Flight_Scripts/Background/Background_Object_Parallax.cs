using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Object_Parallax : MonoBehaviour
{

    private Camera cam;
    private GameObject rocket;
    private Rocket_Game_Manager rocketGameManager;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float objectBotPos, objectTopPos;
    public float rocketBotAlt;
    public float rocketTopAlt;
    private float rocketAltDiff;

    [SerializeField]
    private float objectLeftRightPos;

    [SerializeField]
    bool destroyAtDestination;

    private float camWidth, camHeight;

    public bool firstEnabled = false;
    
    Vector3 pos;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //rocket = GameObject.Find("Rocket");
        rocketGameManager = GameObject.Find("Rocket_Game_Manager").GetComponent<Rocket_Game_Manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        rocketAltDiff = rocketTopAlt - rocketBotAlt;
        camHeight = cam.orthographicSize * 2.0f;
        camWidth = camHeight * cam.aspect;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(firstEnabled && rocketGameManager.gameStarted && rocketGameManager.rocketAltitude >= rocketBotAlt){
            spriteRenderer.enabled = true;
        }
        
        if(spriteRenderer.enabled){
            //Debug.Log("INTERP: " + Mathf.Max((rocketGameManager.rocketAltitude-rocketBotAlt)/rocketAltDiff, 0.0f));
            pos.x = cam.transform.position.x + objectLeftRightPos;
            pos.y = cam.transform.position.y + Mathf.Lerp(objectBotPos, objectTopPos, Mathf.Max((rocketGameManager.rocketAltitude-rocketBotAlt)/rocketAltDiff, 0.0f));
            transform.position = pos;
        }
    }
}
