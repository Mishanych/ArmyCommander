using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArmyCommander.Modules.Economy
{
    public class SpendZoneView : MonoBehaviour
    {
        [SerializeField] private SpendingZone _spendZone;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _previewImage;
        [SerializeField] private Image _progressBar;

        private void Start()
        {
            _progressBar.fillAmount = 0f;
            _costText.text = string.Empty;
        }

        private void OnEnable()
        {
            _spendZone.OnCostChanged.AddListener(UpdateUI);

            TogglePreviewImage(true);
        }

        private void OnDisable()
        {
            _spendZone.OnCostChanged.RemoveListener(UpdateUI);
        }

        private void UpdateUI(int current, int total)
        {
            if (current > 0)
            {
                TogglePreviewImage(false);
            }
            
            _costText.text = $"{current} / {total}";

            if (_progressBar != null)
            {
                _progressBar.fillAmount = (float)current / total;
            }
        }

        private void TogglePreviewImage(bool toEnable)
        {
            if (_previewImage != null)
            {
                _previewImage.gameObject.SetActive(toEnable);
            }
        }
    }
}