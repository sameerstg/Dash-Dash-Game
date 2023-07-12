using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class AdsManager : MonoBehaviour
{
    public bool adclosed = false;
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adBannerUnitId = "ca-app-pub-6051045186783332/5852051039";
#elif UNITY_IPHONE
  private string _adBannerUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adBannerUnitId = "unused";
#endif

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adInterstitialUnitId = "ca-app-pub-6051045186783332/6514396017";
#elif UNITY_IPHONE
  private string _adInterstitialUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adInterstitialUnitId = "unused";
#endif
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adRewardUnitId = "ca-app-pub-6051045186783332/8342483004";
#elif UNITY_IPHONE
  private string _adRewardUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adRewardUnitId = "unused";
#endif

    private RewardedAd rewardedAd;
    BannerView bannerView;
    //string bannerID = "ca-app-pub-6051045186783332/4778223343";
    //public InterstitialAd interstitialAd;
    //string interstitialID = "ca-app-pub-6051045186783332/8548381694";

    //public object Advertisement { get; private set; }
    public static AdsManager _instance;
    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
        RequestInterstitial();
        RequestRewardAd();
    }

    private void RequestRewardAd()
    {
        LoadRewardedAd();
    }

    // Update is called once per frame
    void RequestBanner()
    {
        AdSize adaptiveSize =
               AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(_adBannerUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
    public void RequestInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(_adInterstitialUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
        //interstitialAd = new InterstitialAd();
        //AdRequest request = new AdRequest.Builder().Build();
        
        //interstitialAd.LoadAd(request);
        //adclosed = false;
        //interstitialAd.OnAdClosed += HandleOnAdClosed;
    }

    public void ShowInterstitialAd()
    {
        //adclosed = false;
        //interstitialAd.Show();
        //RequestInterstitial();
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(_adRewardUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                RegisterEventHandlers(ad);

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }
    public delegate void OnRewardDel();
    public OnRewardDel onRewardDel;
    private InterstitialAd interstitialAd;

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                onRewardDel?.Invoke();

            });
        }
    }
    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        adclosed = true;
        if (SceneManager.GetActiveScene().buildIndex < 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}