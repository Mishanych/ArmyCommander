using UnityEngine;

namespace ArmyCommander.Modules.Animations
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int VelX = Animator.StringToHash("VelX");
        private static readonly int VelY = Animator.StringToHash("VelY");
        private static readonly int IsShootingBool = Animator.StringToHash("IsShooting");
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        public void SetMoveSpeed(float x, float y)
        {
            _animator.SetFloat(VelX, x);
            _animator.SetFloat(VelY, y);
        }

        public void SetShooting(bool isShooting)
        {
            _animator.SetBool(IsShootingBool, isShooting);
        }

        public void PlayDie()
        {
            _animator.SetBool(IsShootingBool, false);
            _animator.SetTrigger(DieTrigger);
        }
    }
}