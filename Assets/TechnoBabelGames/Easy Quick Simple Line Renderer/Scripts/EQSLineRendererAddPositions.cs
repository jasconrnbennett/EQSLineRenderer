using UnityEngine;
using System.Linq;

namespace TechnoBabelGames
{
    public class EQSLineRendererAddPositions : MonoBehaviour
    {
        private Transform[] points;

        void Start()
        {
            SetPoints();
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
    }
}

