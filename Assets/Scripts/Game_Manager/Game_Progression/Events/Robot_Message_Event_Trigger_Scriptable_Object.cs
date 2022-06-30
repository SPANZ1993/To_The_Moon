using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;


[CreateAssetMenu(fileName = "Robot_Title_Data_Message_Event_Scriptable_Object", menuName = "ScriptableObjects/Events/Robot_Message_Event_Scriptable_Object", order = 2)]
public class Robot_Message_Event_Trigger_Scriptable_Object : Event_Trigger_Scriptable_Object
{
    public override bool shouldTrigger(){
        bool shouldTrigger = base.shouldTrigger();
        if(shouldTrigger){
            if(Game_Manager.instance.titleData != null && Game_Manager.instance.titleData.Keys.Contains("Robot Message")){ 
                shouldTrigger = true;
            }
            else{
                shouldTrigger = false;
            }
        }
        if(shouldTrigger){
            Debug.Log("Should trigger robot message");
        }
        return shouldTrigger;
    }



    public override void _trigger(){    
        // Fill this in
    }


}
