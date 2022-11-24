using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

namespace ReadyGamesNetwork.Sample.UI
{
    public class EmailSignUpPanel : AbstractPopup
    {
        [SerializeField] private InputField emailInputField;
        [SerializeField] private InputField passwordInputField;
        [SerializeField] private Button signUpButton;

        private string email = string.Empty;
        private string password = string.Empty;

        public event Action<string, string> OnSignUp = null;

        // 6 to 20 characters which contain at least one numeric digit, one uppercase and one lowercase letter
        private Regex passwordRegex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$");
        private Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        private void Start() {
            emailInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            passwordInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        public override void Show(bool isInstant, Action onComplete) {
            base.Show(isInstant, onComplete);

            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
        }

        public void OnCloseClick() {
            UIRoot.singleton.HidePopup<EmailSignUpPanel>();
        }

        public void OnSignUpClick() {
            OnSignUp?.Invoke(email, password);
            UIRoot.singleton.HidePopup<EmailSignUpPanel>();
        }

        public void OnSignInClick() {
            UIRoot.singleton.HidePopup<EmailSignUpPanel>();
            UIRoot.singleton.ShowPopup<EmailSignInPanel>();
        }

        private void OnInputFieldValueChanged(string value) {
            email = emailInputField.text.Trim();
            password = passwordInputField.text.Trim();

            Match emailMatch = emailRegex.Match(email);
            Match passwordMatch = passwordRegex.Match(password);

            signUpButton.interactable = emailMatch.Success && passwordMatch.Success;
        }
    }
}
