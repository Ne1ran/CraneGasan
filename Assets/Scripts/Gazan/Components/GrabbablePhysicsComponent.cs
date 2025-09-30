using UnityEngine;

namespace Gazan.Components
{
    public class GrabbablePhysicsComponent : MonoBehaviour
    {
        private Rigidbody _grabRigidbody = null!;
        private Collider _grabCollider = null!;

        private void Awake()
        {
            _grabRigidbody = GetComponent<Rigidbody>();
            _grabCollider = GetComponent<Collider>();
        }
        
        public void OnGrabbed()
        {
            _grabCollider.isTrigger = true;
            _grabRigidbody.useGravity = false;
        }

        public void OnReleased()
        {
            _grabCollider.isTrigger = false;
            _grabRigidbody.useGravity = true;
        }
    }
}