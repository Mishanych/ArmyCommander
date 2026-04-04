using ArmyCommander.Core;
using ArmyCommander.Modules.Economy;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ArmyCommander.Modules.Enemy
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [Inject] private MoneyTagSpawner _moneyTagSpawner;

        [ContextMenu("Die")]
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var gold = _moneyTagSpawner.Spawn(Random.Range(-3, 3) * new Vector3(1f,0f,1f), 
                    Quaternion.Euler(Random.Range(-90f,90f),Random.Range(-90f,90f),Random.Range(-90f,90f)), CurrencyType.Gold);
            }
            
            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                var silver = _moneyTagSpawner.Spawn(Random.Range(-2,2) * new Vector3(1f,0f,1f), 
                    Quaternion.Euler(Random.Range(-90f,90f),Random.Range(-90f,90f),Random.Range(-90f,90f)), CurrencyType.Silver);
            }
        }
    }
}