using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class Minecart_Controller : MonoBehaviour, ITappable
{



    private bool tapInitiated = false; // The current tap started on the minecart
    private bool tapped = false; // The screen is held down currently on the minecart
    private List<Vector2> curDragLocs = new List<Vector2>();

    


    [SerializeField]
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;


    //public bool startComplete = false;



    public delegate void _MinecartTapped(); // Only minecart manager will be subscribed to this
    public static event _MinecartTapped _MinecartTappedInfo;





    

    // Start is called before the first frame update
    void Start()
    {
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
        //lastEmptiedTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(); // REMOVE
        //startComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If it hasn't yet been initialized by the game manager, then just chill
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            curDragLocs = touchDetection.dragLocs;
        }

    }


    public void onTapStart()
    {

        // Debug.Log("LBC START");
        
        //If we dragged onto
        if (curDragLocs.Count > 1){
            tapped = true;
        }
        // If we tapped onto
        else{
            tapInitiated = true;
            tapped = true;
        }

    }
    
    public void onTapStay()
    {
        // Debug.Log("LBC STAY");
    }



    // We want to do this so we can do different things when the user taps and drags off the sprite
    // and when the user taps and lets go on the sprite
    // (Both of these events call onTapEnd())
    IEnumerator _onTapEnd(bool first)
    {
        yield return new WaitForSeconds(0);
        if(touchDetection.tapInProgress){
            // Debug.Log("Dragged Off");
            if (first){
                tapped = false;
            }
            StartCoroutine(_onTapEnd(false));
        }
        else
        {
            if (tapInitiated && tapped){
                if (_MinecartTappedInfo != null){
                    _MinecartTappedInfo();
                }
            }
            tapped = false;
            tapInitiated = false;

        }
    }


    public void onTapEnd()
    {
        // Debug.Log("LBC END");
        // If we lifted our finger, else if we dragged off the object
        StartCoroutine(_onTapEnd(true));
    }


}
