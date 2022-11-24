using RGN.Modules.Currency;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class CurrenciesTestPopUpItem : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text quantityText;

        public void Init(RGNCurrency currency)
        {
            nameText.text = currency.name;
            quantityText.text = currency.quantity.ToString();
        }
    }
}