using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RGN.Modules.Store;
using RGN.Modules.VirtualItems;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUp : AbstractPopup
    {
        private enum SubWindow
        {
            MainScroll, StoreOffersScroll, VirtualItemsScroll,
            ItemSelector, PriceSelector
        }
        
        [SerializeField] private Button closeButton;
        [SerializeField] private Button storeOffersButton;
        [SerializeField] private Button virtualItemsButton;

        [SerializeField] private GameObject mainScroll;
        [SerializeField] private GameObject storeOffersScroll;
        [SerializeField] private GameObject virtualItemsScroll;
        [SerializeField] private StoreTestPopUpItemSelector itemSelector;
        [SerializeField] private StoreTestPopUpPriceSelector priceSelector;

        private readonly Stack<SubWindow> subWindowStack = new Stack<SubWindow>();

        private string currentOrderOfferId;
        private List<string> currentOrderItems;
        private List<string> currentOrderCurrencies;
        
        private void OnEnable()
        {
            closeButton.onClick.RemoveAllListeners();
            storeOffersButton.onClick.RemoveAllListeners();
            virtualItemsButton.onClick.RemoveAllListeners();
            
            closeButton.onClick.AddListener(HandleCloseClick);
            storeOffersButton.onClick.AddListener(HandleStoreOffersButton);
            virtualItemsButton.onClick.AddListener(HandleVirtualItemsButton);
        }

        public override void Show(bool isInstant, Action onComplete)
        {
            base.Show(isInstant, onComplete);
        
            Init();
        }

        private void Init()
        {
            CloseAllSubWindows();
            ShowSubWindow(SubWindow.MainScroll);
        }

        private void HandleCloseClick()
        {
            CloseCurrentSubWindow();
            
            if (subWindowStack.Count == 0)
            {
                Hide(true, null);
            }
        }

        private void HandleStoreOffersButton()
        {
            ShowSubWindow(SubWindow.StoreOffersScroll);
            
            var storeOffers = GetSubWindowGO(SubWindow.StoreOffersScroll)
                .GetComponent<StoreTestPopUpStoreOffers>();
            storeOffers.Init(OnStoreOfferPurchaseRequest);
        }
        
        private void HandleVirtualItemsButton()
        {
            ShowSubWindow(SubWindow.VirtualItemsScroll);
            
            var virtualItems = GetSubWindowGO(SubWindow.VirtualItemsScroll)
                .GetComponent<StoreTestPopUpVirtualItems>();
            virtualItems.Init(OnVirtualItemPurchaseRequest);
        }

        private void OnStoreOfferPurchaseRequest(StoreOffer storeOffer)
        {
            currentOrderOfferId = storeOffer.id;
            
            ShowSubWindow(SubWindow.ItemSelector);

            var itemSelector = GetSubWindowGO(SubWindow.ItemSelector)
                .GetComponent<StoreTestPopUpItemSelector>();
            itemSelector.Init(storeOffer.itemIds, itemIds => {
                currentOrderItems = itemIds;
                
                ShowSubWindow(SubWindow.PriceSelector);
                
                var priceSelector = GetSubWindowGO(SubWindow.PriceSelector)
                    .GetComponent<StoreTestPopUpPriceSelector>();

                List<PriceInfo> prices = storeOffer.prices
                    .Where(price => price.appIds != null && price.appIds.Contains(RGNCoreBuilder.I.AppIDForRequests))
                    .Where(price => itemIds.Contains(price.itemId))
                    .ToList();
                List<PriceInfo> preparedPrices = PreparePrices(prices, currentOrderItems);
                List<List<PriceInfo>> groupedPrices = GroupPrices(preparedPrices);

                priceSelector.Init(groupedPrices, currencies => {
                    currentOrderCurrencies = currencies;

                    ConfirmOrder();
                });
            });
        }

        private void OnVirtualItemPurchaseRequest(VirtualItem virtualItem)
        {
            currentOrderOfferId = null;
            currentOrderItems = new List<string> { virtualItem.id };
            
            ShowSubWindow(SubWindow.PriceSelector);
            
            var priceSelector = GetSubWindowGO(SubWindow.PriceSelector)
                .GetComponent<StoreTestPopUpPriceSelector>();
            
            List<PriceInfo> prices = virtualItem.prices
                .Where(price => price.appIds != null && price.appIds.Contains(RGNCoreBuilder.I.AppIDForRequests))
                .ToList();
            List<List<PriceInfo>> groupedPrices = GroupPrices(prices);
            
            priceSelector.Init(groupedPrices, currencies => {
                currentOrderCurrencies = currencies;

                ConfirmOrder();
            });
        }

        private async void ConfirmOrder()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();
            
            PurchaseResult purchaseResult = await StoreModule.I.BuyVirtualItemsAsync(
                currentOrderItems,
                currentOrderCurrencies, 
                currentOrderOfferId
            );
            
            StringBuilder purchasedItems = new StringBuilder();
            purchasedItems.Append($"OfferId: {purchaseResult.offerId}");
            purchasedItems.Append("Purchased items: ");
            purchasedItems.Append(Environment.NewLine);
            for (var i = 0; i < purchaseResult.itemIds.Count; i++)
            {
                purchasedItems.Append(purchaseResult.itemIds[i]);
                if (i < purchaseResult.itemIds.Count - 1)
                {
                    purchasedItems.Append(",");
                }
            } 
            
            PopupMessage popupMessage = new PopupMessage { Message = purchasedItems.ToString() };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
            
            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private List<List<PriceInfo>> GroupPrices(List<PriceInfo> prices)
        {
            Dictionary<string, int> groupIndexes = new Dictionary<string, int>();
            List<List<PriceInfo>> groups = new List<List<PriceInfo>>();
            
            foreach (PriceInfo price in prices)
            {
                string group = price.group;
                
                if (string.IsNullOrEmpty(group))
                {
                    groups.Add(new List<PriceInfo> { price });
                }
                else
                {
                    if (groupIndexes.ContainsKey(group))
                    {
                        groups[groupIndexes[group]].Add(price);
                    }
                    else
                    {
                        groupIndexes.Add(group, groups.Count - 1);
                        groups.Add(new List<PriceInfo> { price });
                    }
                }
            }
            
            return groups;
        }
        
        private List<PriceInfo> PreparePrices(List<PriceInfo> prices, List<string> itemIds)
        {
            List<PriceInfo> preparedPrices = new List<PriceInfo>();

            foreach (PriceInfo price in prices)
            {
                if (!itemIds.Contains(price.itemId))
                {
                    continue;
                }

                PriceInfo preparedPrice =
                    preparedPrices.FirstOrDefault(x => x.name == price.name && x.group == price.group);
                if (preparedPrice != null)
                {
                    preparedPrice.quantity += price.quantity;
                }
                else
                {
                    var newPreparedPrice = new PriceInfo(
                        price.appIds,
                        price.itemId,
                        price.name,
                        price.quantity,
                        price.group,
                        price.quantityWithoutDiscount
                    );
                    preparedPrices.Add(newPreparedPrice);
                }
            }

            return preparedPrices;
        }

        private void CloseAllSubWindows()
        {
            mainScroll.SetActive(false);
            storeOffersScroll.SetActive(false);
            virtualItemsScroll.SetActive(false);
            itemSelector.gameObject.SetActive(false);
            priceSelector.gameObject.SetActive(false);
            
            subWindowStack.Clear();
        }

        private void CloseCurrentSubWindow()
        {
            if (subWindowStack.Count == 0)
            {
                return;
            }
            
            SubWindow currSubWindow = subWindowStack.Pop();
            GameObject currSubWindowGO = GetSubWindowGO(currSubWindow);
            currSubWindowGO.SetActive(false);
        }

        private void ShowSubWindow(SubWindow subWindow)
        {
            GameObject subWindowGO = GetSubWindowGO(subWindow);
            subWindowGO.SetActive(true);
            subWindowStack.Push(subWindow);
        }

        private GameObject GetSubWindowGO(SubWindow subWindow)
        {
            return subWindow switch {
                SubWindow.MainScroll => mainScroll,
                SubWindow.StoreOffersScroll => storeOffersScroll,
                SubWindow.VirtualItemsScroll => virtualItemsScroll,
                SubWindow.ItemSelector => itemSelector.gameObject,
                SubWindow.PriceSelector => priceSelector.gameObject,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
