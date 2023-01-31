using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class GenericPopup : AbstractPopup
    {
        [SerializeField] private TMP_Text TitleLabel = default;
        [SerializeField] private TMPro.TextMeshProUGUI MessageLabel = default;
        [SerializeField] private TMP_Text ButtonLabel = default;
        [SerializeField] private Button okayButton;
        [SerializeField] private Button cancelButton;

        private Action Callback;

        public void ShowMessage(PopupMessage _msg)
        {
            TitleLabel.text = _msg.Title;
            MessageLabel.text = _msg.Message;
            ButtonLabel.text = _msg.ButtonText;
            Callback = _msg.Callback;

            okayButton.onClick.AddListener(CloseWindow);
            cancelButton.onClick.AddListener(CloseWindow);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public void CloseWindow()
        {
            
            okayButton.onClick.RemoveListener(CloseWindow);
            cancelButton.onClick.RemoveListener(CloseWindow);
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