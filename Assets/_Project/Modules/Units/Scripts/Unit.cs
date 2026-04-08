using System;
using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using IPoolable = ArmyCommander.Core.IPoolable;
using Random = UnityEngine.Random;

namespace ArmyCommander.Modules.Units
{
    public class Unit : MonoBehaviour, IDamageable, IAttacker, IPoolable
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] protected UnitCombat _combat;

        [Inject] private IUnitManager _unitManager;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private MoneyTagSpawner _moneySpawner;
        
        public float Health => _currentHealth;
        public bool IsDead => _currentHealth <= 0;
        public UnitConfig Config => _config;
        IUnitManager IAttacker.UnitManager => _unitManager;

        private UnitConfig _config;
        private UnitState _currentState;
        private float _currentHealth;
        private Vector3 _formationOffset;

        public void Initialize(UnitConfig config, Vector3 position)
        {
            _config = config;
            _currentHealth = _config.MaxHealth;

            gameObject.SetActive(true);

            _agent.enabled = true;
            _agent.speed = _config.MoveSpeed;
            _agent.stoppingDistance = _config.StoppingDistance;
            _agent.Warp(position); 

            _combat.Initialize(this);

            if (_config.FactionType == FactionType.Player)
                SetupPlayerUnit();
            else
                SetupEnemyUnit();
        }

        public void OnDespawned()
        {
            if (_config.FactionType == FactionType.Player)
            {
                _unitManager.OnAttackCommanded.RemoveListener(StartCombatBehavior);
                _unitManager.OnRallyPointUpdated.RemoveListener(HandleRallyPointUpdate);
            }
    
            _combat.enabled = false;
            _agent.enabled = false;
            gameObject.SetActive(false);
        }

        private void SetupPlayerUnit()
        {
            _unitManager.OnAttackCommanded.AddListener(StartCombatBehavior);
            _unitManager.OnRallyPointUpdated.AddListener(HandleRallyPointUpdate);

            _combat.enabled = false; 

            _currentState = UnitState.MovingToRally;
            HandleRallyPointUpdate(_unitManager.RallyPoint);
        }

        private void SetupEnemyUnit()
        {
            _currentState = UnitState.Combat;
            _combat.enabled = true; 
            _agent.isStopped = true; 
        }

        public void SetRallyOffset(Vector3 offset)
        {
            _formationOffset = offset;
            MoveTo(_unitManager.RallyPoint + _formationOffset);
        }

        private void HandleRallyPointUpdate(Vector3 newRallyPoint)
        {
            if (_currentState == UnitState.MovingToRally)
                MoveTo(newRallyPoint + _formationOffset);
        }

        private void StartCombatBehavior()
        {
            if (_currentState == UnitState.MovingToRally)
            {
                _currentState = UnitState.Combat;
                _unitManager.OnRallyPointUpdated.RemoveListener(HandleRallyPointUpdate);
                _combat.enabled = true;
            }
        }

        public void MoveTo(Vector3 destination)
        {
            if (IsDead || !_agent.enabled || !_agent.isOnNavMesh) return;
        
            _agent.isStopped = false;
            _agent.SetDestination(destination);
        }
        
        public void Stop()
        {
            if (_agent.enabled && _agent.isOnNavMesh)
                _agent.isStopped = true;
        }

        public void PlayAttackAnimation()
        {
            _combat.ApplyDamage();
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;
            if (IsDead) Die();
        }
        
        private void Die()
        {
            var money = _moneySpawner.Spawn(transform.position, Quaternion.identity, _config.DropType);
            AnimateMoneyDrop(money);

            _agent.enabled = false;
            _combat.enabled = false;
            _unitManager.UnregisterUnit(this, _config.FactionType);
            
            DespawnWithDelay().Forget();
        }

        private void AnimateMoneyDrop(MoneyTag money)
        {
            Vector3 jumpTarget = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
            money.transform.DOJump(jumpTarget, 2f, 1, 0.5f);
        }

        private async UniTaskVoid DespawnWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            _unitFactory?.Despawn(this, _config.Prefab);
        }
    }
}