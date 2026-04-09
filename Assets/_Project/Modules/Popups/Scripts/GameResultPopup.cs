using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace ArmyCommander.Modules.Popups
{
    public class GameResultPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _actionButton;

        public void Show(string title, string buttonLabel, Action onAction)
        {
            gameObject.SetActive(true);
            
            _titleText.text = title;
            _buttonText.text = buttonLabel;

            _actionButton.onClick.RemoveAllListeners();
            
            _actionButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                onAction?.Invoke();
            });
        }
    }
}