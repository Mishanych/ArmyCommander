using System;
using ArmyCommander.Core;
using ArmyCommander.Modules.Animations;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Effects;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using IPoolable = ArmyCommander.Core.IPoolable;

namespace ArmyCommander.Modules.Units
{
    public class Unit : MonoBehaviour, IDamageable, IAttacker, IPoolable
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private UnitCombat _combat;
        [SerializeField] private CharacterAnimation _characterAnimation;
        [SerializeField] private HitFlash _hitFlash;

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

            transform.position = position;
            gameObject.SetActive(true);

            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
        
                _agent.enabled = true;
                _agent.speed = _config.MoveSpeed;
                _agent.stoppingDistance = _config.AttackRange;
        
                _agent.Warp(hit.position); 
            }
            else
            {
                _agent.enabled = false; 
            }

            _combat.Initialize(this);
            _characterAnimation.PlaySpawn();

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
            _characterAnimation.SetShooting(true);
            _combat.ApplyDamage();
        }
        
        public void StopAttackAnimation()
        {
            _characterAnimation.SetShooting(false);
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;

            _currentHealth -= amount;
    
            if (_hitFlash != null)
                _hitFlash.PlayFlash();

            if (IsDead)
                Die();
        }
        
        private void Die()
        {
            _characterAnimation.PlayDie();
            
            _moneySpawner.Spawn(transform.position, _config.DropType);

            _agent.enabled = false;
            _combat.enabled = false;
            _unitManager.UnregisterUnit(this, _config.FactionType);
            
            DespawnWithDelay().Forget();
        }

        private async UniTaskVoid DespawnWithDelay()
        {
            // 1. Пауза, щоб юніт просто полежав мертвим
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());

            // 2. Анімація зникання
            float duration = 1f;
    
            // Рухаємо вниз від поточної позиції
            transform.DOMoveY(transform.position.y - 1.5f, duration).SetEase(Ease.InQuad);
            // Зменшуємо масштаб
            transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);

            // 3. Замість .ToUniTask() просто чекаємо ту саму секунду
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: this.GetCancellationTokenOnDestroy());

            // 4. Повертаємо в пул
            _unitFactory?.Despawn(this, _config.Prefab);
        }
        
        private void Update()
        {
            if (_agent.enabled)
            {
                float normalizedSpeed = _agent.velocity.magnitude / _agent.speed;

                _characterAnimation.SetMoveSpeed(normalizedSpeed, 0f);

                if (normalizedSpeed > 0.1f)
                {
                    _characterAnimation.SetShooting(false);
                }
            }
        }
    }
}