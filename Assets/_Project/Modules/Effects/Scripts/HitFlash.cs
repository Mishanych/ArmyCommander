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
    
        // Для URP Lit ці назви є стандартними
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();

            // Якщо список порожній, шукаємо всі рендерери (тіло, голова, броня)
            if (_renderers.Count == 0)
            {
                _renderers.AddRange(GetComponentsInChildren<Renderer>());
            }
        }

        public void PlayFlash()
        {
            // 1. Ставимо білий колір та сильне світіння (Emission)
            foreach (var r in _renderers)
            {
                if (r == null) continue;

                r.GetPropertyBlock(_propBlock);

                // Робимо основний колір білим
                _propBlock.SetColor(BaseColorId, Color.white);
            
                // Робимо світіння дуже яскравим (інтенсивність 5-10), щоб перекрити текстуру
                _propBlock.SetColor(EmissionColorId, Color.white * 10f); 

                r.SetPropertyBlock(_propBlock);
            }

            // 2. Повертаємо все як було
            DOVirtual.DelayedCall(_flashDuration, () =>
            {
                foreach (var r in _renderers)
                {
                    if (r == null) continue;
                    // null очищує PropertyBlock і повертає налаштування самого матеріалу
                    r.SetPropertyBlock(null); 
                }
            });
        }
    }
}