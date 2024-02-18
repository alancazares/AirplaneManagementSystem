using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneData))]
public class PlaneDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlaneData planeData = (PlaneData)target;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Plane Type");
        planeData.planeType = EditorGUILayout.TextField(planeData.planeType);

        EditorGUILayout.LabelField("Model Name");
        planeData.modelName = EditorGUILayout.TextField(planeData.modelName);

        EditorGUILayout.LabelField("Series");
        planeData.series = EditorGUILayout.TextField(planeData.series);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(planeData);
        }
    }
}