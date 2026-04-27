using UnityEngine;

public class ExitPopupController : MonoBehaviour
{
    [Header("Exit Popup")]
    public GameObject exitPopup;

    public void OpenExitPopup()
    {
        if (exitPopup != null)
        {
            exitPopup.SetActive(true);
        }
    }

    public void CloseExitPopup()
    {
        if (exitPopup != null)
        {
            exitPopup.SetActive(false);
        }
    }

    public void ConfirmExit()
    {
        Debug.Log("Quit game requested.");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}