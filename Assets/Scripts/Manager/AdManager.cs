using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Newtonsoft.Json.Linq;

public class AdManager : MonoBehaviour
{
    // Àü¸é±¤°í
    private InterstitialAd interstitial = null;
    private string interstitial_adUnitID = "";

    // ¹è³Ê±¤°í
    private BannerView bannerView = null;
    private string banner_adUnitID = "";

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.

        });
        RequestBanner();
        LoadInterstitial();
    }
    void LoadInterstitial()
    {
#if UNITY_ANDROID
        interstitial_adUnitID = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        interstitial_adUnitID = "ca-app-pub-3940256099942544/4411468910";
#else
        interstitial_adUnitID = "unexpected_platform";
#endif

        if(interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        var adReuest = new AdRequest();
        InterstitialAd.Load(interstitial_adUnitID, adReuest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                    return;
                interstitial = ad;
            });

        interstitial.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitial();
        };
        interstitial.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadInterstitial();
        };
    }
    public void ShowInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
            interstitial.Show();
    }
    // Not Use
    void RegisterInterstitialEvent()
    {
        interstitial.OnAdPaid += (AdValue adValue) =>
        {
            
        };
    }
    // Banner
    void RequestBanner()
    {
#if UNITY_ANDROID
        banner_adUnitID = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        banner_adUnitID = "ca-app-pub-3940256099942544/2934735716";
#else
        banner_adUnitID = "unexpected_platform";
#endif

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //bannerView = new BannerView(adUnitID, adaptiveSize, AdPosition.Bottom);
        bannerView = new BannerView(banner_adUnitID, AdSize.Banner, AdPosition.Bottom);
        
        bannerView.OnBannerAdLoaded += HandleOnAdLoaded;
        bannerView.OnBannerAdLoadFailed += HandleOnAdLoadFailed;
        bannerView.OnAdClicked += HandleOnAdClicked;
        bannerView.OnAdPaid += HandleOnAdPaid;
        bannerView.OnAdImpressionRecorded += HandleOnAdImpressionRecorded;

        var adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }
    private void HandleOnAdLoaded()
    {
        Debug.Log("Banner view loaded an ad with response : " + bannerView.GetResponseInfo());
    }
    private void HandleOnAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : " + error);
    }
    private void HandleOnAdClicked()
    {
        Debug.Log("Banner view was clicked.");
    }
    private void HandleOnAdPaid(AdValue advalue)
    {
        Debug.Log(String.Format("Banner view paid {0} {1}.", advalue.Value, advalue.CurrencyCode));
    }
    private void HandleOnAdImpressionRecorded()
    {
        Debug.Log("Banner view recorded an impression.");
    }
}
