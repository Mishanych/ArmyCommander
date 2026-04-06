using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class RallyPointMarker : MonoBehaviour
    {
        [Inject] private IUnitManager _unitManager;

        private void Start()
        {
            _unitManager.RallyPoint = transform.position;
        }
    }
}