using UnityEngine;
using System.Collections;
using Prime31;
using Prime31.WinPhoneAdMob;



public class WinPhoneAdMobDemoUI : MonoBehaviourGUI
{
#if UNITY_WP8
	
	void Start()
	{
		// add events and log all data for illustration purposes
		WinPhoneAdMob.receivedAdEvent += () =>
		{
			Debug.Log( "receivedAdEvent" );
		};
		
		WinPhoneAdMob.failedToReceiveAdEvent += ( error ) =>
		{
			Debug.Log( "failedToReceiveAdEvent: " + error );
		};
		
		WinPhoneAdMob.showingOverlayEvent += () =>
		{
			Debug.Log( "showingOverlayEvent" );
		};
		
		WinPhoneAdMob.dismissingOverlayEvent += () =>
		{
			Debug.Log( "dismissingOverlayEvent" );
		};
		
		WinPhoneAdMob.interstitialReceivedAdEvent += () =>
		{
			Debug.Log( "interstitialReceivedAdEvent" );
		};
		
		WinPhoneAdMob.interstitialFailedToReceiveAdEvent += ( error ) =>
		{
			Debug.Log( "interstitialFailedToReceiveAdEvent: " + error );
		};
	}

	
	void OnGUI()
	{
		beginColumn();

		
		if( GUILayout.Button( "Display Ad Banner (top)" ) )
		{
			// replace with your ad unit!!!!
			WinPhoneAdMob.createBanner( "ca-app-pub-8386987260001674/5936131941", AdFormat.Banner, AdHorizontalAlignment.Center, AdVerticalAlignment.Top, true );
		}
		
		
		if( GUILayout.Button( "Display Ad Banner (bottom)" ) )
		{
			// replace with your ad unit!!!!
			WinPhoneAdMob.createBanner( "ca-app-pub-8386987260001674/5936131941", AdFormat.Banner, AdHorizontalAlignment.Center, AdVerticalAlignment.Bottom, true );
		}
		
		
		if( GUILayout.Button( "Destroy Ad Banner" ) )
		{
			WinPhoneAdMob.removeBanner();
		}
		
		
		endColumn( true );
		
		
		if( GUILayout.Button( "Load Interstitial" ) )
		{
			// replace with your own interstitial ad unit!
			WinPhoneAdMob.loadInterstitial( "ca-app-pub-8386987260001674/8610396749", true );
		}
		
		
		if( GUILayout.Button( "Show Interstitial" ) )
		{
			WinPhoneAdMob.showInterstitial();
		}
		
		endColumn();
	}

#endif
}