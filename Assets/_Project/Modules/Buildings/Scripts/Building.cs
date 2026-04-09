using System.Collections.Generic;
using ArmyCommander.Modules.Common;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private SpendingZone _spendingZone;
        [SerializeField] private GameObject _visualModel;
        
        [Inject] private List<IBuildingFunction> _functions;
        
        private void Awake()
        {
            _visualModel.SetActive(false);
        }
        private void OnEnable()
        {
            _spendingZone.OnPurchased.AddListener(OnBuildComplete);
        }

        private void OnDisable()
        {
            _spendingZone.OnPurchased.RemoveListener(OnBuildComplete);
        }

        private void OnBuildComplete()
        {
            _visualModel.SetActive(true);

            foreach (IBuildingFunction function in _functions)
            {
                function.Initialize();
            }
        }
    }
}