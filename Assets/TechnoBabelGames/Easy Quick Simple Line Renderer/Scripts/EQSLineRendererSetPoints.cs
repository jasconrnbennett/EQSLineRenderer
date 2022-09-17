using UnityEditor;
using UnityEngine;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(EQSLineRendererAddPositions))]
    public class EQSLineRendererSetPoints : Editor
    {

        EQSLineRendererAddPositions monoScript;
        LineRenderer lineRenderer;
        float lineRendererWidth;
        float shapeSize;
        bool showShapeAdjustments = false;

        private void OnEnable()
        {
            monoScript = (EQSLineRendererAddPositions)target;
            lineRenderer = monoScript.GetComponent<LineRenderer>();
            lineRendererWidth = lineRenderer.startWidth;
            shapeSize = monoScript.shapeSize;
        }

        public override void OnInspectorGUI()
        {
            GUILayoutLineRendererPorperties();
            GUILayout.Space(8);
            EQSLineRendererTool.DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutShapeAdjustments();
            
            
            lineRenderer.startWidth = lineRenderer.endWidth = lineRendererWidth;            
        }

        void GUILayoutLineRendererPorperties()
        {
            EditorGUILayout.LabelField("LineRenderer Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineRendererWidth = EditorGUILayout.Slider("Line Width", lineRendererWidth, 0.01f, 1);

            GUILayout.Space(8);
            bool checkLoop = lineRenderer.loop;
           
            //Debug.Log("Loopsss");
            checkLoop = EditorGUILayout.Toggle("Close Loop", checkLoop);
            if (checkLoop != lineRenderer.loop)
            {
                Debug.Log("In IF");
                Undo.RecordObject(monoScript.gameObject, "Change to Close Loop status");
                Undo.FlushUndoRecordObjects();
                lineRenderer.loop = checkLoop;
            }
            EditorGUI.indentLevel--;
        }

        void GUILayoutShapeAdjustments()
        {
            EditorGUILayout.LabelField("Shape Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            showShapeAdjustments = EditorGUILayout.Foldout(showShapeAdjustments, "Adjust Shape");
            if (showShapeAdjustments)
            {
                EditorGUI.indentLevel++;
                if (monoScript.basicShape == EQSLineRendererAddPositions.BasicShape.Custom)
                {
                    EditorGUILayout.HelpBox("The size of custom shapes cannot be adjusted.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Adjusting these settings will reset the position of the points!", MessageType.Warning);

                    GUILayout.Space(8);

                    shapeSize = EditorGUILayout.Slider("Shape Size", shapeSize, 0.1f, 50);

                    GUILayout.Space(8);

                    EditorGUILayout.HelpBox("'Reset Shape' will set points back into the original shape but will NOT reset the shape size.", MessageType.Info);

                    GUILayout.Space(8);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reset Shape", GUILayout.Width(100), GUILayout.Height(30)) || shapeSize != monoScript.shapeSize)
                    {
                        monoScript.shapeSize = shapeSize;
                        lineRenderer.startWidth = lineRenderer.endWidth = lineRendererWidth;
                        monoScript.DrawBasicShape(monoScript.basicShape);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
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

