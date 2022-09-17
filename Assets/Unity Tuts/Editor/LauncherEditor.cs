using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Launcher))]
public class LauncherEditor : Editor
{
    private void OnSceneGUI()
    {
        var launcher = (Launcher)target;
    }
}
