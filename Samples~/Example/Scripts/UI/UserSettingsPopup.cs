using System;
using RGN.Modules.SignIn;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class UserSettingsPopup : AbstractPopup
    {
        [Header("Authentication")]
        [SerializeField] private Button emailButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private Button signOutButton;

        private EnumAuthProvider tryConnectProvider;

        private const string AccountConnectedMessage = "Your progress is now safe in the cloud!";

        public override void Show(bool isInstant, Action onComplete)
        {
            SetAuthenticationButtons();
            emailButton.onClick.AddListener(OnOpenEmailLoginPanel);
            signOutButton.onClick.AddListener(OnEmailLogout);
            closeButton.onClick.AddListener(OnCloseClick);
            base.Show(isInstant, onComplete);
        }

        private void SetAuthenticationButtons()
        {
            SetAllAuthenticationButton(false);
            signOutButton.gameObject.SetActive(false);

            if (RGNCoreBuilder.I.AuthorizedProviders != EnumAuthProvider.Guest &&
                RGNCoreBuilder.I.AuthorizedProviders != EnumAuthProvider.None)
            {
                signOutButton.gameObject.SetActive(true);
            }
            else
            {
                SetAllAuthenticationButton(true);
            }
        }

        private void SetAllAuthenticationButton(bool value)
        {
            emailButton.gameObject.SetActive(value);
        }

        public void OnCloseClick()
        {
            emailButton.onClick.RemoveListener(OnOpenEmailLoginPanel);
            signOutButton.onClick.RemoveListener(OnEmailLogout);
            closeButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }

        private void OnOpenEmailLoginPanel()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChangedAsync;
            tryConnectProvider = EnumAuthProvider.Email;
            EmailSignInModule.I.TryToSignIn();
        }

        public void OnEmailLogout()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged_SignOut;
            EmailSignInModule.I.SignOut();
            SetActiveSpinner(true);
        }

        public void OnGuestLogout()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged_SignOut;
            GuestSignInModule.I.SignOut();
            SetActiveSpinner(true);
        }

        private void OnAuthenticationChanged_SignOut(EnumLoginState enumLoginState, EnumLoginError error)
        {
            RGNCoreBuilder.I.AuthenticationChanged -= OnAuthenticationChanged_SignOut;
            UIRoot.singleton.HideAllPanels();
            UIRoot.singleton.HidePopup<UserSettingsPopup>();
            SetActiveSpinner(false);
            UIRoot.singleton.ShowPanel<LoadingPanel>();
        }

        private async void OnAuthenticationChangedAsync(EnumLoginState enumLoginState, EnumLoginError error)
        {
            if (enumLoginState == EnumLoginState.Error || enumLoginState == EnumLoginState.Success)
            {
                RGNCoreBuilder.I.AuthenticationChanged -= OnAuthenticationChangedAsync;

                if (enumLoginState == EnumLoginState.Success)
                {
                    await ProfileController.LoadAndCacheAsync();

                    UIRoot.singleton.ShowPanel<HomePanel>();
                    SetAuthenticationButtons();
                }

                SetActiveSpinner(false);

                if (enumLoginState == EnumLoginState.Error)
                {
                    Debug.Log($"enumLoginState: {enumLoginState}, error: {error}");

                    if (error == EnumLoginError.AccountAlreadyLinked)
                    {
                        string message = $"Switching to this {tryConnectProvider} account will override current player settings.";

                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage {
                            Title = "Not Connected!",
                            Message = message,
                            ButtonText = "Yes",
                            Callback = delegate {
                                RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChangedAsync;
                                SetActiveSpinner(true);

                                switch (tryConnectProvider)
                                {
                                    case EnumAuthProvider.Email:
                                        EmailSignInModule.I.TryToSignIn();
                                        break;
                                }
                            }
                        });
                    }
                    else if (error == EnumLoginError.AccountExistsWithDifferentCredentials)
                    {
                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage {
                            Title = "Not Connected!",
                            Message = "Email address is used for different provider, try login with that",
                        });
                    }
                    else
                    {
                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage {
                            Title = "Not Connected!",
                            Message = "Unknown error",
                        });
                    }
                }
                else
                {
                    UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage {
                        Title = "Connected!",
                        Message = AccountConnectedMessage,
                    });
                }
            }
        }

        private void SetActiveSpinner(bool value)
        {
            if (value)
            {
                UIRoot.singleton.ShowPopup<SpinnerPopup>();
            }
            else
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
            }
        }
    }
}
