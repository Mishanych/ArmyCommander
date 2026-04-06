namespace ArmyCommander.Core
{
    public interface IDamageable
    {
        float Health { get; }
        bool IsDead { get; }
        void TakeDamage(float damage);
    }
}