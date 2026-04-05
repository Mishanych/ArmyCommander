using ArmyCommander.Core;
using ArmyCommander.Input;
using ArmyCommander.Modules.Camera;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Player;
using ArmyCommander.Modules.Units;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        private const string MoneyPoolName = "MoneyPool";
        private const string PlayerId = "Player";
        
        [SerializeField] private JoystickInputService _joystick;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private CameraFollower _cameraFollower;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _silverMoneyTagPrefab;
        [SerializeField] private GameObject _goldMoneyTagPrefab;
        [SerializeField] private GameObject _unitPrefab;

        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(_player).AsSingle();
            Container.Bind<CameraFollower>().FromInstance(_cameraFollower).AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            
            Container.BindInterfacesTo<GameStartup>().AsSingle();
            
            InstallEconomy();
            InstallUnits();
        }

        private void InstallEconomy()
        {
            Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoneyTagSpawner>().AsSingle();
            
            BindMoneyPool(CurrencyType.Gold, _goldMoneyTagPrefab);
            BindMoneyPool(CurrencyType.Silver, _silverMoneyTagPrefab);
        }
        
        private void BindMoneyPool(CurrencyType type, GameObject prefab)
        {
            Container.BindMemoryPool<MoneyTag, MoneyTag.Pool>()
                .WithId(type)
                .WithInitialSize(10)
                .FromComponentInNewPrefab(prefab)
                .UnderTransformGroup($"Pool_Money_{type}");
        }
        
        private void InstallUnits()
        {
            Container.Bind<IUnitManager>()
                .WithId(PlayerId)
                .To<UnitManager>()
                .AsSingle()
                .WithArguments(15);

            Container.BindMemoryPool<Unit, Unit.Pool>()
                .WithInitialSize(15)
                .FromComponentInNewPrefab(_unitPrefab)
                .UnderTransformGroup("Pool_Units");
        }
    }
}
