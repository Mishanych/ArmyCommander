using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace ArmyCommander.Modules.Effects
{
    public class HitFlash : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
        [SerializeField] private float _flashDuration = 0.15f;
    
        private MaterialPropertyBlock _propBlock;
    
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();

            if (_renderers.Count == 0)
            {
                _renderers.AddRange(GetComponentsInChildren<Renderer>());
            }
        }

        public void PlayFlash()
        {
            foreach (var r in _renderers)
            {
                if (r == null) continue;

                r.GetPropertyBlock(_propBlock);

                _propBlock.SetColor(BaseColorId, Color.white);
            
                _propBlock.SetColor(EmissionColorId, Color.white * 10f); 

                r.SetPropertyBlock(_propBlock);
            }

            DOVirtual.DelayedCall(_flashDuration, () =>
            {
                foreach (var r in _renderers)
                {
                    if (r == null) continue;
                    r.SetPropertyBlock(null); 
                }
            });
        }
    }
}