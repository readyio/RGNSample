using RGN.Modules.Currency;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class InAppPurchasesTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Text description;
        [SerializeField] private Button buyButton;

        private string productId;

        public delegate void BuyButtonClickDelegate(string itemId);
        public event BuyButtonClickDelegate OnBuyButtonClick;

        private void OnEnable()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(HandleBuyButtonClick);
        }

        public void Init(RGNCurrencyProduct product)
        {
            productId = product.id;
            description.text = product.id + " " + product.currencyName + " " + product.price.ToString() + " "
                + product.quantity.ToString() + " " + product.type + " " + product.promotionalSticker;
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClick?.Invoke(productId);
        }
    }
}
