using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Return the farthest GameObject within an arrayList from a transform or position which does not have a collider between itself and another GameObject")]
	public class ArrayListGetFarthestGameObjectInSight : ArrayListActions
	{
	
			[ActionSection("Set up")]
			
			[RequiredField]
			[Tooltip("The InGameObject with the PlayMaker ArrayList Proxy component")]
			[CheckForComponent(typeof(PlayMakerArrayListProxy))]
			public FsmOwnerDefault gameObject;
			
			[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
			public FsmString reference;
			
			[Tooltip("Compare the distance of the items in the list to the position of this InGameObject")]
			public FsmGameObject distanceFrom;
			
			[Tooltip("If DistanceFrom declared, use OrDistanceFromVector3 as an offset")]
			public FsmVector3 orDistanceFromVector3;
			
			public bool everyframe;
		
			[ActionSection("Raycast Settings")] 
			
			[Tooltip("The line start of the sweep.")]
			public FsmOwnerDefault fromGameObject;
	
			[UIHint(UIHint.Layer)]
			[Tooltip("Pick only from these layers.")]
			public FsmInt[] layerMask;
			
			[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
			public FsmBool invertMask;
			
			[ActionSection("Result")]
			
			[UIHint(UIHint.Variable)]
			public FsmGameObject farthestGameObject;
			
			[UIHint(UIHint.Variable)]
			public FsmInt farthestIndex;
			
			//private GameObject toGameObject = null;
			
			public override void Reset()
			{
			
				gameObject = null;
				reference = null;
				distanceFrom = null;
				orDistanceFromVector3 = null;
				farthestGameObject = null;
				farthestIndex = null;
				
				everyframe = true;
			
				fromGameObject = null;
			
				//toGameObject = null;
	
				
				
				layerMask = new FsmInt[0];
				invertMask = false;

			}
			
			
			public override void OnEnter()
			{
	
				if (! SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				{
					Finish();
				}
				
				DoFindFarthestGo();
				
				if (!everyframe)
				{
					Finish();
				}
				
			}
			
			public override void OnUpdate()
			{
				
				DoFindFarthestGo();
			}
			
			void DoFindFarthestGo()
			{
				
				if (! isProxyValid())
				{
					return;
				}
				
				Vector3 root = orDistanceFromVector3.Value;
				
				GameObject _rootGo = distanceFrom.Value;
				if (_rootGo!=null)
				{
					root += _rootGo.transform.position;
				}
				
				float sqrDist = Mathf.Infinity;
			
				int _index = 0;
				float sqrDistTest;
				foreach(GameObject _go in proxy.arrayList)
				{
					
					if (_go!=null && DoLineCast(_go)) 
					{
						sqrDistTest = (_go.transform.position - root).sqrMagnitude;
						if (sqrDistTest>= sqrDist)
						{
							sqrDist = sqrDistTest;
							farthestGameObject.Value = _go;
							farthestIndex.Value = _index;
						}
					}
					_index++;
				}
	
			}
		
			bool DoLineCast(GameObject toGameObject)
			{
				var go = Fsm.GetOwnerDefaultTarget(fromGameObject);
				
				Vector3 startPos = go.transform.position;
				Vector3 endPos =  toGameObject.transform.position;
				
				RaycastHit rhit;
				
				bool _hit = !Physics.Linecast(startPos,endPos,out rhit, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
				Fsm.RaycastHitInfo = rhit;
			
		
				return _hit;
			}
			
	}

}

