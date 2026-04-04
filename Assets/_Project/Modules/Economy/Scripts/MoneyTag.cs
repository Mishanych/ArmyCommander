using ArmyCommander.Core;
using ArmyCommander.Modules.Player;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyTag : MonoBehaviour, ICollectible, IPoolable<Vector3>
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private int _amount = 1;
        
        private Pool _pool;

        [Inject]
        public void Construct(Pool pool)
        {
            _pool = pool;
        }
        
        public void OnSpawned(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }
        
        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
        
        public void Collect(PlayerCollector playerCollector)
        {
            playerCollector.CurrencyService.Add(_type, _amount);
            _pool?.Despawn(this);
        }
        
        public class Pool : PoolableMemoryPool<Vector3, MoneyTag> { }
    }
}