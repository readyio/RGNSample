using Firebase.Extensions;
using RGN;
using RGN.Modules.Currency;
using System;
using UnityEngine;

namespace ReadyGamesNetwork.Sample
{
    public class StoreController : MonoBehaviour
    {
        public event Action<RGNUserCurrencyData> OnCurrencyDataUpdated = null;

        public RGNCurrencyProductsData productsData = new RGNCurrencyProductsData();
        public RGNUserCurrencyData currencyData = new RGNUserCurrencyData();
        public bool ActualDataLoading { get; private set; } = false;
        public bool ActualData { get; private set; } = false;

        public void RaiseOnCurrencyDataUpdated(RGNUserCurrencyData currencyData)
        {
            this.currencyData = currencyData;
            OnCurrencyDataUpdated?.Invoke(currencyData);
        }

        public async void LoadActualDataFromFirebase()
        {
            ActualDataLoading = true;

            // productsData = await RGNCoreBuilder.I.GetModule<StoreModule>().GetProducts();
            // List<string> products = new List<string>();
            // productsData.products.ForEach((product) => {
            //     products.Add(product.id);
            // });
            // List<string> bundles = new List<string>();
            // productsData.offers.ForEach((offer) =>
            // {
            //     if (offer.isBundle)
            //     {
            //         bundles.Add(offer.offeredProductId);
            //     } 
            // });

            RaiseOnCurrencyDataUpdated(new RGNUserCurrencyData()
            {
                currencies = ProfileController.CurrentUserData.currencies
            });

            ActualDataLoading = false;
            ActualData = true;

            Debug.Log("Actual Products data was loaded");
        }

        public async void PurchaseProduct(string productId)
        {
            await RGNCoreBuilder.I.GetModule<CurrencyModule>().PurchaseCurrencyProduct(productId).ContinueWithOnMainThread(task =>
            {
                currencyData = task.Result;
                OnCurrencyDataUpdated?.Invoke(currencyData);
            });
        }

        public RGNCurrency GetCurrency(string currencyName)
        {
            RGNCurrency firebaseCurrency = currencyData.currencies.Find(x => x.name == currencyName);
            if (firebaseCurrency == null) firebaseCurrency = new RGNCurrency() { name = currencyName, quantity = 0 };
            return firebaseCurrency;
        }
    }
}