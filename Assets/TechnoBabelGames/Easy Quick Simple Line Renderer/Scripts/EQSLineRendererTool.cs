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
        enum Alignment { CameraView, X, Y, Z}
        Alignment alignment;
        bool hiddenSolidColor = true;
        bool hiddenGradient;
        bool hiddenMaterial;
        Material lineMaterial;
        enum LineMaterial { Stretch, Tile}
        LineMaterial lineMaterialEnum;
        bool roundCorners;
        bool roundEndCaps;
        Vector2 scrollPosition;
        //AnimBool hiddenValues;

        [MenuItem("Tools/TechnoBabelGames/EQS Line Renderer")]
        public static void ShowWindow()
        {
            EQSLineRendererTool window = (EQSLineRendererTool)GetWindow(typeof(EQSLineRendererTool), false, "EQS Line Renderer");
            window.minSize = window.maxSize = new Vector2(400, 400);
        }

        private void OnGUI()
        {
            //scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            GUILayout.Space(8);
            GUILayout.Label("Create Line Renderer", EditorStyles.boldLabel);
            GUILayout.Space(8);
            lineName = EditorGUILayout.TextField("Object name", lineName);
            GUILayout.Space(8);
            linePoints = EditorGUILayout.IntSlider("Number of points", linePoints, 2, 10);
            GUILayout.Space(8);

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();

            roundEndCaps = EditorGUILayout.ToggleLeft("Rounded end caps", roundEndCaps);
            roundCorners = EditorGUILayout.ToggleLeft("Rounded corners", roundCorners);            

            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            GUILayout.Space(8);
            alignment = (Alignment)EditorGUILayout.EnumPopup("Axis", alignment);
            GUILayout.Space(8);
            lineWidth = EditorGUILayout.Slider("Line width", lineWidth, 0, 1);
            GUILayout.Space(8);

            GUILayoutToggleHidden();
            GUILayoutLineColor();
            GUILayoutButton();


            GUILayoutTestFields();

            //GUILayout.EndScrollView();
        }

        void GUILayoutToggleHidden()
        {
            //Rect solidColorRect = new Rect();
            //solidColorRect.x = 0;
            //solidColorRect.width = Screen.width / 3f;
            //solidColorRect.height = 20;

            //Rect gradientRect = new Rect();
            //gradientRect.x = Screen.width / 3f;
            //gradientRect.width = Screen.width / 3;

            //Rect materialRect = new Rect();
            //materialRect.x = (Screen.width / 3f) * 2;
            //materialRect.width = Screen.width / 3;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(133.33f));

            if (hiddenSolidColor = EditorGUILayout.ToggleLeft("Solid Color", hiddenSolidColor, GUILayout.Width(133.33f)))
            {
                hiddenGradient = hiddenMaterial = false;
            }


            if (hiddenGradient = EditorGUILayout.ToggleLeft("Gradient", hiddenGradient, GUILayout.Width(133.33f)))
            {
                hiddenSolidColor = hiddenMaterial = false;
            }


            if (hiddenMaterial = EditorGUILayout.ToggleLeft("Texture", hiddenMaterial, GUILayout.Width(133.33f)))
            {
                hiddenSolidColor = hiddenGradient = false;
            }
            EditorGUILayout.EndHorizontal();
        }

        void GUILayoutLineColor()
        {
            GUILayout.Space(8);

            EditorGUI.indentLevel++;

            if (hiddenSolidColor)
            {
                startColor = EditorGUILayout.ColorField("Line color", startColor);
            }
            else if (hiddenGradient)
            {
                startColor = EditorGUILayout.ColorField("Start color", startColor);
                endColor = EditorGUILayout.ColorField("End color", endColor);
            }
            else if (hiddenMaterial)
            {
                lineMaterial = (Material)EditorGUILayout.ObjectField("Material", lineMaterial, typeof(Material), false);
                lineMaterialEnum = (LineMaterial)EditorGUILayout.EnumPopup("Mode", lineMaterialEnum);
            }

            EditorGUI.indentLevel--;
        }

        void GUILayoutButton()
        {
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Line", GUILayout.Width(120), GUILayout.Height(60)))
            {
                CreateAutoLine();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.Label("Warning!");
            GUILayout.Label("DO NOT REARRANGE THE CHILD OBJECTS - DO NOT ADJUST THE ROTATION OF THE OBJECTS", EditorStyles.helpBox);
        }

        void GUILayoutTestFields()
        {
            GUILayout.Space(42);
            float testFloat = Screen.width / 3f;
            GUILayout.Label(testFloat.ToString(), EditorStyles.helpBox);
        }

        private void CreateAutoLine()
        {
            //Create GameOjbect
            GameObject lineContainerGO;
            lineContainerGO = new GameObject(lineName);

            switch (alignment)
            {
                case Alignment.CameraView:
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
            lineContainerGO.AddComponent<EQSLineRendererAddPositions>();
            LineRenderer lineRenderer = lineContainerGO.GetComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = linePoints;

            if (roundEndCaps)
                lineRenderer.numCapVertices = 10;
            if (roundCorners)
                lineRenderer.numCornerVertices = 10;

            if (alignment == Alignment.CameraView)
                lineRenderer.alignment = LineAlignment.View;
            else
                lineRenderer.alignment = LineAlignment.TransformZ;

            if (hiddenMaterial)
            {
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
            }
            else
            {
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            }

            if (hiddenSolidColor)
                startColor = endColor;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;

            //Create Child Objects
            GameObject pointsGO;
            Transform gizmoTarget = lineContainerGO.transform;
            for (int i = 1; i <= linePoints; i++)  //Start at 2 because the parent is point 1
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
            }
        }
    }
}

