using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MsMeta {

	[RequireComponent( typeof( Rigidbody ) )]
	[RequireComponent( typeof( Collider ) )]
	public class Damager : MonoBehaviour {
		public int damage = 1;
		public bool ignoreInvincibility = false;
		public bool disableDamageAfterHit = false;
		public LayerMask hittableLayers;
		public UnityEvent OnDamageableHit;

		protected bool m_CanDamage = true;

		private void Reset() {
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<Collider>().isTrigger = true;
		}

		private void OnTriggerStay(Collider other) {
			if( !m_CanDamage )
				return;

			var d = other.GetComponent<Damageable>();
			if( d == null )
				return;

			//Check if hit object is in layermask
			if( hittableLayers == ( hittableLayers | ( 1 << d.gameObject.layer ) ) ) {
				d.TakeDamage( this, ignoreInvincibility );
				OnDamageableHit.Invoke();

				if( disableDamageAfterHit )
					DisableDamage();
			}
		}
		public void EnableDamage() {
			m_CanDamage = true;
		}

		public void DisableDamage() {
			m_CanDamage = false;
		}
	}
}