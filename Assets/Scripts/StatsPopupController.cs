using UnityEngine;
using TMPro;

public class StatsPopupController : MonoBehaviour
{
    [Header("Popup")]
    public GameObject statsPopup;

    [Header("Text")]
    public TextMeshProUGUI statsContentText;

    public void OpenStatsPopup()
    {
        UpdateStatsText();

        if (statsPopup != null)
        {
            statsPopup.SetActive(true);
        }
    }

    public void CloseStatsPopup()
    {
        if (statsPopup != null)
        {
            statsPopup.SetActive(false);
        }
    }

    private void UpdateStatsText()
    {
        if (statsContentText == null)
        {
            return;
        }

        if (StatsManager.Instance == null)
        {
            statsContentText.text = "Stats are not available.";
            return;
        }

        statsContentText.text =
            "Total Games: " + StatsManager.Instance.TotalGames + "\n" +
            "Player 1 Wins: " + StatsManager.Instance.Player1Wins + "\n" +
            "Player 2 Wins: " + StatsManager.Instance.Player2Wins + "\n" +
            "Draws: " + StatsManager.Instance.Draws + "\n" +
            "Average Duration: " + StatsManager.Instance.FormatTime(StatsManager.Instance.AverageDuration);
    }
}
