using ArmyCommander.Core;
using ArmyCommander.Modules.Animations;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerController : MonoBehaviour, IAttacker, IDamageable
    {
        [Header("Combat Settings")]
        [SerializeField] private UnitConfig _config;
        [SerializeField] private UnitCombat _combat;
        [SerializeField] private CharacterAnimation _characterAnimation;
        
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 100f;
        
        private float _currentHealth;

        [Inject] private IUnitManager _unitManager;

        public float Health => _currentHealth;
        public bool IsDead => _currentHealth <= 0;
        public UnitConfig Config => _config;
        public IUnitManager UnitManager => _unitManager;
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _combat.Initialize(this);
            _combat.enabled = true;
            
            _unitManager.RegisterUnit(this, FactionType.Player);
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            _currentHealth -= damage;
            Debug.Log($"[Player] HP: {_currentHealth}");

            if (IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            _characterAnimation.PlayDie();
            Debug.Log("Player is Dead!");
        }

        public void MoveTo(Vector3 destination) 
        {
            _characterAnimation.SetShooting(false);
        }

        public void Stop() 
        {
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
    }
}