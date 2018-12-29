using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ICInterfaceWindow : ScriptableWizard
{
    [SerializeField]
    public TextAsset pointCSV;

    [SerializeField]
    public TextAsset coilCSV;

    [MenuItem("Window/Inductance Computer")]
    static void Open()
    {
        DisplayWizard<ICInterfaceWindow>("Inductance Computer");
    }

    private T AddChild<T>(string name, Transform t)
        where T : Component
    {
        var go = new GameObject(name);
        go.transform.parent = t;
        return go.AddComponent<T>();
    }

    private void OnWizardCreate()
    {
        var go = new GameObject("Inductance Compute Manager");
        var manager = go.AddComponent<InductanceComputeManager>();

        var field = AddChild<FieldScript>("field", go.transform);
        var point = AddChild<PointScript>("point", go.transform);
        var coil = AddChild<CoilScript>("coil", go.transform);

        point.CSV = pointCSV;
        coil.CSV = coilCSV;
    }
}