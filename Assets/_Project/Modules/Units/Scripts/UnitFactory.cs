using ArmyCommander.Modules.Common;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class UnitFactory : BaseFactory<Unit>
    {
        public UnitFactory(DiContainer container) : base(container) { }

        public Unit Create(UnitConfig config, Vector3 position)
        {
            Unit unit = GetInstance(config.Prefab, position, Quaternion.identity);
            unit.Initialize(config, position);
        
            return unit;
        }
    }
}