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
        var coil = manager.GetComponentInChildren<CoilScript>();
        var point = manager.GetComponentInChildren<PointScript>();
        var field = manager.GetComponentInChildren<FieldScript>();

        GUILayout.Label("Import CSV Files", EditorStyles.boldLabel);
        point.csv = EditorGUILayout.ObjectField("Point CSV", point.csv, typeof(TextAsset), true) as TextAsset;
        coil.csv = EditorGUILayout.ObjectField("Coil CSV", coil.csv, typeof(TextAsset), true) as TextAsset;
        if (GUILayout.Button("Reload"))
        {
            coil.LoadCSV();
        }

        EditorGUILayout.Separator();
        GUILayout.Label("Coil Settings", EditorStyles.boldLabel);

        coil.numberOfPartitionOfRadius = EditorGUILayout.IntField("Integrate for Number of Radius", coil.numberOfPartitionOfRadius);
        coil.numberOfPartitionOfRadians= EditorGUILayout.IntField("Integrate for Number of Radian", coil.numberOfPartitionOfRadians);

        EditorGUILayout.Separator();
        GUILayout.Label("Point Settings", EditorStyles.boldLabel);

        point.gamma = EditorGUILayout.FloatField("Proportional to the Magnetic Susceptibility of the point", point.gamma);

        EditorGUILayout.Separator();
        GUILayout.Label("Field Settings", EditorStyles.boldLabel);

        EditorGUILayout.Separator();
        GUILayout.Label("Actual Using Resouces", EditorStyles.boldLabel);

        
    }
}
