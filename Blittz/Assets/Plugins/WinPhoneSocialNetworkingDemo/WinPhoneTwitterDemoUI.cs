using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using Prime31.WinPhoneSocialNetworking;


public class WinPhoneTwitterDemoUI : MonoBehaviourGUI
{
#if UNITY_WP8
	
	void Start()
	{
		// init must be called before making any Twitter calls!
		// replace with YOUR TWITTER CONSUMER KEY and YOUR TWITTER CONSUMER SECRET!!!
		var didLogin = TwitterAccess.init( "BusafRSHhgtCUQFTkbGMw", "1lzjyhkfO7KfVcmXIJHW5aPBZwrSiVPwrFHs9pUf2U" );
		log( "are we logged into Twitter after init: " + didLogin );
	}
	
	
	private void completionHandler( object obj, string error )
	{
		log( "Twitter request completed: " + obj );
		if( error != null )
			log( "error: " + error );
		
		if( obj != null )
		{
			// the result will be either an IDictionary or an IList depending on the API endpoint called
			if( obj is IList<object> )
			{
				var list = obj as IList<object>;
				log( "We have a List with member count: " + list.Count );
			}
			
			if( obj is IDictionary<string,object> )
			{
				var dict = obj as IDictionary<string,object>;
				log( "We have a Dictionary with key count: " + dict.Keys.Count );
			}
			
			// dump whatever object we got to the log
			Debug.Log( obj );
		}
	}

	
	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Login" ) )
		{
			TwitterAccess.login( didLogin =>
			{
				log( "did user login to Twitter: " + didLogin );
				
				// if login failed lastStatusMessage might contain some information on why (user cancelled, network error, etc)
				if( !didLogin )
					log( "failure reason: " + TwitterAccess.lastStatusMessage );
			});
		}

		
		if( GUILayout.Button( "Logout" ) )
		{
			TwitterAccess.logout();
		}
		
		
		if( GUILayout.Button( "Is Logged In?" ) )
		{
			log( "Twitter is logged in: " + TwitterAccess.isLoggedIn );
		}
		
		
		if( GUILayout.Button( "Get Screen Name" ) )
		{
			log( "Twitter screen name: " + TwitterAccess.screenName );
		}
		

		endColumn( true );
		
		if( GUILayout.Button( "Get Home Timeline" ) )
		{
			TwitterAccess.performGetRequest( "statuses/home_timeline", null, completionHandler );
		}
		
		
		if( GUILayout.Button( "Get Home Timeline (limited to 1 result)" ) )
		{
			var dict = new Dictionary<string,string>();
			dict.Add( "count", "1" );
			TwitterAccess.performGetRequest( "statuses/home_timeline", dict, completionHandler );
		}
		
		
		if( GUILayout.Button( "Post Status Update" ) )
		{
			var dict = new Dictionary<string,string>();
			dict.Add( "status", "I'm posting from a Unity Windows Phone app! The time: " + System.DateTime.Now );
			TwitterAccess.performPostRequest( "statuses/update", dict, completionHandler );
		}
		
		
		if( GUILayout.Button( "Post Status Update + Image" ) )
		{
			StartCoroutine( postStatusUpdateWithScreenshot() );
		}
		
		endColumn( false );
	}
	
	
	private IEnumerator postStatusUpdateWithScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
		// note that ReadPixels doesn't work perfectly on Windows Phone at the moment so you may want to acquire
		// your screenshots with Application.CaputureScreenshot
		var tex = new Texture2D( Screen.width, Screen.height );
		tex.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
		tex.Apply();
		var bytes = tex.EncodeToPNG();
		
		var status = "I'm posting from a Unity Windows Phone app! The time: " + System.DateTime.Now;
		TwitterAccess.postImage( status, bytes, completionHandler );
	}
	
	
	// Note that due to a Unity bug logs will not be displayed unless the DEBUG flag is added to the Platform Custom Defines
	// http://docs.unity3d.com/Documentation/Manual/PlatformDependentCompilation.html
	private void log( string log )
	{
		Debug.Log( log );
		System.Diagnostics.Debug.WriteLine( log );
	}
#endif
}
