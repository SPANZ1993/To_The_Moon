using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sheen_Effect : MonoBehaviour
{

    private Material UIMaterial;
    private float sheenWipeLength;
    private int sheenWipeId;
    float sheenLoc;
    LeanTweenType sheenEaseType;

    float highSheenVal;
    float lowSheenVal;

    void Awake(){
        UIMaterial = gameObject.GetComponent<Image>().material;
        //sheenEaseType = LeanTweenType.easeOutCirc;
        sheenWipeId = -1;
        sheenWipeLength = 3;
        sheenEaseType = LeanTweenType.easeInOutSine;
    
        highSheenVal = 1f;
        lowSheenVal = 0f;

    }


    // Start is called before the first frame update
    void Start()
    {
        sheenLoc = 1;
        UIMaterial.SetFloat("_ShineLocation", sheenLoc);
        startSheenWipe();
    }

    void Update(){
        sheenLoc = UIMaterial.GetFloat("_ShineLocation");
    }

    float startSheenLoc;
    float endSheenLoc;
    void startSheenWipe(){
        if(sheenLoc > .5){
            startSheenLoc = highSheenVal;
            endSheenLoc = lowSheenVal;
        }
        else{
            startSheenLoc = lowSheenVal;
            endSheenLoc = highSheenVal;
        }


        LeanTween.value(gameObject, startSheenLoc, endSheenLoc, sheenWipeLength).setEase(sheenEaseType).setOnUpdate(
                    (value) =>
                    {
                        UIMaterial.SetFloat("_ShineLocation", value);
                    }
                ).setOnComplete(startSheenWipe);
    }


}
