using RGN.Modules.Achievement;
using RGN.Modules.Creator;
using RGN.Modules.Currency;
using RGN.Modules.GameProgress;
using RGN.Modules.SignIn;
using RGN.Modules.Inventory;
using RGN.Modules.Matchmaking;
using RGN.Modules.Store;
using RGN.Modules.UserProfile;
using RGN.Modules.VirtualItems;
using RGN.Modules.Wallets;
using RGN.Sample.UI;
using RGN.Utility;
using TMPro;
using UnityEngine;

namespace RGN.Sample
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap I;
        [SerializeField] private TMP_Text status;

        public bool FirebaseBuilded { get; private set; }

        private void Awake()
        {
            ThrowIf.Field.IsNotNull(I, nameof(I));
            I = this;
            RGNCoreBuilder.AddModule(new AppleSignInModule());
            RGNCoreBuilder.AddModule(new GoogleSignInModule());
            RGNCoreBuilder.AddModule(new FacebookSignInModule());
            RGNCoreBuilder.AddModule(new EmailSignInModule());
            RGNCoreBuilder.AddModule(new GuestSignInModule());
            RGNCoreBuilder.AddModule(new UserProfileModule<GameUserFullProfileData>());
            RGNCoreBuilder.AddModule(new CurrencyModule());
            RGNCoreBuilder.AddModule(new GameModule());
            RGNCoreBuilder.AddModule(new InventoryModule());
            RGNCoreBuilder.AddModule(new StoreModule());
            RGNCoreBuilder.AddModule(new VirtualItemModule());
            RGNCoreBuilder.AddModule(new CreatorModule());
            RGNCoreBuilder.AddModule(new AchievementsModule());
            RGNCoreBuilder.AddModule(new WalletsModule());
            RGNCoreBuilder.AddModule(new MatchmakingModule());
            UIRoot.singleton.ShowPanel<LoadingPanel>();
        }
        private void OnApplicationQuit()
        {
            I = null;
            RGNCoreBuilder.Dispose();
        }

        public async void BuildAsync()
        {
            if (FirebaseBuilded)
            {
                return;
            }
            FirebaseBuilded = true;
            await RGNCoreBuilder.BuildAsync(new Impl.Firebase.Dependencies());
        }

        public void DisplayMessage(string message)
        {
            status.text = status.text + " \n" + message;
        }
    }
}
