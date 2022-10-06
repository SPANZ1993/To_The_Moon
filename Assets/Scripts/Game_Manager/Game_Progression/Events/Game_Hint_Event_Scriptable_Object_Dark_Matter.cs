using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

[CreateAssetMenu(fileName = "Game_Hint_Event_Dark_Matter", menuName = "ScriptableObjects/Events/Game_Hint_Events/Dark_Matter", order = 1)]
public class Game_Hint_Event_Scriptable_Object_Dark_Matter : Game_Hint_Event_Scriptable_Object
{


    [SerializeField]
    private string hintKeyString;
    [SerializeField]
    private float hintWaitTime = 1.5f;

    public override bool shouldTrigger(){
        bool shouldTrigger = base.shouldTrigger();
        Dictionary<string, int> rocketGameBumpCounts = Game_Manager.instance.rocketGameBumpCounts;
        string rocketGameBumpCountsDarkMatterKey = "";
        if(rocketGameBumpCounts == null || !rocketGameBumpCounts.Keys.Select(k => k.StartsWith("Dark_Matter")).Any()){
            shouldTrigger = false;
        }
        return shouldTrigger;
    }

    public override void _trigger(){
        base._trigger();
        Game_Hint_Event gameHintEvent = (Game_Hint_Event)Game_Manager.instance.gameObject.AddComponent(typeof(Game_Hint_Event));
        gameHintEvent.formatFunc = speechFormatFunc;
        gameHintEvent.hintSpeechObjectKeystring = hintKeyString;
        gameHintEvent.waitTime = hintWaitTime;
        Game_Hint_Event.EventEndedInfo += alertEventEnded; 
    }


    public void alertEventEnded(){
        //Debug.Log("SO EVENT OVER!!!");
        base._alertManagerOnEventEnd();
        Game_Hint_Event.EventEndedInfo -= alertEventEnded; 
    }


    public string speechFormatFunc(string s){
        s = Speech_Object_Generator.instance.defaultStringFormatFunc(s);
        s = s.Replace("{particleshield}", Localization_Manager.instance.GetLocalizedString("UI_Experiments", "UI.Experiment.Experiment3.Name"));
        return s;
    }

    
}
