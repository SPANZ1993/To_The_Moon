using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halogen_Light : MonoBehaviour
{

    private Material UIMaterial;
    [SerializeField]
    private float sheenWipeLength;
    [SerializeField]
    private float sheenWaitLength;
    [SerializeField]
    private float startSheenLoc;
    [SerializeField]
    private float endSheenLoc;
    [SerializeField]
    LeanTweenType sheenEaseType;

    private int sheenWipeId;

   

    float highSheenVal;
    float lowSheenVal;

    bool flipped = false;

    void Awake(){
        UIMaterial = gameObject.GetComponent<SpriteRenderer>().material;
        if(gameObject.GetComponent<SpriteRenderer>().flipX){
            flipped = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        UIMaterial.SetFloat("_ShineLocation", startSheenLoc);
        startSheenWipe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startSheenWipe(){
        StartCoroutine(doSheen());
    }

    IEnumerator doSheen(){
        yield return new WaitForSeconds(sheenWaitLength);
        sheenWipeId = LeanTween.value(gameObject, startSheenLoc, endSheenLoc, sheenWipeLength).setEase(sheenEaseType).setOnUpdate(
                    (value) =>
                    {
                        // GameObject tgo = new GameObject();
                        // bool egg = gameObject == tgo;
                        // bool egg2 = startSheenLoc == 0;
                        // bool egg3 = endSheenLoc == 0;
                        // bool egg4 = sheenWipeLength == 0;
                        // Destroy(tgo);

                        UIMaterial.SetFloat("_ShineLocation", !flipped ? value : endSheenLoc-(endSheenLoc-value));
                    }
                ).setOnComplete(startSheenWipe).id;
    }
}
