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
    public Slider VolumeMusicSlider;

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
        VolumeMusicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (PlayerPrefs.HasKey("Volume"))
        {
            float vol = PlayerPrefs.GetFloat("Volume");
            VolumeMusicSlider.value = vol;
            SetMusicVolume(vol);
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
        PlayerPrefs.SetFloat("Volume", vol);
    }
}
