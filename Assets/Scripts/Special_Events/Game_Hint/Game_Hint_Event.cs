using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Game_Hint_Event : MonoBehaviour, IEvent
{

    public string hintSpeechObjectKeystring = "";
    public float waitTime = 1.5f;

    public Func<string, string> formatFunc;


    public delegate void EventEnded();
    public static event EventEnded EventEndedInfo;


    void OnEnable(){
        StartCoroutine(waitThenHint());
    }

    IEnumerator waitThenHint(){
        while(hintSpeechObjectKeystring == ""){
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForSeconds(waitTime);
    
        Speech_Object hint_speech_object = Speech_Object_Generator.instance.buildSpeechObjectWithStartKey(isBlocker:true, 
                                                                                                        keyString:hintSpeechObjectKeystring,
                                                                                                        formatFunc:formatFunc);
        UI_Controller.instance.Display_Speech(speech_object:hint_speech_object, 
                                            callBack:alertEventEnded);
    }


    private void alertEventEnded(){
        if(EventEndedInfo != null){
            EventEndedInfo();
        }
        Destroy(this);
    }

}
