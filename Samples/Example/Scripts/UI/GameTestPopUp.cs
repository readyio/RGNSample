using RGN.Modules.Currency;
using RGN.Modules.GameProgress;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class GameTestPopUp : AbstractPopup
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button onGameCompleteButton;
        [SerializeField] private Button getGameProgressButton;
        [SerializeField] private Button updateLevelProgressButton;
        [SerializeField] private Button getLevelProgressButton;

        public override void Show(bool isInstant, Action onComplete)
        {
            cancelButton.onClick.AddListener(OnCloseClick);
            onGameCompleteButton.onClick.AddListener(OnGameCompleteButtonClick);
            getGameProgressButton.onClick.AddListener(OnGetGameProgressButtonClick);
            updateLevelProgressButton.onClick.AddListener(OnUpdateLevelProgressButtonClick);
            getLevelProgressButton.onClick.AddListener(OnGetLevelProgressButtonClick);
            base.Show(isInstant, onComplete);
        }

        public async void OnGameCompleteButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();

            OnGameCompleteResult onGameCompleteResult = await gameModule.OnGameComplete(new List<Currency>()
            {
                new Currency()
                {
                    name = "rgnTestCoin",
                    quantity = 25
                }
            });

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = $"gameProgress: {JsonUtility.ToJson(onGameCompleteResult.gameProgress)}\n\r" +
                          $"currencies: {JsonUtility.ToJson(onGameCompleteResult.userCurrencies)}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public async void OnGetGameProgressButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();

            GameProgress gameProgress = await gameModule.GetGameProgress();

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = $"gameProgress: {JsonUtility.ToJson(gameProgress)}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public async void OnUpdateLevelProgressButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();

            LevelData levelData = new LevelData()
            {
                modeData = new List<LevelMode>() {
                    new LevelMode() {
                        modeId = 0,
                        currentLevel = 1,
                    },
                    new LevelMode() {
                         modeId = 1,
                        currentLevel = 2,
                    }
                }
            };

            LevelProgressResult<LevelData> levelProgressResult = await gameModule.UpdateLevelProgress(levelData, new List<Currency>()
            {
                new Currency()
                {
                    name = "rgnTestCoin",
                    quantity = 25
                },
            });

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = $"levelProgress: {JsonUtility.ToJson(levelProgressResult)}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public async void OnGetLevelProgressButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();
            LevelProgressResult<LevelData> levelProgressResult = await gameModule.GetLevelProgress<LevelData>();

            UIRoot.singleton.HidePopup<SpinnerPopup>();

            PopupMessage popupMessage = new PopupMessage()
            {
                Message = $"levelProgress: {JsonUtility.ToJson(levelProgressResult)}"
            };
            GenericPopup genericPopup = UIRoot.singleton.GetPopup<GenericPopup>();
            genericPopup.ShowMessage(popupMessage);
            UIRoot.singleton.ShowPopup<GenericPopup>();
        }

        public void OnCloseClick()
        {
            cancelButton.onClick.RemoveListener(OnCloseClick);
            onGameCompleteButton.onClick.RemoveListener(OnGameCompleteButtonClick);
            getGameProgressButton.onClick.RemoveListener(OnGetGameProgressButtonClick);
            updateLevelProgressButton.onClick.RemoveListener(OnUpdateLevelProgressButtonClick);
            getLevelProgressButton.onClick.RemoveListener(OnGetLevelProgressButtonClick);
            Hide(true, null);
        }
    }

    //Custom Class to store level
    [Serializable]
    public class LevelData
    {
        public List<LevelMode> modeData;
    }

    [Serializable]
    public class LevelMode
    {
        public int modeId;
        public int currentLevel;

    }
}
