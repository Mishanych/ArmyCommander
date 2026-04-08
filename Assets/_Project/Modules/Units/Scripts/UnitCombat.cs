using ArmyCommander.Core;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class UnitCombat : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;

        [Inject] private IUnitManager _unitManager;
        [Inject] private ProjectileFactory _projectileFactory;

        private IAttacker _attacker;
        private IDamageable _currentTarget;

        private float _searchTimer;
        private float _attackCooldownTimer;
        private const float SearchInterval = 0.5f;

        public void Initialize(IAttacker attacker)
        {
            _attacker = attacker;
            enabled = true;
        }

        private void Update()
        {
            if (_attacker == null || _attacker.IsDead) return;

            HandleTargeting();

            if (_currentTarget == null || _currentTarget.IsDead)
            {
                _attacker.Stop();
                return;
            }

            HandleCombatBehavior();
        }

        private void HandleTargeting()
        {
            _searchTimer -= Time.deltaTime;
            if (_searchTimer <= 0)
            {
                _currentTarget = _unitManager.GetNearestEnemy(
                    transform.position,
                    _attacker.Config.FactionType);

                if (_currentTarget == null)
                {
                    _attacker.StopAttackAnimation();
                }

                _searchTimer = SearchInterval;
            }
        }

        private void HandleCombatBehavior()
        {
            var targetMB = _currentTarget as MonoBehaviour;
            if (targetMB == null || _currentTarget.IsDead)
            {
                _currentTarget = null;
                _attacker.StopAttackAnimation();
                return;
            }

            float distance = Vector3.Distance(transform.position, targetMB.transform.position);

            if (distance <= _attacker.Config.AttackRange)
            {
                _attacker.Stop();
                RotateTowardsTarget(targetMB.transform.position);
                _attacker.PlayAttackAnimation();

                if (Time.time >= _attackCooldownTimer)
                {
                    ApplyDamage();
                    _attackCooldownTimer = Time.time + _attacker.Config.AttackCooldown;
                }
            }
            else
            {
                _attacker.StopAttackAnimation();

                if (!_attacker.Config.IsStationary)
                {
                    _attacker.MoveTo(targetMB.transform.position);
                }
            }
        }

        private void RotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }

        public void ApplyDamage()
        {
            if (_attacker == null || _currentTarget == null || _currentTarget.IsDead) return;

            if (_attacker.Config.AttackType == AttackType.Ranged)
            {
                Vector3 spawnPos = _firePoint != null ? _firePoint.position : transform.position;

                _projectileFactory.Spawn(
                    _attacker.Config.ProjectilePrefab,
                    spawnPos,
                    _currentTarget,
                    _attacker.Config.Damage);
            }
            else
            {
                _currentTarget.TakeDamage(_attacker.Config.Damage);
            }
        }
    }
}