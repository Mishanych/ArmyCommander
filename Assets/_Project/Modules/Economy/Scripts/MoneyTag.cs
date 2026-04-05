using System.Threading;
using ArmyCommander.Core;
using ArmyCommander.Modules.Player;
using ArmyCommander.Modules.Stacking;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyTag : MonoBehaviour, ICollectible, IStackable, IPoolable<Vector3, Quaternion, IMemoryPool>
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private int _amount = 1;

        [Header("Components")]
        [SerializeField] private Collider _collider;
        [SerializeField] private MoneyVisualizer _visualizer;

        private CancellationTokenSource _cts;
        private IMemoryPool _pool;
        private Transform _parent;

        public CurrencyType Type => _type;
        public int Amount => _amount;
        
        public void UpdateTargetPosition(Vector3 localPosition)
        {
            _visualizer.SetStackTarget(localPosition);
        }
        
        public void OnSpawned(Vector3 position, Quaternion rotation, IMemoryPool pool)
        {
            _pool = pool;
            _parent = transform.parent;
            
            transform.position = position;
            transform.localRotation = rotation;
            gameObject.SetActive(true);
        }
        
        public void OnDespawned()
        {
            _collider.enabled = true;
            transform.SetParent(_parent);
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
        
            transform.SetParent(stackController.StackRoot);
            _collider.enabled = false;
            
            _visualizer.SetStackTarget(targetPos);
        }
        
        public async void AnimateToSpend(Vector3 targetWorldPos)
        {
            PrepareForConsumption();

            _cts = new CancellationTokenSource();
            try
            {
                await _visualizer.PlaySpendAnimation(targetWorldPos, _cts.Token);
                _pool?.Despawn(this);
            }
            catch (System.OperationCanceledException) { }
        }

        private void PrepareForConsumption()
        {
            transform.SetParent(null);
            _collider.enabled = false;
            _visualizer.StopStackFollowing();
        }
        
        public class Pool : PoolableMemoryPool<Vector3, Quaternion, IMemoryPool, MoneyTag> { }
    }
}