using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerOneTurn = true;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
    }
}
