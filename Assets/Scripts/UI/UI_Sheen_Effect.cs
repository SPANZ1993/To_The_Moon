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

    bool flipped = false;

    void Awake(){
        LeanTween.init(10000);
        if(gameObject.GetComponent<Image>() != null){
            UIMaterial = gameObject.GetComponent<Image>().material;
            //Debug.Log("SET MATERIAL TO: " + UIMaterial);
        }
        else{
            UIMaterial = gameObject.GetComponent<SpriteRenderer>().material;
            if(gameObject.GetComponent<SpriteRenderer>().flipX){
                flipped = true;
            }
        }
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

    void OnLevelWasLoaded(){
        UIMaterial = null;
        //Debug.Log("IS TWEENING BEFORE: " + LeanTween.isTweening(sheenWipeId));
        LeanTween.cancel(sheenWipeId);
        //Debug.Log("IS TWEENING AFTER: " + LeanTween.isTweening(sheenWipeId));
        Awake();
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

        // bool egg = gameObject == null;
        // bool egg2 = startSheenLoc == null;
        // bool egg3 = endSheenLoc == null;
        // bool egg4 = sheenWipeLength == null;
        // bool egg5 = sheenEaseType == null;

        // if(egg){
        //     Debug.Log("NULL: " + 1);
        // }
        // if(egg2){
        //     Debug.Log("NULL: " + 2);
        // }
        // if(egg3){
        //     Debug.Log("NULL: " + 3);
        // }
        // if(egg4){
        //     Debug.Log("NULL: " + 4);
        // }
        // if(egg5){
        //     Debug.Log("NULL: " + 5);
        // }

        sheenWipeId = LeanTween.value(gameObject, startSheenLoc, endSheenLoc, sheenWipeLength).setEase(sheenEaseType).setOnUpdate(
                    (value) =>
                    {
                        // GameObject tgo = new GameObject();
                        // bool egg = gameObject == tgo;
                        // bool egg2 = startSheenLoc == 0;
                        // bool egg3 = endSheenLoc == 0;
                        // bool egg4 = sheenWipeLength == 0;
                        // Destroy(tgo);

                        UIMaterial.SetFloat("_ShineLocation", !flipped ? value : 1f-(1f-value));
                    }
                ).setOnComplete(startSheenWipe).id;
    }


}
