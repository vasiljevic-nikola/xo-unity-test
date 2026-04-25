using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private const string TotalGamesKey = "TotalGames";
    private const string Player1WinsKey = "Player1Wins";
    private const string Player2WinsKey = "Player2Wins";
    private const string DrawsKey = "Draws";
    private const string TotalDurationKey = "TotalDuration";

    public int TotalGames { get; private set; }
    public int Player1Wins { get; private set; }
    public int Player2Wins { get; private set; }
    public int Draws { get; private set; }
    public float TotalDuration { get; private set; }

    public float AverageDuration
    {
        get
        {
            if (TotalGames == 0)
            {
                return 0f;
            }

            return TotalDuration / TotalGames;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadStats();
    }

    public void RegisterGameResult(string result, float duration)
    {
        TotalGames++;
        TotalDuration += duration;

        if (result == "P1")
        {
            Player1Wins++;
        }
        else if (result == "P2")
        {
            Player2Wins++;
        }
        else if (result == "DRAW")
        {
            Draws++;
        }

        SaveStats();
    }

    public void ResetStats()
    {
        TotalGames = 0;
        Player1Wins = 0;
        Player2Wins = 0;
        Draws = 0;
        TotalDuration = 0f;

        SaveStats();
    }

    private void LoadStats()
    {
        TotalGames = PlayerPrefs.GetInt(TotalGamesKey, 0);
        Player1Wins = PlayerPrefs.GetInt(Player1WinsKey, 0);
        Player2Wins = PlayerPrefs.GetInt(Player2WinsKey, 0);
        Draws = PlayerPrefs.GetInt(DrawsKey, 0);
        TotalDuration = PlayerPrefs.GetFloat(TotalDurationKey, 0f);
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt(TotalGamesKey, TotalGames);
        PlayerPrefs.SetInt(Player1WinsKey, Player1Wins);
        PlayerPrefs.SetInt(Player2WinsKey, Player2Wins);
        PlayerPrefs.SetInt(DrawsKey, Draws);
        PlayerPrefs.SetFloat(TotalDurationKey, TotalDuration);
        PlayerPrefs.Save();
    }

    public string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
