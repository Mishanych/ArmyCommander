using ArmyCommander.Input;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        
        private IInputService _inputService;
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void FixedUpdate()
        {
            Vector3 direction = new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }
}