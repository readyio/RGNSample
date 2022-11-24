using RGN;
using RGN.Modules.Achievement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public class AchievementsTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<AchievementsTestPopUpItem> items = new List<AchievementsTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete)
        {
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

            AchievementsModule module = RGNCoreBuilder.I.GetModule<AchievementsModule>();
            GetAchievementsResponseData responseData = await module.Get();

            foreach (RGNAchievementDataV2 achievement in responseData.achievements)
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

            AchievementsModule module = RGNCoreBuilder.I.GetModule<AchievementsModule>();
            TriggerAchievementResponseData response = await module.Trigger(achievementId);

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
            Hide(true, null);
        }
    }
}