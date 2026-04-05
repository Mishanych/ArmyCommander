using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class Unit : MonoBehaviour, IPoolable<UnitConfig, Vector3>
    {
        [SerializeField] private NavMeshAgent _agent;
        
        private UnitConfig _config;
        private float _currentHealth;

        public void OnSpawned(UnitConfig config, Vector3 position)
        {
            _config = config;
            
            transform.position = position;
            _currentHealth = _config.MaxHealth;
            
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

        public class Pool : PoolableMemoryPool<UnitConfig, Vector3, Unit> { }
    }
}