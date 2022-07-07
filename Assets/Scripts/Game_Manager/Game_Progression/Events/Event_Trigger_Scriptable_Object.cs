using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;


public abstract class Event_Trigger_Scriptable_Object : ScriptableObject, System.IEquatable<Event_Trigger_Scriptable_Object>
{

    public int EventId { get { return eventId; } private set { eventId = value; } }
    public int EventPriority { get {return eventPriority; } private set {eventPriority = value; }}
    
    public int NumTimesTriggerable { get { return numTimesTriggerable; } private set { numTimesTriggerable = value; } }
    public int NumTimesTriggerablePerLevelOpen { get { return numTimesTriggerablePerLevelOpen; } private set { numTimesTriggerablePerLevelOpen = value; } }
    public int NumTimesTriggerablePerSceneOpen { get { return numTimesTriggerablePerSceneOpen; } private set { numTimesTriggerablePerSceneOpen = value; } }
    public int NumTimesTriggerablePerGameOpen { get { return numTimesTriggerablePerGameOpen; } private set { numTimesTriggerablePerGameOpen = value;} }
    
    
    public bool TriggerableUponLevelWasLoaded { get { return triggerableUponLevelWasLoaded; } private set { triggerableUponLevelWasLoaded = value; } }
    
    public int[] TriggerableLevelIds { get { return triggerableLevelIds; } private set { triggerableLevelIds = value; } }
    public string[] TriggerableSceneNames { get { return triggerableSceneNames; } private set { triggerableSceneNames = value; } }


    // NOTE: -1 for Any of the NumTimesTriggerable+ variables means we can trigger as many times as we want in that context
    [SerializeField]
    protected int eventId;
    // NOTE: Higher priority events will trigger first, if same priority, no guarantee of ordering
    [SerializeField]
    private int eventPriority;
    [SerializeField]
    private int numTimesTriggerable;
    [SerializeField]
    private int numTimesTriggerablePerLevelOpen;
    [SerializeField]
    private int numTimesTriggerablePerSceneOpen;
    [SerializeField]
    private int numTimesTriggerablePerGameOpen;
    [SerializeField]
    private bool triggerableUponLevelWasLoaded; // Can this trigger happen anytime, or only during an OnLevelWasLoaded call?
    [SerializeField]
    private int[] triggerableLevelIds; // What levels can this event happen in... If this has 0 elements we are saying it can happen in any level;
    [SerializeField]
    private string[] triggerableSceneNames; // If none, can be triggered in any scene, but probably don't want that.. dont want things triggering on landing page



    public virtual bool shouldTrigger(){
        bool triggerable = false;
        // Pretty much only checks if the number of times it has been triggered is acceptable
        // Will want to extend this to add further checks
        if( (NumTimesTriggerable == -1 ||
            !Progression_Manager.instance.EventIdToTimesTriggered.Keys.Contains(EventId) || 
            Progression_Manager.instance.EventIdToTimesTriggered[EventId] < NumTimesTriggerable)
            &&
            (NumTimesTriggerablePerLevelOpen == -1 ||
            !Progression_Manager.instance.EventIdToTimesTriggeredThisLevelOpen.Keys.Contains(EventId) || 
            Progression_Manager.instance.EventIdToTimesTriggeredThisLevelOpen[EventId] < NumTimesTriggerablePerLevelOpen)
            &&
            (NumTimesTriggerablePerSceneOpen == -1 ||
            !Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen.Keys.Contains(EventId) || 
            Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen[EventId] < NumTimesTriggerablePerSceneOpen)
            &&
            (NumTimesTriggerablePerGameOpen == -1 ||
            !Progression_Manager.instance.EventIdToTimesTriggeredThisGameOpen.Keys.Contains(EventId) || 
            Progression_Manager.instance.EventIdToTimesTriggeredThisGameOpen[EventId] < NumTimesTriggerablePerGameOpen)
            )
            {
                triggerable = true;
        }
        else{
            if(EventId == 3){
                Debug.Log("FALSE BECAUSE DIDNT PASS CHECKS");
                if((NumTimesTriggerablePerSceneOpen == -1 ||
                    !Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen.Keys.Contains(EventId) || 
                    Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen[EventId] < NumTimesTriggerablePerSceneOpen)){
                        Debug.Log("FALSE BECAUSE DIDNT PASS SCENE OPEN");
                    }
                if((NumTimesTriggerable == -1 ||
            !Progression_Manager.instance.EventIdToTimesTriggered.Keys.Contains(EventId) || 
            Progression_Manager.instance.EventIdToTimesTriggered[EventId] < NumTimesTriggerable)){
                Debug.Log("FALSE BECAUSE DIDNT PASS TOTAL CHECK");
            }
            }
        }

        
        if(!(triggerableLevelIds.Length == 0) && !triggerableLevelIds.Contains(Progression_Manager.instance.CurrentLevelId)){
            if(EventId == 3){
                Debug.Log("FALSE BECAUSE BAD LEVEL ID");
            }
            triggerable = false;
        }

      
        string tmpScene = SceneManager.GetActiveScene().name;
        if(!triggerableSceneNames.Contains(tmpScene)){
            if(EventId == 3){
                Debug.Log("FALSE BECAUSE BAD SCENE NAME");
                foreach(string scene in triggerableSceneNames){
                    Debug.Log("SCENE: " + scene + " =?= " + tmpScene + " -- " + (scene == tmpScene) + " _____ " + (!triggerableSceneNames.Contains(tmpScene)));
                }
            }
            triggerable = false;
        }
      
      
        // if(triggerable){
        //     Debug.Log("MADE IT OUT OF BASE SHOULD TRIGGER");
        // }
        // else{
        //     Debug.Log("BASE THINKS NO SHOULD TRIGGER");
        // }

        return triggerable;
    }


    public void _alertManagerOnEventStart(){
        Progression_Manager.instance.CurrentEventIdInProgress = EventId;
    }


    // Has to be called from child class once it's done running
    public void _alertManagerOnEventEnd(){
        Progression_Manager.instance.CurrentEventIdInProgress = null;

        if(Progression_Manager.instance.EventIdToTimesTriggered.Keys.Contains(EventId)){
            Progression_Manager.instance.EventIdToTimesTriggered[EventId]++;
        }
        else{
            Progression_Manager.instance.EventIdToTimesTriggered[EventId] = 1;
        }


        if(Progression_Manager.instance.EventIdToTimesTriggeredThisLevelOpen.Keys.Contains(EventId)){
            Progression_Manager.instance.EventIdToTimesTriggeredThisLevelOpen[EventId]++;
        }
        else{
            Progression_Manager.instance.EventIdToTimesTriggeredThisLevelOpen[EventId] = 1;
        }
    
    
        if(Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen.Keys.Contains(EventId)){
            Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen[EventId]++;
        }
        else{
            Progression_Manager.instance.EventIdToTimesTriggeredThisSceneOpen[EventId] = 1;
        }

        if(Progression_Manager.instance.EventIdToTimesTriggeredThisGameOpen.Keys.Contains(EventId)){
            Progression_Manager.instance.EventIdToTimesTriggeredThisGameOpen[EventId]++;
        }
        else{
            Progression_Manager.instance.EventIdToTimesTriggeredThisGameOpen[EventId] = 1;
        }


    }

    public void trigger(){
        _alertManagerOnEventStart();
        _trigger();
        //_alertManagerOnEventEnd();
    }

    public abstract void _trigger();



    public bool Equals(Event_Trigger_Scriptable_Object other){
        return EventId == other.EventId;
    }

}
