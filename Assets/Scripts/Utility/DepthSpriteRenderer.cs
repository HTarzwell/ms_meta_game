using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class DepthSpriteRenderer : MonoBehaviour {

	[SerializeField]
	[HideInInspector] public SpriteRenderer sprite;
	public int addedPriority;
	void Start() {
		sprite = GetComponent<SpriteRenderer>();
	}
	void Update() {
		sprite.sortingOrder = 30 - (int)( transform.position.z ) + addedPriority;
	}
}
