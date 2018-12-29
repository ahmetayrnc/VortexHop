using UnityEngine;
using UnityEngine.Advertisements;

using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

using Facebook.Unity;
using GameAnalyticsSDK;


public class AdsManager : MonoBehaviour {
    private BannerView banner;

    private void Start() {
        // Admob
        #if UNITY_ANDROID
            string appId = "";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-0285065574920552/5441037671";
        #endif

        MobileAds.Initialize(appId);

        // Admob Banner
        #if UNITY_ANDROID
            string adUnitId = "";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-0285065574920552~7711127730";
        #endif

        banner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest req = new AdRequest.Builder().Build();
        banner.LoadAd(req);

        // Unity Ads
        #if UNITY_ANDROID
            string gameId = "";
        #elif UNITY_IOS
            string gameId = "2768580";
        #endif

        if (Advertisement.isSupported) {
            Advertisement.Initialize(gameId);
        }
    }

    public static void ShowRewarded(System.Action reward) {
        if (Advertisement.IsReady("rewardedVideo")) {
            var options = new ShowOptions();
            options.resultCallback = (ShowResult result) => {
                if (result == ShowResult.Finished) {
                    // Revive Player
                    reward();
                }
            };

            Advertisement.Show("rewardedVideo", options);
            GameAnalytics.NewDesignEvent("Rewarded Showed");
            FB.LogAppEvent("Rewarded Showed");
        }
    }

    public static void ShowInterstitial() {

        if (Advertisement.IsReady("video")) {
            GameAnalytics.NewDesignEvent("Interstitial Showed");
            FB.LogAppEvent("Interstitial Showed");

            var options = new ShowOptions();
            options.resultCallback = (ShowResult result) => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
            Advertisement.Show("video", options);
        }

    }
}