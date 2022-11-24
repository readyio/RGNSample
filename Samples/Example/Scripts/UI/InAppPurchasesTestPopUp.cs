using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGN;
using RGN.Model;
using RGN.Modules;
using RGN.Modules.Currency;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public class InAppPurchasesTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<InAppPurchasesTestPopUpItem> items = new List<InAppPurchasesTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete) {
            base.Show(isInstant, onComplete);

            Init();
        }

        private async void Init() {
            itemTemplate.SetActive(false);

            foreach (InAppPurchasesTestPopUpItem item in items) {
                Destroy(item.gameObject);
            }
            items.Clear();

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            await InitPurchases();

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async Task InitPurchases() {
            CurrencyModule inAppPurchaseModule = RGNCoreBuilder.I.GetModule<CurrencyModule>();

            RGNCurrencyProductsData productsData = await inAppPurchaseModule.GetInAppPurchaseCurrencyData();

            foreach (RGNCurrencyProduct product in productsData.products) {
                //TODO: you have to register your products in Unity IAP ConfigurationBuilder,
                // use "product.type" to determine Consumable/Non-Consumable products

                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                InAppPurchasesTestPopUpItem item = itemGO.GetComponent<InAppPurchasesTestPopUpItem>();
                item.Init(product);
                item.OnBuyButtonClick += OnBuyProductButtonClick;

                items.Add(item);
            }
        }

        private async void OnBuyProductButtonClick(string productId) {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //TODO: you need to Call IAP purchase method from Unity IAP Package,
            // use OnSuccefullPurchase event of IAP Package for calling of our PurchaseProduct method,
            // we don't do purchase validation on your side
            CurrencyModule inAppPurchaseModule = RGNCoreBuilder.I.GetModule<CurrencyModule>();
            RGNUserCurrencyData currencyData = await inAppPurchaseModule.PurchaseCurrencyProduct(productId);

            string result = "";

            foreach (RGNCurrency currency in currencyData.currencies) {
                result += "/n" + currency.name + " : " + currency.quantity.ToString();
            }

            PopupMessage popupMessage = new PopupMessage() {
                Message = $"currency data : {result}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        public async void OnBuyRGNCoinButtonClick(string iapUUID) {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //TODO: you need to Call IAP purchase method from Unity IAP Package,
            // use OnSuccefullPurchase event of IAP Package for calling of our PurchaseRGNCoin method,
            // we don't do purchase validation on your side
            // You will get isoCurrenyCode and localizedPrice from Unity IAP product's metadata
            // More details : https://docs.unity3d.com/Manual/UnityIAPBrowsingMetadata.html
            CurrencyModule inAppPurchaseModule = RGNCoreBuilder.I.GetModule<CurrencyModule>();
            RGNPurchaseRGNCoinResponseData purchaseRGNCoinResult = await inAppPurchaseModule.PurchaseRGNCoin(iapUUID);

            string result = "";
            if (purchaseRGNCoinResult.success) {
                foreach (RGNCurrency currency in purchaseRGNCoinResult.currencies) {
                    result += "/n" + currency.name + " : " + currency.quantity.ToString();
                }
            }
            else {
                result += "failed";
            }

            PopupMessage popupMessage = new PopupMessage() {
                Message = $"currency data : {result}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }


        public void OnCloseClick() {
            Hide(true, null);
        }
    }
}