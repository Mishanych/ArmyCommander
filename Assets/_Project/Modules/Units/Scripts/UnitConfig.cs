using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Units.Scripts
{
    [CreateAssetMenu(fileName = "NewUnitConfig", menuName = "Army/Unit Config")]
    public class UnitConfig : ScriptableObject
    {
        [Header("Visuals")]
        public Unit Prefab;
        public string UnitName;

        [Header("Movement")]
        public float MoveSpeed = 3.5f;
        public float StoppingDistance = 1.5f;

        [Header("Stats")]
        public float MaxHealth = 100f;
        public float Damage = 10f;
        public float AttackRange = 2f;
        public float AttackCooldown = 1.5f;

        public FactionType FactionType;
    }
}