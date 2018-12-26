using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ICInterfaceWindow : EditorWindow
{
    [MenuItem("Window/Indactance Computer")]
    static void Open()
    {
        GetWindow<InterfaceWindow>
    }
}