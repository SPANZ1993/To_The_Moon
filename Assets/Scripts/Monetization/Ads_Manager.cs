using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Ads_Manager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private enum Platforms{
        iOS,
        Android
    }

    #if UNITY_IOS
    private Platforms platform = Platforms.iOS;
    private string gameId = "4691388";

    private string bannerPlacementId = "Banner_iOS";

    #elif UNITY_ANDROID
    private Platforms platform = Platforms.Android;
    private int gameId = "4691389";

    private string bannerPlacementId = "Banner_Android";
    #endif

    public bool testMode = true; // TODO: Switch this to false for production



    private float timeSinceLastInterstialAd = 0.0f;
    [SerializeField]
    private float minTimeBetweenInterstitialAds = 60.0f;

    [SerializeField]
    private int maxNumberAdLoadRetries = 3;
    private int curBannerAdLoadRetries, curInterstitialAdLoadRetries, curRewardedAdLoadRetries = 0;


    // Tells other classes how a rewarded ad concluded
    // Completed
    // Skipped
    // Unknown
    public delegate void RewardedAdCompleted(UnityAdsShowCompletionState showCompletionState);
    public static event RewardedAdCompleted RewardedAdCompletedInfo;

    public delegate void AdLoadingSuccess(string adUnitId);
    public static event AdLoadingSuccess AdLoadingSuccessInfo;

    public delegate void AdLoadingError(string adUnitId);
    public static event AdLoadingError AdLoadingErrorInfo;

    public delegate void AdShowError(string adUnitId);
    public static event AdShowError AdShowErrorInfo;


    public static Ads_Manager instance;


    void Awake(){
        
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            timeSinceLastInterstialAd = minTimeBetweenInterstitialAds;
        }
        else{
            Destroy(this.gameObject);
        }

    }

    void OnEnable(){
        Debug.Log("SUBSCRIBING TO AD ACCEPTED");
        UI_Controller.AlertRewardedAdAcceptedInfo += onRewardedAdAccepted;
        UI_Controller.AlertRewardedAdRejectedInfo += onRewardedAdRejected;
    }

    void OnDisable(){
        UI_Controller.AlertRewardedAdAcceptedInfo -= onRewardedAdAccepted;
        UI_Controller.AlertRewardedAdRejectedInfo -= onRewardedAdRejected;
    }


    void OnLevelWasLoaded(){
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            showBannerAd();
            loadInterstitialAd();
        }
        else if (SceneManager.GetActiveScene().name == "Mine_Game"){
            showBannerAd();
            loadRewardedAd();
        }
        else if (SceneManager.GetActiveScene().name == "Rocket_Flight"){
            showBannerAd(pos:BannerPosition.TOP_CENTER);
            loadRewardedAd();
        }
    }

    void Start(){
        Advertisement.Initialize(gameId, testMode);
        OnLevelWasLoaded();
    }

    void Update(){
        timeSinceLastInterstialAd += Time.deltaTime;
    }





    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        Debug.Log("AD LOADED " + adUnitId);
        if (AdLoadingSuccessInfo != null){
            AdLoadingSuccessInfo(adUnitId);
        }
    }
 
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        if(adUnitId == "Banner_iOS" || adUnitId == "Banner_Android"){
            if(curBannerAdLoadRetries < maxNumberAdLoadRetries){
                // Load Banner Ad?
            }
            else{

            }
        }
        else if(adUnitId == "Interstitial_iOS" || adUnitId == "Interstitial_Android"){
            if(curInterstitialAdLoadRetries < maxNumberAdLoadRetries){
                curInterstitialAdLoadRetries++;
                loadInterstitialAd();
            }
            else{ // We've tried too many times so give up and alert
                curInterstitialAdLoadRetries = 0;
                if(AdLoadingErrorInfo != null){
                    AdLoadingErrorInfo(adUnitId);
                }
            }
        }
        else if(adUnitId == "Rewarded_iOS" || adUnitId == "Rewarded_Android"){
            if(curRewardedAdLoadRetries < maxNumberAdLoadRetries){
                curRewardedAdLoadRetries++;
                loadRewardedAd();
            }
            else{
                curRewardedAdLoadRetries = 0;
                if(AdLoadingErrorInfo != null){
                    AdLoadingErrorInfo(adUnitId);
                }
            }
        }
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        if (AdShowErrorInfo != null){
            AdShowErrorInfo(adUnitId);
        }
    }
 
    public void OnUnityAdsShowStart(string adUnitId) {
        Debug.Log("STARTED SHOWING " + adUnitId + " AD");
    }
    public void OnUnityAdsShowClick(string adUnitId) {
        Debug.Log("AD " + adUnitId + " CLICK");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        Debug.Log("AD " + adUnitId + " SHOW COMPLETE WITH STATE " + showCompletionState);
        if (adUnitId == "Interstitial_iOS" || adUnitId == "Interstitial_Android"){
            timeSinceLastInterstialAd = 0.0f;
        }
        if(adUnitId == "Rewarded_iOS" || adUnitId == "Rewarded_Android"){
            if(RewardedAdCompletedInfo != null){
                RewardedAdCompletedInfo(showCompletionState);
            }
        }
    }







 
    public void showBannerAd(BannerPosition pos=BannerPosition.BOTTOM_CENTER){
        Advertisement.Banner.SetPosition(pos);
        StartCoroutine(_showBannerAd());
    }

    IEnumerator _showBannerAd(){
        while(!Advertisement.isInitialized){
            yield return new WaitForSeconds(0);
        }
        Advertisement.Banner.Show(bannerPlacementId);
    }


    public void loadInterstitialAd()
    {
        string adUnitId = "";
        switch(platform){
            case Platforms.iOS:
            {
                adUnitId = "Interstitial_iOS";
                break;
            }
            case Platforms.Android:
            {
                adUnitId = "Interstitial_Android";
                break;
            }
            default: break;
        }

        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    public void showInterstitialAd(){

        string adUnitId = "";
        switch(platform){
            case Platforms.iOS:
            {
                adUnitId = "Interstitial_iOS";
                break;
            }
            case Platforms.Android:
            {
                adUnitId = "Interstitial_Android";
                break;
            }
            default: break;
        }

        if (adUnitId != "" && timeSinceLastInterstialAd >= minTimeBetweenInterstitialAds){
            Advertisement.Show(adUnitId, this);
        }
    }



    public void loadRewardedAd()
    {
        string adUnitId = "";
        switch(platform){
            case Platforms.iOS:
            {
                adUnitId = "Rewarded_iOS";
                break;
            }
            case Platforms.Android:
            {
                adUnitId = "Rewarded_Android";
                break;
            }
            default: break;
        }

        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(adUnitId, this);
    }


    public void showRewardedAd(){
        
        string adUnitId = "";
        switch(platform){
            case Platforms.iOS:
            {
                adUnitId = "Rewarded_iOS";
                break;
            }
            case Platforms.Android:
            {
                adUnitId = "Rewarded_Android";
                break;
            }
            default: break;
        }

        if (adUnitId != ""){
            Advertisement.Show(adUnitId, this);
        }
    }



    private void onRewardedAdAccepted(){
        Debug.Log("ACCEPTED REWARDED AD");
        showRewardedAd();
    }
    
    private void onRewardedAdRejected(){
        Debug.Log("REJECTED REWARDED AD");
    }


}
