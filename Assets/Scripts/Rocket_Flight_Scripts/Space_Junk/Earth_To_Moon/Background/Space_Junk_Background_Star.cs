using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class Space_Junk_Background_Star : MonoBehaviour
{
    
    Background_Controller backgroundController;
    Light2D starLight;

    void OnEnable(){
        try{
            if(backgroundController == null){
                backgroundController = GameObject.Find("Background_Manager").GetComponent<Background_Controller>();
                //Debug.Log("BACKGROUND!: " + backgroundController);
            }
            if(starLight == null){
                starLight = GetComponent<Light2D>();
            }
            
            float H = 1;
            float S = 1;
            float V = 1;
            Color.RGBToHSV(backgroundController.getColor(gameObject.transform.position, first:false)[0], out H, out S, out V);

            //Debug.Log("H: " + H + " S: " + S + " V!: " + V);
            starLight.intensity = Mathf.Lerp(0f, 25f, 1f-V);
        }
        catch(System.Exception e){
            //Debug.Log(e);
        }
    }

}
