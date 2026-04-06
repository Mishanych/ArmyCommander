using System;
using System.Threading;
using ArmyCommander.Core;
using ArmyCommander.Infrastructure;
using ArmyCommander.Modules.Units;
using ArmyCommander.Modules.Units.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Building
{
    public class BarracksFunction : MonoBehaviour, IBuildingFunction
    {
        [SerializeField] private BarracksConfig _config;
        [SerializeField] private UnitConfig _unitToSpawn;
        [SerializeField] private string _managerId = "Player";

        [Inject(Id = GameInstaller.EnemyId)]  private Unit.Pool _unitPool;
        [Inject] private DiContainer _container;
        
        private IUnitManager _unitManager;
        private CancellationTokenSource _cts;

        public void Initialize()
        {
            _unitManager = _container.ResolveId<IUnitManager>(_managerId);

            StopSpawning();
            _cts = new CancellationTokenSource();
    
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
                _cts.Token, 
                this.GetCancellationTokenOnDestroy()
            ).Token;

            SpawnUnitsAsync(linkedToken).Forget();
        }

        public void Execute()
        {
            
        }
        
        public void StopSpawning()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid SpawnUnitsAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_config.SpawnCooldown), cancellationToken: token);

                    if (_unitManager.CanSpawn)
                    {
                        Unit unit = _unitPool.Spawn(_unitToSpawn, transform.position, _unitPool);
                        _unitManager.RegisterUnit(unit, FactionType.Player);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[Barracks] Spawning is stopped for {gameObject.name}");
            }
        }
    }
}