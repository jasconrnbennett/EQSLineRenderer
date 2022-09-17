using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelScript myLevelScript = (LevelScript)target;

        myLevelScript.experience= EditorGUILayout.IntField("Experience", myLevelScript.experience);
        EditorGUILayout.LabelField("Level", myLevelScript.Level.ToString());
    }
}
