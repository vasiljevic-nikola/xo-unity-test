using UnityEngine;
using TMPro;

public class CellScript : MonoBehaviour
{
    public TextMeshProUGUI cellText; // Reference to the text inside the cell
    public int cellIndex; // Index of this cell in the board array

    private bool isFilled = false; // Prevent overwriting

    public void OnCellClicked()
    {
        if (isFilled || GameManager.Instance.isGameOver) return;

        if (GameManager.Instance.isPlayerOneTurn)
        {
            cellText.text = "X";
            GameManager.Instance.board[cellIndex] = "X";
        }
        else
        {
            cellText.text = "O";
            GameManager.Instance.board[cellIndex] = "O";
        }

        isFilled = true;

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