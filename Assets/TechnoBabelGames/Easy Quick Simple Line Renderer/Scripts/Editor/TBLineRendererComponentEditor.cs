using UnityEditor;
using UnityEngine;

namespace TechnoBabelGames
{
    [CustomEditor(typeof(TBLineRendererComponent))]
    public class TBLineRendererComponentEditor : Editor
    {
        
        TBLineRendererComponent lineRendererComponent;
        TBLineRenderer lineRendererProperties;
        bool showShapeAdjustments = false;
        float adjustedShapeSize;
        float adjustedLineWidth;
        bool changeCloseLoop;
        Vector3[] m_HandlePosition;
        Vector3[] handlePositions { get { return m_HandlePosition; } set { m_HandlePosition = value; } }

        float range = 0f;

        private void OnEnable()
        {
            lineRendererComponent = (TBLineRendererComponent)target;
            lineRendererProperties = lineRendererComponent.lineRendererProperties;
            lineRendererComponent.GetComponent<LineRenderer>().hideFlags = HideFlags.HideInInspector;
            adjustedShapeSize = lineRendererProperties.shapeSize;
            adjustedLineWidth = lineRendererProperties.lineWidth;
            changeCloseLoop = lineRendererProperties.closeLoop;

            //SetHandles();
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

            EditorGUI.BeginChangeCheck();

            float undoLineWidth = EditorGUILayout.Slider("Line Width", lineRendererProperties.lineWidth, 0.01f, 1);
            if(lineRendererProperties.lineWidth != adjustedLineWidth)
            {
                adjustedLineWidth = lineRendererProperties.lineWidth;
                lineRendererComponent.UpdateLineRendererLineWidth();
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(lineRendererComponent, "Changed Line Width");
                lineRendererProperties.lineWidth = undoLineWidth;
            }

            GUILayout.Space(8);

            EditorGUI.BeginChangeCheck();

            bool undoCloseLoop = EditorGUILayout.Toggle("Close Loop", lineRendererProperties.closeLoop);
            if(lineRendererProperties.closeLoop != changeCloseLoop)
            {
                changeCloseLoop = lineRendererProperties.closeLoop;
                lineRendererComponent.UpdateLineRendererLoop();
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(lineRendererComponent, "Changed Close Loop toggle");
                lineRendererProperties.closeLoop = undoCloseLoop;
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
                if (lineRendererProperties.shape == TBLineRenderer.Shape.None)
                {
                    EditorGUILayout.HelpBox("The size of custom shapes cannot be adjusted.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Adjusting these settings will reset the position of the points!", MessageType.Warning);

                    GUILayout.Space(8);

                    EditorGUI.BeginChangeCheck();

                    float undoShapeSize = EditorGUILayout.Slider("Shape Size", lineRendererProperties.shapeSize, 0.1f, 50);

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

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(lineRendererComponent, "Changed Shape Size");
                        lineRendererProperties.shapeSize = undoShapeSize;
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
                //CreateCustomHandles();
            }

            if(m_HandlePosition == null)
            {
                m_HandlePosition = new Vector3[lineRendererComponent.transform.childCount];
            }

            EditorGUI.BeginChangeCheck();
            Vector3[] newChildPositions = PlaceHandlesOnChildPositions();

            if (EditorGUI.EndChangeCheck())
            {
                //Undo line here!
                handlePositions = newChildPositions;
                MoveChildrenToHandlePositions();
            }            
        }

        Vector3[] PlaceHandlesOnChildPositions()
        {
            Vector3[] newVectors = new Vector3[lineRendererComponent.transform.childCount];
            
            for (int i = 0; i < newVectors.Length; i++)
            {
                Handles.color = Color.yellow;

                newVectors[i] = Handles.PositionHandle(
                    lineRendererComponent.transform.GetChild(i).position,
                    lineRendererComponent.transform.GetChild(i).rotation
                );
            }

            return newVectors;
        }

        void MoveChildrenToHandlePositions()
        {
            for (int i = 0; i < handlePositions.Length; i++)
            {
                lineRendererComponent.transform.GetChild(i).position = handlePositions[i];
            }
        }

        void CreateCustomHandles()
        {
            
            Transform transform = lineRendererComponent.transform;
            Handles.color = Color.yellow;
            HandleUtility.AddControl(0, 5f);
            Handles.ArrowHandleCap(
                0,
                transform.position + new Vector3(0f, 0f, 0f),
                transform.rotation * Quaternion.LookRotation(Vector3.right),
                0.5f,
                EventType.Repaint
            );
            Handles.color = Color.magenta;
            Handles.ArrowHandleCap(
                1,
                transform.position + new Vector3(0f, 0f, 0f),
                transform.rotation * Quaternion.LookRotation(Vector3.up),
                0.5f,
                EventType.Repaint
            );
            Handles.color = Color.white;
            Handles.ArrowHandleCap(
                2,
                transform.position + new Vector3(0f, 0f, 0f),
                transform.rotation * Quaternion.LookRotation(Vector3.forward),
                0.5f,
                EventType.Repaint
            );

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 screenPosition = Handles.matrix.MultiplyPoint(transform.position);

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.Layout:
                    Debug.Log("Layout or Repaint?");
                    HandleUtility.AddControl(
                        controlID,
                        HandleUtility.DistanceToCircle(screenPosition, 1.0f)
                    );
                    break;

                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == controlID)
                    {
                        Debug.Log("MouseDown");
                        // Respond to a press on this handle. Drag starts automatically.
                        GUIUtility.hotControl = controlID;
                        Event.current.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        Debug.Log("MouseUp");
                        // Respond to a release on this handle. Drag stops automatically.
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        Debug.Log("MouseDrag");
                        // Do whatever with mouse deltas here
                        GUI.changed = true;
                        Event.current.Use();

                        range = range + Time.deltaTime;

                        if (range > 0.1f)
                        {
                            Event e = Event.current;
                            Debug.Log(e.mousePosition);
                            range = 0.0f;
                        }
                    }
                    break;
            }

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    Debug.Log("MouseDown2");
                    break;
                case EventType.MouseUp:
                    Debug.Log("MouseUp2");
                    break;
                case EventType.MouseMove:
                    Debug.Log("MouseMove2");
                    break;
                case EventType.MouseDrag:
                    Debug.Log("MouseDrag2");
                    break;
                
                default:
                    break;
            }



        }

    }
}
