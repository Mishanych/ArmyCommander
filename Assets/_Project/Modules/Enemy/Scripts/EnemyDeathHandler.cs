using ArmyCommander.Modules.Economy;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ArmyCommander.Modules.Enemy
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        private MoneyTag.Pool _moneyPool;

        [Inject]
        public void Construct(MoneyTag.Pool moneyPool)
        {
            _moneyPool = moneyPool;
        }

        [ContextMenu("Die")]
        private void Update()
        {
            // Use Keyboard.current instead of Input.GetKeyDown
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var cube = _moneyPool.Spawn(Random.value * Vector3.one);
                Debug.Log("<color=yellow>[Spawner] Spawned cube!</color>");
            }
        }
    }
}