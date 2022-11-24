using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class HomePanel : AbstractPanel
    {
        [SerializeField] private RawImage playerAvatar;
        [SerializeField] private Text playerNameLabel;

        public override void Show(bool isInstant, Action onComplete)
        {
            Refresh();
            base.Show(isInstant, onComplete);
        }

        public void Refresh()
        {
            playerNameLabel.text = ProfileController.CurrentUserData.displayName;
        }

        public override void Hide(bool isInstant, Action onComplete)
        {
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