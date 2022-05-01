using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System;

public abstract class Countdown_Timer_Controller_Base : MonoBehaviour // Make this an abstract class with abstract methods to check the time left and whether the time is done
{

    [SerializeField]
    GameObject waypoint;

    private RectTransform rt;
    private RectTransform textRt;
    private TextMeshProUGUI textTMP;
    private Camera cam;
    private Canvas canvas;
    private Vector3 curScreenLocation;
    private Game_Scaler gameScaler;
    private float screenScaleFactor;

    protected double timeLeft = 33300;
    protected  bool isReady = false;
    protected  abstract void setTimeLeft();
    protected  abstract void setIsReady();


    // Start is called before the first frame update
    protected void Start()
    {
        rt = GetComponent<RectTransform>();
        textRt = rt.GetChild(0).GetComponent<RectTransform>();
        textTMP =  rt.GetChild(0).GetComponent<TextMeshProUGUI>();
        //Debug.Log("TEXTTMP: " + textTMP);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();        


        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(0f, 0f);

        screenScaleFactor = canvas.scaleFactor;

        // Scale This Thing A Little More Than the Rest of the UI
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x*(Mathf.Max(1f/screenScaleFactor, 0f)), rt.sizeDelta.y*(Mathf.Max(1f/screenScaleFactor, 0f)));
        rt.sizeDelta = new Vector2(rt.sizeDelta.x*gameScaler.ScaleX, rt.sizeDelta.y*gameScaler.ScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        setTimerLocation();
        setTimeLeft();
        setIsReady();
        updateTimer();
    }

    void setTimerLocation(){


        void textCornersToAnchors(){
            textRt.localScale = new Vector3(1f, 1f, 1f);
            textRt.offsetMin = textRt.offsetMax = new Vector2(0f, 0f);
        }
    

        curScreenLocation = cam.WorldToScreenPoint(waypoint.transform.position);
        curScreenLocation.x = curScreenLocation.x/screenScaleFactor;
        curScreenLocation.y = curScreenLocation.y/screenScaleFactor;
        //Debug.Log("SCREEN LOC: " + curScreenLocation + " LOCALSCALE: " + rt.localScale + " SIZEDELTA: " + rt.sizeDelta + " GAME SCALE: " + gameScaler.ScaleX + "--" + gameScaler.ScaleY + " GAME SCALE PX: " + gameScaler.ScaleXPx + "--" + gameScaler.ScaleYPx);
        curScreenLocation.x = curScreenLocation.x - ((rt.localScale.x*rt.sizeDelta.x)/2f);
        curScreenLocation.y = curScreenLocation.y - ((rt.localScale.y*rt.sizeDelta.y)/2f);
        
        rt.anchoredPosition = curScreenLocation;
        textCornersToAnchors();
    }

    private void updateTimer(){


        double[] ConvertSectoDay(double n)
        {
            double day = n / (24.0 * 3600.0);
        
            n = n % (24.0 * 3600.0);
            double hour = n / 3600.0;
        
            n %= 3600.0;
            double minutes = n / 60.0 ;
        
            n %= 60.0;
            double seconds = n;


            if(day < 1.0){
                day = 0.0;
                if(hour < 1.0){
                    hour = 0.0;
                    if(minutes < 1.0){
                        minutes = 0;
                        if(seconds < 1.0){
                            seconds = 1.0;
                        }
                    }
                }

            }
            
            return new double[] {Math.Floor(day), Math.Floor(hour), Math.Floor(minutes), Math.Floor(seconds)};
        }



        if(isReady){
            textTMP.text = "READY";
        }
        else{
            double[] timeArr = ConvertSectoDay(timeLeft);
            Console.WriteLine("WORLSKDJFL:S");

            Console.WriteLine("TIMESPAN: " + string.Join(", ", timeArr));


            string displayString = "";
            if(timeArr[0] != 0.0){
                displayString += "D: " + timeArr[0].ToString("0");
            }
            if(timeArr[1] != 0.0){
                if(displayString != ""){
                    displayString += "  ";
                }
                displayString += "H: " + timeArr[1].ToString("0");
            }
            if(timeArr[2] != 0.0){
                if(displayString != ""){
                    displayString += "  ";
                }
                displayString += "M: " + timeArr[2].ToString("0");
            }
            if(timeArr[3] != 0.0){
                if(displayString != ""){
                    displayString += "  ";
                }
                displayString += "S: " + timeArr[3].ToString("0");
            }
            textTMP.text = displayString;
        }

    }
    
}
