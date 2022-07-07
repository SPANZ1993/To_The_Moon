using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifting_Sand : MonoBehaviour
{

    [SerializeField]
    float minTimeBetweenShifts;
    [SerializeField]
    float maxTimeBetweenShifts;

    [SerializeField]
    float maxTotalShiftX;
    [SerializeField]
    float maxTotalShiftY;

    [SerializeField]
    float maxShiftX;
    [SerializeField]
    float maxShiftY;

    Vector2 curOffset;
    Vector2 curDeltaOffset;

    Material sandMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(maxShiftX <= maxTotalShiftX/2f && maxShiftY <= maxTotalShiftY/2f);
        sandMaterial = gameObject.GetComponent<Renderer>().material;   
        curDeltaOffset = new Vector2();
        StartCoroutine(Shift());
    }

    IEnumerator Shift(){
        yield return new WaitForSeconds(Random.Range(minTimeBetweenShifts, maxTimeBetweenShifts));
        curOffset = sandMaterial.GetTextureOffset("_OverlayTex");
        curDeltaOffset.x = Random.Range(0, maxShiftX);
        curDeltaOffset.y = Random.Range(0, maxShiftY);

        if(curOffset.x + curDeltaOffset.x > maxTotalShiftX){
            curDeltaOffset.x = -1f * curDeltaOffset.x;
        }
        if(curOffset.y + curDeltaOffset.y > maxTotalShiftY){
            curDeltaOffset.y = -1f * curDeltaOffset.y;
        }

        sandMaterial.SetTextureOffset("_OverlayTex", curOffset + curDeltaOffset);
        StartCoroutine(Shift());
    }
}
