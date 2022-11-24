using RGN.Modules.VirtualItems;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class InventoryTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Button openVirtualItemTestPopupButton;
        [SerializeField] private Image baseImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Button equipButton;

        private string itemId;

        public delegate void EquipButtonClickDelegate(string itemId);

        private InventoryTestPopUp inventoryTestPopUp;
        private RGNVirtualItem item;
        private bool doesUserOwnTheItem;

        private void OnEnable()
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(HandleEquipButtonClick);
            openVirtualItemTestPopupButton.onClick.RemoveAllListeners();
            openVirtualItemTestPopupButton.onClick.AddListener(OnOpenVirtualItemTestPopupButtonClick);
        }

        public void Init(InventoryTestPopUp parent, RGNVirtualItem virtualItem, bool isUserHave, bool isItemEquipped)
        {
            inventoryTestPopUp = parent;
            item = virtualItem;
            itemId = virtualItem.id;
            doesUserOwnTheItem = isUserHave;
            nameText.text = virtualItem.name;
        }

        private void HandleEquipButtonClick()
        {
            throw new System.NotImplementedException();
            //try
            //{
            //    UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //    InventoryModule inventoryModule = RGNCoreBuilder.I.GetModule<InventoryModule>();
            //    RGNEquipItemResult equipItemResult = await inventoryModule.EquipItem(itemId);

            //    PopupMessage popupMessage = new PopupMessage()
            //    {
            //        Message = $"success: {equipItemResult.success}"
            //    };
            //    GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            //    genericPopup.ShowMessage(popupMessage);
            //    UIRoot.singleton.ShowPopup<GenericPopup>();


            //    inventoryTestPopUp.Init();
            //}
            //finally
            //{
            //    UIRoot.singleton.HidePopup<SpinnerPopup>();
            //}
        }
        private void OnOpenVirtualItemTestPopupButtonClick()
        {
            UIRoot.singleton.ShowPopup<VirtualItemTestPopUp>();
            var itemTestPopup = UIRoot.singleton.GetPopup<VirtualItemTestPopUp>();
            itemTestPopup.Init(item, doesUserOwnTheItem);
        }
    }
}