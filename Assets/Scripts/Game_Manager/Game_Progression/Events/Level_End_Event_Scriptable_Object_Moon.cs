using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(fileName = "Level_End_Event_Scriptable_Object_Moon", menuName = "ScriptableObjects/Events/Level_End_Events/Moon", order = 2)]
public class Level_End_Event_Scriptable_Object_Moon : Level_End_Event_Scriptable_Object
{
    public override void executeLevelCompleteNextLevelNotReady(){
        Debug.Log("LEVEL END MOON: Moon Completed And Not Ready For Next Level");
        base.onEventEnded(nextLevelWasReady:false);
    }

    public override void executeLevelCompleteNextLevelReady(){
        Debug.Log("LEVEL END MOON: Moon Completed And Ready For Next Level");
        base.onEventEnded(nextLevelWasReady:true);
    }

}