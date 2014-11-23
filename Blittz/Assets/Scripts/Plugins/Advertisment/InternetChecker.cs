using System;
using UnityEngine;
using System.Collections;

namespace Misc
{
    public class InternetChecker : MonoBehaviour
    {
        public static InternetChecker Instance { get; private set; }

        public event Action ConnectionStatusChanged;	       

        public bool IsInternetAvailable { get; private set; }

		private void Awake()
		{
			Instance = this;
			IsInternetAvailable = true;
		}       		      
    }
}

