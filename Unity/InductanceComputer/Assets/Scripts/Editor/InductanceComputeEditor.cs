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

        Separator();
        GUILayout.Label("Coil Settings", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(32));

        GUILayout.Label("Number of Partition of Radius for Integration");
        coil.NumberOfPartitionOfRadius = EditorGUILayout.IntField("", coil.NumberOfPartitionOfRadius);
        coil.NumberOfPartitionOfRadius = ValidateUnder(coil.NumberOfPartitionOfRadius, 0);
        GUILayout.Label("Number of Partition of Radian for Integration");
        coil.NumberOfPartitionOfRadians = EditorGUILayout.IntField("", coil.NumberOfPartitionOfRadians);
        coil.NumberOfPartitionOfRadians = ValidateUnder(coil.NumberOfPartitionOfRadians, 0);

        Separator();
        GUILayout.Label("Point Settings", EditorStyles.boldLabel);

        GUILayout.Label("Charge Density of the point");
        point.Sigma = EditorGUILayout.FloatField("", point.Sigma);
        point.Sigma = point.Sigma < 0f ? 0.0000000001f : point.Sigma;
        GUILayout.Label("Proportional to the Magnetic Susceptibility \nof the point");
        point.Gamma = EditorGUILayout.FloatField("", point.Gamma);
        point.Gamma = point.Gamma < 0f ? 0.0000000001f : point.Gamma;

        Separator();
        GUILayout.Label("Field Settings", EditorStyles.boldLabel);

        field.FieldSize = EditorGUILayout.Vector3Field("Field Size", field.FieldSize);
        field.FieldSize = SizeCheck(field.FieldSize);
        field.NumberOfPartition = EditorGUILayout.Vector3IntField("Number of Partition", field.NumberOfPartition);
        field.NumberOfPartition = SizeCheck(field.NumberOfPartition);

        Separator();
        GUILayout.Label("Generic Settings", EditorStyles.boldLabel);
        Information<int>("Total Frame", coil.TimeCount, "frames");
        manager.StartFrame = EditorGUILayout.IntField("Start Frame", manager.StartFrame);
        manager.StartFrame = ValidateUnder(manager.StartFrame, 0);
        manager.StartFrame = ValidateOver(manager.StartFrame, manager.EndFrame);
        manager.EndFrame = EditorGUILayout.IntField("End Frame", manager.EndFrame);
        manager.EndFrame = ValidateUnder(manager.EndFrame, 0);
        manager.EndFrame = ValidateOver(manager.EndFrame, coil.TimeCount);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Folder");
        if (GUILayout.Button("Open"))
        {
            manager.SaveFolder = EditorUtility.SaveFolderPanel("Save Folder", manager.SaveFolder, "Assets");
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(manager.SaveFolder);

        Separator();
        GUILayout.Label("Actual Using Resouces", EditorStyles.boldLabel);

        if (GUILayout.Button("Calculate"))
        {

        }

        Separator();
        GUILayout.Label("Did you finished settings?", EditorStyles.boldLabel);

        if (GUILayout.Button("Yes, I finished. Start Computing."))
        {
            float calculateFrameCount = (float)(manager.EndFrame - manager.StartFrame);
            try
            {
                for (int frame = manager.StartFrame; frame < manager.EndFrame; ++frame)
                {
                    float percentage = (float)(frame - manager.EndFrame) / calculateFrameCount;
                    EditorUtility.DisplayProgressBar("Computing Inductance", string.Format("{0:#.##} %", percentage * 100f), percentage);

                    var fluxDensityOnPoint = point.Compute(coil, frame);
                }
            }
            finally
            {
                // IDisposableが実装されていない
                EditorUtility.ClearProgressBar();
            }
        }
    }

    void Information<T>(string title, T amount, string unit)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(title);
        GUILayout.Label(string.Format("{0}", amount));
        GUILayout.Label(unit);
        GUILayout.EndHorizontal();
    }

    int ValidateUnder(int x, int val)
    {
        return x < val ? val : x;
    }

    int ValidateOver(int x, int top)
    {
        return x < top ? x : top;
    }

    Vector3 SizeCheck(Vector3 target)
    {
        Vector3 size = new Vector3();
        const float under = 0.000000001f;
        size.x = target.x < 0f ? under : target.x;
        size.y = target.y < 0f ? under : target.y;
        size.z = target.z < 0f ? under : target.z;
        return size;
    }

    Vector3Int SizeCheck(Vector3Int target)
    {
        Vector3Int size = new Vector3Int();
        const int under = 1;
        size.x = target.x < 1 ? under : target.x;
        size.y = target.y < 1 ? under : target.y;
        size.z = target.z < 1 ? under : target.z;
        return size;
    }

    void Separator()
    {
        GUILayout.Space(8);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
    }
}
