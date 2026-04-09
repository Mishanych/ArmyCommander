using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Economy
{
    [CreateAssetMenu(fileName = "NewSpendingZoneConfig", menuName = "Economy/Spending Zone Config")]
    public class SpendingZoneConfig : ScriptableObject
    {
        public CurrencyType RequiredType;
        public int TotalCost = 100;
        
        public string BuildingName;
        public float ConsumptionRate = 0.1f;
    }
}