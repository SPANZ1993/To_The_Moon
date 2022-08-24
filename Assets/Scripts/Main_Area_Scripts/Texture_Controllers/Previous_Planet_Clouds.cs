using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previous_Planet_Clouds : MonoBehaviour
{

    [SerializeField]
    float minTimeBetweenShifts;
    [SerializeField]
    float maxTimeBetweenShifts;

    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;

    private Vector2 curTiling;

    private float tmpX;
    private float tmpY;

    [SerializeField]
    float maxShiftX;
    [SerializeField]
    float maxShiftY;


    Material planetMaterial;


    // Start is called before the first frame update
    void Start()
    {
        planetMaterial = gameObject.GetComponent<Renderer>().material;   
        StartCoroutine(ShiftClouds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShiftClouds(){
        yield return new WaitForSeconds(Random.Range(minTimeBetweenShifts, maxTimeBetweenShifts));
        curTiling = planetMaterial.GetTextureScale("_OverlayTex");

        curTiling.x = Mathf.Clamp(curTiling.x + Random.Range(-maxShiftX, maxShiftX), minX, maxX);
        curTiling.y = Mathf.Clamp(curTiling.y + Random.Range(-maxShiftY, maxShiftY), minY, maxY);


        planetMaterial.SetTextureScale("_OverlayTex", curTiling);
        StartCoroutine(ShiftClouds());
    }
}
