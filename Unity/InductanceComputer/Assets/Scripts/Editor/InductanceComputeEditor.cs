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
        point.CSV = EditorGUILayout.ObjectField("Point CSV", point.CSV, typeof(TextAsset), true) as TextAsset;
        coil.CSV = EditorGUILayout.ObjectField("Coil CSV", coil.CSV, typeof(TextAsset), true) as TextAsset;
        if (GUILayout.Button("Reload"))
        {
            coil.LoadCSV();
        }

        EditorGUILayout.Separator();
        GUILayout.Label("Coil Settings", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(32));

        GUILayout.Label("Integrate for Number of Radius");
        coil.NumberOfPartitionOfRadius = EditorGUILayout.IntField("", coil.NumberOfPartitionOfRadius);
        GUILayout.Label("Integrate for Number of Radian");
        coil.NumberOfPartitionOfRadians= EditorGUILayout.IntField("", coil.NumberOfPartitionOfRadians);

        EditorGUILayout.Separator();
        GUILayout.Label("Point Settings", EditorStyles.boldLabel);

        GUILayout.Label("Charge Density of the point");
        point.Sigma = EditorGUILayout.FloatField("", point.Sigma);
        GUILayout.Label("Proportional to the Magnetic Susceptibility \nof the point");
        point.Gamma = EditorGUILayout.FloatField("", point.Gamma);

        EditorGUILayout.Separator();
        GUILayout.Label("Field Settings", EditorStyles.boldLabel);

        field.FieldSize = EditorGUILayout.Vector3Field("Field Size", field.FieldSize);
        field.NumberOfPartition = EditorGUILayout.Vector3IntField("Number of Partition", field.NumberOfPartition);

        EditorGUILayout.Separator();
        GUILayout.Label("Actual Using Resouces", EditorStyles.boldLabel);
    }
}
