using Firebase.Extensions;
using RGN;
using RGN.Modules.Currency;
using System;
using UnityEngine;

namespace RGN.Sample
{
    public class StoreController : MonoBehaviour
    {
        public event Action<UserCurrencyData> OnCurrencyDataUpdated = null;

        public CurrencyProductsData productsData = new CurrencyProductsData();
        public UserCurrencyData currencyData = new UserCurrencyData();
        public bool ActualDataLoading { get; private set; } = false;
        public bool ActualData { get; private set; } = false;

        public void RaiseOnCurrencyDataUpdated(UserCurrencyData currencyData)
        {
            this.currencyData = currencyData;
            OnCurrencyDataUpdated?.Invoke(currencyData);
        }

        public void LoadActualDataFromFirebase()
        {
            ActualDataLoading = true;

            // productsData = await CoreBuilder.I.GetModule<StoreModule>().GetProducts();
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

            RaiseOnCurrencyDataUpdated(new UserCurrencyData()
            {
                currencies = ProfileController.CurrentUserData.currencies
            });

            ActualDataLoading = false;
            ActualData = true;

            Debug.Log("Actual Products data was loaded");
        }

        public async void PurchaseProduct(string productId)
        {
            await RGNCoreBuilder.I.GetModule<CurrencyModule>().PurchaseCurrencyProductAsync(productId).ContinueWithOnMainThread(task =>
            {
                currencyData = task.Result;
                OnCurrencyDataUpdated?.Invoke(currencyData);
            });
        }

        public Currency GetCurrency(string currencyName)
        {
            Currency firebaseCurrency = currencyData.currencies.Find(x => x.name == currencyName);
            if (firebaseCurrency == null) firebaseCurrency = new Currency() { name = currencyName, quantity = 0 };
            return firebaseCurrency;
        }
    }
}
