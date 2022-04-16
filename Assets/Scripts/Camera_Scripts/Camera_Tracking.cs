using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Tracking : MonoBehaviour
{

    [SerializeField]
    public bool active = false;

    [SerializeField]
    private GameObject Game_Scaler;
    private Vector3 prevPos = new Vector3(-1f, -1f, -1f);
    private Vector3 pos = new Vector3(0f, 0f, 0f);

    private Vector3 upperLeftScreen, upperRightScreen, lowerLeftScreen, lowerRightScreen;
    public Vector3 upperLeft {get; private set;}
    public Vector3 upperRight {get; private set;}
    public Vector3 lowerLeft {get; private set;}
    public Vector3 lowerRight {get; private set;}
    private bool displayed = false;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        pos = transform.position;


        upperLeftScreen = new Vector3(0, Screen.height, -10f);
        upperRightScreen = new Vector3(Screen.width, Screen.height, -10f);
        lowerLeftScreen = new Vector3(0, 0, -10f);
        lowerRightScreen = new Vector3(Screen.width, 0, -10f);
   

        upperLeft = cam.ScreenToWorldPoint(upperLeftScreen);
        upperRight = cam.ScreenToWorldPoint(upperRightScreen);
        lowerLeft = cam.ScreenToWorldPoint(lowerLeftScreen);
        lowerRight = cam.ScreenToWorldPoint(lowerRightScreen);

        if (pos == prevPos && !displayed){
            // Debug.Log("----------------------------------------");
            // Debug.Log("CAMERA IS AT: " + pos);
            // Debug.Log("UPPER RIGHT: " + upperRight);
            // Debug.Log("UPPER LEFT: " + upperLeft);
            // Debug.Log("LOWER RIGHT: " + lowerRight);
            // Debug.Log("LOWER LEFT: " + lowerLeft);
            // Debug.Log("----------------------------------------");
            displayed = true;
        }
        else if (pos != prevPos){
            displayed = false;
        }

        prevPos = pos;
        
    }
}
