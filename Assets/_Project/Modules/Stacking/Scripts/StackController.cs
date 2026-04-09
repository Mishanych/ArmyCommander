using System.Collections.Generic;
using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Stacking;
using UnityEngine;

namespace ArmyCommander.Modules.Stacking
{
    public class StackController : MonoBehaviour
    {
        [SerializeField] private Transform _stackRoot;
        [SerializeField] private float _yOffset = 0.1f;

        public Transform StackRoot => _stackRoot;
    
        private List<IStackable> _items = new();
        private IStackingStrategy _strategy = new GroupByTypeStrategy();

        public Vector3 AddToStack(IStackable newItem)
        {
            int index = _strategy.CalculateInsertionIndex(_items, newItem);
        
            _items.Insert(index, newItem);
            NotifyItemsPositionsChanged();

            return GetPosition(index);
        }

        public bool TryPopItem(CurrencyType type, out IStackable item)
        {
            int index = _items.FindLastIndex(i => i.Type == type);
            
            if (index != -1)
            {
                item = _items[index];
                _items.RemoveAt(index);
        
                NotifyItemsPositionsChanged();
                return true;
            }

            item = null;
            return false;
        }

        private void NotifyItemsPositionsChanged()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].UpdateTargetPosition(GetPosition(i));
            }
        }

        private Vector3 GetPosition(int index) => new Vector3(0, index * _yOffset, 0);
    }
}