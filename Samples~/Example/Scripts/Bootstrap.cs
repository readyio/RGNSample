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
using RGN.Modules.Messaging;
using System.Threading.Tasks;

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
            RGNCoreBuilder.AddModule(new EmailSignInModule());
            RGNCoreBuilder.AddModule(new GuestSignInModule());
            RGNCoreBuilder.AddModule(new UserProfileModule());
            RGNCoreBuilder.AddModule(new CurrencyModule());
            RGNCoreBuilder.AddModule(new GameProgressModule());
            RGNCoreBuilder.AddModule(new InventoryModule());
            RGNCoreBuilder.AddModule(new StoreModule());
            RGNCoreBuilder.AddModule(new VirtualItemsModule());
            RGNCoreBuilder.AddModule(new CreatorModule());
            RGNCoreBuilder.AddModule(new AchievementsModule());
            RGNCoreBuilder.AddModule(new WalletsModule());
            RGNCoreBuilder.AddModule(new MatchmakingModule());
            RGNCoreBuilder.AddModule(new MessagingModule());
            UIRoot.singleton.ShowPanel<LoadingPanel>();
        }
        private void OnApplicationQuit()
        {
            I = null;
            RGNCoreBuilder.Dispose();
        }

        public void CreateInstance()
        {
            RGNCoreBuilder.CreateInstance(new Impl.Firebase.Dependencies());
        }

        public Task BuildAsync()
        {
            if (FirebaseBuilded)
            {
                return Task.CompletedTask;
            }
            FirebaseBuilded = true;
            return RGNCoreBuilder.BuildAsync();
        }

        public void DisplayMessage(string message)
        {
            status.text = status.text + " \n" + message;
        }
    }
}
