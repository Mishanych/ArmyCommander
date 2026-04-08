using ArmyCommander.Core;
using UnityEngine;

namespace ArmyCommander.Modules.Units.Scripts
{
    [CreateAssetMenu(fileName = "NewUnitConfig", menuName = "Army/Unit Config")]
    public class UnitConfig : ScriptableObject
    {
        [Header("Visuals")]
        public Unit Prefab;
        public Projectile ProjectilePrefab;
        public string UnitName;

        [Header("Movement")]
        public bool IsStationary;
        public float MoveSpeed = 3.5f;
        public float StoppingDistance = 1.5f;

        [Header("Stats")]
        public float MaxHealth = 100f;
        public AttackType AttackType;
        public float Damage = 10f;
        public float AttackRange = 2f;
        public float AttackCooldown = 1.5f;

        [Header("Loot Settings")]
        public CurrencyType DropType;
        public int DropAmount = 1;
        
        public FactionType FactionType;
    }
}