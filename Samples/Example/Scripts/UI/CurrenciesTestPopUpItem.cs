using RGN.Modules.Currency;
using TMPro;
using UnityEngine;

namespace RGN.Sample.UI
{
    public class CurrenciesTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text quantityText;

        public void Init(Currency currency)
        {
            nameText.text = currency.name;
            quantityText.text = currency.quantity.ToString();
        }
    }
}