using System.Collections.Generic;
using ArmyCommander.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ArmyCommander.Modules.Units
{
    public class UnitManager : IUnitManager
    {
        private readonly List<IDamageable> _playerUnits = new();
        private readonly List<IDamageable> _enemyUnits = new();

        private readonly int _maxUnits;

        public bool IsAttackCommanded { get; private set; }
        public bool CanSpawn => _playerUnits.Count < _maxUnits;

        private Vector3 _rallyPoint;
        public Vector3 RallyPoint 
        { 
            get => _rallyPoint;
            set {
                _rallyPoint = value;
                OnRallyPointUpdated?.Invoke(_rallyPoint);
            }
        }
        
        public UnityEvent OnAttackCommanded { get; set; } = new();
        public UnityEvent<Vector3> OnRallyPointUpdated { get; set; } = new();
        
        public UnitManager(int maxUnits)
        {
            _maxUnits = maxUnits;
        }

        public void RegisterUnit(IDamageable unit, FactionType faction)
        {
            if (faction == FactionType.Player)
            {
                _playerUnits.Add(unit);
            }
            else
            {
                _enemyUnits.Add(unit);
            }
    
            if (unit is Unit soldier && faction == FactionType.Player)
            {
                Vector3 offset = CalculateFormationOffset(_playerUnits.Count - 1);
                soldier.SetRallyOffset(offset);
            }
        }

        public void UnregisterUnit(IDamageable unit, FactionType faction)
        {
            if (faction == FactionType.Player)
            {
                _playerUnits.Remove(unit);
            }
            else
            {
                _enemyUnits.Remove(unit);
            }
        }
        
        private Vector3 CalculateFormationOffset(int index)
        {
            int rows = 5;
            float spacing = 0.5f;
            float x = (index % rows) * spacing;
            float z = (index / rows) * spacing;
            return new Vector3(x, 0, -z);
        }

        public void SetAttackState(bool active)
        {
            if (IsAttackCommanded == active) return;
            
            IsAttackCommanded = active;
            
            if (active)
            {
                OnAttackCommanded?.Invoke();
            }
        }
        
        public IDamageable GetNearestEnemy(Vector3 myPos, FactionType myFaction)
        {
            var targets = (myFaction == FactionType.Player) ? _enemyUnits : _playerUnits;

            if (targets.Count == 0) return null;

            IDamageable closest = null;
            float minDist = float.MaxValue;

            foreach (var t in targets)
            {
                if (t == null || t.IsDead) continue;
                float d = Vector3.SqrMagnitude(t.transform.position - myPos);
                if (d < minDist)
                {
                    minDist = d;
                    closest = t;
                }
            }

            return closest;
        }
    }
}