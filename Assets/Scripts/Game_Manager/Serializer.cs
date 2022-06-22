using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Serializer : MonoBehaviour, ISerialization_Manager
{


    


    Game_Manager gameManager;

    private string filepath;

    [SerializeField]
    private float delayBefore = 0f;
    [SerializeField]
    private float delayAfter = 0f;

    public delegate void SerializationStarted();
    public static event SerializationStarted SerializationStartedInfo;

    public delegate void SerializationEnded();
    public static event SerializationEnded SerializationEndedInfo;

    public static Serializer instance;

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







    // Start is called before the first frame update
    void Start()
    {
        filepath = Path.Combine(Application.persistentDataPath, "To_The_Moon_Save.dat");
        //Debug.Log("SAVE PATH: " + filepath  + "\n EXISTS? " + ((ISerialization_Manager)this).checkForSavedData());
        gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    bool ISerialization_Manager.checkForSavedData(){
        return File.Exists(filepath);
    }


    SaveGameObject ISerialization_Manager.loadSavedData(){
    //     saveGameData = new SaveGameObject(isValid: true, 
    //                         isNewGame: newGame, 
    //                         coins: coins,
    //                         gems: gems,
    //                         offLineMode: prevOffLineMode, 
    //                         cartLastEmptiedTimeUnix: cartLastEmptiedTimeUnix,
    //                         cartCurCoins: cartCurCoins,
    //                         cartCoinsPerSecond: cartCoinsPerSecond,
    //                         cartCapacity: cartCapacity,
    //                         mineGameLastPlayedUnix: mineGameLastPlayedUnix,
    //                         lastLaunchTimeUnix: lastLaunchTimeUnix);
        if (((ISerialization_Manager)this).checkForSavedData()){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.Open);
            SaveGameObject data = (SaveGameObject)bf.Deserialize(file);
            file.Close();
            //Debug.Log("Saved Game Loaded Holy Crap");
            return data;
        }
        else{
            throw new FileNotFoundException("CAN'T LOAD IT DOG");
        }
        return new SaveGameObject();
    }

    private IEnumerator _alertSerializationEnded(){
        yield return new WaitForSeconds(delayAfter);
        if (SerializationEndedInfo != null){
            SerializationEndedInfo();
        }
    }

    
    private IEnumerator _saveGameData(){
        yield return new WaitForSeconds(delayBefore); 
        SaveGameObject data = GenerateSaveGameObject();
        writeSaveGameObjectToDisk(data);
        StartCoroutine(_alertSerializationEnded());
    }

    private SaveGameObject GenerateSaveGameObject(){
        return new SaveGameObject(
                                                    isValid: true, 
                                                    isNewGame: false, 
                                                    coins: gameManager.coins,
                                                    gems: gameManager.gems,
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
                                                    serializedCryptoAveragePrices: Crypto_Manager.instance.serializeCryptoAveragePrices()
                                                );   
    }

    private void writeSaveGameObjectToDisk(SaveGameObject data){
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(filepath);
        bf.Serialize(file, data);
        file.Close();
        //Debug.Log("Game data saved!");
        //Debug.Log("ENDED SAVING SERIALIZER");
    }

    void ISerialization_Manager.saveGameData(){
        //Debug.Log("STARTED SAVING SERIALIZER");
        if (SerializationStartedInfo != null){
            SerializationStartedInfo();
        }
        StartCoroutine(_saveGameData());
    }


    void ISerialization_Manager.saveGameDataSerially(){
        SaveGameObject data = GenerateSaveGameObject();
        writeSaveGameObjectToDisk(data);
    }


    void ISerialization_Manager.deleteSave(){
        File.Delete(filepath);
    }

}
