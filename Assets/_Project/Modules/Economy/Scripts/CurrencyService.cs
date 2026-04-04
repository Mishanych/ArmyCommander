using System;
using System.Collections.Generic;
using ArmyCommander.Core;

namespace ArmyCommander.Modules.Economy
{
    public class CurrencyService
    {
        private readonly Dictionary<CurrencyType, int> _currencies = new();

        public event Action<CurrencyType, int> OnChanged;

        public void Add(CurrencyType type, int amount)
        {
            _currencies.TryAdd(type, 0);

            _currencies[type] += amount;
            OnChanged?.Invoke(type, _currencies[type]);
        }

        public int GetAmount(CurrencyType type)
        {
            return _currencies.TryGetValue(type, out var amount) ? amount : 0;
        }

        public bool HasEnough(CurrencyType type, int amount)
        {
            return GetAmount(type) >= amount;
        }

        public bool TrySpend(CurrencyType type, int amount)
        {
            if (HasEnough(type, amount))
            {
                _currencies[type] -= amount;
                OnChanged?.Invoke(type, _currencies[type]);
                return true;
            }
            
            return false;
        }
    }
}