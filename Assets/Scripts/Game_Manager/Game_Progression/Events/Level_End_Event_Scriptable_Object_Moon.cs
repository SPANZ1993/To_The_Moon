using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(fileName = "Level_End_Event_Scriptable_Object_Moon", menuName = "ScriptableObjects/Events/Level_End_Events/Moon", order = 2)]
public class Level_End_Event_Scriptable_Object_Moon : Level_End_Event_Scriptable_Object
{
    public override void executeLevelCompleteNextLevelNotReady(){
        //Debug.Log("LEVEL END MOON: Moon Completed And Not Ready For Next Level");
        //base.onEventEnded(nextLevelWasReady:false);
        Game_Manager.instance.gameObject.AddComponent(typeof(Next_Level_Not_Ready_Event_Moon));
        Next_Level_Not_Ready_Event_Moon.EventEndedInfo += alertEventEndedNotReady; 
    }

    public void alertEventEndedNotReady(){
        //Debug.Log("SO EVENT OVER!!!");
        base.onEventEnded(nextLevelWasReady:false);
        Next_Level_Not_Ready_Event_Moon.EventEndedInfo -= alertEventEndedNotReady; 
    }




    public override void executeLevelCompleteNextLevelReady(){
        //Debug.Log("LEVEL END MOON: Moon Completed And Ready For Next Level");
        //base.onEventEnded(nextLevelWasReady:true);
        Game_Manager.instance.gameObject.AddComponent(typeof(Next_Level_Ready_Event_Moon));
        Next_Level_Ready_Event_Moon.EventEndedInfo += alertEventEndedReady; 
    }

    public void alertEventEndedReady(){
        //Debug.Log("SO EVENT OVER!!!");
        base.onEventEnded(nextLevelWasReady:true);
        Next_Level_Ready_Event_Moon.EventEndedInfo -= alertEventEndedReady; 
    }



}