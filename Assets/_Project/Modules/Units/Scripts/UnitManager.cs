using System.Collections.Generic;
using UnityEngine;

namespace ArmyCommander.Modules.Units
{
    public class UnitManager : IUnitManager
    {
        protected readonly List<Unit> _activeUnits = new();
        private readonly int _maxUnits;

        public virtual Vector3 RallyPoint { get; set; }
        public bool CanSpawn => _activeUnits.Count < _maxUnits;

        public UnitManager(int maxUnits)
        {
            _maxUnits = maxUnits;
        }

        public virtual void RegisterUnit(Unit unit)
        {
            _activeUnits.Add(unit);
            
            int index = _activeUnits.Count - 1;
            int rows = 5;
    
            float spacing = 0.5f;
            float x = (index % rows) * spacing;
            float z = (index / rows) * spacing;
    
            Vector3 formationOffset = new Vector3(x, 0, -z);
            unit.MoveTo(RallyPoint + formationOffset);
        }

        public void UnregisterUnit(Unit unit)
        {
            _activeUnits.Remove(unit);
        }
    }
}