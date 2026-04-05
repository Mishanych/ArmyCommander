using UnityEngine;

namespace ArmyCommander.Modules.Units
{
    public interface IUnitManager
    {
        bool CanSpawn { get; }
        void RegisterUnit(Unit unit);
        void UnregisterUnit(Unit unit);
        Vector3 RallyPoint { get; set; }
    }
}