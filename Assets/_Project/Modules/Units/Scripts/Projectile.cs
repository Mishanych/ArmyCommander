using ArmyCommander.Core;
using DG.Tweening;
using UnityEngine;
using Zenject;
using IPoolable = ArmyCommander.Core.IPoolable;

namespace ArmyCommander.Modules.Units
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _hitThreshold = 0.15f;
        
        [Inject] private ProjectileFactory _projectileFactory;
        
        private IDamageable _target;
        private Projectile _prefab;
        private float _damage;
        private bool _isActive;

        public void Initialize(IDamageable target, float damage, Projectile prefab)
        {
            _target = target;
            _damage = damage;
            _prefab = prefab;
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (_target == null || _target.IsDead)
            {
                ReturnToPool();
                return;
            }

            Vector3 targetPos = _target.transform.position + Vector3.up;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);

            Vector3 moveDir = (targetPos - transform.position).normalized;
            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }

            if (Vector3.Distance(transform.position, targetPos) < _hitThreshold)
            {
                _target.TakeDamage(_damage);
                ReturnToPool();
            }
        }
        
        public void OnDespawned()
        {
            _isActive = false;
            _target = null;
            _prefab = null;
            
            transform.DOKill();
        }

        private void ReturnToPool()
        {
            if (!_isActive) return;
            _isActive = false;
            
            _projectileFactory?.Despawn(this, _prefab);
        }
    }
}