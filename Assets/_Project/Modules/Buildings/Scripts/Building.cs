using System.Collections.Generic;
using ArmyCommander.Modules.Common;
using ArmyCommander.Modules.Effects;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private SpendingZone _spendingZone;
        [SerializeField] private GameObject _visualModel;
        
        [Header("VFX")]
        [SerializeField] private GameObject _buildCompleteEffectPrefab;
        
        [Inject] private List<IBuildingFunction> _functions;
        [Inject] private EffectInstance.Factory _vfxFactory;
        
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
            SpawnBuildEffect();
            
            foreach (IBuildingFunction function in _functions)
            {
                function.Initialize();
            }
        }
        
        private void SpawnBuildEffect()
        {
            if (_buildCompleteEffectPrefab == null) return;

            var effect = _vfxFactory.Create(_buildCompleteEffectPrefab);
            effect.PlayAt(transform.position);
        }
    }
}