using System;
using RGN.Modules.GameProgress;
using RGN.Modules.SignIn;
using RGN.Modules.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class HomePanel : AbstractPanel
    {
        [SerializeField] private RawImage playerAvatar;
        [SerializeField] private TMP_Text playerNameLabel;
        [SerializeField] private Button editProfileButton;
        [SerializeField] private Button currenciesTestButton;
        [SerializeField] private Button gameTestButton;
        [SerializeField] private Button inventoryTestButton;
        [SerializeField] private Button storeTestButton;
        [SerializeField] private Button inAppPurchaseTestButton;
        [SerializeField] private Button achievementsTestButton;
        [SerializeField] private Button walletsTestButton;
        [SerializeField] private Button matchmakingTestButton;
        [SerializeField] private Button settingsButton;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                EmailSignInModule.I.SignOut();
            }
        }

        public override void Show(bool isInstant, Action onComplete)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            editProfileButton.onClick.AddListener(OnEditProfileButtonClick);
            currenciesTestButton.onClick.AddListener(OnCurrenciesTestButtonClick);
            gameTestButton.onClick.AddListener(OnGameTestButtonClick);
            inventoryTestButton.onClick.AddListener(OnInventoryTestButtonClick);
            storeTestButton.onClick.AddListener(OnStoreTestButtonClick);
            inAppPurchaseTestButton.onClick.AddListener(OnInAppPurchasesTestButtonClick);
            achievementsTestButton.onClick.AddListener(OnAchievementsTestButtonClick);
            walletsTestButton.onClick.AddListener(OnWalletsTestButtonClick);
            matchmakingTestButton.onClick.AddListener(OnMatchmakingTestButtonClick);
            Refresh();
            base.Show(isInstant, onComplete);
        }

        public void Refresh()
        {
            playerNameLabel.text = ProfileController.CurrentUserData.displayName;
        }

        public override void Hide(bool isInstant, Action onComplete)
        {
            settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            editProfileButton.onClick.RemoveListener(OnEditProfileButtonClick);
            currenciesTestButton.onClick.RemoveListener(OnCurrenciesTestButtonClick);
            gameTestButton.onClick.RemoveListener(OnGameTestButtonClick);
            inventoryTestButton.onClick.RemoveListener(OnInventoryTestButtonClick);
            storeTestButton.onClick.RemoveListener(OnStoreTestButtonClick);
            inAppPurchaseTestButton.onClick.RemoveListener(OnInAppPurchasesTestButtonClick);
            achievementsTestButton.onClick.RemoveListener(OnAchievementsTestButtonClick);
            walletsTestButton.onClick.RemoveListener(OnWalletsTestButtonClick);
            matchmakingTestButton.onClick.RemoveListener(OnMatchmakingTestButtonClick);
            base.Hide(isInstant, onComplete);
        }

        public void OnEditProfileButtonClick()
        {
            UIRoot.singleton.ShowPopup<EditProfilePopup>();
        }

        public void OnSettingsButtonClick()
        {
            UIRoot.singleton.ShowPopup<UserSettingsPopup>();
        }

        public void OnCurrenciesTestButtonClick()
        {
            UIRoot.singleton.ShowPopup<CurrenciesTestPopUp>();
        }
        
        public void OnGameTestButtonClick()
        {
            UIRoot.singleton.ShowPopup<GameTestPopUp>();
        }
        
        public void OnInventoryTestButtonClick()
        {
            UIRoot.singleton.ShowPopup<InventoryTestPopUp>();
        }
        
        public void OnStoreTestButtonClick()
        {
            UIRoot.singleton.ShowPopup<StoreTestPopUp>();
        }

        public void OnInAppPurchasesTestButtonClick() 
        {
            UIRoot.singleton.ShowPopup<InAppPurchasesTestPopUp>();
        }
        
        public void OnAchievementsTestButtonClick() 
        {
            UIRoot.singleton.ShowPopup<AchievementsTestPopUp>();
        }
        
        public void OnWalletsTestButtonClick() 
        {
            UIRoot.singleton.ShowPopup<WalletsTestPopUp>();
        }
        
        public void OnMatchmakingTestButtonClick() 
        {
            UIRoot.singleton.ShowPopup<MatchmakingTestPopUp>();
        }
    }
}
