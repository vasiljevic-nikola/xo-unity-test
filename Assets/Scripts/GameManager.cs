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

    private int winningPatternIndex = -1;

    private float matchDuration = 0f;
    private int player1Moves = 0;
    private int player2Moves = 0;

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

        if (winningLine != null)
        {
            winningLine.gameObject.SetActive(false);
        }
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
        isGameOver = true;

        ShowWinningLine();

        if (resultText != null)
        {
            resultText.text = message + "\nDuration: " + FormatTime(matchDuration);
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
    }

    private void ShowWinningLine()
    {
        if (winningLine == null || gameBoardRect == null || winningPatternIndex == -1)
        {
            return;
        }

        winningLine.gameObject.SetActive(true);

        float offset = 140f;
        float straightLineLength = 412f;
        float diagonalLineLength = 585f;

        Vector2 boardCenter = gameBoardRect.anchoredPosition;
        Vector2 localOffset = Vector2.zero;

        float rotationZ = 0f;
        float lineWidth = straightLineLength;

        switch (winningPatternIndex)
        {
            case 0:
                localOffset = new Vector2(0f, offset);
                rotationZ = 0f;
                break;

            case 1:
                localOffset = Vector2.zero;
                rotationZ = 0f;
                break;

            case 2:
                localOffset = new Vector2(0f, -offset);
                rotationZ = 0f;
                break;

            case 3:
                localOffset = new Vector2(-offset, 0f);
                rotationZ = 90f;
                break;

            case 4:
                localOffset = Vector2.zero;
                rotationZ = 90f;
                break;

            case 5:
                localOffset = new Vector2(offset, 0f);
                rotationZ = 90f;
                break;

            case 6:
                localOffset = Vector2.zero;
                rotationZ = -45f;
                lineWidth = diagonalLineLength;
                break;

            case 7:
                localOffset = Vector2.zero;
                rotationZ = 45f;
                lineWidth = diagonalLineLength;
                break;
        }

        winningLine.anchoredPosition = boardCenter + localOffset;
        winningLine.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
        winningLine.sizeDelta = new Vector2(lineWidth, winningLine.sizeDelta.y);
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
        isPlayerOneTurn = true;
        isGameOver = false;
        winningPatternIndex = -1;

        matchDuration = 0f;
        player1Moves = 0;
        player2Moves = 0;

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
        }

        UpdateTimerUI();
        UpdateMoveCounterUI();
    }

    public void RestartGame()
    {
        ResetGameState();

        foreach (CellScript cell in cells)
        {
            if (cell != null)
            {
                cell.ResetCell();
            }
        }

        if (winningLine != null)
        {
            winningLine.gameObject.SetActive(false);
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("PlayScene");
    }
}