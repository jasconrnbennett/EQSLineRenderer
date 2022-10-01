using UnityEditor;
using UnityEngine;
using System;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(TBLineRendererComponent))]
    public class TBLineRendererComponentEditor : Editor
    {
        
        TBLineRendererComponent lineRendererComponent;
        TBLineRenderer lineRendererProperties;
        bool showShapeAdjustments = false;
        float adjustedShapeSize;

        private void OnEnable()
        {
            lineRendererComponent = (TBLineRendererComponent)target;
            lineRendererProperties = lineRendererComponent.lineRendererProperties;
            lineRendererComponent.GetComponent<LineRenderer>().hideFlags = HideFlags.HideInInspector;
            adjustedShapeSize = lineRendererProperties.shapeSize;
        }

        public override void OnInspectorGUI()
        {
            GUILayoutLineRendererPorperties();
            GUILayout.Space(8);
            TBLineRendererTool.DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutShapeAdjustments();
        }

        void GUILayoutLineRendererPorperties()
        {
            EditorGUILayout.LabelField("LineRenderer Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineRendererProperties.lineWidth = EditorGUILayout.Slider("Line Width", lineRendererProperties.lineWidth, 0.01f, 1);

            GUILayout.Space(8);

            lineRendererProperties.closeLoop = EditorGUILayout.Toggle("Close Loop", lineRendererProperties.closeLoop);
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
                if (lineRendererProperties.shape == TBLineRenderer.Shape.None)
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
                    if (GUILayout.Button("Reset Shape", GUILayout.Width(100), GUILayout.Height(30)) || lineRendererProperties.shapeSize != adjustedShapeSize)
                    {
                        adjustedShapeSize = lineRendererProperties.shapeSize;
                        lineRendererComponent.DrawBasicShape();
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
                lineRendererComponent.SetPoints();
            }

        }
    }
}
