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

            switch (shape)
            {
                case BasicShape.Line:
                    //List<Vector2> points = GetPolygonOnACircle(2, 3, Vector2.zero);
                    //Debug.Log(points[0] + ", " + points[1]);
                    if (transform.eulerAngles.z == 90f || transform.eulerAngles == Vector3.zero)
                    {
                        transforms[1].position = new Vector3(transforms[0].position.x + (pointOffset), transforms[0].position.y, transforms[0].position.z);
                    }
                    else
                    {
                        transforms[1].position = new Vector3(transforms[0].position.x, transforms[0].position.y, transforms[0].position.z + (pointOffset));
                    }

                    break;
                case BasicShape.Triangle:
                    List<Vector2> points = GetPolygonOnACircle(3, 3, Vector2.zero);
                    Debug.Log($"{points[0]}, {points[1]}, {points[2]}");

                    for (int i = 0; i < transforms.Length; i++)
                    {
                        transforms[i].localPosition = points[i];
                    }
                    //if (transform.eulerAngles.x == 90f) // Facing Up (Y)
                    //{
                    //    transforms[1].position = new Vector3(transforms[0].position.x + (pointOffset), transforms[0].position.y, transforms[0].position.z);
                    //    transforms[2].position = new Vector3(transforms[0].position.x + (pointOffset / 2), transforms[0].position.y, transforms[0].position.z + (pointOffset / 2));
                    //}
                    //else if (transform.eulerAngles.y == 90f) // Facing Right (X)
                    //{
                    //    transforms[1].position = new Vector3(transforms[0].position.x, transforms[0].position.y, transforms[0].position.z + (pointOffset));
                    //    transforms[2].position = new Vector3(transforms[0].position.x, transforms[0].position.y + (pointOffset / 2), transforms[0].position.z + (pointOffset / 2));
                    //}
                    //else  // Facing Forward (Z) & Camera
                    //{
                    //    transforms[1].position = new Vector3(transforms[0].position.x + (pointOffset), transforms[0].position.y, transforms[0].position.z);
                    //    transforms[2].position = new Vector3(transforms[0].position.x + (pointOffset / 2), transforms[0].position.y + (pointOffset / 2), transforms[0].position.z);
                    //}

                    break;
                case BasicShape.Square:
                    GetPolygonOnACircle(4, 3, Vector2.zero);
                    if (transform.eulerAngles.x == 90f) // Facing Up (Y)
                    {
                        transforms[1].position = new Vector3(transforms[0].position.x + (pointOffset), transforms[0].position.y, transforms[0].position.z);
                        transforms[2].position = new Vector3(transforms[1].position.x, transforms[1].position.y, transforms[1].position.z + (pointOffset));
                        transforms[3].position = new Vector3(transforms[2].position.x - (pointOffset), transforms[2].position.y, transforms[2].position.z);
                    }
                    else if (transform.eulerAngles.y == 90f) // Facing Right (X)
                    {
                        transforms[1].position = new Vector3(transforms[0].position.x, transforms[0].position.y, transforms[0].position.z + (pointOffset));
                        transforms[2].position = new Vector3(transforms[1].position.x, transforms[1].position.y + (pointOffset), transforms[1].position.z);
                        transforms[3].position = new Vector3(transforms[2].position.x, transforms[2].position.y, transforms[2].position.z - (pointOffset));
                    }
                    else  // Facing Forward (Z) & Camera
                    {
                        transforms[1].position = new Vector3(transforms[0].position.x + (pointOffset), transforms[0].position.y, transforms[0].position.z);
                        transforms[2].position = new Vector3(transforms[1].position.x, transforms[1].position.y + (pointOffset), transforms[1].position.z);
                        transforms[3].position = new Vector3(transforms[2].position.x - (pointOffset), transforms[2].position.y, transforms[2].position.z);
                    }

                    break;
                default:
                    break;
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

