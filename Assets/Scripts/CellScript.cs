using UnityEngine;
using TMPro;

public class CellScript : MonoBehaviour
{
    public TextMeshProUGUI cellText; // Reference to the text inside the cell

    private bool isFilled = false; // Prevent overwriting

    public void OnCellClicked()
    {
        if (isFilled) return;

        if (GameManager.Instance.isPlayerOneTurn)
        {
            cellText.text = "X";
        }
        else
        {
            cellText.text = "O";
        }

        isFilled = true;

        GameManager.Instance.SwitchTurn();
    }
}
