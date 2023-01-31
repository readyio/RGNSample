using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class EmailSignInPanel : AbstractPopup
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button signInButton;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button forgotPasswordButton;
        

        private string email = string.Empty;
        private string password = string.Empty;

        public event Action<string, string> OnSignIn = null;

        // 6 to 20 characters which contain at least one numeric digit, one uppercase and one lowercase letter
        private Regex passwordRegex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$");
        private Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        private void Start() {
            emailInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            passwordInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        public override void Show(bool isInstant, Action onComplete) {
            base.Show(isInstant, onComplete);
            
            signUpButton.onClick.AddListener(OnSignUpClick);
            signInButton.onClick.AddListener(OnSignInClick);
            cancelButton.onClick.AddListener(OnCloseClick);
            forgotPasswordButton.onClick.AddListener(OnForgotPasswordClick);
            
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
        }

        public void OnCloseClick() {
            signUpButton.onClick.RemoveListener(OnSignUpClick);
            signInButton.onClick.RemoveListener(OnSignInClick);
            cancelButton.onClick.RemoveListener(OnCloseClick);
            forgotPasswordButton.onClick.RemoveListener(OnForgotPasswordClick);
            
            UIRoot.singleton.HidePopup<EmailSignInPanel>();
        }

        public void OnSignInClick() {
            OnSignIn?.Invoke(email, password);
            UIRoot.singleton.HidePopup<EmailSignInPanel>();
        }

        public void OnSignUpClick() {
            UIRoot.singleton.HidePopup<EmailSignInPanel>();
            UIRoot.singleton.ShowPopup<EmailSignUpPanel>();
        }

        public void OnForgotPasswordClick() {
            UIRoot.singleton.HidePopup<EmailSignInPanel>();
            UIRoot.singleton.ShowPopup<ForgotPasswordPanel>();
        }

        private void OnInputFieldValueChanged(string value) {
            email = emailInputField.text.Trim();
            password = passwordInputField.text.Trim();

            Match emailMatch = emailRegex.Match(email);
            Match passwordMatch = passwordRegex.Match(password);

            signInButton.interactable = emailMatch.Success && passwordMatch.Success;
        }
    }
}