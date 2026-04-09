using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyVisualizer : MonoBehaviour
    {
        private readonly Quaternion _rotationInStack = Quaternion.Euler(90f, 0f, 0f);
        
        [Header("Ground Detection")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundOffset = 0.2f;

        [Header("Stack Settings")]
        [SerializeField] private float _stackSmoothSpeed = 15f;
        
        [Header("Fly Settings")]
        [SerializeField] private float _flyDuration = 0.5f;
        [SerializeField] private float _arcHeight = 1.5f;
        [SerializeField] private AnimationCurve _flyCurve;
        
        [Header("Idle Animation Settings")]
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _bobSpeed = 3f;
        [SerializeField] private float _bobAmount = 0.15f;
        
        [Header("Drop Settings")]
        [SerializeField] private float _jumpPower = 1.2f;
        [SerializeField] private float _jumpDuration = 0.4f;

        private Vector3 _targetLocalPos;
        private Vector3 _idleBasePos;
        private bool _isFollowingStack;
        private bool _isIdling;
        private float _randomOffset;
        
        private void Awake() => _randomOffset = Random.Range(0f, 10f);
        
        public void SetStackTarget(Vector3 localPos)
        {
            StopIdle();
            _targetLocalPos = localPos;
            _isFollowingStack = true;
        }

        public void StopStackFollowing() => _isFollowingStack = false;
        
        public void StartIdle(Vector3 worldPos)
        {
            _idleBasePos = worldPos;
            _isFollowingStack = false;
            _isIdling = true;
        }

        public void StopIdle() => _isIdling = false;

        private void Update()
        {
            if (_isIdling)
            {
                ApplyIdleAnimation();
                return;
            }
            
            if (!_isFollowingStack) return;

            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetLocalPos, Time.deltaTime * _stackSmoothSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _rotationInStack, Time.deltaTime * _stackSmoothSpeed);
        }

        public void PlayDropAnimation(Vector3 spawnPos)
        {
            _isIdling = false;
            _isFollowingStack = false;
            transform.DOKill();

            Vector3 jumpTarget = CalculateGroundPoint(spawnPos);

            transform.DOJump(jumpTarget + Vector3.up * _groundOffset, _jumpPower, 1, _jumpDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => StartIdle(transform.position));
            
            transform.DORotate(new Vector3(0, 360, 0), _jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        }

        public async UniTask PlaySpendAnimation(Vector3 targetWorldPos, CancellationToken token)
        {
            StopAllAnimations();
            Vector3 startPos = transform.position;
            float elapsed = 0;
            while (elapsed < _flyDuration)
            {
                if (token.IsCancellationRequested) return;
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                elapsed += Time.deltaTime;
                float progress = elapsed / _flyDuration;
                Vector3 currentPos = Vector3.Lerp(startPos, targetWorldPos, progress);
                currentPos.y += _flyCurve.Evaluate(progress) * _arcHeight;
                transform.position = currentPos;
            }
        }

        public void StopAllAnimations()
        {
            _isIdling = false;
            _isFollowingStack = false;
            transform.DOKill();
        }
        
        private void ApplyIdleAnimation()
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);

            float sineWave = (Mathf.Sin((Time.time + _randomOffset) * _bobSpeed) + 1f) * 0.5f;
            float newY = _idleBasePos.y + (sineWave * _bobAmount);
            
            transform.position = new Vector3(_idleBasePos.x, newY, _idleBasePos.z);
        }
        
        private Vector3 CalculateGroundPoint(Vector3 startPos)
        {
            float range = 1.2f;
            Vector3 randomPos = startPos + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            
            if (Physics.Raycast(randomPos + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f, _groundLayer))
            {
                return hit.point;
            }
            
            if (Physics.Raycast(startPos + Vector3.up * 5f, Vector3.down, out RaycastHit hitNear, 10f, _groundLayer))
            {
                return hitNear.point;
            }

            return startPos; 
        }
    }
}