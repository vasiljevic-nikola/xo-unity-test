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

    [Header("Cells")]
    public CellScript[] cells;

    [Header("Winning Line")]
    public RectTransform winningLine;

    private int winningPatternIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetBoardData();

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }

        if (winningLine != null)
        {
            winningLine.gameObject.SetActive(false);
        }
    }

    public void SwitchTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
    }

    public bool CheckWin()
    {
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, // Top row
            {3, 4, 5}, // Middle row
            {6, 7, 8}, // Bottom row
            {0, 3, 6}, // Left column
            {1, 4, 7}, // Middle column
            {2, 5, 8}, // Right column
            {0, 4, 8}, // Diagonal top-left to bottom-right
            {2, 4, 6}  // Diagonal top-right to bottom-left
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
            resultText.text = message;
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
    }

    private void ShowWinningLine()
    {
        if (winningLine == null || winningPatternIndex == -1)
        {
            return;
        }

        winningLine.gameObject.SetActive(true);

        // These values match the current 3x3 board layout:
        // Cell Size = 180, Spacing = 10, total visible line length around 560.
        float offset = 190f;
        float straightLineLength = 560f;
        float diagonalLineLength = 790f;

        Vector2 linePosition = Vector2.zero;
        float rotationZ = 0f;
        float lineWidth = straightLineLength;

        switch (winningPatternIndex)
        {
            // Rows
            case 0:
                linePosition = new Vector2(0f, offset);
                rotationZ = 0f;
                lineWidth = straightLineLength;
                break;

            case 1:
                linePosition = new Vector2(0f, 0f);
                rotationZ = 0f;
                lineWidth = straightLineLength;
                break;

            case 2:
                linePosition = new Vector2(0f, -offset);
                rotationZ = 0f;
                lineWidth = straightLineLength;
                break;

            // Columns
            case 3:
                linePosition = new Vector2(-offset, 0f);
                rotationZ = 90f;
                lineWidth = straightLineLength;
                break;

            case 4:
                linePosition = new Vector2(0f, 0f);
                rotationZ = 90f;
                lineWidth = straightLineLength;
                break;

            case 5:
                linePosition = new Vector2(offset, 0f);
                rotationZ = 90f;
                lineWidth = straightLineLength;
                break;

            // Diagonal top-left to bottom-right
            case 6:
                linePosition = Vector2.zero;
                rotationZ = -45f;
                lineWidth = diagonalLineLength;
                break;

            // Diagonal top-right to bottom-left
            case 7:
                linePosition = Vector2.zero;
                rotationZ = 45f;
                lineWidth = diagonalLineLength;
                break;
        }

        winningLine.anchoredPosition = linePosition;
        winningLine.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
        winningLine.sizeDelta = new Vector2(lineWidth, winningLine.sizeDelta.y);
    }

    public void ResetBoardData()
    {
        isPlayerOneTurn = true;
        isGameOver = false;
        winningPatternIndex = -1;

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
        }
    }

    public void RestartGame()
    {
        ResetBoardData();

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