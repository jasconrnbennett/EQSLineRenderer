using UnityEngine;

namespace TechnoBabelGames
{
    public class EQSLinerRendererDrawGizmo : MonoBehaviour
    {
        [HideInInspector]
        public Transform targetPoint;
        [HideInInspector]
        public EQSLineRendererAddPositions parent;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (targetPoint != null)
            {
                Gizmos.DrawLine(this.transform.position, targetPoint.transform.position);                
            }
            Gizmos.DrawWireSphere(this.transform.position, 0.2f);
            //Gizmos.DrawIcon(transform.position, "sv_icon_dot8_sml"/*"scenepicking_pickable-mixed_hover"*/);

            parent.SetPoints();
        }
    }
}

