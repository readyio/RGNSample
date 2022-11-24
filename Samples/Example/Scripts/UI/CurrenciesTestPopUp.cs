using RGN;
using RGN.Modules.Currency;
using RGN.Modules.GameProgress;
using RGN.Modules.UserProfileModule;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public class CurrenciesTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<CurrenciesTestPopUpItem> items = new List<CurrenciesTestPopUpItem>();

        public override async void Show(bool isInstant, Action onComplete)
        {
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
            UserProfileModule<RGNGameUserFullProfileData> userProfileModule =
                RGNCoreBuilder.I.GetModule<UserProfileModule<RGNGameUserFullProfileData>>();

            RGNUserCurrencyData userCurrencyData = await userProfileModule.GetUserCurrencies();
            foreach (RGNCurrency currency in userCurrencyData.currencies)
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
            Hide(true, null);
        }
    }
}