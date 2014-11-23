using UnityEngine;
using System.Collections;
using Prime31;
using Prime31.WinPhoneAdProxy;


public class WinPhoneAdsDemoUI : MonoBehaviourGUI
{
#if UNITY_WP8
	
	void Start()
	{
		// add events and log all data for illustration purposes
		WinPhoneAd.adIsEngagedChangedEvent += ( adUnitId, isEngaged ) =>
		{
			Debug.Log( adUnitId + " isEngaged: " + isEngaged );
		};
		
		WinPhoneAd.adErrorOccurredEvent += ( adUnitId, exc, code ) =>
		{
			Debug.Log( adUnitId + " adErrorOccurredEvent: " + exc.Message + ", code: " + code );	
		};
		
		WinPhoneAd.adRefreshedEvent += ( adUnitId ) =>
		{
			Debug.Log( adUnitId + " adRefreshed" );	
		};
		
		WinPhoneAd.adLoadedEvent += ( adUnitId ) =>
		{
			Debug.Log( adUnitId + " adLoadedEvent" );
		};
	}

	
	void OnGUI()
	{
		beginColumn();

		
		if( GUILayout.Button( "Display Ad Banner (top)" ) )
		{
			Debug.Log( "debug log from Unity. about to display ad on top" );
			var adConfig = new WinPhoneAdConfig()
			{
				adUnitId = "10139900",
				applicationId = "31feffc8-ae72-4d64-b558-dae39e9d3322",
				horizontalAlignment = WinPhoneAdHorizontalAlignment.Center,
				verticalAlignment = WinPhoneAdVerticalAlignment.Top,
				width = 480,
				height = 80
			};
			
			WinPhoneAd.createAdControl( adConfig );
		}
		
		
		if( GUILayout.Button( "Display Ad Banner (bottom with margin)" ) )
		{
			var adConfig = new WinPhoneAdConfig()
			{
				adUnitId = "10143447",
				applicationId = "31feffc8-ae72-4d64-b558-dae39e9d3322",
				horizontalAlignment = WinPhoneAdHorizontalAlignment.Center,
				verticalAlignment = WinPhoneAdVerticalAlignment.Bottom,
				width = 480,
				height = 80,
				margins = new RectOffset( 0, 0, 0, 50 )
			};
			
			WinPhoneAd.createAdControl( adConfig );
		}
		
		
		if( GUILayout.Button( "Destroy Ad Banner" ) )
		{
			WinPhoneAd.removeAd();
		}
		
		
		if( GUILayout.Button( "Refresh Ad Banner" ) )
		{
			WinPhoneAd.refresh();
		}

		
		endColumn();
	}

#endif
}
