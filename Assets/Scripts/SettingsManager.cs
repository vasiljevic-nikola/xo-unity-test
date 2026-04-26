using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private const string BgmKey = "BGM_ENABLED";
    private const string SfxKey = "SFX_ENABLED";

    [Header("Settings Popup")]
    public GameObject settingsPopup;

    [Header("Toggles")]
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    public bool IsBgmEnabled { get; private set; }
    public bool IsSfxEnabled { get; private set; }

    private void Awake()
    {
        Instance = this;
        LoadSettings();
    }

    private void Start()
    {
        SetupToggleListeners();
        ApplyToggleValues();

        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
        }
    }

    private void SetupToggleListeners()
    {
        if (bgmToggle != null)
        {
            bgmToggle.onValueChanged.RemoveAllListeners();
            bgmToggle.onValueChanged.AddListener(SetBgmEnabled);
        }

        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.RemoveAllListeners();
            sfxToggle.onValueChanged.AddListener(SetSfxEnabled);
        }
    }

    public void OpenSettings()
    {
        LoadSettings();
        ApplyToggleValues();

        if (settingsPopup != null)
        {
            settingsPopup.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
        }
    }

    public void SetBgmEnabled(bool isEnabled)
    {
        IsBgmEnabled = isEnabled;
        PlayerPrefs.SetInt(BgmKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("BGM saved: " + isEnabled);
    }

    public void SetSfxEnabled(bool isEnabled)
    {
        IsSfxEnabled = isEnabled;
        PlayerPrefs.SetInt(SfxKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("SFX saved: " + isEnabled);
    }

    private void LoadSettings()
    {
        IsBgmEnabled = PlayerPrefs.GetInt(BgmKey, 1) == 1;
        IsSfxEnabled = PlayerPrefs.GetInt(SfxKey, 1) == 1;
    }

    private void ApplyToggleValues()
    {
        if (bgmToggle != null)
        {
            bgmToggle.SetIsOnWithoutNotify(IsBgmEnabled);
        }

        if (sfxToggle != null)
        {
            sfxToggle.SetIsOnWithoutNotify(IsSfxEnabled);
        }
    }
}