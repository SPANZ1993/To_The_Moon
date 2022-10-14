using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team_Rocket_Star : MonoBehaviour
{

    [SerializeField]
    LeanTweenType GrowEase = LeanTweenType.easeInOutSine;
    [SerializeField]
    LeanTweenType ShrinkEase = LeanTweenType.easeInOutSine;

    [SerializeField]
    float GrowTime = 0.1f;
    [SerializeField]
    float ShrinkTime = 0.25f;
    [SerializeField]
    float LifeTime = 0.05f;

    void OnEnable(){
        transform.localScale = new Vector3();
    }

    // Start is called before the first frame update
    void Start()
    {
        Twinkle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float RandomizeTime(float val, float low = 0.95f, float high = 1.05f){
        return Random.Range(val*low, val*high);
    }

    void Twinkle(){

        float GrowTimeRandomized = RandomizeTime(GrowTime);
        float LifeTimeRandomized = RandomizeTime(LifeTime);
        float ShrinkTimeRandomized = RandomizeTime(ShrinkTime);

        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), GrowTimeRandomized).setEase(GrowEase).setOnComplete(
            () => LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), LifeTimeRandomized).setEase(ShrinkEase).setOnComplete(
                () => LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), ShrinkTimeRandomized).setEase(ShrinkEase).setOnComplete(
                    () => Destroy(gameObject)
                )
            )
        );

    }
}
