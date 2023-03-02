using RGN.Modules.Achievement;
using System;
using System.Collections.Generic;
using RGN.Model.Response;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class AchievementsTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;
        [SerializeField] private Button closeButton;

        private List<AchievementsTestPopUpItem> items = new List<AchievementsTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete)
        {
            
            closeButton.onClick.AddListener(OnCloseClick);
            base.Show(isInstant, onComplete);

            Init();
        }

        private async void Init()
        {
            itemTemplate.SetActive(false);

            foreach (AchievementsTestPopUpItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            List<RGNAchievementDataV2> achievements = await AchievementsModule.I.GetAsync();
            foreach (RGNAchievementDataV2 achievement in achievements)
            {
                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                AchievementsTestPopUpItem item = itemGO.GetComponent<AchievementsTestPopUpItem>();
                item.Init(achievement);
                item.OnTriggerButtonClick += OnTriggerButtonClick;

                items.Add(item);
            }

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async void OnTriggerButtonClick(string achievementId)
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            BaseResponseData response = await AchievementsModule.I.TriggerAsync(achievementId);

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = $"status: {response.status}\n\rmessage: {response.message}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            Init();
        }

        public void OnCloseClick()
        {
            closeButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }
    }
}