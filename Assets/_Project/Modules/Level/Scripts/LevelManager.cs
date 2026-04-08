using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private UnitConfig _playerUnitConfig;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _initialRallyPoint;
        [SerializeField] private int _startingUnitsCount = 3;

        [Inject] private UnitFactory _unitFactory;
        [Inject] private UnitManager _unitManager;
        [Inject] private MoneyTagSpawner _moneySpawner;

        private void Start()
        {
            _unitManager.RallyPoint = _initialRallyPoint.position;

            SpawnInitialPlayerSquad();
            SpawnStartingLoot();
        }

        private void SpawnInitialPlayerSquad()
        {
            for (int i = 0; i < _startingUnitsCount; i++)
            {
                var unit = _unitFactory.Create(_playerUnitConfig, _playerSpawnPoint.position);
                _unitManager.RegisterUnit(unit, FactionType.Player);
            }
        }

        private void SpawnStartingLoot()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = _playerSpawnPoint.position + Random.insideUnitSphere * 2f;
                pos.y = _playerSpawnPoint.position.y;
                _moneySpawner.Spawn(pos, Quaternion.identity, CurrencyType.Silver);
            }
        }
    }
}