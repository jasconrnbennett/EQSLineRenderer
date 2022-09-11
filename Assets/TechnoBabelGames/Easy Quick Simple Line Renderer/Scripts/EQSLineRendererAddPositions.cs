using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;

namespace TechnoBabelGames
{
    public class EQSLineRendererAddPositions : MonoBehaviour
    {
        private Transform[] points;
        public enum BasicShape { None, Line, Triangle, Square}
        public BasicShape basicShape;
        Transform parentTransform;
        public float pointOffset;        

        private void OnEnable()
        {
            parentTransform = transform.GetComponentInParent<Transform>();
        }

        void Start()
        {
            SetPoints();
            //DrawBasicShape(basicShape);
        }

        public void SetPoints()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();

            Vector3 v3;
            var points = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();

            for (int i = 0; i < points.Length; i++)  //Start at 1 because the array includes the parent at index 0
            {

                v3 = new Vector3(points[i].position.x, points[i].position.y, points[i].position.z);
                lineRenderer.SetPosition(i, v3);
            }            
        }

        public void DrawBasicShape(BasicShape shape)
        {
            if (shape == BasicShape.None)
            {
                SetPoints();
                return;
            }

            var transforms = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
            List<Vector2> points = GetPolygonOnACircle(transforms.Length, pointOffset/2, Vector2.zero);

            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].localPosition = points[i];
            }
            
            SetPoints();
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
    }
}

