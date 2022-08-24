using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Ads_Manager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    private bool initialized = false;

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
    private string gameId = "4691389";

    private string bannerPlacementId = "Banner_Android";
    #endif

    // private Platforms platform = Platforms.iOS;
    // private string gameId = "4691388";

    // private string bannerPlacementId = "Banner_iOS";


    public bool testMode = true; // TODO: Switch this to false for production
    private bool bannerAdShouldBeShowing = true; // We tried to show the banner ad, it is either showing or we are waiting for it to load



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



    public delegate void InterstitalAdShow();
    public static event InterstitalAdShow InterstitalAdShowInfo;

    public delegate void RewardedAdShow();
    public static event RewardedAdShow RewardedAdShowInfo;

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
        //Debug.Log("SUBSCRIBING TO AD ACCEPTED");
        UI_Controller.AlertRewardedAdAcceptedInfo += onRewardedAdAccepted;
        UI_Controller.AlertRewardedAdRejectedInfo += onRewardedAdRejected;
    }

    void OnDisable(){
        UI_Controller.AlertRewardedAdAcceptedInfo -= onRewardedAdAccepted;
        UI_Controller.AlertRewardedAdRejectedInfo -= onRewardedAdRejected;
    }


    void OnLevelWasLoaded(){
        if (SceneManager.GetActiveScene().name.StartsWith("Main_Area")){

            IEnumerator __showBannerAd(){
                yield return new WaitForSeconds(0.05f);
                showBannerAd();
            }

            StartCoroutine(__showBannerAd()); // Wait a couple frames so we can initialize some things
            loadInterstitialAd();
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Mine_Game")){
            //showBannerAd();
            hideBannerAd();
            loadRewardedAd();
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Rocket_Flight")){
            //showBannerAd(pos:BannerPosition.TOP_CENTER);
            hideBannerAd();
            loadRewardedAd();
        }
    }

    void Start(){
        if (platform == Platforms.iOS){
            //Debug.Log("IOS");
        }
        else if (platform == Platforms.Android){
            //Debug.Log("ANDROID");
        }
        Initialize();
        OnLevelWasLoaded();
    }

    void Update(){
        timeSinceLastInterstialAd += Time.deltaTime;
    }

    public void Initialize()
    {
        testMode = testMode || IsTestLab();
        Advertisement.Initialize(gameId, testMode, this);
    }

    void IUnityAdsInitializationListener.OnInitializationComplete()
    {
        initialized = true;
    }
    
    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        initialized = false;
    }



    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        //Debug.Log("AD LOADED " + adUnitId);
        if (AdLoadingSuccessInfo != null){
            AdLoadingSuccessInfo(adUnitId);
        }
    }
 
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        //Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
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
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        if (AdShowErrorInfo != null){
            AdShowErrorInfo(adUnitId);
        }
    }
 
    public void OnUnityAdsShowStart(string adUnitId) {
        //Debug.Log("STARTED SHOWING " + adUnitId + " AD");
    }
    public void OnUnityAdsShowClick(string adUnitId) {
        //Debug.Log("AD " + adUnitId + " CLICK");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        //Debug.Log("AD " + adUnitId + " SHOW COMPLETE WITH STATE " + showCompletionState);
        if (adUnitId == "Interstitial_iOS" || adUnitId == "Interstitial_Android"){
            timeSinceLastInterstialAd = 0.0f;
        }
        if(adUnitId == "Rewarded_iOS" || adUnitId == "Rewarded_Android"){
            if(RewardedAdCompletedInfo != null){
                RewardedAdCompletedInfo(showCompletionState);
            }
        }
    }


    // Is this a Google Firebase Test?? If so, we don't want real ads
    public bool IsTestLab()
    {   
        try{
            using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var context = actClass.GetStatic<AndroidJavaObject>("currentActivity");
                var systemGlobal = new AndroidJavaClass("android.provider.Settings$System");
                var testLab = systemGlobal.CallStatic<string>("getString", context.Call<AndroidJavaObject>("getContentResolver"), "firebase.test.lab");
                return testLab == "true";
            }
        }
        catch(System.Exception e){
            return false;
        }
    }



 
    public void showBannerAd(BannerPosition pos=BannerPosition.BOTTOM_CENTER){
        if(!IAP_Manager.instance.ownedNonConsumableProductsIds.Contains("com.eggkidgames.blockchainblastoff.unlockableremoveads") || !IAP_Manager.instance.initialized){
            //Debug.Log("SHOWING BANNER AD");
            // foreach(string id in IAP_Manager.instance.ownedNonConsumableProductsIds){
            //     Debug.Log("BANNER: " + id);
            // }
            //Advertisement.Banner.SetPosition(pos);
            StartCoroutine(_showBannerAd(pos));
        }
        else{
            //Debug.Log("NOT SHOWING BANNER AD");
            try{
                hideBannerAd();
            }
            catch(System.Exception e){
                Debug.Log("Couldn't hide banner ad");
            }
        }
    }

    IEnumerator _showBannerAd(BannerPosition pos){
        while(!Advertisement.isInitialized || !IAP_Manager.instance.initialized){
            yield return new WaitForSeconds(0);
        }
        if(!IAP_Manager.instance.ownedNonConsumableProductsIds.Contains("com.eggkidgames.blockchainblastoff.unlockableremoveads")){
            Advertisement.Banner.SetPosition(pos);
            Advertisement.Banner.Show(bannerPlacementId);
        }
        else{
            // If The IAP Manager Gets Initialized And We Realize We Have Ads Turned Off... 
            try{
                Debug.Log("OOPS WE HAD ADS TURNED OFF");
                hideBannerAd();
            }
            catch(System.Exception e){
                Debug.Log("Couldn't hide banner ad");
            }
        }
    }

    public void hideBannerAd(){
        //Debug.Log("HIDING BANNER AD");
        Advertisement.Banner.Hide();
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
        if(initialized){
            if(!IAP_Manager.instance.ownedNonConsumableProductsIds.Contains("com.eggkidgames.blockchainblastoff.unlockableremoveads")){
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
                    if(InterstitalAdShowInfo != null){
                        InterstitalAdShowInfo();
                    }
                    Advertisement.Show(adUnitId, this);
                }
            }
            else{
                // Just to make sure we hide the banner ad kind of quickly after we buy the upgrade
                try{
                    hideBannerAd();
                }
                catch(System.Exception e){
                    Debug.Log("Couldn't hide banner ad");
                }
            }
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
        //Debug.Log("ACCEPTED REWARDED AD");
        showRewardedAd();
    }
    
    private void onRewardedAdRejected(){
        //Debug.Log("REJECTED REWARDED AD");
    }


}
