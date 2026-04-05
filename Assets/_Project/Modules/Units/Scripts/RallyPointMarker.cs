using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Units
{
    public class RallyPointMarker : MonoBehaviour
    {
        [Inject(Id = "Player")] private IUnitManager _unitManager;

        private void Start()
        {
            _unitManager.RallyPoint = transform.position;
        }
    }
}