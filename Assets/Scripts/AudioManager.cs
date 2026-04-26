using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private const string BgmKey = "BGM_ENABLED";
    private const string SfxKey = "SFX_ENABLED";

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip clickClip;
    public AudioClip popClip;
    public AudioClip winClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ApplyBgmSetting();
    }

    public void ApplyBgmSetting()
    {
        bool bgmEnabled = PlayerPrefs.GetInt(BgmKey, 1) == 1;

        if (bgmSource == null || bgmClip == null)
        {
            return;
        }

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;

        if (bgmEnabled)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.Play();
            }
        }
        else
        {
            bgmSource.Stop();
        }
    }

    public void PlayClick()
    {
        PlaySfx(clickClip);
    }

    public void PlayPop()
    {
        PlaySfx(popClip);
    }

    public void PlayWin()
    {
        PlaySfx(winClip);
    }

    private void PlaySfx(AudioClip clip)
    {
        bool sfxEnabled = PlayerPrefs.GetInt(SfxKey, 1) == 1;

        if (!sfxEnabled || sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}