using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArmyCommander.Input
{
    public class JoystickInputService : MonoBehaviour, IInputService, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _visuals;
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _range = 100f;
        [SerializeField] private float _fadeDuration = 0.15f; // Швидкість появи
        [SerializeField] private float _appearScale = 1.2f;
        
        public Vector2 Axis { get; private set; }

        private void Start()
        {
            _canvasGroup.alpha = 0;
            _visuals.localScale = Vector3.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _visuals.position = eventData.position;
            ShowJoystick();
            
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - (Vector2)_background.position;
            Axis = Vector2.ClampMagnitude(direction, _range) / _range;
        
            _handle.anchoredPosition = Axis * _range;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Axis = Vector2.zero;
            _handle.anchoredPosition = Vector2.zero;

            HideJoystick();
        }
        
        private void ShowJoystick()
        {
            _canvasGroup.DOKill();
            _visuals.DOKill();

            _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutCubic);
        
            _visuals.localScale = Vector3.zero;
            _visuals.DOScale(1f, _fadeDuration).SetEase(Ease.OutBack); 
        }
        
        private void HideJoystick()
        {
            _canvasGroup.DOKill();
            _visuals.DOKill();

            _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InCubic);
            _visuals.DOScale(0f, _fadeDuration).SetEase(Ease.InQuad);
        }
    }
}