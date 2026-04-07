using System.Collections.Generic;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class UnitFactory
    {
        private readonly DiContainer _container;
        
        private readonly Dictionary<Unit, Queue<Unit>> _pools = new();
        
        private Transform _poolsRoot;

        public UnitFactory(DiContainer container)
        {
            _container = container;
        }

        public Unit Create(UnitConfig config, Vector3 position)
        {
            if (!_pools.TryGetValue(config.Prefab, out var queue))
            {
                queue = new Queue<Unit>();
                _pools.Add(config.Prefab, queue);
            }

            Unit unit;

            if (queue.Count > 0)
            {
                unit = queue.Dequeue();
                unit.gameObject.SetActive(true);
                unit.transform.position = position;
            }
            else
            {
                unit = _container.InstantiatePrefabForComponent<Unit>(
                    config.Prefab, 
                    position, 
                    Quaternion.identity, 
                    GetPoolRoot());
            }

            unit.OnSpawned(config, position); 
            
            return unit;
        }

        public void Despawn(Unit unit, Unit prefab)
        {
            unit.gameObject.SetActive(false);
            if (!_pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<Unit>();
                _pools.Add(prefab, queue);
            }
            queue.Enqueue(unit);
        }

        private Transform GetPoolRoot()
        {
            if (_poolsRoot == null)
                _poolsRoot = new GameObject("Dynamic_Pools").transform;
            return _poolsRoot;
        }
    }
}