using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public  abstract class Level_End_Event_Scriptable_Object : Event_Trigger_Scriptable_Object
{
    [SerializeField]
    private int LevelId;


    public override bool shouldTrigger(){
        bool shouldTrigger = base.shouldTrigger();
        //Debug.Log("LEVEL END -- IS THE RECENTLY COMPLETED LEVEL ID EQUAL TO THIS LEVEL ID? " + (Progression_Manager.instance.recentlyCompletedLevelId == LevelId) + " ... SHOULD WE TRIGGER OTHERWISE? " + shouldTrigger + " TIMES TRIGGERED " + Progression_Manager.instance.EventIdToTimesTriggered[base.eventId]);
        //Debug.Log("RECENT: " + Progression_Manager.instance.recentlyCompletedLevelId);
        //Debug.Log("LOOKING FOR: " + LevelId);
        if(shouldTrigger){
            if(Progression_Manager.instance.recentlyCompletedLevelId != LevelId){
                shouldTrigger = false;
            }
            else{
                if(
                    // Make sure the Progression Manager is Tracking This Event
                    Progression_Manager.instance.EventIdToTimesTriggered.Keys.Contains(base.eventId) && 
                    // And this is the highest level we've ever done
                    //Progression_Manager.instance.HighestLevelId == LevelId &&
                    Progression_Manager.instance.HighestLevelId == Progression_Manager.instance.CurrentLevelId && 
                    ( 
                    // We've never done this one before and we don't have the next level
                    (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] == 0  && !Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId)) || 
                    // We've either never done this, or we've only done it once (i.e. the no next level case) but we have a next level to go to now
                    (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] <= 1 && Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId))
                    )
                
                ){
                  shouldTrigger = true;  
                }
                else{
                    //bool a = Progression_Manager.instance.EventIdToTimesTriggered.Keys.Contains(base.eventId);
                    //bool b = Progression_Manager.instance.HighestLevelId == LevelId;
                    //bool c = (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] == 0  && !Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId));
                    //bool d = (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] <= 1 && Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId));
                    //Debug.Log("NEVERMIND " + a + "  " + b);
                    shouldTrigger = false;
                }
            }
        }
        return shouldTrigger;
    }



    public abstract void executeLevelCompleteNextLevelNotReady();
    public abstract void executeLevelCompleteNextLevelReady();

    public override void _trigger(){
        try{
            if(
                (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] == 0  && !Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId))
            ){
                // If we are the highest level ID, but there is not another level yet
                // Debug.Log("LEVEL END: THERE ARE NO MORE LEVELS RODNEY");
                executeLevelCompleteNextLevelNotReady();
            }
            else if(
                    (Progression_Manager.instance.EventIdToTimesTriggered[base.eventId] <= 1 && Progression_Manager.instance.Levels.Select(l=>l.LevelId).Contains(Progression_Manager.instance.Levels.Where(l => l.LevelId == LevelId).ToList()[0].NextLevelId))
                ){
                // If we are the highest level ID, but there is another level above this one
                // Debug.Log("LEVEL END: PROCEED TO NEXT LEVEL HERE");
                executeLevelCompleteNextLevelReady();
            }
            else{
                Debug.LogError("DIDN'T CATCH EITHER SCENARIO FOR TRIGGERING EXECUTE LEVEL COMPLETE WITH EVENT ID: " + EventId);
            }
        }
        catch(System.Exception e){
            Debug.LogError("ERROR WHILE TRYING TO TRIGGER EXECUTE LEVEL COMPLETE WITH EVENT ID: " + EventId + " --- " + e);
        }
    }


    public void onEventEnded(bool nextLevelWasReady){
        //Debug.Log("ALERTING ONBOARDING SEQUENCE OVER");
        base._alertManagerOnEventEnd();
    
        // If the next level was ready then no need to go into free play mode, otherwise, go into free play mode
        Progression_Manager.instance.RocketGameFreePlayMode = !nextLevelWasReady;

        // If we made it to the next level, then unlock all the stuff that completing this level unlocks
        if(nextLevelWasReady){
            // UNLOCK NEW EXPERIMENTS/RESEARCH/RESEARCHERS ASSOCIATED WITH THIS LEVEL
            Level_Scriptable_Object Level = Progression_Manager.instance.getLevelById(LevelId);
            
            // Experiments
            List<ExperimentId> newUnlockedExperimentIds = new List<ExperimentId>(Experiments_Manager.instance.getUnlockedExperimentIds());
            foreach(ExperimentId expId in Level.UnlockedExperimentIds){
                newUnlockedExperimentIds.Add(expId);
            }
            Experiments_Manager.instance.setUnlockedExperimentIds(newUnlockedExperimentIds.Distinct().ToList(), alertExperimentsUpdated:true);
            
            // Research
            List<int> newUnlockedResearchIds = new List<int>(Research_Manager.instance.getUnlockedResearchIds());
            foreach(int researchId in Level.UnlockedResearchIds){
                newUnlockedResearchIds.Add(researchId);
            }
            Research_Manager.instance.setUnlockedResearchIds(newUnlockedResearchIds.Distinct().ToList(), alertResearchersUpdated:true);
            
            // Researchers
            List<int> newUnlockedResearchersIds = new List<int>(Researcher_Manager.instance.getUnlockedResearchersIds());
            foreach(int researcherId in Level.UnlockedResearcherIds){
                newUnlockedResearchersIds.Add(researcherId);
            }
            Researcher_Manager.instance.setUnlockedResearchersIds(newUnlockedResearchersIds.Distinct().ToList());

            // Outfits
            if(Level.UnlockedRobotOutfit != null){
                // Debug.Log("ADDING NEW OUTFIT: " + Level.UnlockedRobotOutfit.ProductId);
                IAP_Manager.instance.addActiveProduct(Level.UnlockedRobotOutfit);
                // Debug.Log("EQUIPPING NEW OUTFIT: " + Level.UnlockedRobotOutfit.ProductId);
                Level.UnlockedRobotOutfit.OnEquip();
            }
        }


        // IEnumerable alertOnboardEnded(){
        //     yield return new WaitForSeconds(0.25f); // Wait just a smidge before starting another event if there is one
        //     base._alertManagerOnEventEnd();
        // }
        // Debug.Log("Alerting progression manager onboarding ended");
        // StartCoroutine(alertOnboardEnded());
    }
}
