using UnityEngine;
using System.Linq;
using UnityEditor;

namespace TechnoBabelGames
{
    public class EQSLineRendererAddPositions : MonoBehaviour
    {
        private Transform[] points;
        public enum BasicShape { None, Line, Triangle}
        public BasicShape basicShape;

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

        private void DrawBasicShape(BasicShape shape)
        {
            if (shape == BasicShape.None)
                return;

            switch (shape)
            {
                case BasicShape.Line:

                    Transform parentTransform = transform.GetComponentInParent<Transform>();

                    if(parentTransform.rotation.x == 90f) //Alignment Y
                    {

                    }
                    else if (parentTransform.rotation.y == 90f) //Alignment X
                    {

                    }
                    else if (parentTransform.rotation.z == 90f) //Alignment Z
                    {

                    }
                    else
                    {

                    }

                    var transforms = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
                    for (int i = 1; i < transforms.Length; i++)
                    {

                    }

                    break;
                case BasicShape.Triangle:
                    break;
                default:
                    break;
            }

        }
    }
}

