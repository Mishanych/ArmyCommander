using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Common
{
    public class BaseFactory<T> where T : MonoBehaviour
    {
        private readonly DiContainer _container;

        private readonly Dictionary<T, Queue<T>> _pools = new();

        private Transform _poolsRoot;

        public BaseFactory(DiContainer container)
        {
            _container = container;
        }

        protected T GetInstance(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (!_pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<T>();
                _pools.Add(prefab, queue);
            }

            T instance;

            if (queue.Count > 0)
            {
                instance = queue.Dequeue();
                instance.gameObject.SetActive(true);
                instance.transform.SetPositionAndRotation(position, rotation);
                if (parent != null) instance.transform.SetParent(parent);
            }
            else
            {
                instance = _container.InstantiatePrefabForComponent<T>(
                    prefab,
                    position,
                    rotation,
                    parent ?? GetPoolRoot());
            }

            return instance;
        }

        public virtual void Despawn(T instance, T prefab)
        {
            if (instance is IPoolable poolable)
            {
                poolable.OnDespawned();
            }

            instance.gameObject.SetActive(false);
            instance.transform.SetParent(GetPoolRoot());

            if (!_pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<T>();
                _pools.Add(prefab, queue);
            }

            queue.Enqueue(instance);
        }

        private Transform GetPoolRoot()
        {
            if (_poolsRoot == null)
                _poolsRoot = new GameObject($"Pool_{typeof(T).Name}").transform;
            return _poolsRoot;
        }
    }
}