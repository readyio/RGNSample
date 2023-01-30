using RGN.Modules.Store;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpStoreOffer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button buyButton;

        private StoreOffer currOffer;

        public delegate void InfoButtonClickDelegate(StoreOffer offer);
        public delegate void BuyButtonClickDelegate(StoreOffer offer);

        public event InfoButtonClickDelegate OnInfoButtonClick;
        public event BuyButtonClickDelegate OnBuyButtonClick;

        private void OnEnable()
        {
            infoButton.onClick.RemoveAllListeners();
            buyButton.onClick.RemoveAllListeners();
            
            infoButton.onClick.AddListener(HandleButtonClick);
            buyButton.onClick.AddListener(HandleBuyButtonClick);
        }

        public void Init(StoreOffer offer)
        {
            currOffer = offer;
            
            nameText.text = offer.name;
        }

        private void HandleButtonClick()
        {
            OnInfoButtonClick?.Invoke(currOffer);
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClick?.Invoke(currOffer);
        }
    }
}
