using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Reticle : MonoBehaviour
{

    public static Reticle instance;


    private SpriteRenderer sprite;
    private Color old_color;

    private bool tapInProgress = false;
    private Touch_Detection touch_detection;
    private CircleCollider2D reticle_collider;

    public GameObject current_start_game_object; // Game Object that the current tap/swipe started on (if there is one)
    public int current_touch_id; // Just an identifier for the current swipe


    public delegate void TapStart(GameObject other);
    public static event TapStart TapStartInfo;

    public delegate void TapStay(GameObject other);
    public static event TapStay TapStayInfo;

    public delegate void TapEnd(GameObject other, bool wasFirst); // Was this the gameobject on which the tap originated?
    public static event TapEnd TapEndInfo;


    void Awake()
    {

        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        touch_detection = GameObject.Find("Input_Detector").GetComponent<Touch_Detection>();
        reticle_collider = this.GetComponent<CircleCollider2D>();
    }
    

    // Update is called once per frame
    void Update()
    {

        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
  
        //Debug.Log("ON TRIGGER ENTER: " + Touch_Detection.instance.touch.phase + " " + Touch_Detection.instance.dragLocs.Count);
        // If either we are starting a tap, or we are on the same tap and entering the original object
        if( (Touch_Detection.instance.touchPhases.Count < 3 || 
            Touch_Detection.instance.touchPhases[Touch_Detection.instance.touchPhases.Count-1]  == TouchPhase.Began ||
            Touch_Detection.instance.touchPhases[Touch_Detection.instance.touchPhases.Count-2] == TouchPhase.Began) 
            ||
            (current_touch_id == Touch_Detection.instance.touchId && current_start_game_object == other.gameObject)
            ){
            //Debug.Log("SETTING OBJECT: " + other);
            current_start_game_object = other.gameObject;
            current_touch_id = Touch_Detection.instance.touchId;
        }
        else{
            //Debug.Log("UNSETTING OLD " + current_touch_id + " NEW " + Touch_Detection.instance.touchId);
            current_start_game_object = null;
            current_touch_id = -1;
        }

        TapStartInfo(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TapStayInfo(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if((!touch_detection.currentlySwiping &&
        //     current_touch_id == Touch_Detection.instance.touchId && 
        //     current_start_game_object == other.gameObject)
        // || 
        // SceneManager.GetActiveScene().name != "Main_Area"){
        //     //Debug.Log("INSIDE BOX?: " + Touch_Detection.instance.reticleInsideDisabledBox(Input.GetTouch(0).position));
        //     TapEndInfo(other.gameObject);
        // }
        if(!touch_detection.currentlySwiping || SceneManager.GetActiveScene().name != "Main_Area"){
            TapEndInfo(other.gameObject, (current_touch_id == Touch_Detection.instance.touchId && current_start_game_object == other.gameObject));
        }
    }


}
