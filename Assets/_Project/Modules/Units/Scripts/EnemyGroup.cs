using ArmyCommander.Core;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class EnemyGroup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UnitConfig _config;
        [SerializeField] private int _count = 3;
        [SerializeField] private float _radius = 2f;

        [Inject] private UnitManager _registry;
        [Inject] private UnitFactory _unitFactory;

        private void Start()
        {
            SpawnGroup();
        }

        private void SpawnGroup()
        {
            for (int i = 0; i < _count; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * _radius;
                Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
                
                Unit enemy = _unitFactory.Create(_config, spawnPos);
                _registry.RegisterUnit(enemy, FactionType.Enemy);
            }
        }
    }
}