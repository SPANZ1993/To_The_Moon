using System;
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

    public delegate void PlayFabLoginSuccess(LoginResult result); // We logged in successfully
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


    public delegate void PlayFabGetAccountInfoSuccess(); // We got data for the user (by name)
    public static event PlayFabGetAccountInfoSuccess PlayFabGetAccountInfoSuccessInfo;

    public delegate void PlayFabGetAccountInfoFailure(); // We failed to get data for the user (by name)
    public static event PlayFabGetAccountInfoFailure PlayFabGetAccountInfoFailureInfo;

    public delegate void PlayFabSetDisplayNameSuccess(UpdateUserTitleDisplayNameResult result); // We set the user's display name
    public static event PlayFabSetDisplayNameSuccess PlayFabSetDisplayNameSuccessInfo;

    public delegate void PlayFabSetDisplayNameFailure(); // We failed set the user's display name
    public static event PlayFabSetDisplayNameFailure PlayFabSetDisplayNameFailureInfo;


    public delegate void PlayFabGetTitleDataSuccess(Dictionary<string, string> titleData); // We got the title data
    public static event PlayFabGetTitleDataSuccess PlayFabGetTitleDataSuccessInfo;

    public delegate void PlayFabGetTitleDataFailure(); // We failed to get the title data
    public static event PlayFabGetTitleDataFailure PlayFabGetTitleDataFailureInfo;

    public delegate void PlayFabGetLeaderboardSuccess(Dictionary<int, Dictionary<string, string>> leaderboardData);
    public static event PlayFabGetLeaderboardSuccess PlayFabGetLeaderboardSuccessInfo;

    public delegate void PlayFabGetLeaderboardFailure();
    public static event PlayFabGetLeaderboardFailure PlayFabGetLeaderboardFailureInfo;


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
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    void OnLoginSuccess(LoginResult result){
        //Debug.Log("LOGGED IN");
        //Debug.Log("PLAYFAB: Successful login/account create! ID: " + result.PlayFabId + " NEW ACCOUNT: " + result.NewlyCreated);
        if (result != null && result.PlayFabId != null && result.NewlyCreated != null){
            if(result.NewlyCreated){
                //Debug.Log("CREATED AN ACCOUNT");
            }
            if(result.NewlyCreated == true && PlayFabAccountCreateSuccessInfo != null){
                PlayFabAccountCreateSuccessInfo();
            }
            else if(result.NewlyCreated == false && PlayFabLoginSuccessInfo != null){
                PlayFabLoginSuccessInfo(result);
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
        //Debug.Log("ERROR LOGGING IN");
        if (PlayFabLoginFailureInfo != null){
            PlayFabLoginFailureInfo();
        }
    }



    public void SetDisplayName(string userDisplayName){
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = userDisplayName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSetDisplayNameSuccess, OnSetDisplayNameError);
    }

    public void OnSetDisplayNameSuccess(UpdateUserTitleDisplayNameResult result){
        //Debug.Log("SET DISPLAY NAME!!");
        if (PlayFabSetDisplayNameSuccessInfo != null){
            PlayFabSetDisplayNameSuccessInfo(result);
        }
    }

    public void OnSetDisplayNameError(PlayFabError error){
        //Debug.Log("SET DISPLAY NAME ERRROR!!");
        if (PlayFabSetDisplayNameFailureInfo != null){
            PlayFabSetDisplayNameFailureInfo();
        }
    }










    public void SaveData(SaveGameObject s){
        if(Game_Manager.instance.userDisplayName != null){
            var request = new UpdateUserDataRequest {
                Data = new Dictionary<string, string>{
                    // The "Saved Game" data is the single source of truth
                    {"Saved Game", JsonConvert.SerializeObject(s)},
                    // The Stuff Below is Purely For Leaderboards... You shouldn't ever
                    // Have to Change these manually in PlayFab
                    {"User Display Name", Game_Manager.instance.userDisplayName},
                    {"Coin Name", s.CoinName},
                    {"Is Patron", s.IsPatron.ToString()},
                    {"Max Coins All Time", s.Metrics.maxCoinsAlltime.ToString()},
                    {"Max Altitude All Time", s.Metrics.maxAltAllTime.ToString()}
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnSaveDataSend, OnError);
        
        
            // Send Leaderboard Stuff
            // Leaderboard has to be integers... So we are going to do everything in intervals of 1000
            // So a 200 on the leaderboard translates to a value of 200,000
            Int32 maxLeaderBoardVal = Int32.MaxValue;
            Int32 leaderboardCoins = Convert.ToInt32(0);
            if(s.Coins/1000.0 >= maxLeaderBoardVal){
                leaderboardCoins = Int32.MaxValue;
            }
            else{
                leaderboardCoins = Convert.ToInt32(Math.Floor(s.Coins/1000.0));
            }
            SendLeaderboard("Most Coins", leaderboardCoins);


            Int32 leaderboardAlt = Convert.ToInt32(0);
            if(s.Metrics.maxAltAllTime/1000.0 >= maxLeaderBoardVal){
                leaderboardAlt = Int32.MaxValue;
            }
            else{
                leaderboardAlt = Convert.ToInt32(Math.Floor(s.Metrics.maxAltAllTime/1000.0));
            }
            SendLeaderboard("Highest Launch", leaderboardAlt);


        }
        else{
            Debug.LogError("Skipped saving because we don't have a name yet");
        }
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
        //Invoke("Login", 10);
        //GetLeaderboardTest();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






    void OnError(PlayFabError error){
        Debug.LogError("PLAYFAB: Error while performing PlayFab function: " + error.GenerateErrorReport());
    }

    public void SendLeaderboard(string statisticName, Int32 value){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    //StatisticName = "Highest Launch",
                    StatisticName = statisticName,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
       // Debug.Log("PLAYFAB: Leaderboard Updated!");
    }

    public void GetLeaderboard(string statisticName, int startPosition=0, int maxCount=10){
        var request = new GetLeaderboardRequest {
            StatisticName = statisticName,
            StartPosition = startPosition,
            MaxResultsCount = maxCount
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnLeaderboardGetError);
    }

    void OnLeaderboardGetError(PlayFabError error){
        //Debug.Log("PLAYFAB: Failed to get leaderboard");
        if (PlayFabGetLeaderboardFailureInfo != null){
            PlayFabGetLeaderboardFailureInfo();
        }
    }


    void OnLeaderboardGet(GetLeaderboardResult result){
        Dictionary<int, Dictionary<string, string>> LeaderBoardDataDict = new Dictionary<int, Dictionary<string, string>>();
        foreach (var item in result.Leaderboard){
            //Debug.Log("PLAYFAB: " + item.Position +  " " + item.DisplayName + " " + item.PlayFabId + " " + item.StatValue);
            LeaderBoardDataDict[item.Position] = new Dictionary<string, string>(){ {"DisplayName", item.DisplayName}, {"PlayFabId", item.PlayFabId.ToString()}, {"StatValue", item.StatValue.ToString()}};
        }
        if(PlayFabGetLeaderboardSuccessInfo != null){
            PlayFabGetLeaderboardSuccessInfo(LeaderBoardDataDict);
        }
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result){
        Dictionary<int, Dictionary<string, string>> LeaderBoardDataDict = new Dictionary<int, Dictionary<string, string>>();
        foreach (var item in result.Leaderboard){
            //Debug.Log("PLAYFAB: " + item.Position +  " " + item.DisplayName + " " + item.PlayFabId + " " + item.StatValue);
            LeaderBoardDataDict[item.Position] = new Dictionary<string, string>(){ {"DisplayName", item.DisplayName}, {"PlayFabId", item.PlayFabId.ToString()}, {"StatValue", item.StatValue.ToString()}};
        }
        if(PlayFabGetLeaderboardSuccessInfo != null){
            PlayFabGetLeaderboardSuccessInfo(LeaderBoardDataDict);
        }
    }


    public void GetLeaderboardAroundPlayer(string statisticName, int maxCount=10){
        var request = new GetLeaderboardAroundPlayerRequest {
            StatisticName = statisticName,
            MaxResultsCount = maxCount
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnLeaderboardGetError);
    }


    public void GetAccountInfo(string playerDisplayName){
        var request = new GetAccountInfoRequest {
            TitleDisplayName = playerDisplayName
        };
        PlayFabClientAPI.GetAccountInfo(request, OnAccountInfoGet, OnAccountInfoGetError);
    }

    void OnAccountInfoGet(GetAccountInfoResult result){
        if(PlayFabGetAccountInfoSuccessInfo != null){
            PlayFabGetAccountInfoSuccessInfo();
        }
    }

    void OnAccountInfoGetError(PlayFabError error){
        if(PlayFabGetAccountInfoFailureInfo != null){
            PlayFabGetAccountInfoFailureInfo();
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
                SendLeaderboard("Beans", 100);
            }
        }
        StartCoroutine(_LeaderboardTest());
    }

    // Remove
    void GetLeaderboardTest(){
        IEnumerator _LeaderboardTest(){
            yield return new WaitForSeconds(45);
            GetLeaderboard("Highest Launch", 0, 10);
            GetLeaderboard("Most Coins", 0, 10);
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

    public void GetTitleData(){
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceieved, onGetTitleDataError);
    }


    void OnTitleDataReceieved(GetTitleDataResult result){
       if (result.Data != null){
           //Debug.Log("PLAYFAB: GOT TITLE DATA!");
           //return;
       }
       else{
            //Debug.Log("PLAYFAB: NO TITLE DATA!");
            result.Data = new Dictionary<string, string>();
       }

       if(PlayFabGetTitleDataSuccessInfo != null){
           PlayFabGetTitleDataSuccessInfo(result.Data);
       }
    }

    void onGetTitleDataError(PlayFabError error){
        if(PlayFabGetTitleDataFailureInfo != null){
            PlayFabGetTitleDataFailureInfo();
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
