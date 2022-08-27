using UnityEditor;
using UnityEngine;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(EQSLineRendererAddPositions))]
    public class EQSLineRendererSetPoints : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Click 'Preview' to draw the line after placing the points", EditorStyles.boldLabel);
            if (GUILayout.Button("Preview"))
            {
                EQSLineRendererAddPositions monoScript = (EQSLineRendererAddPositions)target;
                monoScript.SetPoints();
            }
        }
    }
}

