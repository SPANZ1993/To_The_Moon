using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_Object_Controller : MonoBehaviour, ITappable
{

    // Tap Stuff
    private GameObject Input_Detector;
    private Touch_Detection touchDetection;
    private bool tapInitiated = false; // The current tap started on this object
    private bool tapped = false; // The screen is held down currently on this object
    private List<Vector2> curDragLocs = new List<Vector2>();

    double numTaps = 0.0;

    // Start is called before the first frame update
    void Start()
    {
        Input_Detector = GameObject.Find("Input_Detector");
        touchDetection = Input_Detector.GetComponent<Touch_Detection>();
    }

    // Update is called once per frame
    void Update()
    {
        curDragLocs = touchDetection.dragLocs;
    }


    public delegate void ObjectTapped(string gameObjectName);
    public static event ObjectTapped ObjectTappedInfo;

    public delegate void ObjectTappedStart(string gameObjectName);
    public static event ObjectTappedStart ObjectTappedStartInfo;


    public void onTapStart()
    {

        
        //If we dragged onto
        if (curDragLocs.Count > 1){
            tapped = true;
        }
        // If we tapped onto
        else{
            tapInitiated = true;
            tapped = true;
            if (ObjectTappedStartInfo != null){
                ObjectTappedStartInfo(this.gameObject.name);
            }
        }

    }
    
    public void onTapStay()
    {

    }



    // We want to do this so we can do different things when the user taps and drags off the sprite
    // and when the user taps and lets go on the sprite
    // (Both of these events call onTapEnd())
    IEnumerator _onTapEnd(bool first)
    {
        yield return new WaitForSeconds(0);
        if(touchDetection.tapInProgress){
            if (first){
                tapped = false;
            }
            // StartCoroutine(_onTapEnd(false));
            ObjectTappedInfo(this.gameObject.name); // New
        }
        else
        {
            if (tapInitiated && tapped){
                numTaps++;
                if (ObjectTappedInfo != null){
                    ObjectTappedInfo(this.gameObject.name);
                }
            }
            tapped = false;
            tapInitiated = false;

        }
    }


    public void onTapEnd(bool wasFirst)
    {
        // If we lifted our finger, else if we dragged off the object
        StartCoroutine(_onTapEnd(true));
    }


}
