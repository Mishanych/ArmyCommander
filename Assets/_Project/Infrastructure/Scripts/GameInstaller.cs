using ArmyCommander.Core;
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
        [SerializeField] private GameObject _silverMoneyTagPrefab;
        [SerializeField] private GameObject _goldMoneyTagPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(_player).AsSingle();
            Container.Bind<CameraFollower>().FromInstance(_cameraFollower).AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            
            Container.BindMemoryPool<MoneyTag, MoneyTag.Pool>()
                .WithId(CurrencyType.Gold)
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_goldMoneyTagPrefab)
                .UnderTransformGroup(MoneyPoolName);
            
            Container.BindMemoryPool<MoneyTag, MoneyTag.Pool>()
                .WithId(CurrencyType.Silver)
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_silverMoneyTagPrefab)
                .UnderTransformGroup(MoneyPoolName);
            
            Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyTagSpawner>().AsSingle();
            
            Container.BindInterfacesTo<GameStartup>().AsSingle();
        }
    }
}
