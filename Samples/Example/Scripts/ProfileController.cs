using RGN.Modules.GameProgress;
using RGN.Modules.UserProfile;
using System;
using System.Threading.Tasks;

namespace RGN.Sample
{
    public static class ProfileController
    {
        public static GameUserFullProfileData CurrentUserData;

        public static event Action OnAvatarChanged;

        public static async Task<GameUserFullProfileData> LoadAndCacheAsync()
        {
            CurrentUserData = await RGNCoreBuilder.I.GetModule<UserProfileModule<GameUserFullProfileData>>().GetFullUserProfileAsync(RGNCoreBuilder.I.MasterAppUser.UserId);

            //if (CurrentUserData != null && !string.IsNullOrEmpty(CurrentUserData.avatarPath))
            //{
            //    TaskCompletionSource<Texture2D> callbackAwaiter = new TaskCompletionSource<Texture2D>();

            //    CoreBuilder.I.GetModule<UserProfileModule<GameUserFullProfileData>>().DownloadAvatar(CurrentUserData.userId, (avatar) =>
            //    {
            //        callbackAwaiter.SetResult(avatar);
            //    });

            //    SetAvatar(await callbackAwaiter.Task);
            //}

            return CurrentUserData;
        }


    }
}
