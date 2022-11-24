using RGN;
using RGN.Modules.Wallets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class WalletsTestPopUp : AbstractPopup
    {
        [SerializeField] private TMP_InputField createWalletPasswordInput;
        [SerializeField] private Image createWalletPasswordRequireImage;

        private void Start()
        {
            createWalletPasswordRequireImage.gameObject.SetActive(false);
        }

        public async void OnIsUserHavePrimaryWalletButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            WalletsModule module = RGNCoreBuilder.I.GetModule<WalletsModule>();
            IsUserHavePrimaryWalletResponseData response = await module.IsUserHavePrimaryWallet();

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = JsonUtility.ToJson(response)
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public async void OnGetUserWalletsButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            WalletsModule module = RGNCoreBuilder.I.GetModule<WalletsModule>();
            GetUserWalletsResponseData response = await module.GetUserWallets();

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = JsonUtility.ToJson(response)
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public async void OnCreateUserWalletButtonClick()
        {
            string password = createWalletPasswordInput.text;
            if (string.IsNullOrEmpty(password))
            {
                createWalletPasswordRequireImage.gameObject.SetActive(true);
                return;
            }

            createWalletPasswordRequireImage.gameObject.SetActive(false);

            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            WalletsModule module = RGNCoreBuilder.I.GetModule<WalletsModule>();
            CreateWalletResponseData response = await module.CreateWallet(password);

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = JsonUtility.ToJson(response)
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public void OnCloseClick()
        {
            Hide(true, null);
        }
    }
}