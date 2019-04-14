using UnityEditor;

[CustomEditor( typeof( DepthSpriteRenderer ) )]
public class DepthSpriteRendererEditor : Editor {

	void OnSceneGUI() {
		var obj = target as DepthSpriteRenderer;
		// obj.sprite.sortingOrder = (int)( obj.transform.position.z )
		// serializedObject.

		UnityEngine.SpriteRenderer sr = obj.GetComponent<UnityEngine.SpriteRenderer>();
		sr.sortingOrder = 30 - (int)( obj.transform.position.z );
		EditorUtility.SetDirty( sr );
	}
}
