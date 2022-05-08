using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;


public enum Upgrade{
    Autopilot,
    Lateral_Boosters,
    Particle_Shield,
    Cow_Catcher,
    Turbo_Boost,
    Da_Bomb,
    Gem_Magnet
}


public enum AutopilotReturnState{ // If for some reason, we are unable to proceed due to an obstacle, etc, then we can use this to convey that after the flight
    Normal
}



public class Upgrades_Manager : MonoBehaviour
{


    public Dictionary<Upgrade, bool> upgradesUnlockedDict {get; private set;}
    public Dictionary<Upgrade, int> upgradesNumberDict  {get; private set;}
    public Dictionary<Upgrade, int> upgradesMaxNumberDict  {get; private set;}


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

    // Start is called before the first frame update
    void Start()
    {
        if (upgradesUnlockedDict == null && upgradesNumberDict == null && upgradesMaxNumberDict == null){
            upgradesUnlockedDict = new Dictionary<Upgrade, bool>();
            upgradesNumberDict = new Dictionary<Upgrade, int>();
            upgradesMaxNumberDict = new Dictionary<Upgrade, int>();
            foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
                upgradesUnlockedDict[upgrade] = false;
                upgradesNumberDict[upgrade] = 0;
                upgradesMaxNumberDict[upgrade] = 0;
                switch(upgrade){
                    case Upgrade.Autopilot:
                        upgradesMaxNumberDict[upgrade] = 1;
                        upgradesNumberDict[upgrade] = 1; // REMOVE
                        upgradesUnlockedDict[upgrade] = true; // REMOVE
                        break;
                    case Upgrade.Lateral_Boosters:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    case Upgrade.Particle_Shield:
                        upgradesMaxNumberDict[upgrade] = 1;
                        break;
                    case Upgrade.Cow_Catcher:
                        upgradesMaxNumberDict[upgrade] = 1;
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
        if (upgradesNumberDict[upgrade] <= upgradesMaxNumberDict[upgrade] - number){
            return true;
        }
        else{
            return false;
        }
    }

    public void addUpgrade(Upgrade upgrade, int number){
        if(canAddUpgrade(upgrade, number)){
            upgradesUnlockedDict[upgrade] = true;
            upgradesNumberDict[upgrade] += number;
        }
        else{
            Debug.Log("Tried to add upgrade " + upgrade + " but couldn't");
        }
    }
}
