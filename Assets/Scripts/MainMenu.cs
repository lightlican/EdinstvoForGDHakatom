using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Кнопки")]
    public Button continueButton;
    public Button newGameButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Панели")]
    public GameObject settingsPanel;
    public Button backButton;

    [Header("Настройки")]
    public SettingsManager settingsManager;

    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(NewGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);

        if (backButton != null)
            backButton.onClick.AddListener(CloseSettings);

        CheckContinueAvailability();

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void CheckContinueAvailability()
    {
        bool hasProgress = PlayerPrefs.GetInt("TatarQuestDone", 0) == 1 ||
                          PlayerPrefs.GetInt("RussianQuestDone", 0) == 1 ||
                          PlayerPrefs.GetInt("SiberianQuestDone", 0) == 1 ||
                          PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1 ||
                          PlayerPrefs.GetInt("CaucasusQuestDone", 0) == 1;

        if (!hasProgress && continueButton != null)
        {
            continueButton.interactable = false;
            Text buttonText = continueButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.color = Color.gray;
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("MasterScene");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("NewGameStarted", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MasterScene");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}