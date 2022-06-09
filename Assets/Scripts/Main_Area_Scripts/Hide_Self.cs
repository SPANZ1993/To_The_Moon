using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Self : MonoBehaviour
{
    SpriteRenderer sr;


    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void OnLevelWasLoaded(){
        sr.enabled = false;
    }
}
