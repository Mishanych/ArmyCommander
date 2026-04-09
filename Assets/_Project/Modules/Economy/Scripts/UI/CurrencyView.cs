using ArmyCommander.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Economy
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _textRectTransform;
        
        [Header("Animation Settings")]
        [SerializeField] private float _punchScaleAmount = 0.2f;
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private int _vibrato = 5;
        [SerializeField] private float _elasticity = 1f;

        private CurrencyService _currencyService;

        [Inject]
        public void Construct(CurrencyService currencyService)
        {
            _currencyService = currencyService;
            _currencyService.OnChanged += UpdateView;

            UpdateView(_type, _currencyService.GetAmount(_type));
        }

        private void UpdateView(CurrencyType type, int amount)
        {
            if (type == _type)
            {
                _text.text = amount.ToString();
                AnimateBounce();
            }
        }
        
        private void AnimateBounce()
        {
            _textRectTransform.DOKill(true); 
            
            _textRectTransform.localScale = Vector3.one;
            _textRectTransform.DOPunchScale(Vector3.one * _punchScaleAmount, _duration, _vibrato, _elasticity)
                .SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            if (_currencyService != null)
                _currencyService.OnChanged -= UpdateView;
        }
    }
}