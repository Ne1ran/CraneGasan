using Remote.Components;
using UnityEngine;

namespace Crane
{
    public class CraneController : MonoBehaviour
    {
        [field: SerializeField]
        public float MinZ { get; set; } = -10f;
        [field: SerializeField]
        public float MaxZ { get; set; } = 9f;
        [field: SerializeField]
        public float MinX { get; set; } = -4.5f;
        [field: SerializeField]
        public float MaxX { get; set; } = 4f;
        [field: SerializeField]
        public float MinY { get; set; } = -5f;
        [field: SerializeField]
        public float MaxY { get; set; } = -1f;

        [field: SerializeField]
        public float ForwardMoveSpeed { get; set; } = 3f;
        [field: SerializeField]
        public float BackwardsMoveSpeed { get; set; } = 3f;
        [field: SerializeField]
        public float RightMoveSpeed { get; set; } = 3f;
        [field: SerializeField]
        public float LeftMoveSpeed { get; set; } = 3f;
        [field: SerializeField]
        public float UpMoveSpeed { get; set; } = 3f;
        [field: SerializeField]
        public float DownMoveSpeed { get; set; } = 3f;

        [field: SerializeField]
        public RemoteController RemoteController { get; set; } = null!;

        [field: SerializeField]
        public Transform Crane { get; set; } = null!;
        [field: SerializeField]
        public Transform Wench { get; set; } = null!;
        [field: SerializeField]
        public Transform Hook { get; set; } = null!;

        private float _craneTargetSpeedZ;
        private float _wenchTargetSpeedX;
        private float _hookTargetSpeedY;

        private float _craneSpeedZ;
        private float _wenchSpeedX;
        private float _hookSpeedY;

        private void Awake()
        {
            RemoteController.OnUpClicked += OnUpPressed;
            RemoteController.OnDownClicked += OnDownPressed;
            RemoteController.OnEastClicked += OnEastPressed;
            RemoteController.OnWestClicked += OnWestPressed;
            RemoteController.OnNorthClicked += OnNorthPressed;
            RemoteController.OnSouthClicked += OnSouthPressed;
        }

        private void FixedUpdate()
        {
            if (!HasInput) {
                return;
            }

            float fixedDeltaTime = Time.fixedDeltaTime;

            _craneSpeedZ = Mathf.MoveTowards(_craneSpeedZ, _craneTargetSpeedZ, fixedDeltaTime);
            _wenchSpeedX = Mathf.MoveTowards(_wenchSpeedX, _wenchTargetSpeedX, fixedDeltaTime);
            _hookSpeedY = Mathf.MoveTowards(_hookSpeedY, _hookTargetSpeedY, fixedDeltaTime);

            MoveCrane(fixedDeltaTime);
            MoveWench(fixedDeltaTime);
            MoveHook(fixedDeltaTime);
        }

        private void MoveCrane(float fixedDeltaTime)
        {
            Vector3 cranePosition = Crane.localPosition;
            float prevZ = cranePosition.z;
            float nextZ = Mathf.Clamp(prevZ + _craneSpeedZ * fixedDeltaTime, MinZ, MaxZ);
            cranePosition.z = nextZ;
            Crane.localPosition = cranePosition;

            if ((nextZ >= MaxZ && _craneSpeedZ > 0f) || (nextZ <= MinZ && _craneSpeedZ < 0f)) {
                _craneSpeedZ = 0f;
                _craneTargetSpeedZ = 0f;
            }
        }

        private void MoveWench(float fixedDeltaTime)
        {
            Vector3 wenchPosition = Wench.localPosition;
            float prevX = wenchPosition.x;
            float nextX = Mathf.Clamp(prevX + _wenchSpeedX * fixedDeltaTime, MinX, MaxX);
            wenchPosition.x = nextX;
            Wench.localPosition = wenchPosition;

            if ((nextX >= MaxX && _wenchSpeedX > 0f) || (nextX <= MinX && _wenchSpeedX < 0f)) {
                _wenchSpeedX = 0f;
                _wenchTargetSpeedX = 0f;
            }
        }

        private void MoveHook(float fixedDeltaTime)
        {
            Vector3 hookPosition = Hook.localPosition;
            float prevY = hookPosition.y;
            float nextY = Mathf.Clamp(prevY + _hookSpeedY * fixedDeltaTime, MinY, MaxY);
            hookPosition.y = nextY;
            Hook.localPosition = hookPosition;

            if ((nextY >= MaxY && _hookSpeedY > 0f) || (nextY <= MinY && _hookSpeedY < 0f)) {
                _hookSpeedY = 0f;
                _hookTargetSpeedY = 0f;
            }
        }

        private void OnUpPressed()
        {
            Debug.LogWarning("Up pressed");
            _hookTargetSpeedY += UpMoveSpeed;
            _hookSpeedY = 0f;
        }

        private void OnDownPressed()
        {
            Debug.LogWarning("Down pressed");
            _hookTargetSpeedY -= DownMoveSpeed;
            _hookSpeedY = 0f;
        }

        private void OnEastPressed()
        {
            Debug.LogWarning("East pressed");
            _wenchTargetSpeedX += RightMoveSpeed;
            _wenchSpeedX = 0f;
        }

        private void OnWestPressed()
        {
            Debug.LogWarning("West pressed");
            _wenchTargetSpeedX -= LeftMoveSpeed;
            _wenchSpeedX = 0f;
        }

        private void OnNorthPressed()
        {
            Debug.LogWarning("North pressed");
            _craneTargetSpeedZ += ForwardMoveSpeed;
            _craneSpeedZ = 0f;
        }

        private void OnSouthPressed()
        {
            Debug.LogWarning("South pressed");
            _craneTargetSpeedZ -= BackwardsMoveSpeed;
            _craneSpeedZ = 0f;
        }

        private void OnDestroy()
        {
            RemoteController.OnUpClicked -= OnUpPressed;
            RemoteController.OnDownClicked -= OnDownPressed;
            RemoteController.OnEastClicked -= OnEastPressed;
            RemoteController.OnWestClicked -= OnWestPressed;
            RemoteController.OnNorthClicked -= OnNorthPressed;
            RemoteController.OnSouthClicked -= OnSouthPressed;
        }

        private bool HasInput =>
                (_hookTargetSpeedY > 0 || _hookTargetSpeedY < 0) || (_wenchTargetSpeedX > 0 || _wenchTargetSpeedX < 0)
                || (_craneTargetSpeedZ > 0 || _craneTargetSpeedZ < 0);
    }
}