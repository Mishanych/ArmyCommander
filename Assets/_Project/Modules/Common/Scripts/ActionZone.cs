using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ArmyCommander.Modules.Common
{
    public class ActionZone : BaseZone
    {
        [SerializeField] private bool _oneTimeAction = false;
        [SerializeField] private Button _actionButton;

        public UnityEvent OnActionExecuted { get; set; } = new();

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
            OnActionExecuted?.Invoke();

            if (_oneTimeAction)
            {
                _actionButton.gameObject.SetActive(false);
            }
        }
    }
}