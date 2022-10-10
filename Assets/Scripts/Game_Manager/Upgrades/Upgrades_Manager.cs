using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;


public enum Upgrade{
    Autopilot_System = 1,
    Lateral_Boosters = 2,
    Particle_Shield = 3,
    Cow_Catcher = 4,
    Turbo_Boost = 5,
    Da_Bomb = 6,
    Gem_Magnet = 7
}


public enum AutopilotReturnState{ // If for some reason, we are unable to proceed due to an obstacle, etc, then we can use this to convey that after the flight
    Normal
}



public class Upgrades_Manager : MonoBehaviour
{


    public Dictionary<Upgrade, bool> upgradesUnlockedDict {get; set;} // Have we acquired this upgrade
    public Dictionary<Upgrade, int> upgradesNumberDict  {get; set;}
    public Dictionary<Upgrade, int> upgradesMaxNumberDict  {get; private set;}

    
    public Dictionary<Upgrade, ExperimentId> upgrade2ExperimentId {get; private set;}
    public Dictionary<ExperimentId, Upgrade> experimentId2Upgrade {get; private set;}
    
    // Autopilot
    public bool autopilotFlag = false; // If true, the next rocket game we enter will be an autopilot flight
    public double? autopilotHeight;
    public int? autopilotGems;
    public AutopilotReturnState? autopilotReturnState;
    // End Autopilot


    // Lateral Boosters
    
    // End Lateral Boosters
    
    
    // Particle Shield
    // End Particle Shield
    // Cow Catcher
    // End Cow Catcher
    // Da Bomb
    // End Da Bomb
    // Gem Magnet
    // End Gem Magnet


    public static Upgrades_Manager instance;

    void Awake(){
        if(!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            autopilotFlag = false;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    private void BuildUpgrade2ExperimentIdDict(){
        upgrade2ExperimentId = new Dictionary<Upgrade, ExperimentId>();
        experimentId2Upgrade = new Dictionary<ExperimentId, Upgrade>();
        foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
            //Debug.Log(upgrade + " " + (int)upgrade +  " " + Enum.GetName(typeof(Upgrade), (int)upgrade));
            ExperimentId exp;
            bool foundExp = false;
            foundExp = Enum.TryParse(Enum.GetName(typeof(Upgrade), (int)upgrade), out exp);
            
            if (foundExp){
                if((int)upgrade == (int)exp){
                    //Debug.Log("EXP: " + exp + "   " + " UPGRADE: " + upgrade);
                    upgrade2ExperimentId[upgrade] = exp;
                    experimentId2Upgrade[exp] = upgrade;
                }
                else{
                    throw new InvalidOperationException(Enum.GetName(typeof(Upgrade), (int)upgrade) + " HAS A DIFFERENT VALUE IN EXPERIMENTID ENUM");
                }
            }
            else{
                throw new InvalidOperationException(Enum.GetName(typeof(Upgrade), (int)upgrade) + " UPGRADE HAS NO CORRESPONDING EXPERIMENT");
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (upgrade2ExperimentId == null || experimentId2Upgrade == null){
            BuildUpgrade2ExperimentIdDict();
        }


        if (upgradesUnlockedDict == null && upgradesNumberDict == null){
            upgradesUnlockedDict = new Dictionary<Upgrade, bool>();
            upgradesNumberDict = new Dictionary<Upgrade, int>();
            foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
                upgradesUnlockedDict[upgrade] = false;
                upgradesNumberDict[upgrade] = 0;
                switch(upgrade){
                    case Upgrade.Autopilot_System:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false;
                        break;
                    case Upgrade.Lateral_Boosters:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false; // TODO: Make Some Visual Representation of This
                        break;
                    case Upgrade.Particle_Shield:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false; // TODO: Implement Dark Matter and Particle Shield
                        break;
                    case Upgrade.Cow_Catcher:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false;
                        break;
                    case Upgrade.Turbo_Boost:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false;
                        break;
                    case Upgrade.Da_Bomb:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false;
                        break;
                    case Upgrade.Gem_Magnet:
                        upgradesNumberDict[upgrade] = 0;
                        upgradesUnlockedDict[upgrade] = false;
                        break;
                    default:
                        break;
                }
            }    
        }

        if (upgradesMaxNumberDict == null){
            upgradesMaxNumberDict = new Dictionary<Upgrade, int>();
            foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
                upgradesMaxNumberDict[upgrade] = 0;
                switch(upgrade){
                    case Upgrade.Autopilot_System:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    case Upgrade.Lateral_Boosters:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    case Upgrade.Particle_Shield:
                        upgradesMaxNumberDict[upgrade] = 1; // TODO: Make Some Visual Representation of This
                        break;
                    case Upgrade.Cow_Catcher:
                        upgradesMaxNumberDict[upgrade] = 3;
                        break;
                    case Upgrade.Turbo_Boost:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    case Upgrade.Da_Bomb:
                        upgradesMaxNumberDict[upgrade] = 3;
                        break;
                    case Upgrade.Gem_Magnet:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    default:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                }
            }    
        }
    }

    public void OnLevelWasLoaded(){
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            IEnumerator unsetAutopilotFlag(){
                yield return new WaitForSeconds(0.5f);
                autopilotFlag = false;
            }
            StartCoroutine(unsetAutopilotFlag());
            //Debug.Log("HELLO FROM UPGRADE MANAGER: " + autopilotHeight + " " + autopilotGems + " " + autopilotReturnState);
        }
    }

    public bool canAddUpgrade(Upgrade upgrade, int number){
        Debug.Log("HEY");
        print("A: " + number);
        print("B: " + upgradesMaxNumberDict[upgrade]);
        print("C: " + upgradesNumberDict[upgrade]);
        if (upgradesNumberDict[upgrade] <= upgradesMaxNumberDict[upgrade] - number){
            return true;
        }
        else{
            return false;
        }
    }

    public bool addUpgrade(Upgrade upgrade, int number){
        if(canAddUpgrade(upgrade, number)){
            upgradesUnlockedDict[upgrade] = true;
            upgradesNumberDict[upgrade] += number;
            return true;
        }
        else{
            Debug.Log("Tried to add upgrade " + upgrade + " but couldn't");
            return false;
        }
    }


    public int getUpgradeAddNumber(Upgrade upgrade){
        int upgradeNum = 1;
        if(Experiments_Manager.instance.ExperimentNumExists((int)upgrade)){
            Experiment exp = Experiments_Manager.instance.GetExperimentByExperimentNumber((int)upgrade);
            Duration dur = exp.duration;
            switch(dur){
                case Duration.Permanent:
                    upgradeNum = 1;
                    break;

                case Duration.Three_Launches:
                    upgradeNum = 3;
                    break;

                case Duration.Three_Uses:
                    upgradeNum = 3;
                    break;

                default:
                    break;
            }
        }
        return upgradeNum;
    }
}
