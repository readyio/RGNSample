using RGN.Modules.VirtualItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpVirtualItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button buyButton;

        private VirtualItem currItem;

        public delegate void InfoButtonClickDelegate(VirtualItem item);
        public delegate void BuyButtonClickDelegate(VirtualItem item);

        public event InfoButtonClickDelegate OnInfoButtonClick;
        public event BuyButtonClickDelegate OnBuyButtonClick;

        private void OnEnable()
        {
            infoButton.onClick.RemoveAllListeners();
            buyButton.onClick.RemoveAllListeners();
            
            infoButton.onClick.AddListener(HandleButtonClick);
            buyButton.onClick.AddListener(HandleBuyButtonClick);
        }

        public void Init(VirtualItem item)
        {
            currItem = item;
            
            nameText.text = item.name;
        }

        private void HandleButtonClick()
        {
            OnInfoButtonClick?.Invoke(currItem);
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClick?.Invoke(currItem);
        }
    }
}
