using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Tower_Manager : MonoBehaviour
{


    public bool launched {get; private set;}
    public bool launchComplete;
    private Animator towerAnim;

    void OnEnable()
    {
        Launch_Button_Controller.InitiateLaunchInfo += onLaunchInitiated;
    }

    void OnDisable()
    {
        Launch_Button_Controller.InitiateLaunchInfo -= onLaunchInitiated;
    }

    // Start is called before the first frame update
    void Start()
    {
        towerAnim = GetComponent<Animator>();

        launched = false;
        launchComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        towerAnim.SetBool("Retracting", launched);
        towerAnim.SetBool("Retracted", launchComplete);
    }

    IEnumerator _onLaunchInitiated(bool first){

        if (first){
            launched = true;
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(_onLaunchInitiated(false));
        }
        else{
            launchComplete = true;
        }
    }

    void onLaunchInitiated(){
        StartCoroutine(_onLaunchInitiated(true));
    }
}
