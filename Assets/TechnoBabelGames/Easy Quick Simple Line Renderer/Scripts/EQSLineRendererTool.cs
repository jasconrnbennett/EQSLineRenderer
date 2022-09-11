using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;

namespace TechnoBabelGames
{
    public class EQSLineRendererTool : EditorWindow
    {
        string lineName = "EQS Line Renderer";
        int linePoints = 2;
        float lineWidth = 0.5f;
        Color startColor = Color.white;
        Color endColor = Color.white;
        enum Alignment { FaceCamera, X, Y, Z}
        Alignment alignment;
        Material lineMaterial;
        enum LineColoring { SolidColor, Gradient, Texture}
        LineColoring lineColoring;
        enum LineMaterial { Stretch, Tile}
        LineMaterial lineMaterialEnum;
        enum LinePointsShape { Custom, Line, Triangle, Square, Pentagon, Hexagon, Heptagon, Octagon, Nonagon, Decagon}
        LinePointsShape linePointsShape;
        bool roundCorners;
        bool roundEndCaps;
        Vector2 scrollPosition;
        bool closeLineLoop = false;
        float shapeSize = 3;

        [MenuItem("Tools/TechnoBabelGames/EQS Line Renderer")]
        public static void ShowWindow()
        {
            EQSLineRendererTool window = (EQSLineRendererTool)GetWindow(typeof(EQSLineRendererTool), false, "EQS Line Renderer");
            window.minSize = /*window.maxSize =*/ new Vector2(400, 400);
        }

        private void OnGUI()
        {
            DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutLineVisualProperties();
            GUILayout.Space(8);
            DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutLineColoring();
            GUILayout.Space(8);
            DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutLineTechnicalProperties();
            GUILayout.Space(8);
            DrawUILine(Color.gray);
            GUILayout.Space(8);
            GUILayoutButton();
            GUILayout.Space(8);
            DrawUILine(Color.gray);

            //GUILayoutTestFields();
        }

