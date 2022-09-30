using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dark_Matter_Shader_Controller : MonoBehaviour
{
    [SerializeField]
    float minOutlineGlowVal;
    [SerializeField]
    float maxOutlineGlowVal;

    [SerializeField]
    float minGlowVal;
    [SerializeField]
    float maxGlowVal;

    // [SerializeField]
    // float maxShiftX;
    // [SerializeField]
    // float maxShiftY;

    // Vector2 curOffset;
    // Vector2 curDeltaOffset;

    [SerializeField]
    float glowTweenTime;

    [SerializeField]
    LeanTweenType glowTweenType;

    int glowTweenId;

    [SerializeField]
    Material darkMatterMaterial;

    void OnEnable(){
        darkMatterMaterial = gameObject.GetComponent<Renderer>().material;
        darkMatterMaterial.SetFloat("_OutlineGlow", minOutlineGlowVal);
        darkMatterMaterial.SetFloat("_InnerOutlineGlow", minGlowVal);

        Glow();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(darkMatterMaterial.GetFloat("_InnerOutLineGlow"));
    }

    void Glow(){

        void _glow(float startGlowLerpVal, float endGlowLerpVal, System.Action onComplete){
            glowTweenId = LeanTween.value(gameObject, startGlowLerpVal, endGlowLerpVal, glowTweenTime).setEase(glowTweenType).setOnUpdate(
                (value) =>
                {

                    

                    darkMatterMaterial.SetFloat("_OutlineGlow", Mathf.Lerp(minOutlineGlowVal, maxOutlineGlowVal, value));
                    darkMatterMaterial.SetFloat("_InnerOutlineGlow", Mathf.Lerp(minGlowVal, maxGlowVal, value));
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
