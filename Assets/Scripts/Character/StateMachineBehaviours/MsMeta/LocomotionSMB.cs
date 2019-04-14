using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {

	public class LocomotionSMB : SceneLinkedSMB<PlayerController> {

		public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			animatingObject.UpdateFacing();
			animatingObject.GroundedHorizontalMovement( true );
			animatingObject.GroundedDepthMovement( true );
			animatingObject.CheckForCrouching();
			//animatingObject.CheckForGrounded();
			//animatingObject.CheckForPushing();
			//if( animatingObject.CheckForJumpInput() )
			//    animatingObject.SetVerticalMovement(animatingObject.jumpSpeed);
			if( animatingObject.CheckForMeleeAttackInput() )
				animatingObject.MeleeAttack();
		}
	}
}