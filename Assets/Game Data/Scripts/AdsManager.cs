using System.Collections;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using JetBrains.Annotations;
using UnityEngine;
using Firebase;
using System;
using System.Threading;
public class AdsManager : MonoBehaviour
{
    public static Action rewardAction;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #region Remote config values
    public static bool RC_Banner = true;
    public static int RC_Inter_ad_delay=5;
    public static int RC_Inter_Ingame_popup = 60;
    public static int Inter_nothanks_popup = 0;
    public void Fetch_Inter_ad_delay(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_ad_delay = data.DefaultValue_Number;
    }
    public void Fetch_Inter_Ingame_popup(GVAnalysisManager.FirebaseRemoteData data)
    {
        RC_Inter_Ingame_popup = data.DefaultValue_Number;
    }
    public void Fetch_Inter_nothanks(GVAnalysisManager.FirebaseRemoteData data)
    {
        Inter_nothanks_popup = data.DefaultValue_Number;
    }
    public static bool canShowInterAd = true;
    // other Ads

    #endregion

    private void Start()
    {
        canShowInterAd = true;
    }
    public static void ShowBanner(string placement)
    {
        if (RC_Banner)
            AdController.instance?.ShowBannerAd(AdController.BannerAdTypes.BANNER, placement);
    }
    public static void HideBanner()
    {
        AdController.instance?.HideBannerAd(AdController.BannerAdTypes.BANNER);
    }
    public static bool IsInterstitialAvailable()
    {
        return AdController.instance != null && AdController.instance.IsInterstitialAdAvailable();
    }

    public static void StartAdDelayTimer()
    {
        AdsManager instance = FindObjectOfType<AdsManager>();
        if (instance != null)
        {
            instance.StartCoroutine(instance.AdDelayTimer());
        }
    }

    private IEnumerator AdDelayTimer()
    {
        canShowInterAd = false;
        yield return new WaitForSeconds(RC_Inter_ad_delay);
        canShowInterAd = true;
    }


    public static void ShowInterstitilAd(string placement)
    {
        Debug.Log("Called ShowInterstitilAd CanShowinterAd: " + canShowInterAd);
        if (canShowInterAd)
        {

            if (AdController.instance.IsInterstitialAdAvailable())
            {
                AdController.instance?.ShowAd(AdController.AdType.INTERSTITIAL, placement);
                StartAdDelayTimer();
            }
        }
    }

    public static bool IsRewardedAvailable()
    {
        return AdController.instance != null && AdController.instance.IsRewardedAdAvailable();
    }

    public static void ShowRewardedAd(Action _rewardAction, string placement)
    {
        rewardAction = null;
        rewardAction = _rewardAction;
        AdController.gaveRewardMethod += GiveReward;
        AdController.instance?.ShowAd(AdController.AdType.REWARDED, placement);
        //SendFirebaseEevents("Rewarded_ad_" + placement);
    }

    public static void GiveReward()
    {
        AdController.gaveRewardMethod -= GiveReward;
        rewardAction?.Invoke();
    }

    public static void ShowMrec(string placement)
    {
        if (AdController.instance != null)
            AdController.instance.ShowBannerAd(AdController.BannerAdTypes.NATIVE, placement);
    }
    public static void ShowMrec(string placement, AdController.BannerAdTypes size)
    {
        if (AdController.instance != null)
            AdController.instance.ShowBannerAd(size, placement);
    }

    public static void ChangeMrecPosition(MaxSdkBase.AdViewPosition NativeBannerPos)
    {
        if (AdController.instance != null)
        {
            // Implementation here
        }
    }
    public static void HideMrec()
    {
        if (AdController.instance != null)
            AdController.instance.HideBannerAd(AdController.BannerAdTypes.NATIVE);
    }

    public static void SendFirebaseEevents(string eventName)
    {
        if (GVAnalysisManager.instance != null)
        {
            GVAnalysisManager.instance.CustomEvent(eventName);
        }
    }
}
