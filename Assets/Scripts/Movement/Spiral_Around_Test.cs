using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral_Around_Test : MonoBehaviour
{

    [SerializeField]
    float targetRadius=.1f;
    // [SerializeField]
    // float angle=10f;
    [SerializeField]
    float angleSpeed=2f;
    [SerializeField]
    float radialSpeed=0.5f;
    [SerializeField]
    float radialAcceleration=1.1f;

    void CB(){
        Debug.Log("DONE!");
    }


    void OnEnable(){
        Spiral_Around.startSpiralAround(gameObject, GameObject.Find("Earth"), targetRadius, angleSpeed, radialSpeed, radialAcceleration, callBack:CB);
    }
}
