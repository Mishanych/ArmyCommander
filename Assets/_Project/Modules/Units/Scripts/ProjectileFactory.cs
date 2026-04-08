using ArmyCommander.Core;
using ArmyCommander.Modules.Common;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class ProjectileFactory : BaseFactory<Projectile>
    {
        public ProjectileFactory(DiContainer container) : base(container) { }

        public void Spawn(Projectile prefab, Vector3 position, IDamageable target, float damage)
        {
            Vector3 direction = (target.transform.position - position).normalized;
            Quaternion rotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : Quaternion.identity;

            Projectile projectile = GetInstance(prefab, position, rotation);
            projectile.Initialize(target, damage, prefab);
        }
    }
}