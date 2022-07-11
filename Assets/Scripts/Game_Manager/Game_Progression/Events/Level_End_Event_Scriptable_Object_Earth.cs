using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(fileName = "Level_End_Event_Scriptable_Object_Earth", menuName = "ScriptableObjects/Events/Level_End_Events/Earth", order = 1)]
public class Level_End_Event_Scriptable_Object_Earth : Level_End_Event_Scriptable_Object
{
    public override void executeLevelCompleteNextLevelNotReady(){
        Debug.Log("LEVEL END EARTH: Earth Completed And Not Ready");
        base.onEventEnded(nextLevelWasReady:false);
    }

    public override void executeLevelCompleteNextLevelReady(){
        Debug.Log("LEVEL END EARTH: Earth Completed And Ready");
        base.onEventEnded(nextLevelWasReady:true);
    }

}
