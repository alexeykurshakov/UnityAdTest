// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Depreciated
/*
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.InputDevice)]
	[Tooltip("Sends an Event based on the Orientation of the mobile device.")]
	public class DetectDeviceOrientation : FsmStateAction
	{
		[Tooltip("Note: If device is physically situated between discrete positions, as when (for example) rotated diagonally, system will report Unknown orientation.")]
		public DeviceOrientation orientation;
		[Tooltip("The event to send if the device orientation matches Orientation.")]
		public FsmEvent sendEvent;
		[Tooltip("Repeat every frame. Useful if you want to wait for the orientation to be true.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			orientation = DeviceOrientation.Picture;
			sendEvent = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoDetectDeviceOrientation();
			
			if (!everyFrame)
				Finish();
		}
		

		public override void OnUpdate()
		{
			DoDetectDeviceOrientation();
		}
		
		void DoDetectDeviceOrientation()
		{
			if (Input.deviceOrientation == orientation)
				Fsm.Event(sendEvent);
		}
		
	}
}
*/