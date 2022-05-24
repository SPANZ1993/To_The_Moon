using System.Collections;
using System.Collections.Generic;

using System;


[Serializable]
public class ResearchAssignmentObject{

    public int ResearchId;
    public int ResearcherId;
    public double AssignedTime;
    public double TimeLeft;
    public double ThrustReward;

    public ResearchAssignmentObject(int researchId, int researcherId, double assignedTime, double timeLeft, double thrustReward){
        ResearchId = researchId;
        ResearcherId = researcherId;
        AssignedTime = assignedTime;
        TimeLeft = timeLeft;
        ThrustReward = thrustReward;
    }
}


[Serializable]
public class SaveGameObject
{
    public bool IsValid {get; set;}
    public bool IsNewGame {get; set;}
    public double Coins {get; set;}
    public double Gems {get; set;}
    public bool OffLineMode {get; set;}


    public double CartLastEmptiedTimeUnix {get; set;}
    public double CartCurCoins {get; set;}
    public double CartCoinsPerSecond {get; set;}
    public double CartCapacity {get; set;}
    public double CartCoinsPerSecondUpgradePrice {get; set;}
    public double CartCoinsCapacityUpgradePrice {get; set;}

    
    public double MineGameLastPlayedUnix {get; set;}
    public double MineGameHitCoins {get; set;}
    public double MineGameSolveCoins {get; set;}
    public double MineGameHitCoinsUpgradePrice {get; set;}
    public double MineGameSolveCoinsUpgradePrice {get; set;}


    public double LastLaunchTimeUnix {get; set;}
    public int LaunchesRemaining {get; set;}
    public double PrevLaunchTime{get; set;}

    public List<int> UnlockedResearchIds {get; set;}
    public List<int> UnlockedResearcherIds {get; set;}
    public List<ResearchAssignmentObject> AssignedResearchers {get; set;}

    public List<int> UnlockedExperimentIds {get; set;}

    public Dictionary<Upgrade, bool> UpgradesUnlockedDict {get; private set;}
    public Dictionary<Upgrade, int> UpgradesNumberDict  {get; private set;}

    public Metrics_Object Metrics;


    public SaveGameObject(){
        IsValid = true;
        IsNewGame = true;
        OffLineMode = false;

        Coins = 0.0;
        Gems = 0.0;

        CartLastEmptiedTimeUnix = (double)((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds() - (31536000*10); // 10 years ago
        CartCurCoins = 0.0;
        CartCoinsPerSecond = 1.0;
        CartCapacity = 100.0;
        CartCoinsPerSecondUpgradePrice = 100.0;
        CartCoinsCapacityUpgradePrice = 100.0;

        LastLaunchTimeUnix = (double)((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds() + (31536000*10); // 10 years in the future
        LaunchesRemaining = 3;

        MineGameLastPlayedUnix = (double)((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds() - (31536000*10); // 10 years ago
        MineGameHitCoins = 1;
        MineGameSolveCoins = 10;
        MineGameHitCoinsUpgradePrice = 100.0;
        MineGameSolveCoinsUpgradePrice = 100.0;

        UnlockedResearchIds = new List<int> {1, 2};
        UnlockedResearcherIds = new List<int> {1, 2};
        AssignedResearchers = new List<ResearchAssignmentObject>();

        UnlockedExperimentIds = new List<int> {1, 2};

        UpgradesUnlockedDict = generateUpgradesUnlockedDict();
        UpgradesNumberDict = generateUpgradesNumberDict();

        Metrics = new Metrics_Object();
    }

    private Dictionary<Upgrade, bool> generateUpgradesUnlockedDict(){
        Dictionary<Upgrade, bool> uud = new Dictionary<Upgrade, bool>();
        foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
            uud[upgrade] = false;
            // if (upgrade == Upgrade.Autopilot_System){
            //     uud[upgrade] = true;
            // } // REMOVE
        }
        return uud;
    }

    private Dictionary<Upgrade, int> generateUpgradesNumberDict(){
        Dictionary<Upgrade, int> und = new Dictionary<Upgrade, int>();
        foreach(Upgrade upgrade in Enum.GetValues(typeof(Upgrade))){
            und[upgrade] = 0;
        }
        return und;
    }

    public SaveGameObject(bool isValid, 
                            bool isNewGame, 
                            double coins,
                            double gems,
                            bool offLineMode, 
                            double cartLastEmptiedTimeUnix,
                            double cartCurCoins,
                            double cartCoinsPerSecond,
                            double cartCapacity,
                            double cartCoinsPerSecondUpgradePrice,
                            double cartCoinsCapacityUpgradePrice,
                            double mineGameLastPlayedUnix,
                            double mineGameHitCoins,
                            double mineGameSolveCoins,
                            double mineGameHitCoinsUpgradePrice,
                            double mineGameSolveCoinsUpgradePrice,
                            double lastLaunchTimeUnix,
                            int launchesRemaining,
                            List<int> unlockedResearchIds,
                            List<int> unlockedResearcherIds,
                            List<ResearchAssignmentObject> assignedResearchers,
                            List<int> unlockedExperimentIds,
                            Dictionary<Upgrade, bool> upgradesUnlockedDict,
                            Dictionary<Upgrade, int> upgradesNumberDict,
                            Metrics_Object metrics){

        IsValid = isValid;
        IsNewGame = isNewGame;
        OffLineMode = offLineMode;

        Coins = coins;
        Gems = gems;
        
        CartLastEmptiedTimeUnix = cartLastEmptiedTimeUnix;
        CartCurCoins = cartCurCoins;
        CartCoinsPerSecond = cartCoinsPerSecond;
        CartCapacity = cartCapacity;
        CartCoinsPerSecondUpgradePrice = cartCoinsPerSecondUpgradePrice;
        CartCoinsCapacityUpgradePrice = cartCoinsCapacityUpgradePrice;

        MineGameLastPlayedUnix = mineGameLastPlayedUnix;
        MineGameHitCoins = mineGameHitCoins;
        MineGameSolveCoins = mineGameSolveCoins;
        MineGameHitCoinsUpgradePrice = mineGameHitCoinsUpgradePrice;
        MineGameSolveCoinsUpgradePrice = mineGameSolveCoinsUpgradePrice;
        
        
        LastLaunchTimeUnix = lastLaunchTimeUnix;
        LaunchesRemaining = launchesRemaining;

        UnlockedResearchIds = unlockedResearchIds;
        UnlockedResearcherIds = unlockedResearcherIds;
        AssignedResearchers = assignedResearchers;

        UnlockedExperimentIds = unlockedExperimentIds;
    
        UpgradesUnlockedDict = upgradesUnlockedDict;
        UpgradesNumberDict = upgradesNumberDict; 
    
        Metrics = metrics;
    }

}
