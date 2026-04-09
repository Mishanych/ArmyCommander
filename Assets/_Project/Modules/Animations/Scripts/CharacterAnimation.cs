using DG.Tweening;
using UnityEngine;

namespace ArmyCommander.Modules.Animations
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualModel;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnDuration = 0.5f;
        [SerializeField] private float _jumpPower = 1.5f;

        private static readonly int VelX = Animator.StringToHash("VelX");
        private static readonly int VelY = Animator.StringToHash("VelY");
        private static readonly int IsShootingBool = Animator.StringToHash("IsShooting");
        private static readonly int DieTrigger = Animator.StringToHash("Die");

        public void PlaySpawn()
        {
            _visualModel.DOKill();
            _visualModel.localScale = Vector3.zero;
            _visualModel.DOScale(1f, _spawnDuration).SetEase(Ease.OutBack);
            _visualModel.DOLocalJump(Vector3.zero, _jumpPower, 1, _spawnDuration);
        }
        
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