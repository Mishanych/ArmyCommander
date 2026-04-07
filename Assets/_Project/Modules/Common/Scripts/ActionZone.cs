using ArmyCommander.Modules.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class ActionZone : BaseZone
    {
        [SerializeField] private Button _actionButton;
        [Inject] private IUnitManager _unitManager;

        private void Start()
        {
            _actionButton.gameObject.SetActive(false);
            _actionButton.onClick.AddListener(HandleClick);
        }

        protected override void OnPlayerEntered(Collider player)
        {
            _actionButton.gameObject.SetActive(true);
        }

        protected override void OnPlayerExited(Collider player)
        {
            _actionButton.gameObject.SetActive(false);
        }

        private void HandleClick()
        {
            _unitManager.SetAttackState(true);
            _actionButton.gameObject.SetActive(false);
        }
    }
}