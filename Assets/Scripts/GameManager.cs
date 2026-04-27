using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerOneTurn = true;
    public bool isGameOver = false;

    public string[] board = new string[9];

    [Header("Game Over UI")]
    public GameObject gameOverPopup;
    public TextMeshProUGUI resultText;

    [Header("HUD UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI player1MovesText;
    public TextMeshProUGUI player2MovesText;

    [Header("Cells")]
    public CellScript[] cells;

    [Header("Winning Line")]
    public RectTransform gameBoardRect;
    public RectTransform winningLine;

    [Header("Winning Line Animation")]
    public float winningLineDrawDuration = 0.45f;
    public float popupDelayAfterLine = 0.6f;
    public float drawPopupDelay = 0.4f;

    private int winningPatternIndex = -1;

    private float matchDuration = 0f;
    private int player1Moves = 0;
    private int player2Moves = 0;

    private bool resultAlreadySaved = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetGameState();

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }

        HideWinningLine();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            matchDuration += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void RegisterMoveForCurrentPlayer()
    {
        if (isPlayerOneTurn)
        {
            player1Moves++;
        }
        else
        {
            player2Moves++;
        }

        UpdateMoveCounterUI();
    }

    public void SwitchTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
    }

    public bool CheckWin()
    {
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6}
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (board[a] != "" && board[a] == board[b] && board[b] == board[c])
            {
                winningPatternIndex = i;
                return true;
            }
        }

        winningPatternIndex = -1;
        return false;
    }

    public bool CheckDraw()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == "")
            {
                return false;
            }
        }

        return true;
    }

    public void ShowGameOver(string message)
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        SaveResultToStats(message);

        bool isWin = message == "PLAYER 1 WINS" || message == "PLAYER 2 WINS";

        if (isWin)
        {
            StartCoroutine(ShowWinSequence(message));
        }
        else
        {
            StartCoroutine(ShowDrawSequence(message));
        }
    }

    private IEnumerator ShowWinSequence(string message)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWin();
        }

        yield return StartCoroutine(AnimateWinningLine());

        yield return new WaitForSeconds(popupDelayAfterLine);

        ShowGameOverPopup(message);
    }

    private IEnumerator ShowDrawSequence(string message)
    {
        yield return new WaitForSeconds(drawPopupDelay);

        ShowGameOverPopup(message);
    }

    private IEnumerator AnimateWinningLine()
    {
        if (winningLine == null || gameBoardRect == null || winningPatternIndex == -1)
        {
            yield break;
        }

        float offset = 140f;
        float straightLineLength = 412f;
        float diagonalLineLength = 585f;

        Vector2 boardCenter = gameBoardRect.anchoredPosition;

        Vector2 startPosition = boardCenter;
        float rotationZ = 0f;
        float targetWidth = straightLineLength;

        switch (winningPatternIndex)
        {
            case 0: // Top row
                startPosition = boardCenter + new Vector2(-straightLineLength / 2f, offset);
                rotationZ = 0f;
                targetWidth = straightLineLength;
                break;

            case 1: // Middle row
                startPosition = boardCenter + new Vector2(-straightLineLength / 2f, 0f);
                rotationZ = 0f;
                targetWidth = straightLineLength;
                break;

            case 2: // Bottom row
                startPosition = boardCenter + new Vector2(-straightLineLength / 2f, -offset);
                rotationZ = 0f;
                targetWidth = straightLineLength;
                break;

            case 3: // Left column
                startPosition = boardCenter + new Vector2(-offset, -straightLineLength / 2f);
                rotationZ = 90f;
                targetWidth = straightLineLength;
                break;

            case 4: // Middle column
                startPosition = boardCenter + new Vector2(0f, -straightLineLength / 2f);
                rotationZ = 90f;
                targetWidth = straightLineLength;
                break;

            case 5: // Right column
                startPosition = boardCenter + new Vector2(offset, -straightLineLength / 2f);
                rotationZ = 90f;
                targetWidth = straightLineLength;
                break;

            case 6: // Diagonal top-left to bottom-right
                startPosition = boardCenter + new Vector2(-diagonalLineLength * 0.5f * 0.7071f, diagonalLineLength * 0.5f * 0.7071f);
                rotationZ = -45f;
                targetWidth = diagonalLineLength;
                break;

            case 7: // Diagonal bottom-left to top-right
                startPosition = boardCenter + new Vector2(-diagonalLineLength * 0.5f * 0.7071f, -diagonalLineLength * 0.5f * 0.7071f);
                rotationZ = 45f;
                targetWidth = diagonalLineLength;
                break;
        }

        winningLine.gameObject.SetActive(true);

        winningLine.pivot = new Vector2(0f, 0.5f);
        winningLine.anchoredPosition = startPosition;
        winningLine.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
        winningLine.sizeDelta = new Vector2(0f, winningLine.sizeDelta.y);

        float elapsedTime = 0f;

        while (elapsedTime < winningLineDrawDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / winningLineDrawDuration);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            winningLine.sizeDelta = new Vector2(targetWidth * smoothProgress, winningLine.sizeDelta.y);

            yield return null;
        }

        winningLine.sizeDelta = new Vector2(targetWidth, winningLine.sizeDelta.y);
    }

    private void ShowGameOverPopup(string message)
    {
        if (resultText != null)
        {
            resultText.text = message + "\nDuration: " + FormatTime(matchDuration);
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
    }

    private void SaveResultToStats(string message)
    {
        if (resultAlreadySaved)
        {
            return;
        }

        resultAlreadySaved = true;

        if (StatsManager.Instance == null)
        {
            Debug.LogWarning("StatsManager is missing. Game result was not saved.");
            return;
        }

        if (message == "PLAYER 1 WINS")
        {
            StatsManager.Instance.RegisterGameResult("P1", matchDuration);
        }
        else if (message == "PLAYER 2 WINS")
        {
            StatsManager.Instance.RegisterGameResult("P2", matchDuration);
        }
        else if (message == "DRAW")
        {
            StatsManager.Instance.RegisterGameResult("DRAW", matchDuration);
        }
    }

    private void HideWinningLine()
    {
        if (winningLine != null)
        {
            winningLine.gameObject.SetActive(false);
            winningLine.sizeDelta = new Vector2(0f, winningLine.sizeDelta.y);
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(matchDuration);
        }
    }

    private void UpdateMoveCounterUI()
    {
        if (player1MovesText != null)
        {
            player1MovesText.text = "P1 Moves: " + player1Moves;
        }

        if (player2MovesText != null)
        {
            player2MovesText.text = "P2 Moves: " + player2Moves;
        }
    }

    private string FormatTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void ResetGameState()
    {
        StopAllCoroutines();

        isPlayerOneTurn = true;
        isGameOver = false;
        winningPatternIndex = -1;

        matchDuration = 0f;
        player1Moves = 0;
        player2Moves = 0;
        resultAlreadySaved = false;

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
        }

        UpdateTimerUI();
        UpdateMoveCounterUI();
    }

    public void RestartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }

        ResetGameState();

        foreach (CellScript cell in cells)
        {
            if (cell != null)
            {
                cell.ResetCell();
            }
        }

        HideWinningLine();

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }

        SceneManager.LoadScene("PlayScene");
    }
}