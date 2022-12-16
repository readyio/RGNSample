using RGN.Modules.Currency;
using RGN.Modules.GameProgress;
using RGN.Modules.UserProfile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class CurrenciesTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;
        [SerializeField] private Button cancelButton;

        private List<CurrenciesTestPopUpItem> items = new List<CurrenciesTestPopUpItem>();

        public override async void Show(bool isInstant, Action onComplete)
        {
            cancelButton.onClick.AddListener(OnCloseClick);
            base.Show(isInstant, onComplete);

            itemTemplate.SetActive(false);

            foreach (CurrenciesTestPopUpItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            // we can get user currencies in another way, using ProfileController.CurrentUserData.currencies
            // but loading user currencies from method return 100% actual data
            UserProfileModule<GameUserFullProfileData> userProfileModule =
                RGNCoreBuilder.I.GetModule<UserProfileModule<GameUserFullProfileData>>();

            UserCurrencyData userCurrencyData = await userProfileModule.GetUserCurrencies();
            foreach (Currency currency in userCurrencyData.currencies)
            {
                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                CurrenciesTestPopUpItem item = itemGO.GetComponent<CurrenciesTestPopUpItem>();
                item.Init(currency);

                items.Add(item);
            }

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        public void OnCloseClick()
        {
            cancelButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }
    }
}