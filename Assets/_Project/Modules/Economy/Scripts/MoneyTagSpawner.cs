using ArmyCommander.Core;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Economy
{
    public class MoneyTagSpawner
    {
        private readonly MoneyTag.Pool _goldPool;
        private readonly MoneyTag.Pool _silverPool;

        public MoneyTagSpawner(
            [Inject(Id = CurrencyType.Gold)] MoneyTag.Pool goldPool,
            [Inject(Id = CurrencyType.Silver)] MoneyTag.Pool silverPool)
        {
            _goldPool = goldPool;
            _silverPool = silverPool;
        }

        public MoneyTag Spawn(Vector3 position, CurrencyType type)
        {
            return type switch
            {
                CurrencyType.Gold => _goldPool.Spawn(position, _goldPool),
                CurrencyType.Silver => _silverPool.Spawn(position, _silverPool),
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
    }
}