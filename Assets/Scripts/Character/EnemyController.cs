using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {
	[RequireComponent( typeof( Animator ) )]
	public class EnemyController : MonoBehaviour {

		protected SpriteRenderer m_SpriteRenderer;
		protected Animator m_Animator;

		protected readonly int m_DieHash = Animator.StringToHash( "Die" );
		protected readonly int m_DamagedHash = Animator.StringToHash( "Damaged" );

		// Use this for initialization
		void Start() {
			m_SpriteRenderer = GetComponent<SpriteRenderer>();
			m_Animator = GetComponent<Animator>();
			MsMeta.SceneLinkedSMB<EnemyController>.Initialise( m_Animator, this );

		}

		// Update is called once per frame
		void Update() { }

		public void Die() {
			m_Animator.SetTrigger( m_DieHash );
		}

		public void Damage() {
			m_Animator.SetTrigger( m_DamagedHash );

		}
	}
}