using RGN;
using RGN.Modules.Inventory;
using RGN.Modules.VirtualItems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public class InventoryTestPopUp : AbstractPopup
    {
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private Transform itemContent;

        private List<InventoryTestPopUpItem> items = new List<InventoryTestPopUpItem>();

        public override void Show(bool isInstant, Action onComplete)
        {
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

            InventoryModule inventoryModule = RGNCoreBuilder.I.GetModule<InventoryModule>();
            RGNVirtualItems userVirtualItems = await inventoryModule.GetUserVirtualItems();

            foreach (RGNVirtualItem virtualItem in userVirtualItems.items)
            {
                GameObject itemGO = Instantiate(itemTemplate, itemContent);
                itemGO.SetActive(true);

                InventoryTestPopUpItem item = itemGO.GetComponent<InventoryTestPopUpItem>();

                item.Init(this, virtualItem, true, true);//FIXME: provide real values for booleans here
                items.Add(item);
            }

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async void OnEquipButtonClick(string itemId)
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
            Hide(true, null);
        }
    }
}