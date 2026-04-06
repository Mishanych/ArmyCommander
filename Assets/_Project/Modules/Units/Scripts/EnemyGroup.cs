using ArmyCommander.Core;
using ArmyCommander.Infrastructure;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class EnemyGroup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private UnitConfig _enemyType;
        [SerializeField] private int _count = 3;
        [SerializeField] private float _radius = 2f;

        [Inject(Id = GameInstaller.EnemyId)] private Unit.Pool _unitPool;
        [Inject] private UnitManager _registry;

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

                Unit enemy = _unitPool.Spawn(_enemyType, spawnPos, _unitPool);
            
                _registry.RegisterUnit(enemy, FactionType.Enemy);
            }
        }
    }
}