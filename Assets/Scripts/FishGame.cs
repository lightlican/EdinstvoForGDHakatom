using UnityEngine;
using UnityEngine.UI;

public class FishGame : MonoBehaviour
{
    [Header("UI элементы")]
    public GameObject fishingUI;
    public RectTransform fish;
    public RectTransform targetZone;
    public Text timerText;
    public Text hintText;

    [Header("Настройки")]
    public float totalTime = 30f;
    public float fishSpeed = 100f;
    public float targetSpeed = 300f;
    public float fishChangeDirTime = 1f;

    
    public static FishGame Instance { get; private set; }

    
    private bool isGameActive = false;
    private float currentTime = 0f;
    private float successTime = 0f;
    private float fishDirection = 1f;
    private float timeToNextDirection = 0f;

    
    public FishingSpot currentSpot;

    void Awake()
    {
        
        Instance = this;
    }

    void Start()
    {
        if (fishingUI != null)
            fishingUI.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive) return;

        //движение рыбы
        timeToNextDirection -= Time.deltaTime;
        if (timeToNextDirection <= 0f)
        {
            fishDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
            timeToNextDirection = Random.Range(0.5f, fishChangeDirTime);

            if (Random.Range(0f, 1f) > 0.7f)
                fishDirection *= 2f;
        }

        Vector2 fishPos = fish.anchoredPosition;
        fishPos.x += fishDirection * fishSpeed * Time.deltaTime;

        float maxFishX = 400f;
        if (fishPos.x > maxFishX)
        {
            fishPos.x = maxFishX;
            fishDirection = -1f;
        }
        if (fishPos.x < -maxFishX)
        {
            fishPos.x = -maxFishX;
            fishDirection = 1f;
        }
        fish.anchoredPosition = fishPos;

        
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.K)) moveInput = -1f;
        if (Input.GetKey(KeyCode.L)) moveInput = 1f;

        Vector2 targetPos = targetZone.anchoredPosition;
        targetPos.x += moveInput * targetSpeed * Time.deltaTime;

        float maxTargetX = 400f;
        targetPos.x = Mathf.Clamp(targetPos.x, -maxTargetX, maxTargetX);
        targetZone.anchoredPosition = targetPos;

        
        bool isInZone = IsFishInTargetZone();

        if (isInZone)
        {
            successTime += Time.deltaTime;
            hintText.text = "РЫБА В ЗОНЕ! Держи...";
            hintText.color = Color.green;
            targetZone.GetComponent<Image>().color = new Color(0, 1, 0, 0.8f);
        }
        else
        {
            hintText.text = "РЫБА УШЛА! Двигай зону L/K";
            hintText.color = Color.red;
            targetZone.GetComponent<Image>().color = new Color(1, 0.5f, 0, 0.8f);
        }

        
        currentTime += Time.deltaTime;
        float timeLeft = totalTime - currentTime;
        timerText.text = string.Format("{0:F1} сек", timeLeft);

        
        if (currentTime >= totalTime)
        {
            EndGame();
        }
    }

    bool IsFishInTargetZone()
    {
        float fishLeft = fish.anchoredPosition.x - fish.rect.width / 2;
        float fishRight = fish.anchoredPosition.x + fish.rect.width / 2;
        float targetLeft = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetRight = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        return fishRight > targetLeft && fishLeft < targetRight;
    }

    public void StartGame()
    {
        isGameActive = true;
        currentTime = 0f;
        successTime = 0f;

        fishingUI.SetActive(true);

        fish.anchoredPosition = Vector2.zero;
        targetZone.anchoredPosition = Vector2.zero;

        fishDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
        timeToNextDirection = fishChangeDirTime;


        hintText.text = "УДЕРЖИВАЙ РЫБУ В ЗОНЕ! L/K";
        timerText.text = "30.0 сек";
    }

    void EndGame()
    {
        CancelInvoke();
        isGameActive = false;


        if (currentSpot != null)
        {
            currentSpot.RemoveFishingRod();
        }

        float requiredTime = totalTime * 0.3f;
        bool isSuccess = successTime >= requiredTime;

        if (isSuccess)
        {
            hintText.text = $"УСПЕХ! {successTime:F1}/{requiredTime:F1} сек";
            hintText.color = Color.green;
            timerText.text = "РЫБА ПОЙМАНА!";

            SiberianQuestManager.FishCaught();

            Invoke("CloseUI", 3f);
        }
        else
        {
            hintText.text = $"ПРОВАЛ: {successTime:F1}/{requiredTime:F1} сек";
            hintText.color = Color.red;
            timerText.text = "РЫБА УПЛЫЛА...";
            Invoke("ShowRetryOption", 2f);
        }
    }

    void CloseUI()
    {
        fishingUI.SetActive(false);
    }

    void ShowRetryOption()
    {
        hintText.text += "\n\nНажми R чтобы попробовать снова\nESC чтобы выйти";
    }

    public void RetryGame()
    {
        fishingUI.SetActive(false);
        StartGame();
    }

    public void ExitGame()
    {
        fishingUI.SetActive(false);


        if (currentSpot != null)
        {
            currentSpot.RemoveFishingRod();
        }
    }
}