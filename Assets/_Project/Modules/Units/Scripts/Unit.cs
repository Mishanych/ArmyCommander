using System;
using ArmyCommander.Core;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class Unit : MonoBehaviour, IDamageable, IPoolable<UnitConfig, Vector3, IMemoryPool>
    {
        [SerializeField] private NavMeshAgent _agent;

        [Inject] private UnitManager _unitManager;
        
        public float Health => _currentHealth;
        public bool IsDead => _currentHealth <= 0;
        
        private IMemoryPool _pool;
        private UnitConfig _config;
        private bool _isDead;
        private float _currentHealth;
        private float _maxHealth;

        public void OnSpawned(UnitConfig config, Vector3 position, IMemoryPool pool)
        {
            _config = config;
            transform.position = position;
            _pool = pool;
            
            _currentHealth = _config.MaxHealth;
            _maxHealth = _config.MaxHealth;
            
            _agent.speed = _config.MoveSpeed;
            _agent.stoppingDistance = _config.StoppingDistance;
            _agent.enabled = true;
            
            gameObject.SetActive(true);
        }

        public void MoveTo(Vector3 destination)
        {
            if (_agent.enabled && _agent.isOnNavMesh)
                _agent.SetDestination(destination);
        }

        public void OnDespawned()
        {
            _agent.enabled = false;
            gameObject.SetActive(false);
        }
        
        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;
            
            // Можна додати Blood VFX тут
            
            if (IsDead)
            {
                Die();
            }
        }
        
        private void Die()
        {
            // 1. Зупиняємо логіку
            _agent.isStopped = true;
            _agent.enabled = false;

            // 2. Анімація (перевір назву тригера в аніматорі!)
            //_animator.SetTrigger("Die"); 

            // 3. Прибираємо з реєстру менеджера
            _unitManager.UnregisterUnit(this, _config.FactionType);

            // 4. Повертаємо в пул після паузи (щоб дограла анімація)
            DespawnWithDelay().Forget();
        }

        private async UniTaskVoid DespawnWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f)); // 3 секунди на анімацію смерті
            _pool.Despawn(this);
        }

        public class Pool : PoolableMemoryPool<UnitConfig, Vector3, IMemoryPool, Unit> { }
    }
}