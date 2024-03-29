using System.Threading.Tasks;
using RGN.Modules.GameProgress;
using RGN.Modules.UserProfile;

namespace RGN.Sample
{
    public static class ProfileController
    {
        public static GameUserFullProfileData CurrentUserData;

        //public static event Action OnAvatarChanged;

        public static async Task<GameUserFullProfileData> LoadAndCacheAsync()
        {
            CurrentUserData = await UserProfileModule.I.GetFullUserProfileAsync<GameUserFullProfileData>(RGNCoreBuilder.I.MasterAppUser.UserId);

            //if (CurrentUserData != null && !string.IsNullOrEmpty(CurrentUserData.avatarPath))
            //{
            //    TaskCompletionSource<Texture2D> callbackAwaiter = new TaskCompletionSource<Texture2D>();

            //    UserProfileModule<GameUserFullProfileData>.I.DownloadAvatar(CurrentUserData.userId, (avatar) =>
            //    {
            //        callbackAwaiter.SetResult(avatar);
            //    });

            //    SetAvatar(await callbackAwaiter.Task);
            //}

            return CurrentUserData;
        }


    }
}
