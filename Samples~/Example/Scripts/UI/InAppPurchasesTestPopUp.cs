using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGN.Modules.Currency;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class InAppPurchasesTestPopUp : AbstractPopup
    {
        [SerializeField] private string testId;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<InAppPurchasesTestPopUpItem> items = new List<InAppPurchasesTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete)
        {
            closeButton.onClick.AddListener(OnCloseClick);
            buyButton.onClick.AddListener(OnBuyRGNCoinButtonClick);
            base.Show(isInstant, onComplete);

            InitAsync();
        }

        private async void InitAsync()
        {
            itemTemplate.SetActive(false);

            foreach (InAppPurchasesTestPopUpItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            await InitPurchasesAsync();

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async Task InitPurchasesAsync()
        {
            CurrencyProductsData productsData = await CurrencyModule.I.GetInAppPurchaseCurrencyDataAsync();

            foreach (CurrencyProduct product in productsData.products)
            {
                //TODO: you have to register your products in Unity IAP ConfigurationBuilder,
                // use "product.type" to determine Consumable/Non-Consumable products

                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                InAppPurchasesTestPopUpItem item = itemGO.GetComponent<InAppPurchasesTestPopUpItem>();
                item.Init(product);
                item.OnBuyButtonClick += OnBuyProductButtonClickAsync;

                items.Add(item);
            }
        }

        private void OnBuyRGNCoinButtonClick()
        {
            OnBuyRGNCoinAsync(testId);
        }
        private async void OnBuyProductButtonClickAsync(string productId)
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //TODO: you need to Call IAP purchase method from Unity IAP Package,
            // use OnSuccefullPurchase event of IAP Package for calling of our PurchaseProduct method,
            // we don't do purchase validation on your side
            var currencies = await CurrencyModule.I.PurchaseCurrencyProductAsync(productId);

            string result = "";

            foreach (Currency currency in currencies)
            {
                result += "\n" + currency.name + " : " + currency.quantity.ToString();
            }

            PopupMessage popupMessage = new PopupMessage() {
                Message = $"currency data : {result}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        public async void OnBuyRGNCoinAsync(string iapUUID)
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //TODO: you need to Call IAP purchase method from Unity IAP Package,
            // use OnSuccefullPurchase event of IAP Package for calling of our PurchaseRGNCoin method,
            // we don't do purchase validation on your side
            // You will get isoCurrenyCode and localizedPrice from Unity IAP product's metadata
            // More details : https://docs.unity3d.com/Manual/UnityIAPBrowsingMetadata.html
            var currencies = await CurrencyModule.I.PurchaseRGNCoinAsync(iapUUID, string.Empty, string.Empty);

            string result = "";
            foreach (Currency currency in currencies)
            {
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


        public void OnCloseClick()
        {
            Hide(true, null);
        }
    }
}
