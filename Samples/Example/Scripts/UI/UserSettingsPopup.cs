using System;
using RGN;
using RGN.Modules;
using RGN.Modules.EmailSignIn;
using RGN.Modules.FacebookSignIn;
using RGN.Modules.GuestSignIn;
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

            if (RGNCoreBuilder.I.authorizedProviders != 0)
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

            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
            RGNCoreBuilder.I.GetModule<FacebookSignInModule>().OnSignInFacebook(true);
            SetActiveSpinner(true);
        }

        public void OnFBLogout()
        {
            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged_SignOut;
            RGNCoreBuilder.I.GetModule<FacebookSignInModule>().SignOutFromFacebook();
            SetActiveSpinner(true);
        }

        public void OnGoogleLogin()
        {
            tryConnectProvider = EnumAuthProvider.Google;

            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
            RGNCoreBuilder.I.GetModule<GoogleSignInModule>().OnSignInGoogle(true);
            SetActiveSpinner(true);
        }

        public void OnGoogleLogout()
        {
            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged_SignOut;
            RGNCoreBuilder.I.GetModule<GoogleSignInModule>().SignOutFromGoogle();
            SetActiveSpinner(true);
        }

        public void OnAppleLogin()
        {
            tryConnectProvider = EnumAuthProvider.Apple;

            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
            RGNCoreBuilder.I.GetModule<AppleSignInModule>().OnSignInWithApple(true);
            SetActiveSpinner(true);
        }

        public void OnAppleLogout()
        {
            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged_SignOut;
            RGNCoreBuilder.I.GetModule<AppleSignInModule>().SignOutFromApple();
            SetActiveSpinner(true);
        }

        private void OnEmailSignUp(string email, string password)
        {
            this.email = email;
            this.password = password;

            tryConnectProvider = EnumAuthProvider.Email;

            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
            RGNCoreBuilder.I.GetModule<EmailSignInModule>().OnSignUpWithEmail(email, password);
            SetActiveSpinner(true);
        }

        private void OnEmailLogin(string email, string password)
        {
            this.email = email;
            this.password = password;

            tryConnectProvider = EnumAuthProvider.Email;

            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
            RGNCoreBuilder.I.GetModule<EmailSignInModule>().OnSignInWithEmail(email, password);
            SetActiveSpinner(true);
        }

        public void OnEmailLogout()
        {
            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged_SignOut;
            RGNCoreBuilder.I.GetModule<EmailSignInModule>().SignOutFromEmail();
            SetActiveSpinner(true);
        }

        public void OnGuestLogout()
        {
            RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged_SignOut;
            RGNCoreBuilder.I.GetModule<GuestSignInModule>().SignOutGuest();
            SetActiveSpinner(true);
        }

        private void OnAuthenticationChanged_SignOut(EnumLoginState enumLoginState, EnumLoginError error)
        {
            RGNCoreBuilder.I.OnAuthenticationChanged -= OnAuthenticationChanged_SignOut;
            UIRoot.singleton.HideAllPanels();
            UIRoot.singleton.HidePopup<UserSettingsPopup>();
            SetActiveSpinner(false);
            UIRoot.singleton.ShowPanel<LoadingPanel>();
        }

        private async void OnAuthenticationChanged(EnumLoginState enumLoginState, EnumLoginError error)
        {
            if (enumLoginState == EnumLoginState.Error || enumLoginState == EnumLoginState.Success)
            {
                RGNCoreBuilder.I.OnAuthenticationChanged -= OnAuthenticationChanged;

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
                                RGNCoreBuilder.I.OnAuthenticationChanged += OnAuthenticationChanged;
                                SetActiveSpinner(true);

                                switch (tryConnectProvider)
                                {
                                    case EnumAuthProvider.Apple:
                                        RGNCoreBuilder.I.GetModule<AppleSignInModule>().OnSignInWithApple();
                                        break;
                                    case EnumAuthProvider.Facebook:
                                        RGNCoreBuilder.I.GetModule<FacebookSignInModule>().OnSignInFacebook();
                                        break;
                                    case EnumAuthProvider.Google:
                                        RGNCoreBuilder.I.GetModule<GoogleSignInModule>().OnSignInGoogle();
                                        break;
                                    case EnumAuthProvider.Email:
                                        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                                        {
                                            RGNCoreBuilder.I.GetModule<EmailSignInModule>().OnSignInWithEmail(email, password);
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