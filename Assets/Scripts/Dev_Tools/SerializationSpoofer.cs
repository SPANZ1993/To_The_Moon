// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using System.IO;

// using System;


// public class SerializationSpoofer : MonoBehaviour, ISerialization_Manager
// {

//     [SerializeField]
//     private bool saveFileExists = true;
//     [SerializeField]
//     private bool newGame = false;
//     [SerializeField]
//     private float waitTime = 3.0f;
//     [SerializeField]
//     private float coins = 100.0f;
//     [SerializeField]
//     private float gems = 1.0f;
//     [SerializeField]
//     private bool prevOffLineMode = false;


//     [SerializeField]
//     private double secsSinceCartEmptied = 45.0;
//     private double cartLastEmptiedTimeUnix;
//     [SerializeField]
//     private double cartCurCoins = 0.0f;
//     [SerializeField]
//     private double cartCoinsPerSecond = 1.0;
//     [SerializeField]
//     private double cartCapacity = 100.0;
//     [SerializeField]
//     private double secsSinceMineGamePlayed = 3590.0;
//     private double mineGameLastPlayedUnix;
//     [SerializeField]
//     private double secsSinceLastLaunch = 1780.0;
//     private double lastLaunchTimeUnix;

//     private SaveGameObject saveGameData;
//     private bool waiting = true;


//     public static SerializationSpoofer instance;

//     void Awake()
//     {
//         cartLastEmptiedTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() - secsSinceCartEmptied;
//         mineGameLastPlayedUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() - secsSinceMineGamePlayed;
//         lastLaunchTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() - secsSinceLastLaunch;
//         if (!instance){
//             instance = this;
//             DontDestroyOnLoad(this.gameObject);       
//         }
//         else{
//             Destroy(this.gameObject);
//         }
//     }




//     bool ISerialization_Manager.checkForSavedData(){
//         return saveFileExists;
//     }

//     SaveGameObject ISerialization_Manager.loadSavedData(){
//         saveGameData = new SaveGameObject(isValid: true, 
//                             isNewGame: newGame, 
//                             coins: coins,
//                             gems: gems,
//                             offLineMode: prevOffLineMode, 
//                             cartLastEmptiedTimeUnix: cartLastEmptiedTimeUnix,
//                             cartCurCoins: cartCurCoins,
//                             cartCoinsPerSecond: cartCoinsPerSecond,
//                             cartCapacity: cartCapacity,
//                             mineGameLastPlayedUnix: mineGameLastPlayedUnix,
//                             lastLaunchTimeUnix: lastLaunchTimeUnix);
       
       
//         if (saveFileExists){
//             waiting = true;
//             StartCoroutine(waitToServe());
//             return saveGameData;
//         }
//         else{
//             throw new FileNotFoundException("CAN'T LOAD IT DOG");
//         }
//     }

//     void ISerialization_Manager.saveGameData(SaveGameObject saveGameData){
//         Debug.Log("SAVED!");
//     }

//     IEnumerator waitToServe(){
//         yield return new WaitForSeconds(waitTime);
//         waiting = false;
//     }
// }
