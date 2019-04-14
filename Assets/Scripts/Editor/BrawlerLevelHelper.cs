using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor.AnimatedValues;


[CustomEditor(typeof(BrawlerLevelHelper))]
public class BrawlerLevelHelperEditor : Editor {
    static Color lineColor = Color.red;
    static float lineWidth = 100f;
    static int numberLayers = 10;

    static bool editorOptionsVisible = false;

    SerializedProperty layerStepSizeProp;
    SerializedProperty yOffsetProp;
    //SerializedProperty scalingEnabledProp;
    //SerializedProperty scaleProp;

    void OnEnable() {
        layerStepSizeProp = serializedObject.FindProperty("layerStepSize");
        yOffsetProp = serializedObject.FindProperty("yOffset");
        //scalingEnabledProp = serializedObject.FindProperty("scalingEnabled");
        //scaleProp = serializedObject.FindProperty("scale");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(layerStepSizeProp);
        EditorGUILayout.PropertyField(yOffsetProp);

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.PropertyField(scalingEnabledProp);
        //if( scalingEnabledProp.boolValue )
        //    EditorGUILayout.PropertyField(scaleProp);
        //EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        editorOptionsVisible = EditorGUILayout.Foldout(editorOptionsVisible, "Editor Options");
        if( editorOptionsVisible ) {
            EditorGUI.indentLevel++;
            if( numberLayers != ( numberLayers = EditorGUILayout.IntField("Visible Number of Layers", numberLayers) ) ) {
                EditorUtility.SetDirty(target);
            }
            if( lineWidth != ( lineWidth = EditorGUILayout.FloatField("Line Width", lineWidth) ) ) {
                EditorUtility.SetDirty(target);
            }
            if( lineColor != ( lineColor = EditorGUILayout.ColorField("Color", lineColor) ) ) {
                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI() {
        BrawlerLevelHelper t = target as BrawlerLevelHelper;

        if( !t.enabled )
            return;


        Vector3 start = Vector3.left * lineWidth + ( Vector3.up * yOffsetProp.floatValue );
        Vector3 end = Vector3.right * lineWidth + ( Vector3.up * yOffsetProp.floatValue );
        Vector3 inc = Vector3.up * layerStepSizeProp.floatValue;
        Handles.color = lineColor;
        for( int i = 0; i <= numberLayers; i++ ) {
            Handles.DrawLine(start, end);
            start += inc;
            end += inc;
        }

    }

    //public static void drawBrawlerLayers() {
    //    Vector3 start = Vector3.left * lineWidth + ( Vector3.up * BrawlerLevelHelper.Instance.yOffset );
    //    Vector3 end = Vector3.right * lineWidth + ( Vector3.up * BrawlerLevelHelper.Instance.yOffset );
    //    Vector3 inc = Vector3.up * BrawlerLevelHelper.Instance.layerStepSize;
    //    Handles.color = lineColor;
    //    for( int i = 0; i <= numberLayers; i++ ) {
    //        Handles.DrawLine(start, end);
    //        start += inc;
    //        end += inc;
    //    }
    //}
}
