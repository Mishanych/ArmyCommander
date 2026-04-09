using ArmyCommander.Modules.Level;
using ArmyCommander.Modules.Units;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Common
{
    public class KillZone : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player fell off the map!");
                _levelManager.FinishLevel(false);
            }
            else if (other.TryGetComponent(out Unit unit))
            {
                unit.TakeDamage(float.MaxValue); 
            }
        }
    }
}