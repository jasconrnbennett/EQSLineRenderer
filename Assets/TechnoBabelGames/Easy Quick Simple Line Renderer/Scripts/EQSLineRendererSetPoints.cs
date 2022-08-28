using UnityEditor;
using UnityEngine;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(EQSLineRendererAddPositions))]
    public class EQSLineRendererSetPoints : Editor
    {

        EQSLineRendererAddPositions monoScript;

        private void OnEnable()
        {
            monoScript = (EQSLineRendererAddPositions)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Click 'Preview' if the line does not update", EditorStyles.boldLabel);
            if (GUILayout.Button("Preview"))
            {
                monoScript.SetPoints();
            }
        }

        void OnSceneGUI()
        {

            if (Event.current.type == EventType.Repaint)
            {
                monoScript.SetPoints();
            }
                
        }
    }
}

