using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;


[CreateAssetMenu(fileName = "Robot_Title_Data_Message_Event_Trigger_Scriptable_Object", menuName = "ScriptableObjects/Events/Robot_Title_Data_Message_Event_Trigger_Scriptable_Object", order = 2)]
public class Robot_Title_Data_Message_Event_Trigger_Scriptable_Object : Robot_Message_Event_Trigger_Scriptable_Object
{

    protected override Speech_Object getMessage(){
        try{
            //Debug.Log("TRYING TO GET ROBOT MESSAGE");
            if(Game_Manager.instance.titleData != null && Game_Manager.instance.titleData.Keys.Contains("Robot Message") && Speech_Object_Generator.instance.ValidateSpeechObjectJson(Game_Manager.instance.titleData["Robot Message"])){
                return Speech_Object_Generator.instance.BuildSpeechObjectFromJSON(Game_Manager.instance.titleData["Robot Message"]);
            }
        }
        catch(Exception e){
            Debug.Log("ERROR IN GET MESSAGE " + e);
            return null;
        }
        return null;
    }
}
