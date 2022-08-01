using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(fileName = "Level_End_Event_Scriptable_Object_Earth", menuName = "ScriptableObjects/Events/Level_End_Events/Earth", order = 1)]
public class Level_End_Event_Scriptable_Object_Earth : Level_End_Event_Scriptable_Object
{
    public override void executeLevelCompleteNextLevelNotReady(){
        // Position Camera On Rocket Area'
        Game_Manager.instance.gameObject.AddComponent(typeof(Next_Level_Not_Ready_Event_Earth));
        Next_Level_Not_Ready_Event_Earth.EventEndedInfo += alertEventEndedNotReady; 
    }

    public void alertEventEndedNotReady(){
        //Debug.Log("SO EVENT OVER!!!");
        base.onEventEnded(nextLevelWasReady:false);
        Next_Level_Not_Ready_Event_Earth.EventEndedInfo -= alertEventEndedNotReady; 
    }






    public override void executeLevelCompleteNextLevelReady(){
        //Debug.Log("LEVEL END EARTH: Earth Completed And Ready");
        base.onEventEnded(nextLevelWasReady:true);
    }






}
