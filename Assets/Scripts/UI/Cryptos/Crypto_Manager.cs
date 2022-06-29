using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

using System;
using System.Linq;

public class Crypto_Manager : MonoBehaviour
{

    public static Crypto_Manager instance;

    string k = "66SCoHq98kAvsXhnMeA15LepS5i3kTA1";
    
    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            activeCryptosToPrice = new Dictionary<Crypto_Scriptable_Object, double>();
            activeCryptosToExchangePanel = new Dictionary<Crypto_Scriptable_Object, GameObject>();
            activeCryptosToBalance = new Dictionary<Crypto_Scriptable_Object, double>();
            activeCryptosToAveragePrice = new Dictionary<Crypto_Scriptable_Object, double>();
        }
        else{
            Destroy(this.gameObject);
        }
    }


    [SerializeField]
    private Crypto_Scriptable_Object[] activeCryptos;
    public Dictionary<Crypto_Scriptable_Object, double> activeCryptosToPrice {get; private set;}
    public Dictionary<Crypto_Scriptable_Object, GameObject> activeCryptosToExchangePanel {get; private set;}
    public Dictionary<Crypto_Scriptable_Object, double> activeCryptosToBalance {get; private set;}
    public Dictionary<Crypto_Scriptable_Object, double> activeCryptosToAveragePrice {get; private set;}
    private bool failedPriceGet = false;


    public void Update(){
        foreach(Crypto_Scriptable_Object c in activeCryptosToBalance.Keys){
            //Debug.Log(c.CoinName + " BAL - " + activeCryptosToBalance[c]);
        }
        foreach(Crypto_Scriptable_Object c in activeCryptosToAveragePrice.Keys){
            //Debug.Log(c.CoinName + " AVG - " + activeCryptosToAveragePrice[c]);
        }
    }



    string authenticate(string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }





    public void getPricesActiveCryptos(){
        string uri = Game_Manager.instance.cryptoServerIP + ":" + Game_Manager.instance.cryptoServerPort.ToString() + "/prices?";

        int symbolArgCount = 0;
        foreach(Crypto_Scriptable_Object crypto in activeCryptos){
            if(symbolArgCount != 0){
                uri += "&";
            }
            uri += "symbol=" + crypto.FollowCoinName;
            symbolArgCount++;
        }

        //Debug.Log("URI: " + uri);

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                string authorization = authenticate("User", k);
                webRequest.SetRequestHeader("AUTHORIZATION", authorization);

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        failedPriceGet = true;
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        failedPriceGet = true;
                        break;
                    case UnityWebRequest.Result.Success:
                        //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        Dictionary<string, double> recievedPrices = JsonConvert.DeserializeObject<Dictionary<string, double>>(webRequest.downloadHandler.text);
                        activeCryptosToPrice = new Dictionary<Crypto_Scriptable_Object, double>();
                        foreach(Crypto_Scriptable_Object curCrypto in activeCryptos){
                            if(recievedPrices.Keys.Contains(curCrypto.FollowCoinAbbrev)){
                                activeCryptosToPrice[curCrypto] = recievedPrices[curCrypto.FollowCoinAbbrev];
                            }
                            //Debug.Log("PRICE OF " + curCrypto.CoinName + " IS SET TO " + activeCryptosToPrice[curCrypto]);
                        }
                        break;
                }
            }
        }

        StartCoroutine(GetRequest(uri));
    }

    public void addPanelsToExchange(){


        // Remove all panels first in case we need to reorder
        foreach(Crypto_Scriptable_Object crypto in activeCryptos){
            if(activeCryptosToExchangePanel.Keys.Contains(crypto) && activeCryptosToExchangePanel[crypto] != null){
                activeCryptosToExchangePanel[crypto].transform.SetParent(null);
            }
        }


        foreach(Crypto_Scriptable_Object crypto in activeCryptosToPrice.Keys){
            if(activeCryptosToPrice[crypto] != null){
                if(!activeCryptosToExchangePanel.Keys.Contains(crypto) || activeCryptosToExchangePanel[crypto] == null){
                    Debug.Log("ADDING NEW PANEL: " + crypto.CoinName);
                    GameObject panel = UI_Controller.instance.addExchangePanel(crypto);
                    activeCryptosToExchangePanel[crypto] = panel;
                }
                else{
                    Debug.Log("ADDING OLD PANEL: " + crypto.CoinName);
                    GameObject panel = UI_Controller.instance.addExchangePanel(activeCryptosToExchangePanel[crypto]);
                    activeCryptosToExchangePanel[crypto] = panel;
                }
            }
        }
    }

    public void updateExchangePanel(GameObject panel){
        UI_Controller.instance.updateExchangePanel(panel);
    }

    public void updateAllExchangePanels(){
        foreach(GameObject panel in activeCryptosToExchangePanel.Values){
            updateExchangePanel(panel);
        }
    }

    public Crypto_Scriptable_Object getCoinById(int coinId){
        Crypto_Scriptable_Object crypto = Array.Find(activeCryptos, c => c.CoinId == coinId);
        return crypto;
    }


    public bool buyCoin(int coinId, double amount){
        Crypto_Scriptable_Object crypto = getCoinById(coinId);
        if(crypto != null){
            return buyCoin(crypto, amount);
        }
        return false;
    }
     
    public bool buyCoin(Crypto_Scriptable_Object coin, double amount){
        bool completedTransaction = false;
        bool alreadyOwnedCoin = true;
        if(!activeCryptosToBalance.Keys.Contains(coin)){
            activeCryptosToBalance[coin] = 0.0;
            alreadyOwnedCoin = false;
        }

        if(Game_Manager.instance.coins >= Math.Floor(activeCryptosToPrice[coin]*amount)){
            
            Game_Manager.instance.coins -= Math.Floor(activeCryptosToPrice[coin]*amount);
            completedTransaction = true;
            if(alreadyOwnedCoin){
                activeCryptosToAveragePrice[coin] = ((activeCryptosToAveragePrice[coin] * activeCryptosToBalance[coin]) + (amount * getCoinPrice(coin)))/(activeCryptosToBalance[coin] + amount);
            }
            else{
                activeCryptosToAveragePrice[coin] = getCoinPrice(coin);
            }
            activeCryptosToBalance[coin] += amount;
            Debug.Log("BOUGHT " + amount + " " + coin.CoinName + " FROM CRYPTO MANAGER");
        }
        return completedTransaction;
    }

    public bool sellCoin(int coinId, double amount){
        Crypto_Scriptable_Object crypto = getCoinById(coinId);
        if(crypto != null){
            return sellCoin(crypto, amount);
        }
        return false;
    }

    public bool sellCoin(Crypto_Scriptable_Object coin, double amount){
        bool completedTransaction = false;
        if(activeCryptosToBalance.Keys.Contains(coin) && activeCryptosToBalance[coin] >= amount){
            activeCryptosToBalance[coin] -= amount;
            Game_Manager.instance.coins += Math.Ceiling(activeCryptosToPrice[coin]*amount);
            if(activeCryptosToBalance[coin] <= 0.1){
                activeCryptosToBalance.Remove(coin);
                activeCryptosToAveragePrice.Remove(coin);
            }
            completedTransaction = true;
        }
        return completedTransaction;
    }

    public double getCoinPrice(int coinId){
        Crypto_Scriptable_Object crypto = getCoinById(coinId);
        return getCoinPrice(crypto);
    }

    public double getCoinPrice(Crypto_Scriptable_Object coin){
        return activeCryptosToPrice[coin];
    }
    
    public double getCoinBalance(int coinId){
        Crypto_Scriptable_Object crypto = getCoinById(coinId);
        return getCoinPrice(crypto);
    }

    public double getCoinBalance(Crypto_Scriptable_Object coin){
        return activeCryptosToBalance[coin];
    }

    public double getCoinAveragePrice(int coinId){
        Crypto_Scriptable_Object crypto = getCoinById(coinId);
        return getCoinAveragePrice(crypto);
    }

    public double getCoinAveragePrice(Crypto_Scriptable_Object coin){
        return activeCryptosToAveragePrice[coin];
    }

    private void initializeCryptoBalances(Dictionary<int, double> serializedBalanceDict){
        Crypto_Scriptable_Object curCrypto = null;
        foreach(int id in serializedBalanceDict.Keys){
            curCrypto = getCoinById(id);
            if(curCrypto != null){
                activeCryptosToBalance[curCrypto] = serializedBalanceDict[id];
            }
            curCrypto = null;
        }
    }

    public Dictionary<int, double> serializeCryptoBalances(){
        Dictionary<int, double> serializedBalanceDict = new Dictionary<int, double>();
        foreach(Crypto_Scriptable_Object curCrypto in activeCryptos){
            if(activeCryptosToBalance.Keys.Contains(curCrypto)){
                serializedBalanceDict[curCrypto.CoinId] = activeCryptosToBalance[curCrypto]; 
            }
        }
        return serializedBalanceDict;
    }






    private void initializeCryptoAveragePrices(Dictionary<int, double> serializedAveragePriceDict){
        Crypto_Scriptable_Object curCrypto = null;
        foreach(int id in serializedAveragePriceDict.Keys){
            curCrypto = getCoinById(id);
            if(curCrypto != null){
                activeCryptosToAveragePrice[curCrypto] = serializedAveragePriceDict[id];
            }
            curCrypto = null;
        }
    }

    public Dictionary<int, double> serializeCryptoAveragePrices(){
        Dictionary<int, double> serializedAveragePriceDict = new Dictionary<int, double>();
        foreach(Crypto_Scriptable_Object curCrypto in activeCryptos){
            if(activeCryptosToAveragePrice.Keys.Contains(curCrypto)){
                serializedAveragePriceDict[curCrypto.CoinId] = activeCryptosToAveragePrice[curCrypto]; 
            }
        }
        return serializedAveragePriceDict;
    }



    public void initializeCryptoBalanceAndAveragePrice(Dictionary<int, double> serializedBalanceDict, Dictionary<int, double> serializedAveragePriceDict){
        initializeCryptoBalances(serializedBalanceDict);
        initializeCryptoAveragePrices(serializedAveragePriceDict);

        Crypto_Scriptable_Object curCoin = null;
        foreach(int coinId in serializedBalanceDict.Keys){
            curCoin = getCoinById(coinId);
            if(curCoin != null && activeCryptosToBalance[curCoin] <= 0.1){
                activeCryptosToBalance.Remove(curCoin);
                activeCryptosToAveragePrice.Remove(curCoin);
            }
            curCoin = null;
        }
        curCoin = null;
        foreach(int coinId in serializedAveragePriceDict.Keys){
            curCoin = getCoinById(coinId);
            if(curCoin != null && !activeCryptosToBalance.Keys.Contains(curCoin)){
                activeCryptosToAveragePrice.Remove(curCoin);
            }
            curCoin = null;
        }
    }





}
