using ArmyCommander.Input;
using ArmyCommander.Modules.Camera;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Player;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        private const string MoneyPoolName = "MoneyPool";
        
        [SerializeField] private JoystickInputService _joystick;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private CameraFollower _cameraFollower;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _moneyTagPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(_player).AsSingle();
            Container.Bind<CameraFollower>().FromInstance(_cameraFollower).AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            
            Container.BindMemoryPool<MoneyTag, MoneyTag.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_moneyTagPrefab)
                .UnderTransformGroup(MoneyPoolName);
            
            Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle();
            
            Container.BindInterfacesTo<GameStartup>().AsSingle();
        }
    }
}
