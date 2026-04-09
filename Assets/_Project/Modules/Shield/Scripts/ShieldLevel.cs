using System;
using UnityEngine;

namespace ArmyCommander.Modules.Shield
{
    [Serializable]
    public struct ShieldLevel
    {
        public int Cost;
        public Sprite Icon;
        public float Protection;
    }
}