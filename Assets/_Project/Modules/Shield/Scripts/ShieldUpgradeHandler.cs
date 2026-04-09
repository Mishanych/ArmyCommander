using ArmyCommander.Modules.Common;
using ArmyCommander.Modules.Effects;
using ArmyCommander.Modules.Player;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Shield
{
    public class ShieldUpgradeHandler : MonoBehaviour
    {
        [SerializeField] private SpendingZone _zone;
        [SerializeField] private ShieldConfig _config;

        [SerializeField] private ShieldZoneUI _zoneUI;
        [SerializeField] private GameObject _shieldUpgradeEffectPrefab;
        
        [Inject] private PlayerProvider _playerProvider;
        [Inject] private EffectInstance.Factory _effectFactory;
        
        private int _currentLevelIndex = 0;

        private void Start()
        {
            UpdateZonePrice();
            UpdateVisuals();
        }

        private void OnEnable()
        {
            _zone.OnPurchased.AddListener(HandleUpgrade);
        }

        private void OnDisable()
        {
            _zone.OnPurchased.RemoveListener(HandleUpgrade);
        }

        private void HandleUpgrade()
        {
            if (_config.IsMaxLevel(_currentLevelIndex)) return;

            _currentLevelIndex++;

            var currentLevelData = _config.GetLevel(_currentLevelIndex);
            if (_playerProvider.PlayerHealth != null)
            {
                _playerProvider.PlayerHealth.UpdateShieldStatus(currentLevelData.Protection, currentLevelData.Icon);
                SpawnVFX(_shieldUpgradeEffectPrefab);
            }

            UpdateVisuals();
            UpdateZonePrice();
            
            Debug.Log($"Shield Upgraded to Level {_currentLevelIndex}!");
        }

        private void UpdateZonePrice()
        {
            if (_config.IsMaxLevel(_currentLevelIndex))
            {
                _zone.enabled = false;
                return;
            }

            int nextCost = _config.Levels[_currentLevelIndex + 1].Cost;
            _zone.SetRequiredAmount(nextCost);
        }

        private void UpdateVisuals()
        {
            bool isMax = _config.IsMaxLevel(_currentLevelIndex);
            _zoneUI.UpdateLevelDisplay(_currentLevelIndex, isMax);
        }
        
        private void SpawnVFX(GameObject prefab)
        {
            if (prefab == null) return;
            
            var effect = _effectFactory.Create(prefab);
            effect.PlayAt(_playerProvider.PlayerHealth.transform);
        }
    }
}