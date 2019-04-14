using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MsMeta {

	public class Damageable : MonoBehaviour {
		// Start is called before the first frame update
		public int maxHitPoints = 5;
		public bool invulnerableAfterDamage = true;
		public float invulnerabilityDuration = 3f;

		protected bool m_Invulnerable;
		protected float m_InulnerabilityTimer;
		protected int m_CurrentHealth;
		public int CurrentHealth { get { return m_CurrentHealth; } }
		protected Collider m_Collider;

		[Tooltip( "The angle from the which that damageable is hitable. Always in the world XZ plane, with the forward being rotate by hitForwardRoation" )]
		[Range( 0.0f, 360.0f )]
		public float hitAngle = 360.0f;
		[Tooltip( "Allow to rotate the world forward vector of the damageable used to define the hitAngle zone" )]
		[Range( 0.0f, 360.0f )]
		public float hitForwardRotation = 360.0f;

		public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvulnerable, OnBecomeVulnerable, OnGainHealth, OnHealthSet;

		System.Action schedule;

		void Start() {
			m_CurrentHealth = maxHitPoints;
			OnHealthSet.Invoke();
			DisableInvulnerability();
		}
		void Update() {
			if( m_Invulnerable ) {
				m_InulnerabilityTimer -= Time.deltaTime;

				if( m_InulnerabilityTimer <= 0f ) {
					m_Invulnerable = false;
					OnBecomeVulnerable.Invoke();
				}
			}
		}

		public void TakeDamage(Damager damager, bool ignoreInvincible = false) {
			//ignore damage if already dead. TODO : may have to change that if we want to detect hit on death...
			if( ( m_Invulnerable && !ignoreInvincible ) || m_CurrentHealth <= 0 )
				return;

			if( m_Invulnerable && !ignoreInvincible ) {
				OnHitWhileInvulnerable.Invoke();
				return;
			}

			Vector3 forward = transform.forward;
			forward = Quaternion.AngleAxis( hitForwardRotation, transform.up ) * forward;

			//we project the direction to damager to the plane formed by the direction of damage
			Vector3 positionToDamager = damager.transform.position - transform.position;
			positionToDamager -= transform.up * Vector3.Dot( transform.up, positionToDamager );

			if( Vector3.Angle( forward, positionToDamager ) > hitAngle * 0.5f )
				return;

			m_Invulnerable = true;
			m_CurrentHealth -= damager.damage;

			if( m_CurrentHealth <= 0 )
				schedule += OnDeath.Invoke;
			else {
				OnReceiveDamage.Invoke();
				if( invulnerableAfterDamage )
					EnableInvulnerability();
			}
		}

		void LateUpdate() {
			if( schedule != null ) {
				schedule();
				schedule = null;
			}
		}

		public void GainHealth(int amount) {
			m_CurrentHealth += amount;

			if( m_CurrentHealth > maxHitPoints )
				m_CurrentHealth = maxHitPoints;

			OnHealthSet.Invoke();
			OnGainHealth.Invoke();
		}

		public void SetHealth(int amount) {
			m_CurrentHealth = amount;

			OnHealthSet.Invoke();
		}

		public void EnableInvulnerability(bool ignoreTimer = false) {
			m_Invulnerable = true;
			//technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
			m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
		}

		public void DisableInvulnerability() {
			m_Invulnerable = false;
		}
	}
}