using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Units
{
    public interface IUnitManager
    {
        bool CanSpawn { get; }
        void RegisterUnit(Unit unit, FactionType factionType);
        void UnregisterUnit(Unit unit, FactionType faction);
        Vector3 RallyPoint { get; set; }
    }
}