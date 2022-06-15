using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

using System.Linq;

public class Crypto_Manager : MonoBehaviour
{

    public static Crypto_Manager instance;
    
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


    [SerializeField]
    private Crypto_Scriptable_Object[] activeCryptos;
    public Dictionary<Crypto_Scriptable_Object, double> activeCryptosToPrice {get; private set;}
    private bool failedPriceGet = false;


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

        Debug.Log("URI: " + uri);

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
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
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        Dictionary<string, double> recievedPrices = JsonConvert.DeserializeObject<Dictionary<string, double>>(webRequest.downloadHandler.text);
                        activeCryptosToPrice = new Dictionary<Crypto_Scriptable_Object, double>();
                        foreach(Crypto_Scriptable_Object curCrypto in activeCryptos){
                            if(recievedPrices.Keys.Contains(curCrypto.FollowCoinAbbrev)){
                                activeCryptosToPrice[curCrypto] = recievedPrices[curCrypto.FollowCoinAbbrev];
                            }
                            Debug.Log("PRICE OF " + curCrypto.CoinName + " IS SET TO " + activeCryptosToPrice[curCrypto]);
                        }
                        break;
                }
            }
        }

        StartCoroutine(GetRequest(uri));
    }
}
