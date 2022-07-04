using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public class Progression_Manager : MonoBehaviour
{


    // Don't do a damn thing if you haven't been initialized yet
    public bool initialized = false;

    public Level_Scriptable_Object[] Levels { get { return levels; } private set { levels = value; } }
    public int CurrentLevelId { get { return currentLevelId; } private set { currentLevelId = value; } }
    public int HighestLevelId { get {return highestLevelId;} private set { highestLevelId = value; } }


    [SerializeField]
    private Level_Scriptable_Object[] levels;
    [SerializeField]
    private int currentLevelId = 0;
    [SerializeField]
    private int highestLevelId = 0;


    // TODO: IMPLEMENT GAMETIME SINCE EVENT && REALTIME SINCE EVENT FINISHED DICTS
    public Event_Trigger_Scriptable_Object[] Events {get { return events; } private set { events = value; } }
    public Dictionary<int, int> EventIdToTimesTriggered {get; private set;}
    public Dictionary<int, int> EventIdToTimesTriggeredThisLevelOpen {get; private set;}
    public Dictionary<int, int> EventIdToTimesTriggeredThisSceneOpen {get; private set;}
    public Dictionary<int, int> EventIdToTimesTriggeredThisGameOpen {get; private set;}
    private Queue<Event_Trigger_Scriptable_Object> EventQueue;
    public int? CurrentEventIdInProgress; // If not null, means an event is happening right now


    [SerializeField]
    private Event_Trigger_Scriptable_Object[] events;

    



    public static Progression_Manager instance;

    void Awake()
    {

        if (instance == null){
            Debug.Assert(Events.Select(e => e.EventId).ToList().Count() == Events.Select(e => e.EventId).Distinct().ToList().Count());
            Debug.Assert(Levels.Select(l => l.LevelId).ToList().Count() == Levels.Select(e => e.LevelId).Distinct().ToList().Count());

            initialized = false;

            CurrentLevelId = 0;
            HighestLevelId = 0;

            EventIdToTimesTriggered = new Dictionary<int, int>();
            EventIdToTimesTriggeredThisLevelOpen = new Dictionary<int, int>();
            EventIdToTimesTriggeredThisSceneOpen = new Dictionary<int, int>();
            EventIdToTimesTriggeredThisGameOpen = new Dictionary<int, int>();
            foreach(int id in Events.Select(e => e.EventId)){
                EventIdToTimesTriggered[id] = 0;
                EventIdToTimesTriggeredThisLevelOpen[id] = 0;
                EventIdToTimesTriggeredThisSceneOpen[id] = 0;
                 EventIdToTimesTriggeredThisGameOpen[id] = 0;
            }


            EventQueue = new Queue<Event_Trigger_Scriptable_Object>();

            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(initialized){
            AddEventsToQueue();
            StartNextEvent();
        }
    }



    public void OnLevelWasLoaded(){
        if(initialized){
            foreach(Event_Trigger_Scriptable_Object e in Events){
                EventIdToTimesTriggeredThisSceneOpen[e.EventId] = 0;
            }

            AddEventsToQueue(Events.Where(e => e.TriggerableUponLevelWasLoaded));
            StartNextEvent();
        }
    }




    public Level_Scriptable_Object getLevelById(int id){
        Level_Scriptable_Object l = Array.Find(Levels, level => level.LevelId == id);
        if (l == null)
        {
            Debug.LogWarning("Level with ID: " + id + " not found");
            return null;
        }
        return l;
    }

    public Level_Scriptable_Object getCurrentLevel(){
        return getLevelById(currentLevelId);
    }

    public Level_Scriptable_Object getHighestLevel(){
        return getLevelById(highestLevelId);
    }












    public void AddEventsToQueue(){
        AddEventsToQueue(Events);
    }



    public void AddEventsToQueue(IEnumerable<Event_Trigger_Scriptable_Object> PossibleEvents){
        if(CurrentEventIdInProgress == null){
            foreach(Event_Trigger_Scriptable_Object e in PossibleEvents.OrderBy(e=>e.EventPriority)){
                if(e.shouldTrigger() && !EventQueue.Contains(e) && CurrentEventIdInProgress == null){
                    EventQueue.Enqueue(e);
                }
            }
        }
        else{
            //Debug.Log("CURRENTLY IN AN EVENT... WAITING TO ADD EVENTS");
        }
    }




    public void StartNextEvent(){
        if(EventQueue.Count != 0){
            EventQueue = new Queue<Event_Trigger_Scriptable_Object>(EventQueue.OrderBy(e => e.EventPriority).ToList());
            Event_Trigger_Scriptable_Object curEvent = EventQueue.Dequeue();
            // If we should still trigger the event then trigger it
            if(curEvent.shouldTrigger()){
                curEvent.trigger();
            }
        }
    }











    public Dictionary<int, Dictionary<string, int>> serializeEventsState(){
        Dictionary<int, Dictionary<string, int>> eventsStateDict = new Dictionary<int, Dictionary<string, int>>();
        foreach(Event_Trigger_Scriptable_Object e in events){
            Dictionary<string, int> curEventStateDict = new Dictionary<string, int>();
            if(!EventIdToTimesTriggered.Keys.Contains(e.EventId)){
                curEventStateDict["Triggered"] = 0;
            }
            else{
                curEventStateDict["Triggered"] = EventIdToTimesTriggered[e.EventId];
            }

            if(!EventIdToTimesTriggeredThisLevelOpen.Keys.Contains(e.EventId)){
                curEventStateDict["TriggeredThisLevelOpen"] = 0;
            }
            else{
                curEventStateDict["TriggeredThisLevelOpen"] = EventIdToTimesTriggeredThisLevelOpen[e.EventId];
            }
            eventsStateDict[e.EventId] = curEventStateDict;
        }

        return eventsStateDict;
    }


    public void initializeEventsState(Dictionary<int, Dictionary<string, int>> serializedEventsState){

        if(EventIdToTimesTriggered == null){
            EventIdToTimesTriggered = new Dictionary<int, int>();
        }
        if(EventIdToTimesTriggeredThisLevelOpen == null){
            EventIdToTimesTriggeredThisLevelOpen = new Dictionary<int, int>();
        }
        if(EventIdToTimesTriggeredThisSceneOpen == null){
            EventIdToTimesTriggeredThisSceneOpen = new Dictionary<int, int>();
        }
        if(EventIdToTimesTriggeredThisGameOpen == null){
            EventIdToTimesTriggeredThisGameOpen = new Dictionary<int, int>();
        }


        foreach(Event_Trigger_Scriptable_Object e in events){
            if(serializedEventsState.Keys.Contains(e.EventId)){
                if(serializedEventsState[e.EventId] != null && serializedEventsState[e.EventId]["Triggered"] != null){
                    EventIdToTimesTriggered[e.EventId] = serializedEventsState[e.EventId]["Triggered"];
                }
                else{
                    EventIdToTimesTriggered[e.EventId] = 0;
                }

                if(serializedEventsState[e.EventId] != null && serializedEventsState[e.EventId]["TriggeredThisLevelOpen"] != null){
                    EventIdToTimesTriggeredThisLevelOpen[e.EventId] = serializedEventsState[e.EventId]["TriggeredThisLevelOpen"];
                }
                else{
                    EventIdToTimesTriggeredThisLevelOpen[e.EventId] = 0;
                }
            }
            else{
                EventIdToTimesTriggered[e.EventId] = 0;
                EventIdToTimesTriggeredThisLevelOpen[e.EventId] = 0;
            }

            EventIdToTimesTriggeredThisSceneOpen[e.EventId] = 0;
            EventIdToTimesTriggeredThisGameOpen[e.EventId] = 0;
        }

        initialized = true;
    }

}
