using RGN.Impl.Firebase.Core;
using RGN.Modules;
using RGN.Modules.Achievement;
using RGN.Modules.Creator;
using RGN.Modules.Currency;
using RGN.Modules.EmailSignIn;
using RGN.Modules.FacebookSignIn;
using RGN.Modules.GameProgress;
using RGN.Modules.GuestSignIn;
using RGN.Modules.Inventory;
using RGN.Modules.Matchmaking;
using RGN.Modules.UserProfile;
using RGN.Modules.VirtualItems;
using RGN.Modules.Wallets;
using RGN.Sample.UI;
using TMPro;
using UnityEngine;

namespace RGN.Sample
{
    public class Bootstrap : MonoBehaviour
    {
        public static Bootstrap I;
        [SerializeField] private ApplicationStore applicationStore;
        [SerializeField] private TMP_Text status;

        public ApplicationStore ApplicationStore => applicationStore;
        public bool FirebaseBuilded { get; private set; }

        public void Awake()
        {
            I = this;
            RGNCoreBuilder.AddModule(new AppleSignInModule());
            RGNCoreBuilder.AddModule(new GoogleSignInModule(applicationStore.googleSignInWebClientID));
            RGNCoreBuilder.AddModule(new FacebookSignInModule());
            RGNCoreBuilder.AddModule(new EmailSignInModule());
            RGNCoreBuilder.AddModule(new GuestSignInModule());
            RGNCoreBuilder.AddModule(new UserProfileModule<GameUserFullProfileData>(applicationStore.RGNStorageURL));
            RGNCoreBuilder.AddModule(new CurrencyModule());
            RGNCoreBuilder.AddModule(new GameModule());
            RGNCoreBuilder.AddModule(new InventoryModule());
            RGNCoreBuilder.AddModule(new VirtualItemModule(applicationStore.RGNStorageURL));
            RGNCoreBuilder.AddModule(new CreatorModule());
            RGNCoreBuilder.AddModule(new AchievementsModule());
            RGNCoreBuilder.AddModule(new WalletsModule());
            RGNCoreBuilder.AddModule(new MatchmakingModule());
            UIRoot.singleton.ShowPanel<LoadingPanel>();
        }

        public async void StartFirebaseBuilding()
        {
            if (FirebaseBuilded) return;
            FirebaseBuilded = true;

            var appOptions = new AppOptions()
            {
                ApiKey = applicationStore.RGNMasterApiKey,
                AppId = applicationStore.RGNMasterAppID,
                ProjectId = applicationStore.RGNMasterProjectId
            };

            await RGNCoreBuilder.Build(
                new Impl.Firebase.Dependencies(
                    appOptions,
                    applicationStore.RGNStorageURL),
                appOptions,
               applicationStore.RGNStorageURL,
               applicationStore.RGNAppId);
            if (applicationStore.usingEmulator)
            {
                RGNCore rgnCore = (RGNCore)RGNCoreBuilder.I;
                var firestore = rgnCore.readyMasterFirestore;
                string firestoreHost = applicationStore.emulatorServerIp + applicationStore.firestorePort;
                bool firestoreSslEnabled = false;
                firestore.UserEmulator(firestoreHost, firestoreSslEnabled);
                rgnCore.readyMasterFunction.UseFunctionsEmulator(applicationStore.emulatorServerIp + applicationStore.functionsPort);
                //TODO: storage, auth, realtime db
            }
        }

        public void DisplayMessage(string message)
        {
            status.text = status.text + " \n" + message;
        }
    }
}