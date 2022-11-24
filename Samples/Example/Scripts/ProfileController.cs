using RGN;
using RGN.Modules;
using RGN.Modules.GameProgress;
using RGN.Modules.UserProfileModule;
using System;
using System.Threading.Tasks;

namespace ReadyGamesNetwork.Sample
{
    public static class ProfileController
    {
        public static RGNGameUserFullProfileData CurrentUserData;

        public static event Action OnAvatarChanged;

        public static async Task<RGNGameUserFullProfileData> LoadAndCacheAsync()
        {
            CurrentUserData = await RGNCoreBuilder.I.GetModule<UserProfileModule<RGNGameUserFullProfileData>>().GetFullUserProfile(RGNCoreBuilder.I.masterAppUser.UserId);

            //if (CurrentUserData != null && !string.IsNullOrEmpty(CurrentUserData.avatarPath))
            //{
            //    TaskCompletionSource<Texture2D> callbackAwaiter = new TaskCompletionSource<Texture2D>();

            //    RGNCoreBuilder.I.GetModule<UserProfileModule<RGNGameUserFullProfileData>>().DownloadAvatar(CurrentUserData.userId, (avatar) =>
            //    {
            //        callbackAwaiter.SetResult(avatar);
            //    });

            //    SetAvatar(await callbackAwaiter.Task);
            //}

            return CurrentUserData;
        }


    }
}