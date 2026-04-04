using ArmyCommander.Core;
using ArmyCommander.Modules.Player;
using ArmyCommander.Modules.Stacking;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyTag : MonoBehaviour, ICollectible, IStackable, IPoolable<Vector3, Quaternion>
    {
        private readonly Quaternion _rotationInStack = Quaternion.Euler(0f,0f,-90f);
        
        [SerializeField] private CurrencyType _type;
        [SerializeField] private int _amount = 1;

        [Header("Components")]
        [SerializeField] private Collider _collider;

        private Vector3 _targetLocalPos;
        private bool _isMoving;
        private float _smoothSpeed = 15f;

        public CurrencyType Type => _type;
        public void UpdateTargetPosition(Vector3 localPosition)
        {
            _targetLocalPos = localPosition;
            _isMoving = true;
        }
        
        private void Update()
        {
            if (!_isMoving) return;

            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetLocalPos, Time.deltaTime * _smoothSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _rotationInStack, Time.deltaTime * _smoothSpeed);

            if (Vector3.Distance(transform.localPosition, _targetLocalPos) < 0.01f)
            {
                transform.localPosition = _targetLocalPos;
                transform.localRotation = _rotationInStack;
                _isMoving = false;
            }
        }
        
        public void OnSpawned(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.localRotation = rotation;
            gameObject.SetActive(true);
        }
        
        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
        
        public void Collect(PlayerCollector playerCollector)
        {
            JumpToStack(playerCollector.StackController);
            playerCollector.CurrencyService.Add(_type, _amount);
        }
        
        private void JumpToStack(StackController stackController)
        {
            Vector3 targetPos = stackController.AddToStack(this);
            
            _targetLocalPos = targetPos;
        
            transform.SetParent(stackController.StackRoot);
            _collider.enabled = false;
        }
        
        public class Pool : PoolableMemoryPool<Vector3, Quaternion, MoneyTag> { }
    }
}