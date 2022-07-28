using System.Collections;
using System.Collections.Generic;

using System;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;

using UnityEngine;

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
    public double Thrust {get; set;}
    public bool OffLineMode {get; set;}
    public string CoinName {get; set;}


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

    public float MusicSoundLevel {get; set;}
    public float SoundFxSoundLevel {get; set;}

    public TextSpeed SpeedText {get; set;}

    public List<int> UnlockedResearchIds {get; set;}
    public List<int> UnlockedResearcherIds {get; set;}
    public List<ResearchAssignmentObject> AssignedResearchers {get; set;}

    public List<int> UnlockedExperimentIds {get; set;}

    public Dictionary<Upgrade, bool> UpgradesUnlockedDict {get; private set;}
    public Dictionary<Upgrade, int> UpgradesNumberDict  {get; private set;}

    public Metrics_Object Metrics;

    public Dictionary<int, double> SerializedCryptoBalances {get; private set;}
    public Dictionary<int, double> SerializedCryptoAveragePrices {get; private set;}

    public List<string> OwnedNonConsumableProductsIds;

    public int CurRobotClothesId {get; set;}

    public int CurShipSkinId {get; set;}


    public Dictionary<int, Dictionary<string, int>> SerializedEventsState {get; private set;}
    public int CurrentLevelId;
    public int HighestLevelId;
    public bool RocketGameFreePlayMode;
    public bool RocketGameFreePlayModeManuallySet;


    public bool IsPatron;

    public SaveGameObject(){
        IsValid = true;
        IsNewGame = true;
        OffLineMode = false;
        CoinName = null;

        Coins = 0.0;
        Gems = 0.0;
        Thrust = 100.0;

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

        MusicSoundLevel = 1f;
        SoundFxSoundLevel = 1f;

        SpeedText = TextSpeed.Medium;

        UnlockedResearchIds = new List<int> {1, 2};
        UnlockedResearcherIds = new List<int> {1, 2};
        AssignedResearchers = new List<ResearchAssignmentObject>();

        UnlockedExperimentIds = new List<int> {1, 2};

        UpgradesUnlockedDict = generateUpgradesUnlockedDict();
        UpgradesNumberDict = generateUpgradesNumberDict();

        Metrics = new Metrics_Object();

        SerializedCryptoBalances = new Dictionary<int, double>();
        SerializedCryptoAveragePrices = new Dictionary<int, double>();

        OwnedNonConsumableProductsIds = new List<string>{"com.eggkidgames.blockchainblastoff.robotOutfitMiner", "com.eggkidgames.blockchainblastoff.robotOutfitPatron", "com.eggkidgames.blockchainblastoff.shipSkinDefault", "com.eggkidgames.blockchainblastoff.shipSkinPatron"};

        CurRobotClothesId = 0; // Mine outfit

        CurShipSkinId = 0; // Default Ship Skin

        SerializedEventsState = new Dictionary<int, Dictionary<string, int>>(); // Progression manager should read this as no event has ever occured yet... which is what we want on a new game
        CurrentLevelId = 0;
        HighestLevelId = 0;
        RocketGameFreePlayMode = false;
        RocketGameFreePlayModeManuallySet = false;

        IsPatron = false;
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
                            double thrust,
                            bool offLineMode,
                            string coinName,
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
                            float musicSoundLevel,
                            float soundFxSoundLevel,
                            TextSpeed speedText,
                            List<int> unlockedResearchIds,
                            List<int> unlockedResearcherIds,
                            List<ResearchAssignmentObject> assignedResearchers,
                            List<int> unlockedExperimentIds,
                            Dictionary<Upgrade, bool> upgradesUnlockedDict,
                            Dictionary<Upgrade, int> upgradesNumberDict,
                            Metrics_Object metrics, 
                            Dictionary<int, double> serializedCryptoBalances,
                            Dictionary<int, double> serializedCryptoAveragePrices,
                            List<string> ownedNonConsumableProductsIds,
                            int curRobotClothesId,
                            int curShipSkinId,
                            Dictionary<int, Dictionary<string, int>> serializedEventsState,
                            int currentLevelId,
                            int highestLevelId,
                            bool rocketGameFreePlayMode,
                            bool rocketGameFreePlayModeManuallySet,
                            bool isPatron
                            ){

        IsValid = isValid;
        IsNewGame = isNewGame;
        OffLineMode = offLineMode;

        Coins = coins;
        Gems = gems;
        Thrust = thrust;
        CoinName = coinName;
        
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

        MusicSoundLevel = musicSoundLevel;
        SoundFxSoundLevel = soundFxSoundLevel;

        SpeedText = speedText;

        UnlockedResearchIds = unlockedResearchIds;
        UnlockedResearcherIds = unlockedResearcherIds;
        AssignedResearchers = assignedResearchers;

        UnlockedExperimentIds = unlockedExperimentIds;
    
        UpgradesUnlockedDict = upgradesUnlockedDict;
        UpgradesNumberDict = upgradesNumberDict; 
    
        Metrics = metrics;
    
        SerializedCryptoBalances = serializedCryptoBalances;
        SerializedCryptoAveragePrices = serializedCryptoAveragePrices;

        OwnedNonConsumableProductsIds = ownedNonConsumableProductsIds;

        CurRobotClothesId = curRobotClothesId;

        CurShipSkinId = curShipSkinId;


        SerializedEventsState = serializedEventsState;
        CurrentLevelId = currentLevelId;
        HighestLevelId = highestLevelId;
        RocketGameFreePlayMode = rocketGameFreePlayMode;
        RocketGameFreePlayModeManuallySet = rocketGameFreePlayModeManuallySet;

        IsPatron = isPatron;
    }


    // Can't do this because PlayFab Only Lets you Send 10 Keys at a Time :c
    // public Dictionary<string, string> ToStringDict(){

    //     Type[] nonJsonTypes = new Type[]{typeof(string), typeof(double), typeof(int), typeof(float)};

    //     Dictionary<string, string> D = new Dictionary<string, string>();
    //     PropertyInfo[] properties = typeof(SaveGameObject).GetProperties();
    //     foreach (PropertyInfo property in properties)
    //     {
    //         if(nonJsonTypes.Contains(property.PropertyType)){
    //             D[property.Name] = property.GetValue(this).ToString();
    //         }
    //         else{
    //             D[property.Name] = JsonConvert.SerializeObject(property.GetValue(this));
    //         }
    //     }

    //     return D;
    // }
}
