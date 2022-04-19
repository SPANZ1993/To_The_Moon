using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;



// TODO: DO WE WANT TO USE THE GAMECENTER / ANDROID LEADERBOARDS? IT'S NICE TO BE ABLE TO QUERY THE LEADERBOARD, WHICH YOU CAN DO WITH PLAYFAB


public class PlayFab_Manager : MonoBehaviour
{

    public static PlayFab_Manager instance;

    void Awake(){
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
        Login();
        //UpdateLeaderboardTest();
        //GetLeaderboardTest();
        //Invoke("GetUnixTimeServer", 15);
        GetTitleDataTest();
    }


    void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result){
        Debug.Log("PLAYFAB: Successful login/account create!");
    }

    void OnError(PlayFabError error){
        Debug.Log("PLAYFAB: Error while performing PlayFab function \n" + error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Highest Launch",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("PLAYFAB: Leaderboard Updated!");
    }

    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest {
            StatisticName = "Highest Launch",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }


    void OnLeaderboardGet(GetLeaderboardResult result){
        foreach (var item in result.Leaderboard){
            Debug.Log("PLAYFAB: " + item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }



    void GetUnixTimeServer(){
        var request = new ExecuteCloudScriptRequest {
            FunctionName = "GetServerTime"
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnError);
    }

    void OnExecuteSuccess(ExecuteCloudScriptResult result){
        if (result.FunctionResult != null){
            Debug.Log("PLAYFAB: GOT THIS FROM CLOUDSCRIPT RESULT: " + result.FunctionResult);
        }
        else{
            Debug.Log("PLAYFAB: GOT A NULL RESULT BACK FROM CLOUDSCRIPT");
        }
        //Debug.Log("PLAYFAB: GOT THIS FROM CLOUDSCRIPT " + result.FunctionResult.ToString());
    }


    // Remove
    void UpdateLeaderboardTest(){
        float time = 0;
        IEnumerator _LeaderboardTest(){
            yield return new WaitForSeconds(0);
            if(time < 15){
                time += Time.deltaTime;
                StartCoroutine(_LeaderboardTest());
            }
            else{
                SendLeaderboard(100);
            }
        }
        StartCoroutine(_LeaderboardTest());
    }

    // Remove
    void GetLeaderboardTest(){
        float time = 0;
        IEnumerator _LeaderboardTest(){
            yield return new WaitForSeconds(0);
            if(time < 15){
                time += Time.deltaTime;
                StartCoroutine(_LeaderboardTest());
            }
            else{
                GetLeaderboard();
            }
        }
        StartCoroutine(_LeaderboardTest());
    }



    void SaveData(){
        SaveGameObject s = new SaveGameObject();
        var request = new UpdateUserDataRequest {
            Data = new Dictionary<string, string>{
                {"Saved Game", JsonConvert.SerializeObject(s)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }


    void OnDataSend(UpdateUserDataResult result){
        Debug.Log("PLAYFAB: Sent data successfully!");
    }


    // void SaveDataTest(){
    //     float time = 0;
    //     IEnumerator _SaveDataTest(){
    //         yield return new WaitForSeconds(0);
    //         if(time < 15){
    //             time += Time.deltaTime;
    //             StartCoroutine(_SaveDataTest());
    //         }
    //         else{
    //             ;
    //         }
    //     }
    //     StartCoroutine(_SaveDataTest());
    // }

    void GetTitleData(){
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceieved, OnError);
    }


    void OnTitleDataReceieved(GetTitleDataResult result){
       if (result.Data == null || result.Data.ContainsKey("Message") == false){
           Debug.Log("PLAYFAB: NO MESSAGE!");
           return;
       }
       else{
           Debug.Log("PLAYFAB: GOT MESSAGE --- " + result.Data["Message"]);
       }
    }


    // Remove
    void GetTitleDataTest(){
        float time = 0;
        IEnumerator _GetTitleDataTest(){
            yield return new WaitForSeconds(0);
            if(time < 15){
                time += Time.deltaTime;
                StartCoroutine(_GetTitleDataTest());
            }
            else{
                GetTitleData();
            }
        }
        StartCoroutine(_GetTitleDataTest());
    }



}
