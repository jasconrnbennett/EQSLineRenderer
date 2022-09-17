using UnityEditor;
using UnityEngine;
using System;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(EQSLineRendererAddPositions))]
    public class EQSLineRendererSetPoints : Editor
    {

        EQSLineRendererAddPositions monoScript;
        EQSLineRendererProperties lineRendererProperties;
        LineRenderer lineRenderer;
        //float lineRendererWidth;
        //float shapeSize;
        bool showShapeAdjustments = false;

        private void OnEnable()
        {
            monoScript = (EQSLineRendererAddPositions)target;
            lineRenderer = monoScript.GetComponent<LineRenderer>();
            lineRendererProperties = new EQSLineRendererProperties();
            lineRendererProperties.lineWidth = lineRenderer.startWidth;
            lineRendererProperties.shapeSize = monoScript.shapeSize;
            lineRendererProperties.closeLoop = lineRenderer.loop;
            //lineRendererWidth = lineRenderer.startWidth;
            //shapeSize = monoScript.shapeSize;
        }

        public override void OnInspectorGUI()
        {
            GUILayoutLineRendererPorperties();
            GUILayout.Space(8);
            EQSLineRendererTool.DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutShapeAdjustments();
            
            
            lineRenderer.startWidth = lineRenderer.endWidth = lineRendererProperties.lineWidth;            
        }

        void GUILayoutLineRendererPorperties()
        {
            EditorGUILayout.LabelField("LineRenderer Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineRendererProperties.lineWidth = EditorGUILayout.Slider("Line Width", lineRendererProperties.lineWidth, 0.01f, 1);

            GUILayout.Space(8);

            lineRendererProperties.closeLoop = EditorGUILayout.Toggle("Close Loop", lineRendererProperties.closeLoop);
            lineRenderer.loop = lineRendererProperties.closeLoop;
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

                    lineRendererProperties.shapeSize = EditorGUILayout.Slider("Shape Size", lineRendererProperties.shapeSize, 0.1f, 50);

                    GUILayout.Space(8);

                    EditorGUILayout.HelpBox("'Reset Shape' will set points back into the original shape but will NOT reset the shape size.", MessageType.Info);

                    GUILayout.Space(8);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reset Shape", GUILayout.Width(100), GUILayout.Height(30)) || lineRendererProperties.shapeSize != monoScript.shapeSize)
                    {
                        monoScript.shapeSize = lineRendererProperties.shapeSize;
                        lineRenderer.startWidth = lineRenderer.endWidth = lineRendererProperties.lineWidth;
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

    [Serializable]
    public class EQSLineRendererProperties
    {
        public bool closeLoop;
        public float lineWidth;
        public float shapeSize;
    }
}

