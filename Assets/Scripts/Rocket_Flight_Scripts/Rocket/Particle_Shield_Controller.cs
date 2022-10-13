using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Collections.Concurrent;

public class Particle_Shield_Controller : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private float displayScreenHeightMultiplier = 1f;

    private SpriteRenderer rend;

    private ConcurrentDictionary<GameObject, float> DarkMatterObjectsDict;

    [SerializeField]
    private LeanTweenType GlowEaseType = LeanTweenType.easeInOutSine;
    [SerializeField]
    private float GlowEaseTime = 1;
    private int glowTweenId;

    private Material ParticleShieldMaterial;


    private Camera cam;
    private float ScreenHeight;

    void Awake(){
        ScreenHeight = Camera.main.orthographicSize * 2.0f;
        DarkMatterObjectsDict = new ConcurrentDictionary<GameObject, float>();

        Ship_Skin_Scriptable_Object curShipSkin = Ship_Skin_Manager.instance.getCurSkin();

        rend = GetComponent<SpriteRenderer>();
        //Set Sprite
        if(curShipSkin != null && curShipSkin.ParticleShieldSprite != null){
            rend.sprite =curShipSkin.ParticleShieldSprite;
        }
        else{
            rend.sprite = defaultSprite;
        }

        ParticleShieldMaterial = GetComponent<SpriteRenderer>().material;

        //Set Alpha to 0
        UpdateSprite();
        Glow();
        ParticleShieldMaterial.SetFloat("_OutlineAlpha", 0f);
    }


    void OnEnable(){
        Space_Junk_Dark_Matter.AlertDarkMatterEnabledInfo += OnDarkMatterEnabled;
        Space_Junk_Dark_Matter.AlertDarkMatterDisabledInfo += OnDarkMatterDisabled;
    }

    void OnDisable(){
        Space_Junk_Dark_Matter.AlertDarkMatterEnabledInfo -= OnDarkMatterEnabled;
        Space_Junk_Dark_Matter.AlertDarkMatterDisabledInfo -= OnDarkMatterDisabled;
    }


    void Start(){

    }

    void Update(){
        UpdateDistances();
        UpdateSprite();
    }

    void OnDarkMatterEnabled(GameObject DarkMatter){
        DarkMatterObjectsDict.GetOrAdd(DarkMatter, Vector3.Distance(transform.position, DarkMatter.transform.position));
    }


    void OnDarkMatterDisabled(GameObject DarkMatter){
        // Wait a bit to ensure the sprite fades slowly and not instantly
        StartCoroutine(WaitThenRemove(DarkMatter, 30f));
    }

    void RemoveDarkMatterFromDict(GameObject DarkMatter){
        float tmp = 0;
        DarkMatterObjectsDict.TryRemove(DarkMatter, out tmp);
    }

    IEnumerator WaitThenRemove(GameObject DarkMatter, float waitTime){
        yield return new WaitForSeconds(waitTime);
        RemoveDarkMatterFromDict(DarkMatter);
    }



    void UpdateDistances(){
        foreach(GameObject DarkMatter in DarkMatterObjectsDict.Keys){
            DarkMatterObjectsDict[DarkMatter] = Vector3.Distance(transform.position, DarkMatter.transform.position); 
        }
    }

    void UpdateSprite(){
        if(DarkMatterObjectsDict.Count == 0){
            SetAlpha(0f);
        }
        else{
            float minDist = DarkMatterObjectsDict.Values.Min();
            if(minDist <= ScreenHeight*displayScreenHeightMultiplier){
                Debug.Log("SETTING ALPHA TO: " + Mathf.Lerp(0f, 1f, ((ScreenHeight*displayScreenHeightMultiplier)-minDist)/(ScreenHeight*displayScreenHeightMultiplier))); // GET THIS CALCULATION (RATIO) CORRECT
                SetAlpha(Mathf.Lerp(0f, 1f, ((ScreenHeight*displayScreenHeightMultiplier)-minDist)/(ScreenHeight*displayScreenHeightMultiplier)));
            }
            else{
                SetAlpha(0f);
            }
        }
    }


    void SetAlpha(float alpha){
        Color tmp = rend.color;
        tmp.a = alpha;
        rend.color = tmp;
        if(alpha >= 0.15f){
            ParticleShieldMaterial.SetFloat("_OutlineAlpha", 1f);
        }
        else{
            ParticleShieldMaterial.SetFloat("_OutlineAlpha", alpha);
        }
    }


    void Glow(){

        void _glow(float startGlowLerpVal, float endGlowLerpVal, System.Action onComplete){
            glowTweenId = LeanTween.value(gameObject, startGlowLerpVal, endGlowLerpVal, GlowEaseTime).setEase(GlowEaseType).setOnUpdate(
                (value) =>
                {
                    ParticleShieldMaterial.SetFloat("_GhostColorBoost", Mathf.Lerp(0, 5, value));
                }
            ).setOnComplete(onComplete).id;
        }

        void _glowUp(){
            _glow(0, 1, _glowDown);
        }

        void _glowDown(){
            _glow(1, 0, _glowUp);
        }

        _glowUp();
    }

}
