using UnityEngine;
using HutongGames.PlayMaker;
using Game;
using Misc;

namespace Game.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	public class NguiSetRectAlpha : FsmStateAction
	{
		[RequiredField] [CheckForComponent(typeof(UIRect))]
        [HutongGames.PlayMaker.Tooltip("The GameObject on which there is a UIRect")]
		public FsmOwnerDefault gameObject;
		
		[HutongGames.PlayMaker.Tooltip("The label")]
		[RequiredField] public FsmFloat alpha;
		
		[HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active. Useful to change the text over time")]
		public bool everyFrame;
		
		UIRect rect;
	
		public override void Reset()
		{
			gameObject = null;
			rect = null;
		}
		
		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				LogWarning("no InGameObject");
				Finish();
				return;
			}
			
			rect = go.GetComponent<UIRect>();
			if (rect == null)
			{
				LogWarning("no UIRect");
				Finish();
				return;
			}
			
			DoSetAlpha();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoSetAlpha();
		}
		
		void DoSetAlpha()
		{
			rect.alpha = alpha.Value;
		}
	}
}
