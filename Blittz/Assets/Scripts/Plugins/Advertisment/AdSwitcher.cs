using UnityEngine;
using System.Collections;

public class AdSwitcher : MonoBehaviour 
{
	private void OnGUI()
	{
		if (GUILayout.Button (AdsManager.Instance.IsShown ? "Hide" : "Show"))
		{
			if (AdsManager.Instance.IsShown)
			{
				AdsManager.Instance.HideBanner();
			}
			else
			{
				AdsManager.Instance.ShowBanner();
			}
		}
	}
}
