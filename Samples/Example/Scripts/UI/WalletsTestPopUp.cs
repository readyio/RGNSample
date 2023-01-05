using RGN.Modules.Wallets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class WalletsTestPopUp : AbstractPopup
    {
        [SerializeField] private TMP_InputField createWalletPasswordInput;
        [SerializeField] private Image createWalletPasswordRequireImage;

        [SerializeField] private Button isUserHavePrimaryWalletButton;
        [SerializeField] private Button getUserWalletsButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button createUserWalletButton;

        private void Start()
        {
            createWalletPasswordRequireImage.gameObject.SetActive(false);
        }

        public override void Show(bool isInstant, Action onComplete)
        {
            base.Show(isInstant, onComplete);

            isUserHavePrimaryWalletButton.onClick.AddListener(OnIsUserHavePrimaryWalletButtonClick);
            createUserWalletButton.onClick.AddListener(OnCreateUserWalletButtonClick);
            getUserWalletsButton.onClick.AddListener(OnGetUserWalletsButtonClick);
            cancelButton.onClick.AddListener(OnCloseClick);
        }

        public async void OnIsUserHavePrimaryWalletButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            WalletsModule module = RGNCoreBuilder.I.GetModule<WalletsModule>();
            IsUserHavePrimaryWalletResponseData response = await module.IsUserHavePrimaryWalletAsync();

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
            GetUserWalletsResponseData response = await module.GetUserWalletsAsync();

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
            CreateWalletResponseData response = await module.CreateWalletAsync(password);

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
            isUserHavePrimaryWalletButton.onClick.RemoveListener(OnIsUserHavePrimaryWalletButtonClick);
            createUserWalletButton.onClick.RemoveListener(OnCreateUserWalletButtonClick);
            getUserWalletsButton.onClick.RemoveListener(OnGetUserWalletsButtonClick);
            cancelButton.onClick.RemoveListener(OnCloseClick);

            Hide(true, null);
        }
    }
}
