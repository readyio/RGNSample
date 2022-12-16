using RGN.Modules.VirtualItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject nftTag;
        [SerializeField] private Button buyButton;

        private string itemId;

        public delegate void BuyButtonClickDelegate(string itemId);

        public event BuyButtonClickDelegate OnBuyButtonClick;

        private void OnEnable()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(HandleBuyButtonClick);
        }

        public void Init(VirtualItem virtualItem)
        {
            itemId = virtualItem.id;
            nftTag.SetActive(true /* TODO: virtualItem.isNFT*/);
            nameText.text = virtualItem.name;
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClick?.Invoke(itemId);
        }
    }
}