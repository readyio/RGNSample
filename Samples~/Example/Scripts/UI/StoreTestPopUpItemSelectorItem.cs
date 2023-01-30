using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpItemSelectorItem : MonoBehaviour
    {
        [SerializeField] private Color selectedColor = Color.gray;
        [SerializeField] private Color unselectedColor = Color.white;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button button;
        
        public bool IsSelected { get; private set; }

        public string ItemId { get; private set; }

        public event Action OnSelectStateChanged;
        
        private void OnEnable()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClick);

            UpdateSelectedView();
        }

        public void Init(string itemId)
        {
            ItemId = itemId;
            label.text = itemId;
        }

        private void UpdateSelectedView()
        {
            image.color = IsSelected ? selectedColor : unselectedColor;
        }

        private void HandleClick()
        {
            IsSelected = !IsSelected;
            UpdateSelectedView();
            OnSelectStateChanged?.Invoke();
        }
    }
}
