using ArmyCommander.Modules.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ArmyCommander.Modules.Economy
{
    public class SpendingZone : MonoBehaviour
    {
        [SerializeField] private SpendingZoneConfig _config;
        
        public UnityEvent OnPurchased = new();
        public UnityEvent<int, int> OnCostChanged = new();

        public SpendingZoneConfig Config => _config;
        
        private int _remainingCost;
        private bool _isPlayerInZone;
        private float _timer;
        private PlayerCollector _targetPlayer;

        private void Start()
        {
            if (_config != null)
            {
                _remainingCost = _config.TotalCost;
                OnCostChanged?.Invoke(_config.TotalCost - _remainingCost, _config.TotalCost);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCollector collector))
            {
                _targetPlayer = collector;
                _isPlayerInZone = true;
            }
        }
        
        private void OnTriggerExit(Collider other) => _isPlayerInZone = false;
        
        private void Update()
        {
            if (!_isPlayerInZone || _remainingCost <= 0) return;

            _timer += Time.deltaTime;
            if (_timer >= _config.ConsumptionRate)
            {
                _timer = 0;
                TryConsume();
            }
        }
        
        private void TryConsume()
        {
            if (_remainingCost <= 0) return;
            if (!_targetPlayer.CurrencyService.HasEnough(_config.RequiredType, 1)) return;
            
            if (_targetPlayer.StackController.TryPopItem(_config.RequiredType, out var item))
            {
                if (item is MoneyTag money)
                {
                    _targetPlayer.CurrencyService.Subtract(money.Type, money.Amount);
                    _remainingCost -= money.Amount;
                    
                    int currentPaid = _config.TotalCost - _remainingCost;
                    OnCostChanged?.Invoke(currentPaid, _config.TotalCost);

                    Vector3 center = transform.position;
                    Vector3 visualTarget = center + Random.insideUnitSphere * 0.5f;
                    visualTarget.y = center.y;

                    money.AnimateToSpend(visualTarget);
                }

                if (_remainingCost <= 0)
                {
                    OnPurchased?.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}