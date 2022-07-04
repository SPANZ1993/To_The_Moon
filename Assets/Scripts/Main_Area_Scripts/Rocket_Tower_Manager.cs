using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Tower_Manager : MonoBehaviour
{


    public bool launched {get; private set;}
    public bool launchComplete;
    public bool retractComplete;
    private Animator towerAnim;

    void OnEnable()
    {
        Scene_Manager.InitiateLaunchInfo += onLaunchInitiated;
    }

    void OnDisable()
    {
        Scene_Manager.InitiateLaunchInfo -= onLaunchInitiated;
    }

    // Start is called before the first frame update
    void Start()
    {
        towerAnim = GetComponent<Animator>();

        launched = false;
        launchComplete = false;
        retractComplete = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator _onLaunchInitiated(bool first){
        IEnumerator _stopRetracting(float retracttime){
            yield return new WaitForSeconds(retracttime*0.51f); // Need to do this because only half o
            launched = false;
            retractComplete = true;
            towerAnim.SetBool("Retracting", false);
            towerAnim.SetBool("Retracted", true);
        }


        if (first){
            Sound alarmSound = Audio_Manager.instance.GetSound("Rocket_Launch_Alarm");
            launched = true;
            towerAnim.SetBool("Retracting", true);
            towerAnim.SetBool("Retracted", false);
            if(!Audio_Manager.instance.IsPlaying(alarmSound)){
                Audio_Manager.instance.Play(alarmSound);
            }
            AnimationClip[] clips = towerAnim.runtimeAnimatorController.animationClips;
            float retractClipLength = 0f;
            foreach(AnimationClip clip in clips)
            {   
                if(clip.name == "Rocket_Tower_Retract"){
                    retractClipLength = clip.length;
                }
            }
            //Debug.Log(retractClipLength);
            StartCoroutine(_stopRetracting(retractClipLength));
            yield return new WaitForSeconds(Mathf.Max(alarmSound.clip.length, retractClipLength));
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
