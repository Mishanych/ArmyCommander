
using UnityEngine;

namespace ArmyCommander.Modules.Player
{
    public class BillboardUI : MonoBehaviour
    {
        private Transform _mainCameraTransform;

        private void Start()
        {
            if (UnityEngine.Camera.main != null)
            {
                _mainCameraTransform = UnityEngine.Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (_mainCameraTransform == null) return;

            Vector3 targetPosition = _mainCameraTransform.position;
            targetPosition.y = transform.position.y;

            transform.LookAt(targetPosition);
        }
    }
}