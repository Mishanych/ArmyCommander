using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArmyCommander.Input
{
    public class JoystickInputService : MonoBehaviour, IInputService, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float VisibleAlpha = 1f;
        private const float HiddenAlpha = 0f;
        private const float FadeDuration = 0.15f;
        
        [SerializeField] private RectTransform _visuals;
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public Vector2 Axis { get; private set; }
        
        private float _range = 100f;

        private void Start()
        {
            _canvasGroup.alpha = HiddenAlpha;
            _visuals.localScale = Vector3.zero;
        }
        public void Enable(bool isActive)
        {
            gameObject.SetActive(isActive);
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

            _canvasGroup.DOFade(VisibleAlpha, FadeDuration).SetEase(Ease.OutCubic);
        
            _visuals.localScale = Vector3.zero;
            _visuals.DOScale(VisibleAlpha, FadeDuration).SetEase(Ease.OutBack); 
        }
        
        private void HideJoystick()
        {
            _canvasGroup.DOKill();
            _visuals.DOKill();

            _canvasGroup.DOFade(HiddenAlpha, FadeDuration).SetEase(Ease.InCubic);
            _visuals.DOScale(HiddenAlpha, FadeDuration).SetEase(Ease.InQuad);
        }
    }
}