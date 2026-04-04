using ArmyCommander.Modules.Player;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 15, -10);
        [SerializeField] private float _smoothTime = 0.25f;

        private Transform _currentTarget;
        private Vector3 _currentVelocity = Vector3.zero;

        public void SetTarget(Transform target)
        {
            _currentTarget = target;
        }

        [Inject]
        public void Construct(PlayerMovement player)
        {
            SetTarget(player.transform);
        }

        private void LateUpdate()
        {
            if (_currentTarget == null) return;

            Vector3 targetPosition = _currentTarget.position + _offset;

            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref _currentVelocity, 
                _smoothTime
            );
        }
    }
}