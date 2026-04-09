using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArmyCommander.Modules.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxHealth = 100f;

        [Header("UI - Health")]
        [SerializeField] private GameObject _healthBarRoot;
        [SerializeField] private Slider _healthSlider;

        [Header("UI - Shield")]
        [SerializeField] private Image _shieldIcon;
        [SerializeField] private GameObject _shieldRoot;

        private float _currentHealth;
        private float _protection = 0f;
        private bool _isUIInitialized = false;

        public event Action OnDeath;
        public bool IsDead => _currentHealth <= 0;
        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _healthBarRoot.SetActive(false);
            _shieldRoot.SetActive(false);
        }

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;

            if (_healthSlider != null)
                _healthSlider.value = 1f;

            Debug.Log($"[Health] Initialized with {_maxHealth} HP");
        }

        public void UpdateShieldStatus(float protectionValue, Sprite icon)
        {
            _protection = Mathf.Clamp01(protectionValue);

            if (_shieldIcon != null && icon != null)
            {
                _shieldIcon.sprite = icon;
                
                if (_shieldRoot != null) 
                    _shieldRoot.SetActive(_protection > 0);
            }
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            float finalDamage = damage * (1f - _protection);

            if (!_isUIInitialized && _healthBarRoot != null)
            {
                _healthBarRoot.SetActive(true);
                _isUIInitialized = true;
            }

            _currentHealth -= finalDamage;

            if (_healthSlider != null)
                _healthSlider.value = _currentHealth / _maxHealth;

            if (IsDead) 
                OnDeath?.Invoke();
        }
    }
}