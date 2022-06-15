using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }
            }
        }

        StartCoroutine(GetRequest(uri));
    }
}
