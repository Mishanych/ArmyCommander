using ArmyCommander.Modules.Common;
using ArmyCommander.Modules.Player;
using UnityEngine;
using UnityEngine.Events;

namespace ArmyCommander.Modules.Economy
{
    public class SpendingZone : BaseZone
    {
        [SerializeField] private SpendingZoneConfig _config;
        
        public UnityEvent OnPurchased = new();
        public UnityEvent<int, int> OnCostChanged = new();

        public SpendingZoneConfig Config => _config;
        
        private int _remainingCost;
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

        protected override void OnPlayerEntered(Collider player)
        {
            _targetPlayer = player.GetComponent<PlayerCollector>();
        }

        protected override void OnPlayerExited(Collider player)
        {
            _targetPlayer = null;
        }

        private void Update()
        {
            if (!IsPlayerInside || _remainingCost <= 0) return;

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
                    int amountNeeded = Mathf.Min(money.Amount, _remainingCost);

                    if (amountNeeded <= 0) return;

                    if (_targetPlayer.CurrencyService.TrySpend(money.Type, amountNeeded))
                    {
                        _remainingCost -= amountNeeded;

                        int currentPaid = _config.TotalCost - _remainingCost;
                        OnCostChanged?.Invoke(currentPaid, _config.TotalCost);

                        Vector3 center = transform.position;
                        Vector3 visualTarget = center + Random.insideUnitSphere * 0.5f;
                        visualTarget.y = center.y;
                        
                        money.AnimateToSpend(visualTarget);
                    }
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