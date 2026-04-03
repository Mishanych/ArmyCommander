using ArmyCommander.Input;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private JoystickInputService _joystick;

        public override void InstallBindings()
        {
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            Container.BindInterfacesTo<GameStartup>().AsSingle();
        }
    }
}
