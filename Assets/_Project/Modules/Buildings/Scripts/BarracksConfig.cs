using UnityEngine;

namespace ArmyCommander.Modules.Units
{
    [CreateAssetMenu(fileName = "BarracksConfig", menuName = "Army/Buildings/Barracks Config")]
    public class BarracksConfig : ScriptableObject
    {
        public string BuildingName;
        public float SpawnCooldown = 5f;
    }
}