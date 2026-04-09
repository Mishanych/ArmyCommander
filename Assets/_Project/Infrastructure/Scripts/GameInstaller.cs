using ArmyCommander.Core;
using ArmyCommander.Input;
using ArmyCommander.Modules.Camera;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Effects;
using ArmyCommander.Modules.Level;
using ArmyCommander.Modules.Player;
using ArmyCommander.Modules.Units;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        private const int DefaultMoneyPoolSize = 10;
        private const int DefaultPlayerUnitsPoolSize = 10;
        
        [SerializeField] private JoystickInputService _joystick;
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private CameraFollower _cameraFollower;
        
        [Header("UI")]
        [SerializeField] private LevelManager _levelManager;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _silverMoneyTagPrefab;
        [SerializeField] private GameObject _goldMoneyTagPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerProvider>().AsSingle();
            Container.Bind<PlayerMovement>().FromInstance(_player).AsSingle();
            Container.Bind<CameraFollower>().FromInstance(_cameraFollower).AsSingle();
            Container.Bind<IInputService>().FromInstance(_joystick).AsSingle();
            Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
            
            Container.BindInterfacesTo<GameStartup>().AsSingle();
            
            InstallEconomy();
            InstallUnits();
            
            Container.BindFactory<Object, EffectInstance, EffectInstance.Factory>()
                .FromFactory<PrefabFactory<EffectInstance>>();
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
                .WithInitialSize(DefaultMoneyPoolSize)
                .FromComponentInNewPrefab(prefab)
                .UnderTransformGroup($"Pool_Money_{type}");
        }
        
        private void InstallUnits()
        {
            Container.BindInterfacesAndSelfTo<UnitManager>()
                .AsSingle()
                .WithArguments(DefaultPlayerUnitsPoolSize);

            Container.Bind<UnitFactory>().AsSingle();
            Container.Bind<ProjectileFactory>().AsSingle();
        }
    }
}
