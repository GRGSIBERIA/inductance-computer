using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InductanceComputeManager))]
[CanEditMultipleObjects]
public class InductanceComputeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var manager = target as InductanceComputeManager;

        manager.pointCSV = EditorGUILayout.ObjectField("Point CSV", manager.pointCSV, typeof(TextAsset), true) as TextAsset;
        manager.coilCSV = EditorGUILayout.ObjectField("Coil CSV", manager.coilCSV, typeof(TextAsset), true) as TextAsset;
    }
}
