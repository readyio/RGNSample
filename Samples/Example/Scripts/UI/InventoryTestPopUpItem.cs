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

        private string itemId;

        public delegate void EquipButtonClickDelegate(string itemId);

        private VirtualItem item;

        private void OnEnable()
        {
            openVirtualItemTestPopupButton.onClick.RemoveAllListeners();
            openVirtualItemTestPopupButton.onClick.AddListener(OnOpenVirtualItemTestPopupButtonClick);
        }

        public void Init(VirtualItem virtualItem)
        {
            item = virtualItem;
            itemId = virtualItem.id;
            nameText.text = virtualItem.name;
        }
        
        private void OnOpenVirtualItemTestPopupButtonClick()
        {
            UIRoot.singleton.ShowPopup<VirtualItemTestPopUp>();
            var itemTestPopup = UIRoot.singleton.GetPopup<VirtualItemTestPopUp>();
            itemTestPopup.Init(item, true);
        }
    }
}
