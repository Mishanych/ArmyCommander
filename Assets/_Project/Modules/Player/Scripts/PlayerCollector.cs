using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using ArmyCommander.Modules.Stacking;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private StackController _stackController;
        
        [Inject] public CurrencyService CurrencyService;
        public StackController StackController => _stackController;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                collectible.Collect(this);
            }
        }
    }
}