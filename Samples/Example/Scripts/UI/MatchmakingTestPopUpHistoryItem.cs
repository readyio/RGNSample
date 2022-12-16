using RGN.Modules.GameProgress;
using RGN.Modules.Matchmaking;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Sample.UI
{
    public class MatchmakingTestPopUpHistoryItem : MonoBehaviour
    {
        [SerializeField] private Color winColor;
        [SerializeField] private Color loseColor;
        [SerializeField] private Color drawColor;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void Init(MatchmakingRecord historyRecord)
        {
            GameUserFullProfileData userData = ProfileController.CurrentUserData;

            string userId = userData.userId;
            bool isHost = historyRecord.hostId == userId;
            float userScore = isHost ? historyRecord.hostScore : historyRecord.opponentScore;
            float opponentScore = isHost ? historyRecord.opponentScore : historyRecord.hostScore;
            bool isWin = userScore > opponentScore;
            bool isDraw = Mathf.Approximately(userScore, opponentScore);
            DateTime finishedAt = new DateTime(1970, 1, 1).AddMilliseconds(historyRecord.finishedAt);

            backgroundImage.color = isDraw ? drawColor : isWin ? winColor : loseColor;
            dateText.text = finishedAt.ToString(CultureInfo.InvariantCulture);
            scoreText.text = userScore.ToString(CultureInfo.InvariantCulture);
        }
    }
}