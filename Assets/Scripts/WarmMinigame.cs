using UnityEngine;
using UnityEngine.UI;

public class WarmMinigame : MonoBehaviour
{
    public GameObject minigamePanel;
    public Slider warmthSlider;
    public Text instructionText;
    public Text timerText;

    public float gameDuration = 15f;
    public float heatDropPerSecond = 5f;
    public float heatPerClick = 10f;

    private bool isGameActive = false;
    private float currentHeat = 100f;
    private float currentTime = 0f;
    private bool gameEnded = false;

    private NorthernQuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<NorthernQuestManager>();
        if (minigamePanel != null)
            minigamePanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive || gameEnded) return;

        currentTime += Time.deltaTime;
        float timeLeft = gameDuration - currentTime;
        timerText.text = $"{Mathf.CeilToInt(timeLeft)} сек";

        currentHeat -= heatDropPerSecond * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHeat += heatPerClick;
        }

        currentHeat = Mathf.Clamp(currentHeat, 0, 100);
        warmthSlider.value = currentHeat;

        if (currentHeat <= 0)
        {
            FailGame();
            return;
        }

        if (currentTime >= gameDuration && currentHeat > 0)
        {
            WinGame();
        }
    }

    public void StartMinigame()
    {
        isGameActive = true;
        gameEnded = false;
        currentHeat = 100f;
        currentTime = 0f;

        minigamePanel.SetActive(true);
        warmthSlider.value = 100f;
        timerText.text = $"{gameDuration} сек";

        PlayerMovement.SetMovement(false);
    }

    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        isGameActive = false;

        if (questManager != null)
            questManager.WarmthGameWon();

        Invoke("CloseMinigame", 2f);
    }

    void FailGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        isGameActive = false;

        Invoke("CloseMinigame", 2f);
    }

    void CloseMinigame()
    {
        minigamePanel.SetActive(false);
        PlayerMovement.SetMovement(true);
    }
}