using UnityEngine;

namespace ArmyCommander.Modules.Common
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseZone : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        
        protected bool IsPlayerInside;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                IsPlayerInside = true;
                OnPlayerEntered(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                IsPlayerInside = false;
                OnPlayerExited(other);
            }
        }

        protected abstract void OnPlayerEntered(Collider player);
        protected abstract void OnPlayerExited(Collider player);
    }
}