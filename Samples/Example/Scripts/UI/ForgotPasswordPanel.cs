using RGN;
using RGN.Modules.EmailSignIn;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class ForgotPasswordPanel : AbstractPopup
    {
        [SerializeField] private InputField emailInputField;

        public override void Show(bool isInstant, Action onComplete)
        {
            base.Show(isInstant, onComplete);

            emailInputField.text = string.Empty;
        }

        public void OnCancelClick()
        {
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
