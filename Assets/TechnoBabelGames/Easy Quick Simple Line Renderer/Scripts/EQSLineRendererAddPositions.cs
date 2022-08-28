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
        Transform parentTransform;
        float pointOffset = 3f;

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

            switch (shape)
            {
                case BasicShape.Line:

                    var transforms = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
                    transforms[1].position = new Vector3(transforms[0].position.x, transforms[0].position.y, transforms[0].position.z + (pointOffset));

                    break;
                case BasicShape.Triangle:
                    break;
                default:
                    break;
            }

            SetPoints();
        }
    }
}

