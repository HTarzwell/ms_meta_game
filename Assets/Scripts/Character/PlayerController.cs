using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {
	[RequireComponent( typeof( CharacterController ) )]
	[RequireComponent( typeof( Animator ) )]
	public class PlayerController : MonoBehaviour {

		// public float maxForwardSpeed = 8f;        // How fast Ellen can run.
		public float groundSpeed = 2f;
		public float gravity = 20f;               // How fast Ellen accelerates downwards when airborne.
		public float jumpSpeed = 10f;             // How fast Ellen takes off when jumping.
		public bool canAttack;                    // Whether or not Ellen can swing her staff.

		const float k_GroundedRayDistance = 1f;
		const float k_JumpAbortSpeed = 10f;
		const float k_MinEnemyDotCoeff = 0.2f;
		const float k_StickingGravityProportion = 0.3f;

		public bool IsGrounded { get; protected set; }
		public Vector3 Velocity { get; protected set; }

		public Damager punch;

		protected bool m_IsGrounded = true;            // Whether or not Ellen is currently standing on the ground.
		protected bool m_PreviouslyGrounded = true;    // Whether or not Ellen was standing on the ground last frame.
		protected bool m_ReadyToJump;                  // Whether or not the input state and Ellen are correct to allow jumping.
		protected float m_DesiredForwardSpeed;         // How fast Ellen aims be going along the ground based on input.
		protected float m_ForwardSpeed;                // How fast Ellen is currently going along the ground.
		protected float m_VerticalSpeed;               // How fast Ellen is currently moving up or down.
		protected PlayerInput m_Input;                 // Reference used to determine how Ellen should move.
		protected CharacterController m_CharCtrl;      // Reference used to actually move Ellen.
		protected Animator m_Animator;                 // Reference used to make decisions based on Ellen's current animation and to set parameters.
		protected SpriteRenderer m_SpriteRenderer;

		// Vector3 m_PreviousPosition;
		Vector3 m_NextMovement;
		Vector2[] m_RaycastPositions = new Vector2[3];

		bool startFlipped;

		//Animator Variables
		protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash( "HorizontalSpeed" );
		protected readonly int m_HashVerticalSpeedPara = Animator.StringToHash( "VerticalSpeed" );
		protected readonly int m_HashCrouchingPara = Animator.StringToHash( "Crouching" );
		protected readonly int m_HashMeleeAttackPara = Animator.StringToHash( "MeleeAttack" );

		void Awake() {
			m_Input = GetComponent<PlayerInput>();
			m_Animator = GetComponent<Animator>();
			m_SpriteRenderer = GetComponent<SpriteRenderer>();
			m_CharCtrl = GetComponent<CharacterController>();

			// meleeWeapon.SetOwner( gameObject );
			// s_Instance = this;
		}

		void Start() {
			SceneLinkedSMB<PlayerController>.Initialise( m_Animator, this );
			m_NextMovement = Vector3.zero;

			punch.disableDamageAfterHit = true;
			punch.DisableDamage();
			startFlipped = m_SpriteRenderer.flipX;
		}

		void FixedUpdate() {
			Velocity = m_CharCtrl.velocity;

			m_CharCtrl.Move( m_NextMovement );
			m_Animator.SetFloat( m_HashHorizontalSpeedPara, MsMeta.PlayerInput.Instance.Horizontal.Value );
			m_Animator.SetFloat( m_HashVerticalSpeedPara, MsMeta.PlayerInput.Instance.Vertical.Value );

			m_NextMovement = Vector3.zero;
		}

		// public void Move(Vector3 movement) {
		// 	m_NextMovement += movement;
		// }

		// public void Teleport(Vector3 position) {
		// Vector3 delta = position - m_CurrentPosition;
		// m_PreviousPosition += delta;
		// m_CurrentPosition = position;
		// m_Rigidbody2D.MovePosition( position );
		// }

		// =======================================
		// From 2D Player Character Script
		// =======================================

		// public void SetMoveVector(Vector2 newMoveVector) {
		// 	m_NextMovement = newMoveVector;
		// }

		public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f) {
			m_NextMovement.x = useInput ? MsMeta.PlayerInput.Instance.Horizontal.Value * groundSpeed * speedScale * Time.fixedDeltaTime : 0f;

		}

		public void GroundedDepthMovement(bool useInput, float speedScale = 1f) {
			m_NextMovement.z = useInput ? MsMeta.PlayerInput.Instance.Vertical.Value * groundSpeed * speedScale * Time.fixedDeltaTime : 0f;
		}

		public void CheckForCrouching() {
			m_Animator.SetBool( m_HashCrouchingPara, MsMeta.PlayerInput.Instance.Crouch.Held );
		}

		public bool CheckForMeleeAttackInput() {
			return MsMeta.PlayerInput.Instance.MeleeAttack.Down;
		}

		public void MeleeAttack() {
			m_Animator.SetTrigger( m_HashMeleeAttackPara );
		}

		public void EnableMeleeAttack() {
			punch.EnableDamage();
		}

		public void DisableMeleeAttack() {
			punch.DisableDamage();
		}

		public void UpdateFacing() {
			bool faceLeft = MsMeta.PlayerInput.Instance.Horizontal.Value < 0f;
			bool faceRight = MsMeta.PlayerInput.Instance.Horizontal.Value > 0f;

			if( faceLeft && !m_SpriteRenderer.flipX ) {
				m_SpriteRenderer.flipX = true;
				punch.transform.localPosition = new Vector3( 0 - punch.transform.localPosition.x, punch.transform.localPosition.y, punch.transform.localPosition.z );
			}
			else if( faceRight && m_SpriteRenderer.flipX ) {
				m_SpriteRenderer.flipX = false;
				punch.transform.localPosition = new Vector3( 0 - punch.transform.localPosition.x, punch.transform.localPosition.y, punch.transform.localPosition.z );
			}
		}
	}
}