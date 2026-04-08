using ArmyCommander.Input;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private Animator _animator;
        
        private IInputService _inputService;
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void FixedUpdate()
        {
            Vector3 direction = new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.Translate(direction * _speed * Time.fixedDeltaTime, Space.World);
                RotateTowards(direction);
            }
            
            _animator.SetFloat("VelX", direction.x);
            _animator.SetFloat("VelY", direction.y);
        }
        
        private void RotateTowards(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                _rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}