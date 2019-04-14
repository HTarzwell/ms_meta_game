using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {
	public class CrouchingSMB : SceneLinkedSMB<PlayerController> {

		public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			//animatingObject.UpdateFacing();
			animatingObject.GroundedHorizontalMovement( false );
			animatingObject.GroundedDepthMovement( false );
			animatingObject.CheckForCrouching();
			//animatingObject.CheckForGrounded();
			//animatingObject.CheckForPushing();
			//if( animatingObject.CheckForJumpInput() )
			//    animatingObject.SetVerticalMovement(animatingObject.jumpSpeed);
			//else if( animatingObject.CheckForMeleeAttackInput() )
			//    animatingObject.MeleeAttack();
		}
	}
}
