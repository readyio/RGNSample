using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class GenericPopup : AbstractPopup
    {
        [SerializeField] private Text TitleLabel = default;
        [SerializeField] private TMPro.TextMeshProUGUI MessageLabel = default;
        [SerializeField] private Text ButtonLabel = default;

        private Action Callback;

        public void ShowMessage(PopupMessage _msg)
        {
            TitleLabel.text = _msg.Title;
            MessageLabel.text = _msg.Message;
            ButtonLabel.text = _msg.ButtonText;
            Callback = _msg.Callback;

            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public void CloseWindow()
        {
            Hide(true, Callback);
        }
    }

    public class PopupMessage
    {
        public string Title;
        public string Message;
        public string ButtonText = "OK";
        public Action Callback = null;
    }
}