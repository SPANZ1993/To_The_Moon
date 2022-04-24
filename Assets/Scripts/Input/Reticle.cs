using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{

    public static Reticle instance;


    private SpriteRenderer sprite;
    private Color old_color;

    private bool tapInProgress = false;
    private Touch_Detection touch_detection;
    private CircleCollider2D reticle_collider;

    public GameObject current_game_object;



    public delegate void TapStart(GameObject other);
    public static event TapStart TapStartInfo;

    public delegate void TapStay(GameObject other);
    public static event TapStay TapStayInfo;

    public delegate void TapEnd(GameObject other);
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
        TapStartInfo(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TapStayInfo(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TapEndInfo(other.gameObject);
    }


}
