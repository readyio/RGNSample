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
                RGNCore.I.AuthenticationChanged += FirebaseManager_OnAuthenticationChanged;
                GuestSignInModule.I.TryToSignIn();
            }
            else
            {
                Bootstrap.I.BuildAsync();
                RGNCore.I.AuthenticationChanged += FirebaseManager_OnAuthenticationChanged;
            }

            base.Show(isInstant, onComplete);
        }

        public override void Hide(bool isInstant, Action onComplete)
        {
            RGNCore.I.AuthenticationChanged -= FirebaseManager_OnAuthenticationChanged;
            base.Hide(isInstant, onComplete);
        }


        private async void FirebaseManager_OnAuthenticationChanged(EnumLoginState enumLoginState, EnumLoginError error)
        {
            switch (enumLoginState)
            {
                case EnumLoginState.Error:
                    Bootstrap.I.DisplayMessage("Failed: " + error.ToString());
                    break;
                case EnumLoginState.Success:
                    {
                        GameUserFullProfileData userProfileData = await ProfileController.LoadAndCacheAsync();
                        if (userProfileData == null)
                        {
                            Bootstrap.I.DisplayMessage("Error: No Account found");
                            //No Account found, Signout current user. 
                            RGNCore.I.SignOutRGN();

                            return;
                        }
                        else
                        {
                            //Successfull login
                            Bootstrap.I.DisplayMessage("Success login and user \n" +
                                "UID :" + userProfileData.userId + "\n" +
                                "Display Name :" + userProfileData.displayName + "\n" +
                                "Short UID :" + userProfileData.shortUID + "\n");
                            Hide(true, null);
                            UIRoot.singleton.ShowPanel<HomePanel>();
                        }

                        //Load other data from here

                        break;
                    }
                case EnumLoginState.NotLoggedIn:
                    {
                        Bootstrap.I.DisplayMessage("User Not Logged In");
                        GuestSignInModule.I.TryToSignIn();
                    }
                    break;
            }
        }
    }
}
