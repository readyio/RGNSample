using RGN.Modules.SignIn;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    
    public class ForgotPasswordPanel : AbstractPopup
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button resetPasswordButton;
        [SerializeField] private TMP_InputField emailInputField;

        public override void Show(bool isInstant, Action onComplete)
        {
            base.Show(isInstant, onComplete);
            
            cancelButton.onClick.AddListener(OnCancelClick);
            resetPasswordButton.onClick.AddListener(OnResetPassswordClick);
            
            emailInputField.text = string.Empty;
        }

        public void OnCancelClick()
        {
            cancelButton.onClick.RemoveListener(OnCancelClick);
            resetPasswordButton.onClick.RemoveListener(OnResetPassswordClick);
            
            UIRoot.singleton.HidePopup<ForgotPasswordPanel>();
            UIRoot.singleton.ShowPopup<EmailSignInPanel>();
        }


        public void OnResetPassswordClick()
        {
            RGNCoreBuilder.I.GetModule<EmailSignInModule>().SendPasswordResetEmail(emailInputField.text.Trim());
            UIRoot.singleton.HidePopup<ForgotPasswordPanel>();
        }
    }
}
