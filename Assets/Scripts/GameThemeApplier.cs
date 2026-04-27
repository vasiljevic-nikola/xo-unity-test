using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameThemeApplier : MonoBehaviour
{
    private const string ThemeKey = "SELECTED_THEME";

    public static Color XColor { get; private set; }
    public static Color OColor { get; private set; }

    [Header("Main Colors")]
    public Image backgroundImage;
    public Image gameBoardImage;
    public Image winningLineImage;

    [Header("Cells")]
    public Image[] cellImages;

    private void Start()
    {
        ApplySelectedTheme();
    }

    public void ApplySelectedTheme()
    {
        int selectedTheme = PlayerPrefs.GetInt(ThemeKey, 0);

        if (selectedTheme == 0)
        {
            ApplyClassicTheme();
        }
        else
        {
            ApplyNeonTheme();
        }
    }

    private void ApplyClassicTheme()
    {
        Color backgroundColor = new Color32(48, 78, 120, 255);
        Color boardColor = new Color32(150, 155, 165, 255);
        Color cellColor = new Color32(245, 245, 245, 255);
        Color lineColor = new Color32(210, 40, 40, 255);

        XColor = Color.black;
        OColor = Color.black;

        ApplyColors(backgroundColor, boardColor, cellColor, lineColor);
    }

    private void ApplyNeonTheme()
    {
        Color backgroundColor = new Color32(20, 20, 35, 255);
        Color boardColor = new Color32(45, 45, 70, 255);
        Color cellColor = new Color32(30, 30, 50, 255);
        Color lineColor = new Color32(255, 80, 180, 255);

        XColor = new Color32(0, 230, 255, 255);
        OColor = new Color32(255, 80, 180, 255);

        ApplyColors(backgroundColor, boardColor, cellColor, lineColor);
    }

    private void ApplyColors(Color backgroundColor, Color boardColor, Color cellColor, Color lineColor)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundColor;
        }

        if (gameBoardImage != null)
        {
            gameBoardImage.color = boardColor;
        }

        if (winningLineImage != null)
        {
            winningLineImage.color = lineColor;
        }

        foreach (Image cellImage in cellImages)
        {
            if (cellImage != null)
            {
                cellImage.color = cellColor;
            }
        }
    }
}
