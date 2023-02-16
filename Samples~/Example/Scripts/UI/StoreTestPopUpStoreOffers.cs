using System.Collections.Generic;
using RGN.Modules.Store;
using UnityEngine;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpStoreOffers : MonoBehaviour
    {
        [SerializeField] private StoreTestPopUpStoreOffer itemTemplate;
        [SerializeField] private Transform itemContent;
        
        private readonly List<StoreTestPopUpStoreOffer> items = new List<StoreTestPopUpStoreOffer>();
        
        public delegate void BuyStoreOfferRequestDelegate(StoreOffer offer);
        private event BuyStoreOfferRequestDelegate buyRequestCallback;
        
        private void Start()
        {
            itemTemplate.gameObject.SetActive(false);
        }

        public void Init(BuyStoreOfferRequestDelegate buyRequestCallback)
        {
            this.buyRequestCallback = buyRequestCallback;
            
            ClearItems();
            CreateItems();
        }

        private async void CreateItems()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();
            
            StoreOffer[] storeOffers = await StoreModule.I.GetByAppIdsAsync(new [] { "io.getready.rgntest" }, int.MaxValue);
            
            foreach (StoreOffer storeOffer in storeOffers)
            {
                // ignore offers without prices
                if (storeOffer.prices == null || storeOffer.prices.Count == 0)
                {
                    continue;
                }
                
                StoreTestPopUpStoreOffer item = Instantiate(itemTemplate, itemContent);
                item.gameObject.SetActive(true);
                item.Init(storeOffer);
                item.OnInfoButtonClick += OnInfoButtonClick;
                item.OnBuyButtonClick += OnBuyBuyButtonClick;
            
                items.Add(item);
            }
            
            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private void ClearItems()
        {
            itemTemplate.gameObject.SetActive(false);
            
            foreach (StoreTestPopUpStoreOffer item in items)
            {
                Destroy(item.gameObject);
            }
            
            items.Clear();
        }
        
        private void OnInfoButtonClick(StoreOffer offer)
        {
            PopupMessage popupMessage = new PopupMessage { Message = JsonUtility.ToJson(offer) };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        private void OnBuyBuyButtonClick(StoreOffer offer)
        {
            buyRequestCallback?.Invoke(offer);
        }
    }
}
