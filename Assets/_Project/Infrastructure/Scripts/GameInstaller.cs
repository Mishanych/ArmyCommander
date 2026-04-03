using ArmyCommander.Camera;
using ArmyCommander.Input;
using ArmyCommander.Player;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private JoystickInputService _joystick;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private CameraFollower _cameraFollower;

        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(_player).AsSingle();
            Container.Bind<CameraFollower>().FromInstance(_cameraFollower).AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            
            Container.BindInterfacesTo<GameStartup>().AsSingle();
        }
    }
}
