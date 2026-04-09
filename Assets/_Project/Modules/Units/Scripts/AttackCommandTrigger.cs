using ArmyCommander.Modules.Common;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class AttackCommandTrigger : MonoBehaviour
    {
        [SerializeField] private ActionZone _zone;
        [Inject] private IUnitManager _unitManager;

        private void OnEnable()
        {
            _zone.OnActionExecuted.AddListener(CommandAttack);
        }

        private void OnDisable()
        {
            _zone.OnActionExecuted.RemoveListener(CommandAttack);
        }

        private void CommandAttack()
        {
            _unitManager.SetAttackState(true);
        }
    }
}