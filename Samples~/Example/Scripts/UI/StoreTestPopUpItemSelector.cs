using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpItemSelector : MonoBehaviour
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private StoreTestPopUpItemSelectorItem itemTemplate;
        [SerializeField] private Transform itemContent;

        private readonly List<StoreTestPopUpItemSelectorItem> items =
            new List<StoreTestPopUpItemSelectorItem>();

        private Action<List<string>> onSelect;

        private void Start()
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(HandleConfirmButtonClick);
            
            itemTemplate.gameObject.SetActive(false);
        }

        public void Init(List<string> itemIds, Action<List<string>> onSelect)
        {
            this.onSelect = onSelect;
            
            foreach (StoreTestPopUpItemSelectorItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
            
            for (var i = 0; i < itemIds.Count; i++)
            {
                StoreTestPopUpItemSelectorItem item = Instantiate(itemTemplate, itemContent);
                item.gameObject.SetActive(true);
                item.Init(itemIds[i]);
                item.OnSelectStateChanged += OnItemSelectStateChanged;
                items.Add(item);
            }

            UpdateConfirmButtonInteractable();
        }

        private void OnItemSelectStateChanged()
        {
            UpdateConfirmButtonInteractable();
        }

        private void UpdateConfirmButtonInteractable()
        {
            bool anyItemSelected = items.Any(item => item.IsSelected);
            confirmButton.interactable = anyItemSelected;
        }

        private void HandleConfirmButtonClick()
        {
            List<string> itemIds = new List<string>();
            
            foreach (StoreTestPopUpItemSelectorItem item in items)
            {
                if (item.IsSelected)
                {
                    itemIds.Add(item.ItemId);
                }
            }
            
            onSelect?.Invoke(itemIds);
        }
    }
}
