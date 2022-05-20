using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;



// TODO: DO WE WANT TO USE THE GAMECENTER / ANDROID LEADERBOARDS? IT'S NICE TO BE ABLE TO QUERY THE LEADERBOARD, WHICH YOU CAN DO WITH PLAYFAB


public class PlayFab_Manager : MonoBehaviour
{


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Stuff we want to keep

    public static PlayFab_Manager instance;


    SaveGameObject loadedGame;


    
    public delegate void PlayFabAccountCreateSuccess(); // We tried to login, couldn't find an existing user, then created a new account
    public static event PlayFabAccountCreateSuccess PlayFabAccountCreateSuccessInfo;

    public delegate void PlayFabLoginSuccess(); // We logged in successfully
    public static event PlayFabLoginSuccess PlayFabLoginSuccessInfo;

    public delegate void PlayFabLoginFailure(); // We failed to login for some reason
    public static event PlayFabLoginFailure PlayFabLoginFailureInfo;

    public delegate void PlayFabSaveDataSuccess(); // We successfully got t
    public static event PlayFabSaveDataSuccess PlayFabSaveDataSuccessInfo;
    
    public delegate void PlayFabSaveDataFailure(); // We failed to login for some reason
    public static event PlayFabSaveDataFailure PlayFabSaveDataFailureInfo;

    public delegate void PlayFabGetUnixTimeSuccess(double unixTime); // We succesfully fetched the server time
    public static event PlayFabGetUnixTimeSuccess PlayFabGetUnixTimeSuccessInfo;
    
    public delegate void PlayFabGetUnixTimeFailure(); // We failed to get the server time
    public static event PlayFabGetUnixTimeFailure PlayFabGetUnixTimeFailureInfo;

    public delegate void PlayFabGetSaveDataSuccess(SaveGameObject loadedData); // We succesfully fetched the saved data
    public static event PlayFabGetSaveDataSuccess PlayFabGetSaveDataSuccessInfo;
    
    public delegate void PlayFabGetSaveDataFailure(); // We failed to get the saved data
    public static event PlayFabGetSaveDataFailure PlayFabGetSaveDataFailureInfo;


    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }




    public void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result){
        //Debug.Log("PLAYFAB: Successful login/account create! ID: " + result.PlayFabId + " NEW ACCOUNT: " + result.NewlyCreated);
        if (result != null && result.PlayFabId != null && result.NewlyCreated != null){
            if(result.NewlyCreated == true && PlayFabAccountCreateSuccessInfo != null){
                PlayFabAccountCreateSuccessInfo();
            }
            else if(result.NewlyCreated == false && PlayFabLoginSuccessInfo != null){
                PlayFabLoginSuccessInfo();
            }
            else if (PlayFabLoginFailureInfo != null){ // Something is fricked.. Just treat it as a login error
                PlayFabLoginFailureInfo();
            }
        }
        else if (PlayFabLoginFailureInfo != null){
            PlayFabLoginFailureInfo();
        }
    }

    void OnLoginError(PlayFabError error){
        if (PlayFabLoginFailureInfo != null){
            PlayFabLoginFailureInfo();
        }
    }



    public void SaveData(SaveGameObject s){
        // SaveGameObject s = new SaveGameObject();
        var request = new UpdateUserDataRequest {
            Data = new Dictionary<string, string>{
                {"Saved Game", JsonConvert.SerializeObject(s)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveDataSend, OnError);
    }


    void OnSaveDataSend(UpdateUserDataResult result){
        //Debug.Log("PLAYFAB: Sent data successfully!");
        if (PlayFabSaveDataSuccessInfo != null){
            PlayFabSaveDataSuccessInfo();
        }
    }


    public void LoadData(){
        var request = new GetUserDataRequest {};
        PlayFabClientAPI.GetUserData(request, OnSaveDataRecieveSuccess, OnSaveDataRecieveError);
    }
    
    void OnSaveDataRecieveSuccess(GetUserDataResult result){
        //Debug.Log("GOT IT: " + result);
        if(result.Data != null && result.Data.ContainsKey("Saved Game") == true){
            //Debug.Log("GOT IN HERE: " + result.Data["Saved Game"].Value);
            SaveGameObject s = JsonConvert.DeserializeObject<SaveGameObject>(result.Data["Saved Game"].Value);
            //SaveGameObject s = new SaveGameObject();
            //Debug.Log("SGO COINS: " + s.Coins);
            if (PlayFabGetSaveDataSuccessInfo != null){
                PlayFabGetSaveDataSuccessInfo(s);
            }
        }
        else{
            if (PlayFabGetSaveDataFailureInfo != null){
                PlayFabGetSaveDataFailureInfo();
            }
        }
    }

    void OnSaveDataRecieveError(PlayFabError error){
        if (PlayFabGetSaveDataFailureInfo != null){
            PlayFabGetSaveDataFailureInfo();
        }
    }



    public void GetServerTime(){
        var request = new ExecuteCloudScriptRequest {
            FunctionName = "GetServerTime"
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnGetUnixTimeSuccess, OnGetUnixTimeFailure);
    }

    void OnGetUnixTimeSuccess(ExecuteCloudScriptResult result){
        //Debug.Log("PLAYFAB: GOT THIS FROM CLOUDSCRIPT " + result.FunctionResult.ToString() + " AND IT'S A " + result.FunctionResult.GetType());
        if (result.FunctionResult != null){
            if(PlayFabGetUnixTimeSuccessInfo != null){
                PlayFabGetUnixTimeSuccessInfo(System.Convert.ToDouble(result.FunctionResult));
            }
        }
        else{ // If we didn't get the time then just say it's a failure
            if(PlayFabGetUnixTimeFailureInfo != null){
                PlayFabGetUnixTimeFailureInfo();
            }
        }
    }

    void OnGetUnixTimeFailure(PlayFabError error){
        if(PlayFabGetUnixTimeFailureInfo != null){
            PlayFabGetUnixTimeFailureInfo();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Login();
        //UpdateLeaderboardTest();
        //GetLeaderboardTest();
        //Invoke("GetUnixTimeServer", 15);
        //GetTitleDataTest();
        Invoke("Login", 10);
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






    void OnError(PlayFabError error){
        //Debug.Log("PLAYFAB: Error while performing PlayFab function \n" + error.GenerateErrorReport());
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
       // Debug.Log("PLAYFAB: Leaderboard Updated!");
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
            //Debug.Log("PLAYFAB: " + item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
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



    // void SaveData(){
    //     SaveGameObject s = new SaveGameObject();
    //     var request = new UpdateUserDataRequest {
    //         Data = new Dictionary<string, string>{
    //             {"Saved Game", JsonConvert.SerializeObject(s)}
    //         }
    //     };
    //     PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    // }


    // void OnDataSend(UpdateUserDataResult result){
    //     Debug.Log("PLAYFAB: Sent data successfully!");
    // }


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
           //Debug.Log("PLAYFAB: NO MESSAGE!");
           return;
       }
       else{
           //Debug.Log("PLAYFAB: GOT MESSAGE --- " + result.Data["Message"]);
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
