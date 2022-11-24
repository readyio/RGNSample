using RGN;
using RGN.Modules.Currency;
using RGN.Modules.GameProgress;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamesNetwork.Sample.UI
{
    public class GameTestPopUp : AbstractPopup
    {
        public async void OnGameCompleteButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();

            RGNOnGameCompleteResult onGameCompleteResult = await gameModule.OnGameComplete(new List<RGNCurrency>()
            {
                new RGNCurrency()
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

        public async void GetGameProgressButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();

            RGNGameProgress gameProgress = await gameModule.GetGameProgress();

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

            RGNLevelProgressResult<LevelData> levelProgressResult = await gameModule.UpdateLevelProgress(levelData, new List<RGNCurrency>()
            {
                new RGNCurrency()
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

        public async void GetLevelProgressButtonClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            GameModule gameModule = RGNCoreBuilder.I.GetModule<GameModule>();
            RGNLevelProgressResult<LevelData> levelProgressResult = await gameModule.GetLevelProgress<LevelData>();

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