using ArmyCommander.Input;
using ArmyCommander.Modules.Animations;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float MinDistanceThreshold = 0.01f;
        
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;
        
        [SerializeField] private CharacterAnimation _characterAnimation;
        
        private IInputService _inputService;
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void FixedUpdate()
        {
            Vector3 direction = new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);

            if (direction.sqrMagnitude > MinDistanceThreshold)
            {
                transform.Translate(direction * _speed * Time.fixedDeltaTime, Space.World);
                RotateTowards(direction);
            }
            
            _characterAnimation.SetMoveSpeed(direction.x, direction.z);
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