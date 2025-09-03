using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance;

    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Settings UI")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider MusicSlider;
    public Slider SFXSlider;

    private Resolution[] resolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1420, height = 960 },
        new Resolution { width = 600,  height = 400 }
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        Screen.fullScreen = true;
        fullscreenToggle.isOn = true;

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new System.Collections.Generic.List<string> {
            "1920x1080", "1420x960", "600x400"
        });

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float vol = PlayerPrefs.GetFloat("MusicVolume");
            MusicSlider.value = vol;
            SetMusicVolume(vol);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float vol = PlayerPrefs.GetFloat("SFXVolume");
            SFXSlider.value = vol;
            SetSFXVolume(vol);
        }
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetMusicVolume(float vol)
    {
        AudioManager.Instance.SetMusicVolume(vol);
        PlayerPrefs.SetFloat("MusicVolume", vol);
    }

    public void SetSFXVolume(float vol)
    {
        AudioManager.Instance.SetSFXVolume(vol);
        PlayerPrefs.SetFloat("SFXVolume", vol);
    }
}