        //Width & Rounded Corners/End Caps
        void GUILayoutLineVisualProperties()
        {
            EditorGUILayout.LabelField("Visual Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineWidth = EditorGUILayout.Slider("Line width", lineWidth, 0.01f, 1);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            roundEndCaps = EditorGUILayout.Toggle("Rounded End Caps", roundEndCaps);
            roundCorners = EditorGUILayout.Toggle("Rounded Corners", roundCorners);
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        void GUILayoutLineColoring()
        {
            EditorGUILayout.LabelField("Line Coloring", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineColoring = (LineColoring)EditorGUILayout.EnumPopup("Coloring Type", lineColoring);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            switch (lineColoring)
            {
                case LineColoring.SolidColor:
                    startColor = EditorGUILayout.ColorField("Line Color", startColor);
                    break;
                case LineColoring.Gradient:
                    startColor = EditorGUILayout.ColorField("Start Color", startColor);
                    endColor = EditorGUILayout.ColorField("End Color", endColor);
                    break;
                case LineColoring.Texture:
                    lineMaterial = (Material)EditorGUILayout.ObjectField("Material", lineMaterial, typeof(Material), false);
                    if (lineMaterial == null)
                    {
                        //EditorGUIUtility.SetIconSize(new Vector2(-0.1f, -0.1f));
                        EditorGUILayout.HelpBox("Material is required", MessageType.Info, false);
                    }
                    lineMaterialEnum = (LineMaterial)EditorGUILayout.EnumPopup("Mode", lineMaterialEnum);
                    break;
                default:
                    break;
            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        void GUILayoutLineTechnicalProperties()
        {
            EditorGUILayout.LabelField("Technical Properties", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            linePointsShape = (LinePointsShape)EditorGUILayout.EnumPopup("Starting Shape", linePointsShape);

            switch (linePointsShape)
            {
                case LinePointsShape.Custom:
                    GUILayout.Space(8);

                    EditorGUI.indentLevel++;
                    linePoints = EditorGUILayout.IntField("Points", linePoints);
                    if(linePoints < 2)
                        linePoints = 2;

                    GUILayout.Space(8);

                    closeLineLoop = EditorGUILayout.Toggle("Close Loop", closeLineLoop);
                    EditorGUI.indentLevel--;
                    break;
                case LinePointsShape.Line:
                    GUILayoutShapeSize();
                    linePoints = 2;
                    closeLineLoop = false;
                    break;
                case LinePointsShape.Triangle:
                    GUILayoutShapeSize();
                    linePoints = 3;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Square:
                    GUILayoutShapeSize();
                    linePoints = 4;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Pentagon:
                    GUILayoutShapeSize();
                    linePoints = 5;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Hexagon:
                    GUILayoutShapeSize();
                    linePoints = 6;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Heptagon:
                    GUILayoutShapeSize();
                    linePoints = 7;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Octagon:
                    GUILayoutShapeSize();
                    linePoints = 8;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Nonagon:
                    GUILayoutShapeSize();
                    linePoints = 9;
                    closeLineLoop = true;
                    break;
                case LinePointsShape.Decagon:
                    GUILayoutShapeSize();
                    linePoints = 10;
                    closeLineLoop = true;
                    break;
                default:
                    break;
            }

            GUILayout.Space(8);

            alignment = (Alignment)EditorGUILayout.EnumPopup("Facing Axis", alignment);
            EditorGUI.indentLevel--;
        }

        void GUILayoutButton()
        {
            EditorGUILayout.LabelField("Create GameObject", EditorStyles.boldLabel);

            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            lineName = EditorGUILayout.TextField("Object Name", lineName);
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(lineColoring == LineColoring.Texture && lineMaterial == null);
            if (GUILayout.Button("Create Line", GUILayout.Width(120), GUILayout.Height(60)))
            {
                CreateAutoLine();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            GUILayout.Space(12);
            EditorGUILayout.HelpBox("DO NOT REARRANGE THE CHILD OBJECTS - DO NOT ADJUST THE ROTATION OF THE OBJECTS", MessageType.Warning);
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        void GUILayoutShapeSize()
        {
            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            shapeSize = EditorGUILayout.Slider("Shape Size", shapeSize, 0.5f, 10);
            EditorGUI.indentLevel--;
        }

        void GUILayoutTestFields()
        {
            GUILayout.Space(12);
            //float testFloat = Screen.width / 3f;
            float testIntX = EditorGUIUtility.GetIconSize().x;
            float testIntY = EditorGUIUtility.GetIconSize().y;
            string testString = testIntX.ToString() +", "+ testIntY.ToString();
            GUILayout.Label(testString, EditorStyles.helpBox);
        }

        private void CreateAutoLine()
        {
            //Create GameOjbect
            GameObject lineContainerGO;
            lineContainerGO = new GameObject(lineName);

            switch (alignment)
            {
                case Alignment.FaceCamera:
                    lineContainerGO.transform.Rotate(0, 0, 0);
                    break;
                case Alignment.X:
                    lineContainerGO.transform.Rotate(0, 90, 0);
                    break;
                case Alignment.Y:
                    lineContainerGO.transform.Rotate(90, 0, 0);
                    break;
                case Alignment.Z:
                    lineContainerGO.transform.Rotate(0, 0, 90);
                    break;
                default:
                    break;
            }

            //Add Line Renderer
            lineContainerGO.AddComponent<LineRenderer>();
            EQSLineRendererAddPositions eqsLineRendererAddPositions = lineContainerGO.AddComponent<EQSLineRendererAddPositions>();
            LineRenderer lineRenderer = lineContainerGO.GetComponent<LineRenderer>();            

            lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = linePoints;

            if (roundEndCaps)
                lineRenderer.numCapVertices = 10;
            if (roundCorners)
                lineRenderer.numCornerVertices = 10;

            if (closeLineLoop)
                lineRenderer.loop = true;
            else
                lineRenderer.loop = false;

            if (alignment == Alignment.FaceCamera)
                lineRenderer.alignment = LineAlignment.View;
            else
                lineRenderer.alignment = LineAlignment.TransformZ;

            switch (lineColoring)
            {
                case LineColoring.SolidColor:
                    lineRenderer.startColor = startColor;
                    lineRenderer.endColor = startColor;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    break;
                case LineColoring.Gradient:
                    lineRenderer.startColor = startColor;
                    lineRenderer.endColor = endColor;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    break;
                case LineColoring.Texture:
                    lineRenderer.material = lineMaterial;
                    switch (lineMaterialEnum)
                    {
                        case LineMaterial.Stretch:
                            lineRenderer.textureMode = LineTextureMode.Stretch;
                            break;
                        case LineMaterial.Tile:
                            lineRenderer.textureMode = LineTextureMode.Tile;
                            break;
                        default:
                            break;
                    }
                    lineRenderer.startColor = lineRenderer.endColor = Color.white;
                    break;
                default:
                    break;
            }

            switch (linePointsShape)
            {
                case LinePointsShape.Custom:
                    eqsLineRendererAddPositions.basicShape = EQSLineRendererAddPositions.BasicShape.None;
                    break;
                case LinePointsShape.Line:
                    eqsLineRendererAddPositions.basicShape = EQSLineRendererAddPositions.BasicShape.Line;
                    break;
                case LinePointsShape.Triangle:
                    eqsLineRendererAddPositions.basicShape = EQSLineRendererAddPositions.BasicShape.Triangle;
                    break;
                case LinePointsShape.Square:
                    eqsLineRendererAddPositions.basicShape = EQSLineRendererAddPositions.BasicShape.Square;
                    break;
                default:
                    break;
            }

            eqsLineRendererAddPositions.pointOffset = shapeSize;


            //Create Child Objects
            GameObject pointsGO;
            Transform gizmoTarget = lineContainerGO.transform;
            for (int i = 1; i <= linePoints; i++)
            {
                if (i == linePoints)
                {
                    pointsGO = new GameObject("Ending Point");
                    pointsGO.AddComponent<EQSLinerRendererDrawGizmo>();
                    pointsGO.GetComponent<EQSLinerRendererDrawGizmo>().targetPoint = gizmoTarget;
                }
                else
                {
                    pointsGO = new GameObject("Point " + i);
                    pointsGO.AddComponent<EQSLinerRendererDrawGizmo>();
                    if (i != 1)
                        pointsGO.GetComponent<EQSLinerRendererDrawGizmo>().targetPoint = gizmoTarget;
                    gizmoTarget = pointsGO.transform;
                }
                pointsGO.transform.SetParent(lineContainerGO.transform);
                pointsGO.GetComponent<EQSLinerRendererDrawGizmo>().parent = eqsLineRendererAddPositions;
            }

            eqsLineRendererAddPositions.DrawBasicShape(eqsLineRendererAddPositions.basicShape);
        }
    }
}

