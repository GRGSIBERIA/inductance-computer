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

    private void OnWizardCreate()
    {
        var go = new GameObject("Inductance Compute Manager");
    }
}