using System;
using System.Collections.Generic;
using RGN.Modules.VirtualItems;
using UnityEngine;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpPriceSelector : MonoBehaviour
    {
        [SerializeField] private StoreTestPopUpPriceSelectorItem itemTemplate;
        [SerializeField] private Transform itemContent;

        private readonly List<StoreTestPopUpPriceSelectorItem> items =
            new List<StoreTestPopUpPriceSelectorItem>();

        private void Start()
        {
            itemTemplate.gameObject.SetActive(false);
        }

        public void Init(List<List<PriceInfo>> groupedPrices, Action<List<string>> onSelect)
        {
            foreach (StoreTestPopUpPriceSelectorItem item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
            
            for (var i = 0; i < groupedPrices.Count; i++)
            {
                StoreTestPopUpPriceSelectorItem item = Instantiate(itemTemplate, itemContent);
                item.gameObject.SetActive(true);
                item.Init(groupedPrices[i], onSelect);
                items.Add(item);
            }
        }
    }
}
