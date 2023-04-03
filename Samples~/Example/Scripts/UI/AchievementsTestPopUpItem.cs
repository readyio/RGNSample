using RGN.Modules.Achievement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class AchievementsTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Image baseImage;
        [SerializeField] private TMP_Text idText;
        [SerializeField] private Button triggerButton;

        private string achievementId;

        public delegate void TriggerButtonClickDelegate(string achievementId);

        public event TriggerButtonClickDelegate OnTriggerButtonClick;

        private void OnEnable()
        {
            triggerButton.onClick.RemoveAllListeners();
            triggerButton.onClick.AddListener(HandleTriggerButtonClick);
        }

        public void Init(AchievementData data)
        {
            achievementId = data.achievementId;

            idText.text = achievementId;
            baseImage.color = Color.white;
            triggerButton.gameObject.SetActive(true);
        }

        private void HandleTriggerButtonClick()
        {
            OnTriggerButtonClick?.Invoke(achievementId);
        }
    }
}
