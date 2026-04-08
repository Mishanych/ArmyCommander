using ArmyCommander.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ArmyCommander.Modules.Units
{
    public interface IUnitManager
    {
        bool CanSpawn { get; }
        bool IsAttackCommanded { get; }
        Vector3 RallyPoint { get; set; }
        
        UnityEvent OnAttackCommanded { get; set; }
        UnityEvent<Vector3> OnRallyPointUpdated { get; set; }
        
        void RegisterUnit(IDamageable unit, FactionType factionType);
        void UnregisterUnit(IDamageable unit, FactionType faction);
        void SetAttackState(bool active);

        IDamageable GetNearestEnemy(Vector3 myPos, FactionType myFaction);
    }
}