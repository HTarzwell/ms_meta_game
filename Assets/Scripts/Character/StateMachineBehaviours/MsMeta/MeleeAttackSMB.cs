using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {

	public class MeleeAttackSMB : SceneLinkedSMB<PlayerController> {

		public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animatingObject.EnableMeleeAttack();
		}

		public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animatingObject.GroundedHorizontalMovement( false );
			animatingObject.GroundedDepthMovement( false );
		}

		public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animatingObject.DisableMeleeAttack();
		}
	}
}
