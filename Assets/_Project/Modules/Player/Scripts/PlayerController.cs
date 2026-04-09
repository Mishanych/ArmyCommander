using System;
using ArmyCommander.Core;
using ArmyCommander.Modules.Animations;
using ArmyCommander.Modules.Level;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerController : MonoBehaviour, IAttacker, IDamageable
    {
        [SerializeField] private GameObject _stack;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Combat Settings")]
        [SerializeField] private UnitConfig _config;
        [SerializeField] private UnitCombat _combat;
        [SerializeField] private CharacterAnimation _characterAnimation;
        
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 100f;
        
        [Header("UI")]
        [SerializeField] private GameObject _healthBarRoot;
        [SerializeField] private Slider _healthSlider;
        
        private float _currentHealth;
        private bool _isUIInitialized = false;

        [Inject] private IUnitManager _unitManager;
        [Inject] private LevelManager _levelManager;

        public float Health => _currentHealth;
        public bool IsDead => _currentHealth <= 0;
        public UnitConfig Config => _config;
        public IUnitManager UnitManager => _unitManager;
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
            
            if (_healthBarRoot != null)
                _healthBarRoot.SetActive(false);
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

            if (!_isUIInitialized && _healthBarRoot != null)
            {
                _healthBarRoot.SetActive(true);
                _isUIInitialized = true;
            }

            _currentHealth -= damage;
    
            if (_healthSlider != null)
                _healthSlider.value = _currentHealth / _maxHealth;

            if (IsDead) 
                Die().Forget();
        }

        private async UniTaskVoid Die()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            
            _stack.SetActive(false);
            _healthBarRoot.SetActive(false);
            _characterAnimation.PlayDie();
    
            Debug.Log("Player is Dead! Waiting for animation...");

            try 
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.5f), 
                    cancellationToken: this.GetCancellationTokenOnDestroy());

                _levelManager.FinishLevel(false);
            }
            catch (OperationCanceledException) { }
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