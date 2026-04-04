using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [Inject] public CurrencyService CurrencyService;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                collectible.Collect(this);
            }
        }
    }
}