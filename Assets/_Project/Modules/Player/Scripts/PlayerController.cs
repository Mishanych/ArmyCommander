using System;
using ArmyCommander.Core;
using ArmyCommander.Modules.Animations;
using ArmyCommander.Modules.Level;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerController : MonoBehaviour, IAttacker, IDamageable
    {
        [FormerlySerializedAs("_health")] [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private GameObject _stack;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Combat Settings")]
        [SerializeField] private UnitConfig _config;
        [SerializeField] private UnitCombat _combat;
        [SerializeField] private CharacterAnimation _characterAnimation;

        [Inject] private IUnitManager _unitManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private PlayerProvider _playerProvider;

        public bool IsDead => _playerHealth != null && _playerHealth.IsDead;
        public float Health => _playerHealth != null ? _playerHealth.CurrentHealth : 0f;

        public UnitConfig Config => _config;
        public IUnitManager UnitManager => _unitManager;

        public void TakeDamage(float damage)
        {
            if (_playerHealth != null)
                _playerHealth.TakeDamage(damage);
        }

        private void Awake() 
        {
            _playerProvider.PlayerHealth = _playerHealth;
        }

        private void Start()
        {
            _combat.Initialize(this);
            _combat.enabled = true;
            
            _unitManager.RegisterUnit(this, FactionType.Player);
        }

        private void OnEnable()
        {
            if (_playerHealth != null)
                _playerHealth.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            if (_playerHealth != null)
                _playerHealth.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Die().Forget();
        }

        private async UniTaskVoid Die()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            
            _stack.SetActive(false);
            _characterAnimation.PlayDie();
    
            Debug.Log("Player is Dead! CharacterAnimation is playing...");

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