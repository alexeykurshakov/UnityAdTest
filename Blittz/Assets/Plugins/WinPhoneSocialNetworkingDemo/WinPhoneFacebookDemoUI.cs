using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using Prime31.WinPhoneSocialNetworking;
using FacebookWp8 = SocialNetworking.Facebook;

public class WinPhoneFacebookDemoUI : MonoBehaviourGUI
{
#if UNITY_WP8
	
	// common event handler used for all graph requests from the Facebook class that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.Log( error );
		else
			Debug.Log( Json.encode( result ) );
	}
	
	
	void Start()
	{
		// the appId should be set before making any Facebook calls!
		// replace with YOUR FACEBOOK APP ID!!!
		FacebookAccess.applicationId = "360718324042173";
		
		// to use the Facebook class due to some WinPhone bugs you need to give it a GameObject and a
		// MonoBehaviour script to work with. This only needs to be done once as the GO will be persisted
		if( !GameObject.Find( "WinPhoneFacebook" ) )
		{
			var go = new GameObject( "WinPhoneFacebook" );
			var mb = go.AddComponent<MonoBehaviourGUI>();
            FacebookWp8.instance.prepareForMetroUse(go, mb);
		}
		
		// optionally enable debugging of requests
		//Facebook.instance.debugRequests = true;
	}
	
	
	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Login" ) )
		{
			// Facebook login will not work unless the redirectUrl used is exactly as it appears in the Facebook developer
			// website under the Website with Facebook Login Site URL field
			var appSecret = "4f6d3b16c08a858f36596906a9b269dc";
			var permissions = new string[] { "publish_actions", "publish_stream", "email" };
			FacebookAccess.login( "http://prime31.com/facebook", appSecret, permissions, ( accessToken, error ) =>
			{
				if( error != null )
					Debug.Log( "Error logging in: " + error );
				else
					Debug.Log( "Login successful. Access token: " + accessToken );
			});
		}

		
		if( GUILayout.Button( "Logout" ) )
		{
			FacebookAccess.logout();
		}
		
		
		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			var isLoggedIn = FacebookAccess.isSessionValid();
			Debug.Log( "Facebook is session valid: " + isLoggedIn );
		}
		
		
		if( GUILayout.Button( "Get Access Token" ) )
		{
			var token = FacebookAccess.accessToken;
			Debug.Log( "access token: " + token );
		}
		
		
		if( GUILayout.Button( "Show apprequests Dialog" ) )
		{
			// prepare the required dialog parameters (message is required)
			var parameters = new Dictionary<string,object>();
			parameters.Add( "message", "Hey, check this out!" );
			parameters.Add( "title", "My Neat Game" );

			FacebookAccess.showDialog( "apprequests", parameters, dialogResultUrl =>
			{
				Debug.Log( "dialog completed with url: " + dialogResultUrl );
			});
		}
		
		
		if( GUILayout.Button( "Show stream.publish Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
            var parameters = new Dictionary<string,object>
            {
                    { "link", "http://prime31.com" },
                    { "name", "link name goes here" },
                    { "picture", "http://prime31.com/assets/images/prime31logo.png" },
                    { "caption", "the caption for the image is here" }
            };

			FacebookAccess.showDialog( "stream.publish", parameters, dialogResultUrl =>
			{
				Debug.Log( "dialog completed with url: " + dialogResultUrl );
			});
		}

		endColumn( true );
		
		
		if( GUILayout.Button( "Post Image" ) )
		{
			StartCoroutine( postScreenshot() );
		}
		
		
		if( GUILayout.Button( "Graph Request (me)" ) )
		{
            FacebookWp8.instance.get("me", completionHandler);
		}
		
		
		if( GUILayout.Button( "Post Message" ) )
		{
            FacebookWp8.instance.postMessage("Hello from Windows 8", completionHandler);
		}
		
		
		if( GUILayout.Button( "Post Message & Extras" ) )
		{
			var parameters = new Dictionary<string,object>
			{
				{ "message", "link post from Unity Windows 8" },
				{ "link", "http://prime31.com" },
				{ "name", "prime[31]" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "prime[31] logo" }
			};
            FacebookWp8.instance.post("me/feed", parameters, completionHandler);
		}

		
		if( GUILayout.Button( "Get Scores" ) )
		{
            FacebookWp8.instance.getScores("me", completionHandler);
		}
		
		
		if( GUILayout.Button( "Post Score" ) )
		{
			// note that posting a score requires publish_actions permissions!
            FacebookWp8.instance.postScore(5899, didPost =>
			{
				Debug.Log( "score did post: " + didPost );
			});
		}
		
		
		endColumn();
	}
	
	
	private IEnumerator postScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
		// get screenshot
		var tex = new Texture2D( Screen.width, Screen.height );
		tex.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0, false );
		tex.Apply();
		var bytes = tex.EncodeToPNG();
		Destroy( tex );

        FacebookWp8.instance.postImage(bytes, "im an image posted from Metro", completionHandler);
	}
#endif
}
