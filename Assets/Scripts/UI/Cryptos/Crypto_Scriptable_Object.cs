using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Crypto", menuName = "ScriptableObjects/CryptoScriptableObject", order = 1)]
public class Crypto_Scriptable_Object : ScriptableObject
{

    public string CoinName { get { return coinName; } private set { coinName = value; } }
    public string CoinAbbrev { get { return coinAbbrev; } private set { coinAbbrev = value; } }
    public string CoinDescription { get { return coinDescription; } private set { coinDescription = value; } }
    public int CoinId { get { return coinId; } private set { coinId = value; } }

    public string FollowCoinName { get { return followCoinName; } private set { followCoinName = value; } }
    public string FollowCoinAbbrev { get { return followCoinAbbrev; } private set { followCoinAbbrev = value; } }
    
    [SerializeField]
    private string coinName;
    [SerializeField]
    private string coinAbbrev;
    [SerializeField]
    private string coinDescription;
    [SerializeField]
    private string followCoinName;
    [SerializeField]
    private string followCoinAbbrev;
    [SerializeField]
    private int coinId;


    public Image displayImage;



}