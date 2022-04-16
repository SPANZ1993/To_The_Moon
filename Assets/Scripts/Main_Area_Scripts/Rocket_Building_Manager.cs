using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Building_Manager : MonoBehaviour, ITappable
{


    // Tap Stuff
    //[SerializeField]
    //private GameObject Input_Detector;
    private Touch_Detection touchDetection;
    private bool tapInitiated = false; // The current tap started on the minecart
    private bool tapped = false; // The screen is held down currently on the minecart
    private List<Vector2> curDragLocs = new List<Vector2>();


    public delegate void RocketBuildingTapped();
    public static event RocketBuildingTapped RocketBuildingTappedInfo;

    // Start is called before the first frame update
    void Start()
    {
        touchDetection = GameObject.Find("Input_Detector").GetComponent<Touch_Detection>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //Debug.Log("YEET");
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
                //Debug.Log("TAPPED THE ROCKET BUILDING");
                if (RocketBuildingTappedInfo != null){
                    RocketBuildingTappedInfo();
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