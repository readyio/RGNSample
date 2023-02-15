using System.Collections.Generic;
using System.Globalization;
using RGN.Modules.Matchmaking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class MatchmakingTestPopUp : AbstractPopup
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject idleBlock;
        [SerializeField] private Button findMatchButton;
        [SerializeField] private Button openHistoryButton;
        [SerializeField] private GameObject activeMatchBlock;
        [SerializeField] private TextMeshProUGUI matchIdText;
        [SerializeField] private TMP_InputField scoreInput;
        [SerializeField] private Button submitScoreButton;
        [SerializeField] private GameObject submittedScoreBlock;
        [SerializeField] private Button submittedScoreOkButton;
        [SerializeField] private GameObject historyBlock;
        [SerializeField] private MatchmakingTestPopUpHistoryItem historyItemTemplate;
        [SerializeField] private Transform historyItemContent;

        private MatchmakingModule matchmakingModule;
        private string currentMatchGameType;
        private string currentMatchId;
        private List<MatchmakingTestPopUpHistoryItem> historyItems;

        private void Start()
        {
            matchmakingModule = MatchmakingModule.I;

            idleBlock.SetActive(true);
            activeMatchBlock.SetActive(false);
            submittedScoreBlock.SetActive(false);
            historyBlock.SetActive(false);
            historyItemTemplate.gameObject.SetActive(false);

            closeButton.onClick.AddListener(OnButtonCloseClick);
            findMatchButton.onClick.AddListener(OnButtonFindMatchClick);
            openHistoryButton.onClick.AddListener(OnButtonOpenHistoryClick);
            submitScoreButton.onClick.AddListener(OnButtonSubmitScoreClick);
            submittedScoreOkButton.onClick.AddListener(OnButtonSubmittedScoreOkClick);
        }

        private void OnButtonCloseClick()
        {
            if (historyBlock.activeSelf)
            {
                historyBlock.SetActive(false);
                idleBlock.SetActive(true);
            }
            else if (submittedScoreBlock.activeSelf)
            {
                submittedScoreBlock.SetActive(false);
                idleBlock.SetActive(true);
            }
            else if (activeMatchBlock.activeSelf)
            {
                activeMatchBlock.SetActive(false);
                idleBlock.SetActive(true);
            }
            else
            {
                Hide(true, null);
            }
        }

        private async void OnButtonFindMatchClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            currentMatchGameType = "practice";

            StartMatchResponseData response = await matchmakingModule.StartMatchAsync(currentMatchGameType);
            currentMatchId = response.matchId;
            matchIdText.text = currentMatchId;

            scoreInput.text = string.Empty;

            idleBlock.SetActive(false);
            activeMatchBlock.SetActive(true);

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async void OnButtonOpenHistoryClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            idleBlock.SetActive(false);
            historyBlock.SetActive(true);

            if (historyItems != null)
            {
                foreach (MatchmakingTestPopUpHistoryItem item in historyItems)
                {
                    Destroy(item.gameObject);
                }

                historyItems = null;
            }

            historyItems = new List<MatchmakingTestPopUpHistoryItem>();

            GetMatchmakingHistoryResponseData response = await matchmakingModule.GetHistoryAsync();
            List<MatchmakingRecord> historyRecords = response.history;
            foreach (var historyRecord in historyRecords)
            {
                MatchmakingTestPopUpHistoryItem historyItem = Instantiate(historyItemTemplate, historyItemContent);
                historyItem.gameObject.SetActive(true);
                historyItem.Init(historyRecord);

                historyItems.Add(historyItem);
            }

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private async void OnButtonSubmitScoreClick()
        {
            UIRoot.singleton.ShowPopup<SpinnerPopup>();

            float.TryParse(scoreInput.text, out var score);
            scoreInput.text = score.ToString(CultureInfo.InvariantCulture);

            SubmitMatchScoreResponseData response = await matchmakingModule.SubmitMatchScoreAsync(currentMatchGameType, currentMatchId, score);
            currentMatchGameType = string.Empty;
            currentMatchId = string.Empty;

            activeMatchBlock.SetActive(false);
            submittedScoreBlock.SetActive(true);

            UIRoot.singleton.HidePopup<SpinnerPopup>();
        }

        private void OnButtonSubmittedScoreOkClick()
        {
            submittedScoreBlock.SetActive(false);
            idleBlock.SetActive(true);
        }
    }
}
