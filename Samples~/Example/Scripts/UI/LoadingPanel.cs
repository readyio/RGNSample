using System;
using RGN.Modules.GameProgress;
using RGN.Modules.SignIn;

namespace RGN.Sample.UI
{
    public class LoadingPanel : AbstractPanel
    {
        public override async void Show(bool isInstant, Action onComplete)
        {
            if (Bootstrap.I.FirebaseBuilded)
            {
                RGNCore.I.AuthenticationChanged += OnAuthenticationChangedAsync;
                GuestSignInModule.I.TryToSignInAsync();
            }
            else
            {
                Bootstrap.I.CreateInstance();
                RGNCore.I.AuthenticationChanged += OnAuthenticationChangedAsync;
                await Bootstrap.I.BuildAsync();

                string instanceId = await Firebase.Analytics.FirebaseAnalytics.GetAnalyticsInstanceIdAsync();
                UnityEngine.Debug.Log("instanceId: " + instanceId);
            }

            base.Show(isInstant, onComplete);
        }

        public override void Hide(bool isInstant, Action onComplete)
        {
            RGNCore.I.AuthenticationChanged -= OnAuthenticationChangedAsync;
            base.Hide(isInstant, onComplete);
        }


        private async void OnAuthenticationChangedAsync(AuthState authState)
        {
            switch (authState.LoginState)
            {
                case EnumLoginState.Error:
                    Bootstrap.I.DisplayMessage("Failed: " + authState.LoginResult.ToString());
                    break;
                case EnumLoginState.Success:
                    {
                        GameUserFullProfileData userProfileData = await ProfileController.LoadAndCacheAsync();
                        if (userProfileData == null)
                        {
                            Bootstrap.I.DisplayMessage("Error: No Account found");
                            //No Account found, Signout current user. 
                            GuestSignInModule.I.SignOut();

                            return;
                        }
                        else
                        {
                            //Successfull login
                            Bootstrap.I.DisplayMessage("Success login and user \n" +
                                "UID :" + userProfileData.userId + "\n" +
                                "Display Name :" + userProfileData.displayName + "\n");
                            Hide(true, null);
                            UIRoot.singleton.ShowPanel<HomePanel>();
                        }

                        //Load other data from here

                        break;
                    }
                case EnumLoginState.NotLoggedIn:
                    {
                        Bootstrap.I.DisplayMessage("User Not Logged In");
                        GuestSignInModule.I.TryToSignInAsync();
                    }
                    break;
            }
        }
    }
}
