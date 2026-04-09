using UnityEngine;

namespace ArmyCommander.Modules.Shield
{
    [CreateAssetMenu(fileName = "ShieldConfig", menuName = "Configs/ShieldConfig")]
    public class ShieldConfig : ScriptableObject
    {
        public ShieldLevel[] Levels;

        public ShieldLevel GetLevel(int index)
        {
            if (index >= Levels.Length) return Levels[Levels.Length - 1];
            return Levels[index];
        }

        public bool IsMaxLevel(int currentIndex) => currentIndex >= Levels.Length - 1;
    }
}