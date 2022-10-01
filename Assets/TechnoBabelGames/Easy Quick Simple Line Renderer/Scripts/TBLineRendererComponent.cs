using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoBabelGames
{
    [RequireComponent(typeof(LineRenderer))]
    public class TBLineRendererComponent : MonoBehaviour
    {

        public TBLineRenderer lineRendererProperties;
        private LineRenderer lineRenderer;

        private void OnEnable()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnValidate()
        {
            if (lineRenderer == null)
                return;
            
            if (lineRenderer.startWidth != lineRendererProperties.lineWidth)
                UpdateLineRendererLineWidth();

            if (lineRenderer.loop != lineRendererProperties.closeLoop)
                UpdateLineRendererLoop();
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

            if (lineRendererProperties.roundedEndCaps)
                lineRenderer.numCapVertices = 10;

            if (lineRendererProperties.roundedCorners)
                lineRenderer.numCornerVertices = 10;

            if (lineRendererProperties.axis == TBLineRenderer.Axis.FaceCamera)
            {
                lineRenderer.alignment = LineAlignment.View;
            }
            else
            {
                lineRenderer.alignment = LineAlignment.TransformZ;
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
            Vector3 v3;

            for (int i = 0; i < transform.childCount; i++)  //Start at 1 because the array includes the parent at index 0
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

        void UpdateLineRendererLineWidth()
        {
            lineRenderer.startWidth = lineRenderer.endWidth = lineRendererProperties.lineWidth;
        }

        void UpdateLineRendererLoop()
        {
            lineRenderer.loop = lineRendererProperties.closeLoop;
        }
    }
}
