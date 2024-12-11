using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Newtonsoft.Json.Linq;

public class AdManager : MonoBehaviour
{
    private BannerView bannerView = null;
    private string adUnitID = "";

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.

        });
        RequestBanner();
    }

    void RequestBanner()
    {
#if UNITY_ANDROID
        adUnitID = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        adUnitID = "ca-app-pub-3940256099942544/2934735716";
#else
        adUnitID = "unexpected_platform";
#endif

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //bannerView = new BannerView(adUnitID, adaptiveSize, AdPosition.Bottom);
        bannerView = new BannerView(adUnitID, AdSize.Banner, AdPosition.Bottom);
        
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
