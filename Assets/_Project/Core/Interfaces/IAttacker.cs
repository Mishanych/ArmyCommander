using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using UnityEngine;

namespace ArmyCommander.Core
{
    public interface IAttacker
    {
        UnitConfig Config { get; }
        IUnitManager UnitManager { get; }
        bool IsDead { get; }

        void MoveTo(Vector3 destination);
        void Stop();
        void PlayAttackAnimation();
    }
}