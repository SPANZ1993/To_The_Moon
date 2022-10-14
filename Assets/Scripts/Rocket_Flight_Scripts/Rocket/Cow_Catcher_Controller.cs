using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow_Catcher_Controller : MonoBehaviour
{

    [SerializeField]
    float GlowOffsetTime = 1;

    [SerializeField]
    float minGlowVal = 0;

    [SerializeField]
    float maxGlowVal = 2;

    [SerializeField]
    float GlowTime = 1;

    [SerializeField]
    float FadeTime = 1;

    [SerializeField]
    float MaxHitGlow = 5;

    [SerializeField]
    LeanTweenType GlowEase;

    [SerializeField]
    LeanTweenType FadeEase;

    [SerializeField]
    LeanTweenType SizeEase;



    GameObject CowCatcher1;
    GameObject CowCatcher2;
    GameObject CowCatcher3;


    Material cowCatcherFillMaterial1;
    Material cowCatcherFillMaterial2;
    Material cowCatcherFillMaterial3;

    int cowCatcherFillGlowId1;
    int cowCatcherFillGlowId2;
    int cowCatcherFillGlowId3;

    void Awake(){
        // try{
        //     Debug.Assert(GlowOffsetTime >= GlowTime*2f);
        // }
        // catch(System.Exception e){
        //     Debug.LogError("COW CATCHER GLOW OFFSET TIME MUST BE AT LEAST DOUBLE THE GLOW TIME");
        // }


        CowCatcher1 = GameObject.Find("Cow_Catcher_1");
        CowCatcher2 = GameObject.Find("Cow_Catcher_2");
        CowCatcher3 = GameObject.Find("Cow_Catcher_3");


        if(!Upgrades_Manager.instance.upgradesUnlockedDict[Upgrade.Cow_Catcher]){
            GameObject.Find("Cow_Catcher_1_Fill").GetComponent<Renderer>().enabled = false;
            GameObject.Find("Cow_Catcher_2_Fill").GetComponent<Renderer>().enabled = false;
            GameObject.Find("Cow_Catcher_3_Fill").GetComponent<Renderer>().enabled = false;
            
            GameObject.Find("Cow_Catcher_1_Outline").GetComponent<Renderer>().enabled = false;
            GameObject.Find("Cow_Catcher_2_Outline").GetComponent<Renderer>().enabled = false;
            GameObject.Find("Cow_Catcher_3_Outline").GetComponent<Renderer>().enabled = false;
        }



        cowCatcherFillMaterial1 = GameObject.Find("Cow_Catcher_1_Fill").GetComponent<Renderer>().material;
        cowCatcherFillMaterial2 = GameObject.Find("Cow_Catcher_2_Fill").GetComponent<Renderer>().material;
        cowCatcherFillMaterial3 = GameObject.Find("Cow_Catcher_3_Fill").GetComponent<Renderer>().material;

        //Glow();
        StartCoroutine(StartGlow());
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator StartGlow(){
        Glow(1);
        yield return new WaitForSeconds(GlowOffsetTime);
        Glow(2);
        yield return new WaitForSeconds(GlowOffsetTime);
        Glow(3);
    }



    void Glow(int glowNum){

        Material glowMaterial = cowCatcherFillMaterial1;
        if(glowNum == 1){
            glowMaterial = cowCatcherFillMaterial1;
        }
        else if(glowNum == 2){
            glowMaterial = cowCatcherFillMaterial2;
        }
        else if(glowNum == 3){
            glowMaterial = cowCatcherFillMaterial3;
        }

        int _glow(Material glowMaterial, float startGlowLerpVal, float endGlowLerpVal, System.Action onComplete){
            int glowTweenId = LeanTween.value(gameObject, startGlowLerpVal, endGlowLerpVal, GlowTime).setEase(GlowEase).setOnUpdate(
                (value) =>
                {
                    glowMaterial.SetFloat("_Glow", Mathf.Lerp(minGlowVal, maxGlowVal, value));
                }
            ).setOnComplete(onComplete).id;
            return glowTweenId;
        }

        void _glowUp(){
            int gid = _glow(glowMaterial, 0, 1, _glowDown);
            if(glowNum == 1){
                cowCatcherFillGlowId1 = gid;
            }
            else if(glowNum == 2){
                cowCatcherFillGlowId2 = gid;
            }
            else if(glowNum == 3){
                cowCatcherFillGlowId3 = gid;
            }
        }

        void _glowDown(){
            int gid = _glow(glowMaterial, 1, 0, _glowUp);
            if(glowNum == 1){
                cowCatcherFillGlowId1 = gid;
            }
            else if(glowNum == 2){
                cowCatcherFillGlowId2 = gid;
            }
            else if(glowNum == 3){
                cowCatcherFillGlowId3 = gid;
            }
        }   

        _glowUp();
    }
    
    public void Fade(int fadeNum){

        GameObject FadeObject = GameObject.Find("Cow_Catcher_1");
        Material glowMaterial;
        if(fadeNum == 1){
            FadeObject = GameObject.Find("Cow_Catcher_1");
            glowMaterial = cowCatcherFillMaterial1;
            LeanTween.cancel(cowCatcherFillGlowId1);
        }
        else if(fadeNum == 2){
            FadeObject = GameObject.Find("Cow_Catcher_2");
            glowMaterial = cowCatcherFillMaterial2;
            LeanTween.cancel(cowCatcherFillGlowId2);
        }
        else if(fadeNum == 3){
            FadeObject = GameObject.Find("Cow_Catcher_3");
            glowMaterial = cowCatcherFillMaterial3;
            LeanTween.cancel(cowCatcherFillGlowId3);
        }
        else{
            Debug.LogError("Tried to fade nonexistent cow catcher... " + fadeNum);
            return;
        }

        float curGlowVal = glowMaterial.GetFloat("_Glow");
        Vector3 curScale = FadeObject.transform.localScale;
        Vector3 bigScale = curScale * 3f;

        LeanTween.alpha(FadeObject, 0f, FadeTime).setEase(FadeEase);
        LeanTween.scale(FadeObject, bigScale, FadeTime).setEase(SizeEase);
        LeanTween.value(FadeObject, curGlowVal, MaxHitGlow, FadeTime/2f).setEase(GlowEase).setOnUpdate(
                (value) =>
                {
                    glowMaterial.SetFloat("_Glow", Mathf.Lerp(curGlowVal, MaxHitGlow, value));
                }
            ).setOnComplete(
                () => LeanTween.value(FadeObject, MaxHitGlow, curGlowVal, FadeTime/2f).setEase(GlowEase).setOnUpdate(
                        (value) =>
                        {
                            glowMaterial.SetFloat("_Glow", Mathf.Lerp(MaxHitGlow, 0f, value));
                        }
                )
            ).setEase(GlowEase);
    }

}
