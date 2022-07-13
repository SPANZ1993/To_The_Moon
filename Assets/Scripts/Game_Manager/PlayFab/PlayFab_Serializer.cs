using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

public class PlayFab_Serializer : MonoBehaviour, ISerialization_Manager
{

    Game_Manager gameManager;
    PlayFab_Manager playFabManager;

    [SerializeField]
    private float delayBefore = 1f;
    [SerializeField]
    private float delayAfter = 1f;

    private bool currentlySaving = false;
    private bool currentlyLoading = false;

    public delegate void SerializationStarted();
    public static event SerializationStarted SerializationStartedInfo;

    public delegate void SerializationEnded();
    public static event SerializationEnded SerializationEndedInfo;


    public static PlayFab_Serializer instance;

    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);       
        }
        else{
            Destroy(this.gameObject);
        }
    }

    void OnEnable(){
        PlayFab_Manager.PlayFabSaveDataSuccessInfo += onPlayFabSaveDataComplete;
        PlayFab_Manager.PlayFabSaveDataFailureInfo += onPlayFabSaveDataComplete;
    }

    void OnDisable(){
        PlayFab_Manager.PlayFabSaveDataSuccessInfo -= onPlayFabSaveDataComplete;
        PlayFab_Manager.PlayFabSaveDataFailureInfo -= onPlayFabSaveDataComplete;
    }


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        playFabManager = GameObject.Find("PlayFab_Manager").GetComponent<PlayFab_Manager>();
    }




    private IEnumerator _alertSerializationEnded(){
        IEnumerator waitForSaveComplete(){
            while(currentlySaving){
                yield return new WaitForSeconds(0);
            }
            if (SerializationEndedInfo != null){
                SerializationEndedInfo();
            }
        }

        yield return new WaitForSeconds(delayAfter);
        StartCoroutine(waitForSaveComplete());
    }

    private IEnumerator _saveGameData(){
        yield return new WaitForSeconds(delayBefore);
        currentlySaving = true;
        SaveGameObject data = GenerateSaveGameObject();
        playFabManager.SaveData(data);
        StartCoroutine(_alertSerializationEnded());
    }

    private void onPlayFabSaveDataComplete(){
        currentlySaving = false;
    }


    private SaveGameObject GenerateSaveGameObject(){
        return new SaveGameObject(
                                                    isValid: true, 
                                                    isNewGame: false, 
                                                    coins: gameManager.coins,
                                                    gems: gameManager.gems,
                                                    thrust: gameManager.thrust,
                                                    coinName: gameManager.coinName,
                                                    offLineMode: gameManager.offLineMode, 
                                                    cartLastEmptiedTimeUnix: gameManager.mineCartLastEmptiedTimeUnix,
                                                    cartCurCoins: gameManager.mineCartCurCoins,
                                                    cartCoinsPerSecond: gameManager.mineCartCoinsPerSecond,
                                                    cartCapacity: gameManager.mineCartCoinsCapacity,
                                                    cartCoinsPerSecondUpgradePrice: gameManager.mineCartCoinsPerSecondUpgradePrice,
                                                    cartCoinsCapacityUpgradePrice: gameManager.mineCartCoinsCapacityUpgradePrice,
                                                    mineGameLastPlayedUnix: gameManager.mineGameLastPlayedUnix,
                                                    mineGameHitCoins: gameManager.mineGameHitCoins,
                                                    mineGameSolveCoins: gameManager.mineGameSolveCoins,
                                                    mineGameHitCoinsUpgradePrice: gameManager.mineGameHitCoinsUpgradePrice,
                                                    mineGameSolveCoinsUpgradePrice: gameManager.mineGameSolveCoinsUpgradePrice,
                                                    lastLaunchTimeUnix: gameManager.prevLaunchTimeUnix,
                                                    launchesRemaining: gameManager.remainingLaunches,
                                                    musicSoundLevel: gameManager.musicSoundLevel,
                                                    soundFxSoundLevel: gameManager.soundFxSoundLevel,
                                                    speedText: gameManager.textSpeed,
                                                    unlockedResearchIds: gameManager.unlockedResearchIds,
                                                    unlockedResearcherIds: gameManager.unlockedResearcherIds,
                                                    assignedResearchers: gameManager.assignedResearchers,
                                                    unlockedExperimentIds: gameManager.unlockedExperimentIds,
                                                    upgradesUnlockedDict: gameManager.upgradesUnlockedDict,
                                                    upgradesNumberDict: gameManager.upgradesNumberDict,
                                                    metrics: gameManager.metrics,
                                                    serializedCryptoBalances: Crypto_Manager.instance.serializeCryptoBalances(),
                                                    serializedCryptoAveragePrices: Crypto_Manager.instance.serializeCryptoAveragePrices(),
                                                    ownedNonConsumableProductsIds: IAP_Manager.instance.ownedNonConsumableProductsIds,
                                                    curRobotClothesId: Robot_Outfit_Manager.instance.CurOutfitID,
                                                    curShipSkinId: Ship_Skin_Manager.instance.CurSkinID,
                                                    serializedEventsState: Progression_Manager.instance.serializeEventsState(),
                                                    currentLevelId: Progression_Manager.instance.CurrentLevelId,
                                                    highestLevelId: Progression_Manager.instance.HighestLevelId,
                                                    rocketGameFreePlayMode: Progression_Manager.instance.RocketGameFreePlayMode,
                                                    rocketGameFreePlayModeManuallySet: Progression_Manager.instance.RocketGameFreePlayModeManuallySet
                                                );   
    }


    bool ISerialization_Manager.checkForSavedData(){
        return false;
    }


    SaveGameObject ISerialization_Manager.loadSavedData(){
        return new SaveGameObject();
    }


    void ISerialization_Manager.saveGameData(){
        //Debug.Log("STARTED SAVING SERIALIZER");
        if (SerializationStartedInfo != null){
            SerializationStartedInfo();
        }
        StartCoroutine(_saveGameData());
    }


    void ISerialization_Manager.saveGameDataSerially(){
        // Just Kinda Send It Out There and Hope It Saves
        //Debug.Log("TRYNA SAVE BEBE");
        if (SceneManager.GetActiveScene().name != "Landing_Page"){
            SaveGameObject data = GenerateSaveGameObject();
            playFabManager.SaveData(data);
        }
    }


    void ISerialization_Manager.deleteSave(){

    }

}
