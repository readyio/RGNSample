using System;
using RGN;
using RGN.Model;
using RGN.Modules;
using RGN.Modules.GameProgress;
using RGN.Modules.UserProfileModule;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyGamesNetwork.Sample.UI
{
    public class EditProfilePopup : AbstractPopup
    {
        [SerializeField] private RawImage playerAvatar;
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private Button continueButton;

        public override void Show(bool isInstant, Action onComplete)
        {
            playerNameInput.text = ProfileController.CurrentUserData.displayName;
            base.Show(isInstant, onComplete);
        }
        public override void Hide(bool isInstant, Action onComplete)
        {
            base.Hide(isInstant, onComplete);
        }

        public void OnEditAvatarClick()
        {
            // We are using Native Gallery https://github.com/yasirkula/UnityNativeGallery
            // You can use Whatever library you want to use for accessing picture from user's device.
            /* 
            SetActiveSpinner(true);
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        SetActiveSpinner(false);
                        return;
                    }

                    texture = TextureUtils.DuplicateTexture(texture);
                    texture = TextureUtils.CropTexture(texture, 200, 200);

                    UpdateAvatar(texture);
                }
                else
                {
                    SetActiveSpinner(false);
                }
            }, "Select a PNG image", "image/png");
            */
        }

        public void OnPlayerNameInputValueChange(string val)
        {
            //Check if username is empty or not.
            continueButton.interactable = !string.IsNullOrEmpty(val);
        }

        public void OnContinueClick()
        {

            if (!ProfileController.CurrentUserData.displayName.Equals(playerNameInput.text))
            {
                SetActiveSpinner(true);
                UpdateProfileAsync(playerNameInput.text);
            }
            else
            {
                Hide(true, null);
                UIRoot.singleton.ShowPanel<HomePanel>();
            }
        }

        private async void UpdateProfileAsync(string playerName)
        {
            await RGNCoreBuilder.I.GetModule<UserProfileModule<RGNGameUserFullProfileData>>().UpdateProfileAsync(playerName);
            ProfileController.CurrentUserData.displayName = playerName;

            SetActiveSpinner(false);

            Hide(true, null);
            UIRoot.singleton.ShowPanel<HomePanel>();
        }

        public void OnCloseClick()
        {
            Hide(true, null);
            UIRoot.singleton.ShowPanel<HomePanel>();
        }


        private void SetActiveSpinner(bool value)
        {
            if (value)
            {
                UIRoot.singleton.ShowPopup<SpinnerPopup>();
            }
            else
            {
                UIRoot.singleton.HidePopup<SpinnerPopup>();
            }
        }
    }
}