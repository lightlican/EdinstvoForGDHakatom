using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("UI")]
    public Slider masterVolumeSlider;
    public TextMeshProUGUI volumeValueText;

    [Header("Resolution Buttons")]
    public Button res1080Button;
    public Button res720Button;
    public Button res900Button;
    public Button res1050Button;
    public Button res2KButton;        
    public Button res768Button;        

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
            return;
        }

        LoadSettings();
    }

    void Start()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        if (res1080Button != null)
            res1080Button.onClick.AddListener(() => SetResolution(1920, 1080));

        if (res720Button != null)
            res720Button.onClick.AddListener(() => SetResolution(1280, 720));

        if (res900Button != null)
            res900Button.onClick.AddListener(() => SetResolution(1600, 900));

        if (res1050Button != null)
            res1050Button.onClick.AddListener(() => SetResolution(1680, 1050));

        if (res2KButton != null)
            res2KButton.onClick.AddListener(() => SetResolution(2560, 1440));

        if (res768Button != null)
            res768Button.onClick.AddListener(() => SetResolution(1366, 768));
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(value * 100) + "%";

        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
    }

    void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = savedVolume;
            if (volumeValueText != null)
                volumeValueText.text = Mathf.RoundToInt(savedVolume * 100) + "%";
        }

        int width = PlayerPrefs.GetInt("ResolutionWidth", 1920);
        int height = PlayerPrefs.GetInt("ResolutionHeight", 1080);
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}