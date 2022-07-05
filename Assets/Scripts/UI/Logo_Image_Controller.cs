using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class Logo_Image_Controller : MonoBehaviour
{

    [SerializeField]
    float timeBetweenSheenWipe;
    
    [SerializeField]
    float sheenWipeLength;
    int sheenWipeId;

    [SerializeField]
    LeanTweenType sheenEaseType;

    [SerializeField]
    float timeUntilStartBurn;
    [SerializeField]
    float burnTime;
    [SerializeField]
    LeanTweenType burnEaseType;
    
    [SerializeField]
    float timeFromEndBurnToTransition;
    [SerializeField]
    string nextSceneName;

    Material logoImageMaterial;



    float sheenLoc;


    void Awake(){
        logoImageMaterial = gameObject.GetComponent<Image>().material;
        //sheenEaseType = LeanTweenType.easeOutCirc;
        sheenWipeId = -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        sheenLoc = 1;
        logoImageMaterial.SetFloat("_ShineLocation", sheenLoc);
        Invoke("startSheenWipe", Random.Range(0f, timeBetweenSheenWipe));
        Invoke("startBurn", timeUntilStartBurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int startSheenWipe(){
        return LeanTween.value(gameObject, 1f, 0.0f, sheenWipeLength).setEase(sheenEaseType).setOnUpdate(
                    (value) =>
                    {
                        //transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
                        logoImageMaterial.SetFloat("_ShineLocation", value);
                    }
                ).setOnComplete(waitForNextSheen).id;
    }

    void waitForNextSheen(){
        Invoke("startSheenWipe", timeBetweenSheenWipe);
    }


    int startBurn(){
        return LeanTween.value(gameObject, -0.1f, 1f, burnTime).setEase(burnEaseType).setOnUpdate(
                    (value) =>
                    {
                        //transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
                        logoImageMaterial.SetFloat("_FadeAmount", value);
                    }
                ).setOnComplete(transitionScene).id;
    }

    void transitionScene(){
        IEnumerator _waitThenTransition(){
            yield return new WaitForSeconds(timeFromEndBurnToTransition);
            SceneManager.LoadScene(sceneName: nextSceneName);
        }
        StartCoroutine(_waitThenTransition());
    }
}
