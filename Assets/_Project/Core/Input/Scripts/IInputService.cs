using UnityEngine;

namespace ArmyCommander.Input
{
    public interface IInputService
    {
        void Enable(bool isActive);
        Vector2 Axis { get; }
    }
}
