using UnityEngine;
using TMPro;

public class CellScript : MonoBehaviour
{
    public TextMeshProUGUI cellText;
    public int cellIndex;

    private bool isFilled = false;

    public void OnCellClicked()
    {
        if (isFilled || GameManager.Instance.isGameOver)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPop();
        }

        if (GameManager.Instance.isPlayerOneTurn)
        {
            cellText.text = "X";
            cellText.color = GameThemeApplier.XColor;
            GameManager.Instance.board[cellIndex] = "X";
        }
        else
        {
            cellText.text = "O";
            cellText.color = GameThemeApplier.OColor;
            GameManager.Instance.board[cellIndex] = "O";
        }

        isFilled = true;

        GameManager.Instance.RegisterMoveForCurrentPlayer();

        if (GameManager.Instance.CheckWin())
        {
            if (GameManager.Instance.isPlayerOneTurn)
            {
                GameManager.Instance.ShowGameOver("PLAYER 1 WINS");
            }
            else
            {
                GameManager.Instance.ShowGameOver("PLAYER 2 WINS");
            }
        }
        else if (GameManager.Instance.CheckDraw())
        {
            GameManager.Instance.ShowGameOver("DRAW");
        }
        else
        {
            GameManager.Instance.SwitchTurn();
        }
    }

    public void ResetCell()
    {
        isFilled = false;

        if (cellText != null)
        {
            cellText.text = "";
        }
    }
}