using System.Collections.Generic;
using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Units
{
    public class UnitManager : IUnitManager
    {
        // Розділяємо списки
        private readonly List<Unit> _playerUnits = new();
        private readonly List<Unit> _enemyUnits = new();

        private readonly int _maxUnits;

        public virtual Vector3 RallyPoint { get; set; }
        public bool CanSpawn => _playerUnits.Count < _maxUnits;

        public UnitManager(int maxUnits)
        {
            _maxUnits = maxUnits;
        }

        // Оновлений метод реєстрації
        public void RegisterUnit(Unit unit, FactionType faction)
        {
            if (faction == FactionType.Player)
            {
                _playerUnits.Add(unit);
                ApplyFormation(unit, _playerUnits.Count - 1);
            }
            else
            {
                _enemyUnits.Add(unit);
            }
        }

        public void UnregisterUnit(Unit unit, FactionType faction)
        {
            if (faction == FactionType.Player) _playerUnits.Remove(unit);
            else _enemyUnits.Remove(unit);
        }

        // Твоя логіка з офсетами (виніс у метод для чистоти)
        private void ApplyFormation(Unit unit, int index)
        {
            int rows = 5;
            float spacing = 0.5f;
            float x = (index % rows) * spacing;
            float z = (index / rows) * spacing;
            Vector3 formationOffset = new Vector3(x, 0, -z);
            unit.MoveTo(RallyPoint + formationOffset);
        }

        // Нова фіча: пошук найближчого ворога для солдата
        public Unit GetNearestEnemy(Vector3 myPos, FactionType myFaction)
        {
            var targets = (myFaction == FactionType.Player) ? _enemyUnits : _playerUnits;

            if (targets.Count == 0) return null;

            Unit closest = null;
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