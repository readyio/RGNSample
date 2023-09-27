using RGN.Modules.GameProgress;
using RGN.Modules.UserProfile;
using System;
using RGN.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class EditProfilePopup : AbstractPopup
    {
        [SerializeField] private RawImage playerAvatar;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button playerAvatarButton;

        public override async void Show(bool isInstant, Action onComplete)
        {
            saveButton.onClick.AddListener(OnSaveClick);
            closeButton.onClick.AddListener(OnCloseClick);
            playerAvatarButton.onClick.AddListener(OnEditAvatarClick);
            playerNameInput.text = ProfileController.CurrentUserData.displayName;

            base.Show(isInstant, onComplete);
            
            byte[] profilePictureBytes = await UserProfileModule.I
                .DownloadAvatarImageAsync(ProfileController.CurrentUserData.userId, ImageSize.Small);
            if (profilePictureBytes != null)
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(profilePictureBytes);
                texture.Apply();
                playerAvatar.texture = texture;
            }
        }
        public override void Hide(bool isInstant, Action onComplete)
        {
            saveButton.onClick.RemoveListener(OnSaveClick);
            closeButton.onClick.RemoveListener(OnCloseClick);
            playerAvatarButton.onClick.RemoveListener(OnEditAvatarClick);
            base.Hide(isInstant, onComplete);
        }

        public void OnEditAvatarClick()
        {
            // We are using Native Gallery https://github.com/yasirkula/UnityNativeGallery
            // You can use Whatever library you want to use for accessing picture from user's device.
            
            SetActiveSpinner(true);
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(async (path) =>
            {
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 512, false);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        SetActiveSpinner(false);
                        return;
                    }

                    await UserProfileModule.I.UploadAvatarImageAsync(texture.EncodeToPNG());
                    playerAvatar.texture = texture;
                    
                    SetActiveSpinner(false);
                }
                else
                {
                    SetActiveSpinner(false);
                }
            }, "Select a PNG image", "image/png");
            
        }

        public void OnPlayerNameInputValueChange(string val)
        {
            //Check if username is empty or not.
            saveButton.interactable = !string.IsNullOrEmpty(val);
        }

        public void OnSaveClick()
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
            await UserProfileModule.I.SetDisplayNameAndBioAsync(playerName);
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
