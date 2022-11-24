using RGN.Modules.VirtualItems;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class StoreTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Text nameText;
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

        public void Init(RGNVirtualItem virtualItem)
        {
            itemId = virtualItem.id;
            nftTag.SetActive(/*FIXME: virtualItem.isNFT*/false);
            nameText.text = virtualItem.name;
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClick?.Invoke(itemId);
        }
    }
}