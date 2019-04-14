using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MsMeta {

	[RequireComponent( typeof( Rigidbody ) )]
	[RequireComponent( typeof( Collider ) )]
	public class PushableOnTrack : MonoBehaviour {

		public Vector3 targetDestination;
		public float speed = 1;
		public LayerMask interactingLayers;

		private bool m_enabled;

		[Range( 0.0f, 360.0f )]
		public float hitAngle = 360.0f;
		[Range( 0.0f, 360.0f )]
		public float hitForwardRotation = 360.0f;
		public UnityEngine.Events.UnityEvent onComplete;

		void Start() {
			GetComponent<Rigidbody>().isKinematic = true;
		}

		public void Enable() {
			m_enabled = true;
		}

		void Update() { }

		void OnTriggerStay(Collider col) {
			Debug.Log( "Colliding" );
			if( transform.position != targetDestination && m_enabled ) {
				Vector3 forward = transform.forward;
				forward = Quaternion.AngleAxis( hitForwardRotation, transform.up ) * forward;

				//we project the direction to damager to the plane formed by the direction of damage
				Vector3 positionToDamager = col.transform.position - transform.position;
				positionToDamager -= transform.up * Vector3.Dot( transform.up, positionToDamager );
				Debug.Log( "Cacl Angle" );


				if( Vector3.Angle( forward, positionToDamager ) > hitAngle * 0.5f )
					return;

				Debug.Log( "Move" );
				transform.position = Vector3.MoveTowards( transform.position, targetDestination, speed * Time.fixedDeltaTime );
				if( transform.position == targetDestination )
					onComplete.Invoke();
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected() {
			Vector3 forward = transform.forward;
			forward = Quaternion.AngleAxis( hitForwardRotation, transform.up ) * forward;

			if( Event.current.type == EventType.Repaint ) {
				UnityEditor.Handles.color = Color.blue;
				UnityEditor.Handles.ArrowHandleCap( 0, transform.position, Quaternion.LookRotation( forward ), 1.0f,
					EventType.Repaint );
			}


			UnityEditor.Handles.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			forward = Quaternion.AngleAxis( -hitAngle * 0.5f, transform.up ) * forward;
			UnityEditor.Handles.DrawSolidArc( transform.position, transform.up, forward, hitAngle, 1.0f );
		}
#endif
	}
}