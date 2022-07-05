using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: MAKE THIS A PART OF THE TOUCH DETECTION SCRIPT


public class Tap_Handler : MonoBehaviour
{
    private ITappable tappable;

    void Start()
    {
        tappable = this.GetComponent<ITappable>();
    }


    void OnEnable()
    {
        Touch_Detection.TapStartInfo += TapStartListener;
        Touch_Detection.TapStayInfo += TapStayListener;
        Touch_Detection.TapEndInfo += TapEndListener;
    }

    void OnDisable()
    {
        Touch_Detection.TapStartInfo -= TapStartListener;
        Touch_Detection.TapStayInfo -= TapStayListener;
        Touch_Detection.TapEndInfo -= TapEndListener;
    }

    
    void TapStartListener(GameObject other)
    {
        if(other == this.gameObject){
            tappable.onTapStart();
        }
    }
    
    void TapStayListener(GameObject other)
    {
        if(other == this.gameObject){
            tappable.onTapStay();
        }
    }

    void TapEndListener(GameObject other, bool wasFirst)
    {
        if(other == this.gameObject){
            tappable.onTapEnd(wasFirst);
        }
    }
}
