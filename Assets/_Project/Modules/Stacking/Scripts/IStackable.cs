using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Stacking
{
    public interface IStackable
    {
        CurrencyType Type { get; }
        void UpdateTargetPosition(Vector3 localPosition);
    }
}