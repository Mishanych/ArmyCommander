using UnityEngine;

namespace ArmyCommander.Core
{
    public interface IDamageable
    {
        float Health { get; }
        bool IsDead { get; }
        Transform transform { get; }
        void TakeDamage(float damage);
    }
}