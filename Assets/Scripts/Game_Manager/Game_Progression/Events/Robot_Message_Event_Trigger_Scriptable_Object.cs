using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;



public abstract class Robot_Message_Event_Trigger_Scriptable_Object : Event_Trigger_Scriptable_Object
{


    protected abstract Speech_Object getMessage();


    public override bool shouldTrigger(){
        bool shouldTrigger = base.shouldTrigger();
        bool bst = shouldTrigger;
        if(shouldTrigger){ 
            try{
                if(getMessage() != null){
                    shouldTrigger = true;
                }
                else{
                    //Debug.Log("Get Message is NUll");
                    shouldTrigger = false;
                }
            }
            catch(Exception e){
                Debug.LogError(e.ToString());
                shouldTrigger = false;
            }
        }
        return shouldTrigger;
    }



    public override void _trigger(){    
        // Fill this in
        try{
            Robot_Manager.instance.messageQueue.Enqueue(getMessage());
        }
        catch(Exception e){
            Debug.LogError("TRIED TO ENQUEUE THE ROBOT TITLE SPEECH.. BUT RAN INTO AN ISSUE: " + e.ToString());
        }
        base._alertManagerOnEventEnd();
    }


}
