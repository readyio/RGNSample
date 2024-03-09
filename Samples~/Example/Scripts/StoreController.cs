using RGN.Modules.Currency;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RGN.Sample
{
    public class StoreController : MonoBehaviour
    {
        public event Action<List<Currency>> OnCurrencyDataUpdated = null;

        public CurrencyProductsData productsData = new CurrencyProductsData();
        public List<Currency> currencies;
        public bool ActualDataLoading { get; private set; } = false;
        public bool ActualData { get; private set; } = false;

        public void RaiseOnCurrencyDataUpdated(List<Currency> currencies)
        {
            this.currencies = currencies;
            OnCurrencyDataUpdated?.Invoke(currencies);
        }

        public void LoadActualDataFromFirebase()
        {
            ActualDataLoading = true;

            // productsData = await StoreModule.I.GetProducts();
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

            RaiseOnCurrencyDataUpdated(ProfileController.CurrentUserData.currencies);

            ActualDataLoading = false;
            ActualData = true;

            Debug.Log("Actual Products data was loaded");
        }

        public async void PurchaseProduct(string productId)
        {
            OnCurrencyDataUpdated?.Invoke(await CurrencyModule.I.PurchaseCurrencyProductAsync(productId));
        }

        public Currency GetCurrency(string currencyName)
        {
            Currency firebaseCurrency = currencies.Find(x => x.name == currencyName);
            if (firebaseCurrency == null) firebaseCurrency = new Currency() { name = currencyName, quantity = 0 };
            return firebaseCurrency;
        }
    }
}
