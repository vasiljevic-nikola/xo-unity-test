using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads the main gameplay scene.
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Loads the play menu scene.
    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    // Quits the application.
    public void QuitGame()
    {
        Application.Quit();
    }
}
