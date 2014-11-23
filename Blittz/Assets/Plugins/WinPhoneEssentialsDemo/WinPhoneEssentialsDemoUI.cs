using UnityEngine;
using System.Collections;
using Prime31;
using Prime31.WinPhoneEssentials;


public class WinPhoneEssentialsDemoUI : MonoBehaviourGUI
{
	public GameObject quad;

#if UNITY_WP8
	

	void Start()
	{
		Push.channelUriUpdatedEvent += uri =>
		{
			Debug.Log( "Push channel updated with new uri: " + uri );
		};
	}

	
	void OnGUI()
	{
		beginColumn();
		

		if( GUILayout.Button( "Update Application Live Tile (standard)" ) )
		{
			// first, create the tile data
			var tileData = new StandardTileData();
			tileData.backContent = "I'm on the back";
			tileData.backTitle = "BACK TITLE";
			tileData.title = "Live Tile Title";
			tileData.count = 12;

			// now update the tile
			Tiles.updateApplicationTile( tileData );
		}


		if( GUILayout.Button( "Create Live Tile (Flip)" ) )
		{
			// first, create the tile data
			var tileData = new FlipTileData();
			tileData.backContent = "Back of the Tile";
			tileData.backBackgroundImage = "http://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/Breastfeeding-icon-med.svg/202px-Breastfeeding-icon-med.svg.png";
			tileData.backTitle = "Back Title Here";
			tileData.backgroundImage = "http://cdn.memegenerator.net/instances/250x250/38333070.jpg";
			tileData.smallBackgroundImage = "Assets/Tiles/FlipCycleTileSmall.png";
			tileData.title = "Flip Tile Title";
			tileData.wideBackBackgroundImage = "http://blogs.msdn.com/cfs-filesystemfile.ashx/__key/communityserver-blogs-components-weblogfiles/00-00-00-91-03-metablogapi/5775.WideTileAfter_5F00_49305C14.jpg";
			tileData.wideBackContent = "Wide Back Content";
			tileData.wideBackgroundImage = "Assets/Tiles/FlipCycleTileLarge.png";
			tileData.count = 3;
			
			// now update the tile
			Tiles.createOrUpdateSecondaryLiveTile( "flippy-tile", tileData );
		}


		if( GUILayout.Button( "Create Live Tile (Iconic)" ) )
		{
			// first, create the tile data
			var tileData = new IconicTileData();
			tileData.iconImage = "http://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/Breastfeeding-icon-med.svg/202px-Breastfeeding-icon-med.svg.png";
			tileData.backgroundColor = Prime31.WinPhoneEssentials.Color.colorFromARGB( 255, 40, 255, 40 );
			tileData.smallIconImage = "http://cdn.memegenerator.net/instances/250x250/38333070.jpg";
			tileData.wideContent1 = "Wide content 1";
			tileData.wideContent2 = "Wide content 2";
			tileData.wideContent3 = "Wide content 3";
			tileData.title = "Live Tile Title";
			tileData.count = 3;

			// now update the tile
			Tiles.createOrUpdateSecondaryLiveTile( "my-tile", tileData );
		}


		if( GUILayout.Button( "Get Total Active Live Tiles" ) )
		{
			Debug.Log( "total live tiles: " + Tiles.totalActiveLiveTiles() );
		}

		
		if( GUILayout.Button( "Delete Live Tile" ) )
		{
			Tiles.deleteSecondaryLiveTile( "my-tile" );
		}
		
		
		if( GUILayout.Button( "Open Push Channel" ) )
		{
			Push.openChannel( "test-push-channel" );
		}
		
		
		if( GUILayout.Button( "Get App Launch Uri" ) )
		{
			Debug.Log( "app launch uri: " + Push.uriUsedToOpenApplication );
		}
		
		endColumn( true );


		if( GUILayout.Button( "Show SMS Composer" ) )
		{
			Sharing.showSMSComposer( "5558675309", "Howdy. Let's dine tonight" );
		}
		
		
		if( GUILayout.Button( "Show Link Share UI" ) )
		{
			Sharing.shareLink( "http://prime31.com", "prime[31] web page", "good stuff!" );
		}
		
		
		if( GUILayout.Button( "Show Email Composer" ) )
		{
			Sharing.showEmailComposer( "do it up tonight! " + Application.platform, "We gotta rock some multiplayer", null, null, null );
		}
		
		
		if( GUILayout.Button( "Prompt for Photo" ) )
		{
			Sharing.promptForPhoto( true, ( didComplete, photoBytes ) =>
			{
				Debug.Log( "photo chooser result: " + didComplete );

				if( didComplete )
				{
					Debug.Log( "photo chooser successful. image size: " + photoBytes.Length );
					// we process the image in a coroutine so that it happens on the proper Unity thread
					StartCoroutine( loadRawPhotoToTexture( photoBytes ) );
				}
			});
		}


		if( GUILayout.Button( "Show Share Status UI" ) )
		{
			Sharing.showShareStatusUI( "Going to the beach!" );
		}
		
		
		if( GUILayout.Button( "Show Marketplace Review" ) )
		{
			Sharing.showMarketplaceReviewPage();
		}
		
		
		if( GUILayout.Button( "Show Web Page" ) )
		{
			Sharing.showWebBrowser( "http://prime31.com" );
		}

		endColumn();
	}


	private IEnumerator loadRawPhotoToTexture( byte[] photoBytes )
	{
		yield return null;

		var tex = new Texture2D( 0, 0 );
		tex.LoadImage( photoBytes );
		quad.renderer.material.mainTexture = tex;
	}

#endif
}
