using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class MobAdsBanner : MonoBehaviour
{
    private BannerView bannerView;

#if UNITY_ANDROID
    private const string bannerUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private const string bannerUnitId = "";
#else
    private const string bannerUnitId = "unexpected platform";
#endif


    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(bannerUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (bannerView == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
    }

    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
