using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swaying_Grass : MonoBehaviour
{
    [SerializeField]
    float leftToRightSway;
    [SerializeField]
    float upAndDownSway;
    [SerializeField]
    float swayLength;

    [SerializeField]
    float swayWaitTime;

    [SerializeField]
    LeanTweenType swayEaseType;


    [SerializeField]
    float maxXTilingChange;
    [SerializeField]
    float maxYTilingChange;

    [SerializeField]
    float maxTotalXTilingChange;
    [SerializeField]
    float maxTotalYTilingChange;

    [SerializeField]
    float tileChangeMinTime;
    [SerializeField]
    float tileChangeMaxTime;
    [SerializeField]
    LeanTweenType tileChangeEaseType;


    private Vector2 targetTiling;
    private Vector2 originalTiling;
    private Vector2 curTiling;


    private Vector2 startSwayLoc;
    private Vector2 endSwayLoc;

    bool firstSway = true;
    float curSwayDirection = 1f;

    Material grassMaterial;

    // Start is called before the first frame update
    void Start()
    {
        grassMaterial = gameObject.GetComponent<Renderer>().material;
        startSwayLoc = new Vector2();
        endSwayLoc = new Vector2();
        targetTiling = new Vector2();
        curTiling = new Vector2();
        originalTiling = grassMaterial.GetTextureScale("_OverlayTex");
        updateMatAction = updateMaterial;
        updateTilingAction = updateTiling;
        Sway();
        changeTiling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void updateMaterial(Vector2 value){
        grassMaterial.SetTextureOffset("_OverlayTex", value);
    }
    
    System.Action<Vector2> updateMatAction;

    void _Sway(){
        curSwayDirection *= -1;
        startSwayLoc = grassMaterial.GetTextureOffset("_OverlayTex");
        endSwayLoc.x = leftToRightSway;
        endSwayLoc.y = upAndDownSway;
        endSwayLoc *= curSwayDirection;
        if(firstSway){
            endSwayLoc *= 0.5f;
            firstSway = false;
        }

        LeanTween.value(gameObject, (Vector2)startSwayLoc, (Vector2)endSwayLoc, swayLength).setEase(swayEaseType).setOnUpdate(
            updateMatAction
        ).setOnComplete(Sway);
    }

    IEnumerator _waitThenSway(){
        yield return new WaitForSeconds(swayWaitTime);
        _Sway();
    }

    void Sway(){
        if(swayWaitTime >= 0){
            StartCoroutine(_waitThenSway());
        }
        else{
            Sway();
        }
    }




    void updateTiling(Vector2 value){
        grassMaterial.SetTextureScale("_OverlayTex", value);
    }
    
    System.Action<Vector2> updateTilingAction;



    void changeTiling(){
        targetTiling.x = Random.Range(0, maxXTilingChange);
        targetTiling.y = Random.Range(0, maxYTilingChange);

        if (Random.value < .5){
            targetTiling.x *= -1;
        }
        if (Random.value < .5){
            targetTiling.y *= -1;
        }

        curTiling = grassMaterial.GetTextureScale("_OverlayTex");
        targetTiling.x += curTiling.x;
        targetTiling.y += curTiling.y; 

        if(targetTiling.x > originalTiling.x + maxXTilingChange){
            targetTiling.x =  originalTiling.x + maxXTilingChange;
        }
        else if(targetTiling.x < originalTiling.x - maxXTilingChange){
            targetTiling.x = originalTiling.x - maxXTilingChange;
        }
        
        if(targetTiling.y > originalTiling.y + maxYTilingChange){
            targetTiling.y =  originalTiling.y + maxYTilingChange;
        }
        else if(targetTiling.y < originalTiling.y - maxYTilingChange){
            targetTiling.y = originalTiling.y - maxYTilingChange;
        }
        

        //rend.material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
        LeanTween.value(gameObject, (Vector2)curTiling, (Vector2)targetTiling, Random.Range(tileChangeMinTime, tileChangeMaxTime)).setEase(tileChangeEaseType).setOnUpdate(
            updateTilingAction
        ).setOnComplete(changeTiling);
    }

}
