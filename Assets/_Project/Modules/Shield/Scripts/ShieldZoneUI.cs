using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ArmyCommander.Modules.Shield
{
    public class ShieldZoneUI : MonoBehaviour
    {
        private const float PunchScaleIntensity = 0.2f;
        private const float PunchDuration = 0.3f;
        
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private string _prefix = "Lvl. : ";

        public void UpdateLevelDisplay(int level, bool isMax)
        {
            if (_headerText == null || !gameObject.activeInHierarchy) return;

            _headerText.text = isMax ? "MAX LVL" : $"{_prefix}{level + 1}";
    
            _headerText.transform.DOKill();
            _headerText.transform.DOPunchScale(Vector3.one * PunchScaleIntensity, PunchDuration)
                .SetLink(_headerText.gameObject);
        }
    }
}