using System;
using RGN.Modules.SignIn;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class UserSettingsPopup : AbstractPopup
    {
        [Header("Authentication")] 
        [SerializeField] private Button googleButton;
        [SerializeField] private Button facebookButton;
        [SerializeField] private Button appleButton;
        [SerializeField] private Button emailButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private EmailSignUpPanel emailSignUpPanel;
        [SerializeField] private EmailSignInPanel emailSignInPanel;
        
        [SerializeField] private Button signOutButton;

        private EnumAuthProvider tryConnectProvider;
        private string email = string.Empty;
        private string password = string.Empty;
        
        private const string AccountConnectedMessage = "Your progress is now safe in the cloud!";

        private void Start()
        {
            emailSignUpPanel.OnSignUp += OnEmailSignUp;
            emailSignInPanel.OnSignIn += OnEmailLogin;
        }

        public override void Show(bool isInstant, Action onComplete)
        {
            email = string.Empty;
            password = string.Empty;

            SetAuthenticationButtons();
            googleButton.onClick.AddListener(OnGoogleLogin);
            facebookButton.onClick.AddListener(OnFBLogin);
            appleButton.onClick.AddListener(OnAppleLogin);
            emailButton.onClick.AddListener(OnOpenEmailLoginPanel);
            signOutButton.onClick.AddListener(OnEmailLogout);
            closeButton.onClick.AddListener(OnCloseClick);
            base.Show(isInstant, onComplete);
        }
        
        private void SetAuthenticationButtons()
        {
            SetAllAuthenticationButton(false);
            signOutButton.gameObject.SetActive(false);

            if (RGNCoreBuilder.I.AuthorizedProviders != 0)
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
            googleButton.gameObject.SetActive(value);
            facebookButton.gameObject.SetActive(value);
            emailButton.gameObject.SetActive(value);
#if UNITY_IPHONE
            appleButton.gameObject.SetActive(value);
#endif
        }

        public void OnCloseClick()
        {
            googleButton.onClick.RemoveListener(OnGoogleLogin);
            facebookButton.onClick.RemoveListener(OnFBLogin);
            appleButton.onClick.RemoveListener(OnAppleLogin);
            emailButton.onClick.RemoveListener(OnOpenEmailLoginPanel);
            signOutButton.onClick.RemoveListener(OnEmailLogout);
            closeButton.onClick.RemoveListener(OnCloseClick);
            Hide(true, null);
        }

        public void OnOpenEmailLoginPanel()
        {
            email = string.Empty;
            password = string.Empty;
            UIRoot.singleton.ShowPopup<EmailSignInPanel>();
        }

        public void OnFBLogin()
        {
            tryConnectProvider = EnumAuthProvider.Facebook;

            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
            FacebookSignInModule.I.TryToSignIn(true);
            SetActiveSpinner(true);
        }

        public void OnFBLogout()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged_SignOut;
            FacebookSignInModule.I.SignOut();
            SetActiveSpinner(true);
        }

        public void OnGoogleLogin()
        {
            tryConnectProvider = EnumAuthProvider.Google;

            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
            GoogleSignInModule.I.TryToSignIn(true);
            SetActiveSpinner(true);
        }

        public void OnGoogleLogout()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged_SignOut;
            GoogleSignInModule.I.SignOut();
            SetActiveSpinner(true);
        }

        public void OnAppleLogin()
        {
            tryConnectProvider = EnumAuthProvider.Apple;

            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
            AppleSignInModule.I.TryToSignIn(true);
            SetActiveSpinner(true);
        }

        public void OnAppleLogout()
        {
            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged_SignOut;
            AppleSignInModule.I.SignOut();
            SetActiveSpinner(true);
        }

        private void OnEmailSignUp(string email, string password)
        {
            this.email = email;
            this.password = password;

            tryConnectProvider = EnumAuthProvider.Email;

            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
            EmailSignInModule.I.TryToSignIn(email, password, true);
            SetActiveSpinner(true);
        }

        private void OnEmailLogin(string email, string password)
        {
            this.email = email;
            this.password = password;

            tryConnectProvider = EnumAuthProvider.Email;

            RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
            EmailSignInModule.I.TryToSignIn(email, password);
            SetActiveSpinner(true);
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

        private async void OnAuthenticationChanged(EnumLoginState enumLoginState, EnumLoginError error)
        {
            if (enumLoginState == EnumLoginState.Error || enumLoginState == EnumLoginState.Success)
            {
                RGNCoreBuilder.I.AuthenticationChanged -= OnAuthenticationChanged;

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

                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage
                        {
                            Title = "Not Connected!",
                            Message = message,
                            ButtonText = "Yes",
                            Callback = delegate
                            {
                                RGNCoreBuilder.I.AuthenticationChanged += OnAuthenticationChanged;
                                SetActiveSpinner(true);

                                switch (tryConnectProvider)
                                {
                                    case EnumAuthProvider.Apple:
                                        AppleSignInModule.I.TryToSignIn();
                                        break;
                                    case EnumAuthProvider.Facebook:
                                        FacebookSignInModule.I.TryToSignIn();
                                        break;
                                    case EnumAuthProvider.Google:
                                        GoogleSignInModule.I.TryToSignIn();
                                        break;
                                    case EnumAuthProvider.Email:
                                        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                                        {
                                            EmailSignInModule.I.TryToSignIn(email, password);
                                        }
                                        break;
                                }
                            }
                        });
                    }
                    else if (error == EnumLoginError.AccountExistsWithDifferentCredentials)
                    {
                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage
                        {
                            Title = "Not Connected!",
                            Message = "Email address is used for different provider, try login with that",
                        });
                    }
                    else
                    {
                        UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage
                        {
                            Title = "Not Connected!",
                            Message = "Unknown error",
                        });
                    }
                }
                else
                {
                    UIRoot.singleton.GetPopup<GenericPopup>().ShowMessage(new PopupMessage
                    {
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
