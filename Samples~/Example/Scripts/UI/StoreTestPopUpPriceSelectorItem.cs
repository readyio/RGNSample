using System;
using System.Collections.Generic;
using System.Text;
using RGN.Modules.VirtualItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class StoreTestPopUpPriceSelectorItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button button;

        private List<PriceInfo> groupedPrices;
        private Action<string[]> onClick;
        
        private void OnEnable()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClick);
        }

        public void Init(List<PriceInfo> groupedPrices, Action<string[]> onClick)
        {
            this.groupedPrices = groupedPrices;
            this.onClick = onClick;

            StringBuilder labelBuilder = new StringBuilder();
            for (int i = 0; i < groupedPrices.Count; i++)
            {
                labelBuilder.Append(groupedPrices[i].name);
                labelBuilder.Append(":");
                labelBuilder.Append(groupedPrices[i].quantity);

                if (i < groupedPrices.Count - 1)
                {
                    labelBuilder.Append(", ");
                }
            }
            label.text = labelBuilder.ToString();
        }

        private void HandleClick()
        {
            string[] currencies = new string[groupedPrices.Count];
            for (int i = 0; i < groupedPrices.Count; i++)
            {
                currencies[i] = groupedPrices[i].name;
            }
            onClick?.Invoke(currencies);
        }
    }
}
