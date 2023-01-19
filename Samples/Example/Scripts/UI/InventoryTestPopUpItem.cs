using RGN.Modules.VirtualItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class InventoryTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Button openVirtualItemTestPopupButton;
        [SerializeField] private Image baseImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button equipButton;

        private string itemId;

        public delegate void EquipButtonClickDelegate(string itemId);

        private VirtualItem item;

        private void OnEnable()
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(HandleEquipButtonClick);
            openVirtualItemTestPopupButton.onClick.RemoveAllListeners();
            openVirtualItemTestPopupButton.onClick.AddListener(OnOpenVirtualItemTestPopupButtonClick);
        }

        public void Init(VirtualItem virtualItem)
        {
            item = virtualItem;
            itemId = virtualItem.id;
            nameText.text = virtualItem.name;
        }

        private void HandleEquipButtonClick()
        {
            throw new System.NotImplementedException();
            //try
            //{
            //    UIRoot.singleton.ShowPopup<SpinnerPopup>();

            //    EquipItemResult equipItemResult = await InventoryModule.I.EquipItem(itemId);

            //    PopupMessage popupMessage = new PopupMessage()
            //    {
            //        Message = $"success: {equipItemResult.success}"
            //    };
            //    GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            //    genericPopup.ShowMessage(popupMessage);
            //    UIRoot.singleton.ShowPopup<GenericPopup>();


            //    inventoryTestPopUp.InitAsync();
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
            itemTestPopup.Init(item, true);
        }
    }
}
