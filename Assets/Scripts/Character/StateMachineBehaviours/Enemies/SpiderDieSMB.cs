using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {
	public class SpiderDieSMB : SceneLinkedSMB<EnemyController> {

		public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			GameObject.Destroy( animatingObject.gameObject );
		}
	}
}