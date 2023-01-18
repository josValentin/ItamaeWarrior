using UnityEngine;
using System.Collections;
using Yodo1.MAS;
using System;

public class Yodo1AdsManager : MonoBehaviour
{
    private static Yodo1AdsManager _Instance;


    [SerializeField] private bool enableYodo1 = true;

    private static bool _EnableYodo1;

    void Start()
    {
        _EnableYodo1 = enableYodo1;

        if (!_EnableYodo1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (_Instance != null)
        {
            Destroy(this.gameObject);

            // Show Ad with probability
            float probability = 0.15f;

            bool result = UnityEngine.Random.Range(0f, 1f) < probability;

            if (result)
            {
                Debug.Log("Show Intersitial Ad");
                ShowIntersitialAd();
            }

            //
            return;
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


        Yodo1AdBuildConfig config = new Yodo1AdBuildConfig().enableAdaptiveBanner(true).enableUserPrivacyDialog(true);
        Yodo1U3dMas.SetAdBuildConfig(config);
        SetPrivacy(true, false, false);
        SetDelegates();
        InitializeSdk();
        //Yodo1U3dMas.ShowBannerAd();
    }

    public static void ShowBanner_Top()
    {
        if (!_EnableYodo1)
            return;

        int align = Yodo1U3dBannerAlign.BannerTop | Yodo1U3dBannerAlign.BannerHorizontalCenter;
        Yodo1U3dMas.ShowBannerAd(align);
    }

    public static void ShowBanner_Bottom()
    {
        if (!_EnableYodo1)
            return;

        int align = Yodo1U3dBannerAlign.BannerBottom | Yodo1U3dBannerAlign.BannerHorizontalCenter;
        Yodo1U3dMas.ShowBannerAd(align);
    }

    public static void ShowRewardedAd()
    {
        if (!_EnableYodo1)
            return;

        Yodo1U3dMas.ShowRewardedAd();
    }

    public static void ShowIntersitialAd()
    {
        if (!_EnableYodo1)
            return;

        Yodo1U3dMas.ShowInterstitialAd();

    }



    private void SetPrivacy(bool gdpr, bool coppa, bool ccpa)
    {
        Yodo1U3dMas.SetGDPR(gdpr);
        Yodo1U3dMas.SetCOPPA(coppa);
        Yodo1U3dMas.SetCCPA(ccpa);
    }

    private void InitializeSdk()
    {

        Yodo1U3dMas.InitializeSdk();
    }

    private void SetDelegates()
    {
        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] InitializeDelegate, success:" + success + ", error: \n" + error.ToString());

            if (success)
            {

            }
            else
            {

            }
        });

        Yodo1U3dMas.SetBannerAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] BannerdDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Banner ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Banner ad has been shown.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Banner ad error, " + error.ToString());
                    break;
            }
        });

        Yodo1U3dMas.SetInterstitialAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] InterstitialAdDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been shown.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Interstital ad error, " + error.ToString());
                    break;
            }

        });

        Yodo1U3dMas.SetRewardedAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] RewardVideoDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Reward video ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Reward video ad has shown successful.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Reward video ad error, " + error);
                    break;
                case Yodo1U3dAdEvent.AdReward:
                    Debug.Log("[Yodo1 Mas] Reward video ad reward, give rewards to the player.");
                    PlayerPrefs.SetInt("TotalTickets", PlayerPrefs.GetInt("TotalTickets") + 85);
                    break;
            }

        });
    }

    public static void SetRewardAdDelegates(Action OnAdReward, Action OnAdClosed, Action OnAdError = null, Action OnAdOpened = null)
    {
        if (_Instance == null)
            return;

        Yodo1U3dMas.SetRewardedAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) =>
        {
            Debug.Log("[Yodo1 Mas] RewardVideoDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:

                    OnAdClosed?.Invoke();

                    break;
                case Yodo1U3dAdEvent.AdOpened:

                    OnAdOpened?.Invoke();

                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Reward video ad error, " + error);
                    OnAdError?.Invoke();
                    break;
                case Yodo1U3dAdEvent.AdReward:

                    OnAdReward?.Invoke();

                    break;
            }

        });
    }

}
