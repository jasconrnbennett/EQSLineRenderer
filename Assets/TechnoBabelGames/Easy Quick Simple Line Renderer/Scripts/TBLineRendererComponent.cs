using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TechnoBabelGames
{
    [RequireComponent(typeof(LineRenderer))]
    public class TBLineRendererComponent : MonoBehaviour
    {

        public TBLineRenderer lineRendererProperties;
        private LineRenderer lineRenderer;

        private void Reset()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();                
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable"); //Do we ever get here?
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }

        public void SetLineRendererProperties()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            UpdateLineRendererLineWidth();
            UpdateLineRendererLoop();
            lineRenderer.positionCount = lineRendererProperties.linePoints;            
            lineRenderer.startColor = lineRendererProperties.startColor;
            lineRenderer.endColor = lineRendererProperties.endColor;
            lineRenderer.textureMode = (LineTextureMode)lineRendererProperties.textureMode;

            if (lineRendererProperties.texture != null)
                lineRenderer.material = lineRendererProperties.texture;
            else
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            if (lineRendererProperties.roundedEndCaps)
                lineRenderer.numCapVertices = 10;

            if (lineRendererProperties.roundedCorners)
                lineRenderer.numCornerVertices = 10;

            switch (lineRendererProperties.axis)
            {
                case TBLineRenderer.Axis.FaceCamera:
                    lineRenderer.alignment = LineAlignment.View;
                    transform.Rotate(0, 0, 0);
                    break;
                case TBLineRenderer.Axis.X:
                    lineRenderer.alignment = LineAlignment.TransformZ;
                    transform.Rotate(0, 90, 0);
                    break;
                case TBLineRenderer.Axis.Y:
                    lineRenderer.alignment = LineAlignment.TransformZ;
                    transform.Rotate(90, 0, 0);
                    break;
                case TBLineRenderer.Axis.Z:
                    lineRenderer.alignment = LineAlignment.TransformZ;
                    transform.Rotate(0, 0, 90);
                    break;
                default:
                    break;
            }
        }

        public void DrawBasicShape()
        {
            if(lineRendererProperties.shape == TBLineRenderer.Shape.None)
            {
                SetPoints();
                return;
            }

            List<Vector2> points = GetPolygonOnACircle(transform.childCount, lineRendererProperties.shapeSize / 2, Vector2.zero);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = points[i];
            }

            SetPoints();
        }

        public void SetPoints()
        {
            if (lineRenderer == null)
                SetLineRendererProperties();            
            
            Vector3 v3;

            for (int i = 0; i < transform.childCount; i++)
            {
                v3 = new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, transform.GetChild(i).position.z);
                lineRenderer.SetPosition(i, v3);
            }
        }

        public List<Vector2>
        GetPolygonOnACircle(int numberOfPoints, float radius, Vector2 center)
        {
            List<Vector2> points = new List<Vector2>();

            if (numberOfPoints < 1)
            {
                return points;
            }

            float sliceAngle = (2 * Mathf.PI) / numberOfPoints;

            float currentAngle = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector2 point = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;
                point += center;
                points.Add(point);
                currentAngle += sliceAngle;
            }

            return points;
        }

        public void UpdateLineRendererLineWidth()
        {
            if (lineRenderer == null || lineRendererProperties == null)
                return;

            lineRenderer.startWidth = lineRenderer.endWidth = lineRendererProperties.lineWidth;
        }

        public void UpdateLineRendererLoop()
        {
            if (lineRenderer == null || lineRendererProperties == null)
                return;

            lineRenderer.loop = lineRendererProperties.closeLoop;
        }
    }
}
