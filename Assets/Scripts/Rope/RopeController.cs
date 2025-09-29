using UnityEngine;

namespace Rope
{
    public class RopeController : MonoBehaviour
    {
        [field: SerializeField]
        public Transform StartTarget { get; set; }
        [field: SerializeField]
        public Transform EndTarget { get; set; }

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            _lineRenderer.positionCount = 2;
        }

        private void LateUpdate()
        {
            if (!StartTarget || !EndTarget) {
                return;
            }

            Vector3 pos1 = StartTarget.position;
            Vector3 pos2 = EndTarget.position;

            _lineRenderer.SetPosition(0, pos1);
            _lineRenderer.SetPosition(1, pos2);
        }
    }
}