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
                return true;
            }
        }

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

        if (resultText != null)
        {
            resultText.text = message;
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
    }

    public void ResetBoardData()
    {
        isPlayerOneTurn = true;
        isGameOver = false;

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
        }
    }

    public void RestartGame()
    {
        Debug.Log("RESTART BUTTON CLICKED");

        ResetBoardData();

        foreach (CellScript cell in cells)
        {
            if (cell != null)
            {
                cell.ResetCell();
            }
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