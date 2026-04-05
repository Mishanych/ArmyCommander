using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyVisualizer : MonoBehaviour
    {
        private readonly Quaternion _rotationInStack = Quaternion.Euler(0f,0f,-90f);
        
        [SerializeField] private float _stackSmoothSpeed = 15f;
        [SerializeField] private float _flyDuration = 0.5f;

        [SerializeField] private float _arcHeight = 1.5f;
        [SerializeField] private AnimationCurve _flyCurve;

        private Vector3 _targetLocalPos;
        private bool _isFollowingStack;

        public void SetStackTarget(Vector3 localPos)
        {
            _targetLocalPos = localPos;
            _isFollowingStack = true;
        }

        public void StopStackFollowing() => _isFollowingStack = false;

        private void Update()
        {
            if (!_isFollowingStack) return;

            transform.localPosition =
                Vector3.Lerp(transform.localPosition, _targetLocalPos, Time.deltaTime * _stackSmoothSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _rotationInStack,
                Time.deltaTime * _stackSmoothSpeed);
        }

        public async UniTask PlaySpendAnimation(Vector3 targetWorldPos, CancellationToken token)
        {
            StopStackFollowing();

            Vector3 startPos = transform.position;

            float elapsed = 0;
            while (elapsed < _flyDuration)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                elapsed += Time.deltaTime;
                float progress = elapsed / _flyDuration;

                Vector3 currentPos = Vector3.Lerp(startPos, targetWorldPos, progress);
                currentPos.y += _flyCurve.Evaluate(progress) * _arcHeight;

                transform.position = currentPos;
            }
        }
    }
}