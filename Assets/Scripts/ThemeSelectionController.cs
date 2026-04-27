using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ThemeSelectionController : MonoBehaviour
{
    private const string ThemeKey = "SELECTED_THEME";

    [Header("Theme Popup")]
    public GameObject themePopup;

    [Header("Theme Buttons")]
    public Button classicThemeButton;
    public Button neonThemeButton;

    [Header("Theme Button Texts")]
    public TextMeshProUGUI classicThemeText;
    public TextMeshProUGUI neonThemeText;

    private int selectedThemeIndex = 0;

    private readonly Color normalColor = new Color32(245, 245, 245, 255);
    private readonly Color hoverColor = new Color32(220, 220, 255, 255);
    private readonly Color pressedColor = new Color32(190, 190, 230, 255);
    private readonly Color selectedColor = new Color32(90, 35, 150, 255);

    private readonly Color normalTextColor = Color.black;
    private readonly Color selectedTextColor = Color.white;

    private void Start()
    {
        selectedThemeIndex = PlayerPrefs.GetInt(ThemeKey, 0);

        if (themePopup != null)
        {
            themePopup.SetActive(false);
        }

        UpdateThemeButtonVisuals();
    }

    public void OpenThemePopup()
    {
        if (themePopup != null)
        {
            themePopup.SetActive(true);
        }

        UpdateThemeButtonVisuals();
    }

    public void SelectClassicTheme()
    {
        selectedThemeIndex = 0;
        PlayerPrefs.SetInt(ThemeKey, selectedThemeIndex);
        PlayerPrefs.Save();

        UpdateThemeButtonVisuals();
    }

    public void SelectNeonTheme()
    {
        selectedThemeIndex = 1;
        PlayerPrefs.SetInt(ThemeKey, selectedThemeIndex);
        PlayerPrefs.Save();

        UpdateThemeButtonVisuals();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void CloseThemePopup()
    {
        if (themePopup != null)
        {
            themePopup.SetActive(false);
        }
    }

    private void UpdateThemeButtonVisuals()
    {
        ApplyButtonVisual(classicThemeButton, classicThemeText, selectedThemeIndex == 0);
        ApplyButtonVisual(neonThemeButton, neonThemeText, selectedThemeIndex == 1);
    }

    private void ApplyButtonVisual(Button button, TextMeshProUGUI buttonText, bool isSelected)
    {
        if (button == null || buttonText == null)
        {
            return;
        }

        ColorBlock colors = button.colors;

        if (isSelected)
        {
            colors.normalColor = selectedColor;
            colors.highlightedColor = selectedColor;
            colors.selectedColor = selectedColor;
            buttonText.color = selectedTextColor;
        }
        else
        {
            colors.normalColor = normalColor;
            colors.highlightedColor = hoverColor;
            colors.selectedColor = hoverColor;
            buttonText.color = normalTextColor;
        }

        colors.pressedColor = pressedColor;
        colors.disabledColor = new Color32(120, 120, 120, 255);
        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.1f;

        button.colors = colors;
    }
}