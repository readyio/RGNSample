using RGN.Modules.Inventory;
using RGN.Modules.VirtualItems;
using System;
using ThirdPartyDeveloper.Aeria.DemoWeaponUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class VirtualItemTestPopUp : AbstractPopup
    {
        [SerializeField] private RectTransform scrollContent;
        [SerializeField] private TextMeshProUGUI nameTitleText;
        [SerializeField] private TMP_InputField scopeOrUpgradeLevelInputField;
        [SerializeField] private TextMeshProUGUI evaluationResultText;

        [SerializeField] private Button setScopeExampleToInputFieldButton;
        [SerializeField] private Button itemToStringButton;
        [SerializeField] private Button getUpgradesButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button tryParsePropertiesButton;
        [SerializeField] private Button printCurrentUpgradeValuesPlusNextButton;
        [SerializeField] private Button closeButton;

        private VirtualItem item;
        private bool doesUserOwnTheItem;

        public override void Show(bool isInstant, Action onComplete)
        {
            base.Show(isInstant, onComplete);

            setScopeExampleToInputFieldButton.onClick.RemoveAllListeners();
            setScopeExampleToInputFieldButton.onClick.AddListener(OnSetScopeExampleToInputFieldButtonClick);
            itemToStringButton.onClick.RemoveAllListeners();
            itemToStringButton.onClick.AddListener(OnItemToStringButtonClick);
            getUpgradesButton.onClick.RemoveAllListeners();
            getUpgradesButton.onClick.AddListener(OnGetUpgradesButtonClick);
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
            tryParsePropertiesButton.onClick.RemoveAllListeners();
            tryParsePropertiesButton.onClick.AddListener(OnTryParsePropertiesButtonClick);
            printCurrentUpgradeValuesPlusNextButton.onClick.RemoveAllListeners();
            printCurrentUpgradeValuesPlusNextButton.onClick.AddListener(OnPrintCurrentUpgradeValuesPlusNextButtonClick);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseClick);
        }

        internal void Init(VirtualItem item, bool doesUserOwnTheItem)
        {
            this.item = item;
            this.doesUserOwnTheItem = doesUserOwnTheItem;
            nameTitleText.text = item.name;
            SetEvaluationResultTextAndUpdateContentHeight(item.ToString());
        }

        private void OnSetScopeExampleToInputFieldButtonClick()
        {
            scopeOrUpgradeLevelInputField.text = "{\"itemUpgrades\": {\"default\": 42}}";
        }
        private void OnItemToStringButtonClick()
        {
            SetEvaluationResultTextAndUpdateContentHeight(item.ToString());
        }
        private void OnGetUpgradesButtonClick()
        {
            if (!doesUserOwnTheItem)
            {
                ShowPopupAndPrintErrorMessage("Error: User does not own the item, can't get the upgrades");
                return;
            }
            try
            {
                UIRoot.singleton.ShowPopup<SpinnerPopup>();

                //var result = await InventoryModule.I.GetVirtualItemUpgradesAsync(item.id);

                //string message = "No upgrades";
                //if (result.itemUpgrades != null && result.itemUpgrades.Count > 0)
                //{
                //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //    for (int i = 0; i < result.itemUpgrades.Count; ++i)
                //    {
                //        var item = result.itemUpgrades[i];
                //        sb.Append("For upgrade ID: '");
                //        sb.Append(item.upgradeId);
                //        sb.Append("' upgrade level is: ");
                //        sb.AppendLine(item.upgradeLevel.ToString());
                //    }
                //    message = sb.ToString();
                //}
                //SetEvaluationResultTextAndUpdateContentHeight(message);
                //Debug.Log(message);
            }
            catch (Exception ex)
            {
                ShowPopupAndPrintErrorMessage(ex.Message);
            }
            finally
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
            }
        }
        private async void OnUpgradeButtonClick()
        {
            if (!doesUserOwnTheItem)
            {
                ShowPopupAndPrintErrorMessage("Error: User does not own the item, can't upgrade");
                return;
            }
            string upgradeLevelStr = scopeOrUpgradeLevelInputField.text;
            if (string.IsNullOrWhiteSpace(upgradeLevelStr))
            {
                ShowPopupAndPrintErrorMessage("Please provide upgrade level integer");
                return;
            }
            if (!int.TryParse(upgradeLevelStr, out int upgradeLevel))
            {
                ShowPopupAndPrintErrorMessage("Can not parse upgrade level from: " + upgradeLevelStr);
                return;
            }
            try
            {
                UIRoot.singleton.ShowPopup<SpinnerPopup>();

                var result = await InventoryModule.I.UpgradeAsync(item.id, upgradeLevel);

                string message = "No upgrades";
                if (result.Count > 0)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("Upgrades: ");
                    for (int i = 0; i < result.Count; ++i)
                    {
                        var upgrade = result[i];
                        sb.Append("\t ID: ");
                        sb.Append(upgrade.upgradeId);
                        sb.Append(", level: ");
                        sb.AppendLine(upgrade.upgradeLevel.ToString());
                    }
                    message = sb.ToString();
                }
                SetEvaluationResultTextAndUpdateContentHeight(message);
                Debug.Log(message);
            }
            catch (Exception ex)
            {
                ShowPopupAndPrintErrorMessage(ex.Message);
            }
            finally
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
            }
        }
        private void OnTryParsePropertiesButtonClick()
        {
            Debug.Log("Trying to parse the json: " + item.properties);
            var propsForCurrentApp = item.properties.Find(item => item.appIds.Contains("rgn.test"));
            string json = propsForCurrentApp.json;
            WeaponDTO dto = JsonUtility.FromJson<WeaponDTO>(json);
            string dtoToString = dto.ToString();
            SetEvaluationResultTextAndUpdateContentHeight(dtoToString);
            Debug.Log("Parse result: " + dtoToString);
        }
        private void OnPrintCurrentUpgradeValuesPlusNextButtonClick()
        {
            if (!doesUserOwnTheItem)
            {
                ShowPopupAndPrintErrorMessage("Error: User does not own the item, can't get the upgrades");
                return;
            }
            var propsForCurrentApp = item.properties.Find(item => item.appIds.Contains("rgn.test"));
            string properties = propsForCurrentApp.json;
            if (string.IsNullOrWhiteSpace(properties))
            {
                ShowPopupAndPrintErrorMessage("Error: The item json are null or white space");
                return;
            }
            try
            {
                UIRoot.singleton.ShowPopup<SpinnerPopup>();

                //var result = await InventoryModule.I.GetVirtualItemUpgradesAsync(item.id);

                //int upgradeLevelIndex = 0;
                //if (result.itemUpgrades != null && result.itemUpgrades.Count > 0)
                //{
                //    upgradeLevelIndex = result.itemUpgrades[0].upgradeLevel;
                //}
                //WeaponDTO dto = JsonUtility.FromJson<WeaponDTO>(json);
                //if (upgradeLevelIndex < 0 || upgradeLevelIndex >= dto.Upgrades.Length)
                //{
                //    ShowPopupAndPrintErrorMessage("Error: Current upgrade level exists the upgrades in json: " + upgradeLevelIndex);
                //    return;
                //}

                //UpgradeDTO currentUpgrade = dto.Upgrades[upgradeLevelIndex];
                //UpgradeDTO nextUpgrade = upgradeLevelIndex < dto.Upgrades.Length - 2 ? dto.Upgrades[upgradeLevelIndex + 1] : null;

                //var sb = new System.Text.StringBuilder();
                //AddValueForField(currentUpgrade.Damage, nextUpgrade?.Damage, nameof(currentUpgrade.Damage), sb);
                //AddValueForField(currentUpgrade.UserAccuracy, nextUpgrade?.UserAccuracy, nameof(currentUpgrade.UserAccuracy), sb);
                //AddValueForField(currentUpgrade.Accuracy, nextUpgrade?.Accuracy, nameof(currentUpgrade.Accuracy), sb);
                //AddValueForField(currentUpgrade.UserFireRate, nextUpgrade?.UserFireRate, nameof(currentUpgrade.UserFireRate), sb);
                //AddValueForField(currentUpgrade.FireRate, nextUpgrade?.FireRate, nameof(currentUpgrade.FireRate), sb);
                //AddValueForField(currentUpgrade.Reload, nextUpgrade?.Reload, nameof(currentUpgrade.Reload), sb);
                //AddValueForField(currentUpgrade.Range, nextUpgrade?.Range, nameof(currentUpgrade.Range), sb);

                //string message = sb.ToString();
                //SetEvaluationResultTextAndUpdateContentHeight(message);
                //Debug.Log(message);
            }
            catch (Exception ex)
            {
                ShowPopupAndPrintErrorMessage(ex.Message);
            }
            finally
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
            }
        }

        private static void AddValueForField(float value, float? nextValue, string valueName, System.Text.StringBuilder sb)
        {
            sb.Append(valueName).Append(": ").Append(value);
            if (nextValue != null)
            {
                float diff = nextValue.Value - value;
                diff = Mathf.Round(diff * 100f) / 100f;
                if (diff > 0)
                {
                    sb.Append("<color=green>+");
                }
                else
                {
                    sb.Append("<color=red>");
                }
                sb.Append(diff);
                sb.Append("</color>");
            }
            else
            {
                sb.Append(" Max Level");
            }
            sb.AppendLine();
        }

        private void OnCloseClick()
        {
            Hide(true, null);
        }

        private void ShowPopupAndPrintErrorMessage(string message)
        {
            PopupMessage popupMessage = new PopupMessage()
            {
                Message = message,
            };
            Debug.LogError(popupMessage.Message);
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }
        private void SetEvaluationResultTextAndUpdateContentHeight(string text)
        {
            evaluationResultText.text = text;
            scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, evaluationResultText.preferredHeight);
        }
    }
}
