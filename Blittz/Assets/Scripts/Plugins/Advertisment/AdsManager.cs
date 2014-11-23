using System;
using System.Configuration;
using UnityEngine;
#if UNITY_WP8
using Prime31.WinPhoneAdProxy;
using StoreWP8 = Prime31.WinPhoneStore.Store;
using GoogleAdmob = Prime31.WinPhoneAdMob;
#endif

public class AdsManager : MonoBehaviour
{  
    public static AdsManager Instance { get; private set; }
    
    public bool IsDisabled
    {
		get { return false; }
        private set
        {            
        }
    }

    private enum AdType
    {
        Admob,
        Adduplex,
        Microsoft,
        Max
    }

    private AdType _currentAdType;

    public bool IsShown { get; private set; }

    private int _count;

    private void Awake()
    {
        Instance = this;        
        _currentAdType = AdType.Adduplex;        
		
#if UNITY_WP8
		GoogleAdmob.WinPhoneAdMob.receivedAdEvent += () =>
		{
			if (!IsShown || _currentAdType != AdType.Admob)
			{
				GoogleAdmob.WinPhoneAdMob.removeBanner();
			}
		};

		GoogleAdmob.WinPhoneAdMob.interstitialReceivedAdEvent += () =>
		{
			if (!IsShown || _currentAdType != AdType.Admob)
			{
				GoogleAdmob.WinPhoneAdMob.removeBanner();
			}
		};
#endif
    }

    public void ShowBanner()
    {
		if (IsDisabled)
			return;

#if UNITY_WP8		
       	if (IsShown)
        {
            HideBanner();            
        }
        switch (_currentAdType)
        {
            case AdType.Admob:
                GoogleAdmob.WinPhoneAdMob.createBanner("ca-app-pub-7814749540601690/1541179769", GoogleAdmob.AdFormat.Banner,
                    GoogleAdmob.AdHorizontalAlignment.Center,
                    GoogleAdmob.AdVerticalAlignment.Bottom, false);
                break;

            case AdType.Adduplex:
                Interop.RaiseShowAdControl();
                break;

            case AdType.Microsoft:
				var adConfig = new WinPhoneAdConfig();
				adConfig.adUnitId = "Image480_80";
				adConfig.applicationId = "test_client";
				adConfig.horizontalAlignment = WinPhoneAdHorizontalAlignment.Center;
				adConfig.verticalAlignment = WinPhoneAdVerticalAlignment.Bottom;
				adConfig.width = 480;
				adConfig.height = 80;
				adConfig.margins = new RectOffset();
                WinPhoneAd.createAdControl(adConfig); 
                break;
        }
        IsShown = true;
#endif
    }

    public void HideBanner()
    {
		if (IsDisabled)
			return;

#if UNITY_WP8
        if (!IsShown)
            return;

        switch (_currentAdType)
        {
            case AdType.Admob:
                GoogleAdmob.WinPhoneAdMob.removeBanner();
                break;

            case AdType.Adduplex:
                Interop.RaiseHideAdControl();
                break;

            case AdType.Microsoft:
                WinPhoneAd.removeAd();
                break;
        }

        var iAddType = 1 + ((int)_currentAdType);
        if (iAddType == (int)AdType.Max)
            _currentAdType = AdType.Admob;
        else
            _currentAdType = (AdType)iAddType;
        IsShown = false;
#endif
    }
   
    public void RemoveAds()
    {
#if UNITY_WP8
        if (IsDisabled)
            return;

        StoreWP8.requestProductPurchase("Remove_Ads", (receipt, error) =>
        {
            if (receipt != null)
            {
                Debug.Log("purchase completed with receipt: " + receipt);
				HideBanner();
                IsDisabled = true;                
            }                
            else if (error != null)
            {
                Debug.Log("error purchasing product: " + error);
            }                
        });
#endif      
    }	   
}
