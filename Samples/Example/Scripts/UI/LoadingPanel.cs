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
                RGNCoreBuilder.I.OnAuthenticationChanged += FirebaseManager_OnAuthenticationChanged;
                RGNCoreBuilder.I.GetModule<GuestSignInModule>().SignInAsGuest();
            }
            else
            {
                Bootstrap.I.BuildAsync();
                RGNCoreBuilder.I.OnAuthenticationChanged += FirebaseManager_OnAuthenticationChanged;
            }

            base.Show(isInstant, onComplete);
        }

        public override void Hide(bool isInstant, Action onComplete)
        {
            RGNCoreBuilder.I.OnAuthenticationChanged -= FirebaseManager_OnAuthenticationChanged;
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
                            RGNCoreBuilder.I.SignOutRGN();

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
                        RGNCoreBuilder.I.GetModule<GuestSignInModule>().SignInAsGuest();
                    }
                    break;
            }
        }



    }
}
