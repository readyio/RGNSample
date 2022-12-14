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
            VirtualItemModule virtualItemModule = RGNCoreBuilder.I.GetModule<VirtualItemModule>();

            var virtualItems = await virtualItemModule.GetVirtualItemsByIdsAsync(virtualItemsIds);

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

            InventoryModule inventoryModule = RGNCoreBuilder.I.GetModule<InventoryModule>();

            var userVirtualItems = await inventoryModule.GetByAppIdAsync(RGNCoreBuilder.I.AppIDForRequests);

            foreach (var inventoryData in userVirtualItems.items)
            {
                virtualItemsIds.Add(inventoryData.id);
            }

            return virtualItemsIds;
        }

        private void OnEquipButtonClick(string itemId)
        {
            //UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //InventoryModule inventoryModule = RGNCoreBuilder.I.GetModule<InventoryModule>();
            //RGNEquipItemResult equipItemResult = await inventoryModule.EquipItem(itemId);

            //PopupMessage popupMessage = new PopupMessage()
            //{
            //    Message = $"success: {equipItemResult.success}"
            //};
            //GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            //genericPopup.ShowMessage(popupMessage);
            //UIRoot.singleton.ShowPopup<GenericPopup>();

            //UIRoot.singleton.HidePopup<SpinnerPopup>();

            //Init();
        }

        public void OnCloseClick()
        {
            cancelButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }
    }
}
