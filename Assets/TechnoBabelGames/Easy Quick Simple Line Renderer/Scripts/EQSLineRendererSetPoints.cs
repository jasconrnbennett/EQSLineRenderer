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
            GUILayout.Label("Click to reset to the original basic shape", EditorStyles.boldLabel);
            if (GUILayout.Button("Draw Shape"))
            {
                monoScript.DrawBasicShape(monoScript.basicShape);
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

