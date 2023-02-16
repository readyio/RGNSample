using System.Collections.Generic;
using RGN.Modules.VirtualItems;
using UnityEngine;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpVirtualItems : MonoBehaviour
    {
        [SerializeField] private StoreTestPopUpVirtualItem itemTemplate;
        [SerializeField] private Transform itemContent;
        
        private readonly List<StoreTestPopUpVirtualItem> items = new List<StoreTestPopUpVirtualItem>();

        public delegate void BuyVirtualItemRequestDelegate(VirtualItem item);
        private event BuyVirtualItemRequestDelegate buyRequestCallback;
        
        private void Start()
        {
            itemTemplate.gameObject.SetActive(false);
        }

        public void Init(BuyVirtualItemRequestDelegate buyRequestCallback)
        {
            this.buyRequestCallback = buyRequestCallback;
            
            ClearItems();
            CreateItems();
        }

        private async void CreateItems()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();
            
            List<VirtualItem> virtualItems = await VirtualItemsModule.I.GetAllVirtualItemsByAppIdsAsync(
                new List<string> { "io.getready.rgntest" },
                1000);

            foreach (VirtualItem virtualItem in virtualItems)
            {
                // ignore virtual items without price
                if (virtualItem.prices == null || virtualItem.prices.Count == 0)
                {
                    continue;
                }
                
                StoreTestPopUpVirtualItem item = Instantiate(itemTemplate, itemContent);
                item.gameObject.SetActive(true);
                item.Init(virtualItem);
                item.OnInfoButtonClick += OnInfoButtonClick;
                item.OnBuyButtonClick += OnBuyBuyButtonClick;
            
                items.Add(item);
            }
            
            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private void ClearItems()
        {
            itemTemplate.gameObject.SetActive(false);
            
            foreach (StoreTestPopUpVirtualItem item in items)
            {
                Destroy(item.gameObject);
            }
            
            items.Clear();
        }
        
        private void OnInfoButtonClick(VirtualItem item)
        {
            PopupMessage popupMessage = new PopupMessage { Message = JsonUtility.ToJson(item) };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        private void OnBuyBuyButtonClick(VirtualItem item)
        {
            buyRequestCallback?.Invoke(item);
        }
    }
}
