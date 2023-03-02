using RGN.Modules.Inventory;
using RGN.Modules.VirtualItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class InventoryTestPopUp : AbstractPopup
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<InventoryTestPopUpItem> items = new List<InventoryTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete)
        {
            cancelButton.onClick.AddListener(OnCloseClick);
            base.Show(isInstant, onComplete);

            Init();
        }

        internal async void Init()
        {
            itemTemplate.SetActive(false);

            foreach (InventoryTestPopUpItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            List<string> virtualItemsIds = await GetVirtualItemsInventoryIdsAsync();
            if (virtualItemsIds.Count == 0)
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
                return;
            }
            var virtualItems = await VirtualItemsModule.I.GetVirtualItemsByIdsAsync(virtualItemsIds);

            foreach (VirtualItem virtualItem in virtualItems)
            {
                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                InventoryTestPopUpItem item = itemGO.GetComponent<InventoryTestPopUpItem>();

                item.Init(virtualItem);
                items.Add(item);
            }

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async Task<List<string>> GetVirtualItemsInventoryIdsAsync() {
            List<string> virtualItemsIds = new List<string>();

            var userVirtualItems = await InventoryModule.I.GetAllForCurrentAppAsync();

            foreach (var inventoryData in userVirtualItems)
            {
                virtualItemsIds.Add(inventoryData.virtualItemId);
            }

            return virtualItemsIds;
        }

        public void OnCloseClick()
        {
            cancelButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }
    }
}
