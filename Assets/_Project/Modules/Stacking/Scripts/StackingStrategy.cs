using System.Collections.Generic;

namespace ArmyCommander.Modules.Stacking
{
    public interface IStackingStrategy
    {
        int CalculateInsertionIndex(List<IStackable> items, IStackable newItem);
    }
    
    public class GroupByTypeStrategy : IStackingStrategy
    {
        public int CalculateInsertionIndex(List<IStackable> items, IStackable newItem)
        {
            int lastIndex = items.FindLastIndex(i => i.Type == newItem.Type);
            return (lastIndex != -1) ? lastIndex + 1 : items.Count;
        }
    }
}