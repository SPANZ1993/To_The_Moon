using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_Level_Not_Ready_Event_Moon : MonoBehaviour, IEvent
{


    public delegate void EventEnded();
    public static event EventEnded EventEndedInfo;


    
    void OnEnable(){
        startNotReadySpeech();
    }

    void startNotReadySpeech(){

        IEnumerator _waitThenSpeak(){
            float t = 0f;
            while(t<0.5f){
                Touch_Detection.instance.disableReticle();
                Touch_Detection.instance.disableSwipes();
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            yield return new WaitForSeconds(2.5f);
            startNextLevelNotReadySpeech(1);
        }
        // Touch Detection Automatically Enables the Reticle during the OnLevelWasLoaded sequence...
        // So we're just going to disable it into oblivion for a couple seconds
        IEnumerator _makeSureReticleDisabled(){
            float timer = 0;
            while(timer < 3){
                Touch_Detection.instance.disableReticle(disableswipes:true);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        // Similarly... camera sets its own location during start() so really just hammer it
        IEnumerator _makeSureCameraMoved(){
            float timer = 0;
            Move_Camera cam = GameObject.Find("Main Camera").GetComponent<Move_Camera>();
            while(timer < 3){
                cam.setToLocation(2);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        StartCoroutine(_makeSureCameraMoved());
        StartCoroutine(_makeSureReticleDisabled());
        StartCoroutine(_waitThenSpeak());
    }
    
    void _startNotReadySpeech(){
        startNextLevelNotReadySpeech(1);
    }

    // void startSecondNotReadySpeechAfterDelay(){

    //     IEnumerator _waitThenSpeak(){
    //         float t = 0f;
    //         while(t<0.5f){
    //             Touch_Detection.instance.disableReticle();
    //             Touch_Detection.instance.disableSwipes();
    //             yield return new WaitForEndOfFrame();
    //             t += Time.deltaTime;
    //         }
    //         yield return new WaitForSeconds(2f);
    //         startNextLevelNotReadySpeech(2);
    //     }
    //     StartCoroutine(_waitThenSpeak());
    // }

    void resumeAfterNotReadyEvent(){
        Touch_Detection.instance.enableReticle(immediately:true, enableswipes:true);
        if(EventEndedInfo != null){
            EventEndedInfo();
        }

        Touch_Detection.instance.enableReticle(immediately:true);
        Touch_Detection.instance.enableSwipes(immediately:true);
        Destroy(this);
    }


    private void startNextLevelNotReadySpeech(int nextLevelNotReadySpeechNum){
        string keystring = "";
        if(nextLevelNotReadySpeechNum == 1){
            keystring = "Events_Script.NextLevelNotReady.Moon.1.1";
            UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:true, keyString:keystring, formatFunc:null), callBack:resumeAfterNotReadyEvent);
        }
        // else if (nextLevelNotReadySpeechNum == 2){
        //     keystring = "Events_Script.NextLevelNotReady.Earth.2.1";
        //     UI_Controller.instance.Display_Speech(Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:true, keyString:keystring, formatFunc:null), callBack:resumeAfterNotReadyEvent);
        // }

    }

}
