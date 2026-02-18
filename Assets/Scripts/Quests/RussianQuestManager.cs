using UnityEngine;
using UnityEngine.UI;

public class RussianQuestManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public GameObject blacksmith;
    public GameObject forge;
    public GameObject anvil;
    public GameObject waterBarrel;
    public GameObject crystal;
    public Text questHintText;

    [Header("Настройки раундов")]
    public int currentRound = 1;
    public int maxRounds = 10;
    public int baseLength = 2;        
    public int roundLengthIncrease = 1; 

    private int targetLength;          

    [Header("UI Мини-игры")]
    public GameObject minigamePanel;
    public Text sequenceText;
    public Text attemptsText;

    [Header("Предметы в руках")]
    public GameObject ingotPrefab;        
    public GameObject horseshoePrefab;    
    public Transform holdPoint;           

    [Header("Настройки сложности")]
    public float rememberTime = 2f;
    public int maxAttempts = 3;

    
    private bool questStarted = false;
    private bool hasIngot = false;
    private bool forgingSuccess = false;
    private bool questCompleted = false;

    
    private string[] currentSequence;
    private int currentAttempt = 0;
    private int currentStep = 0;
    private float rememberTimer = 0f;
    private bool isRemembering = false;
    private bool isPlaying = false;

    
    private GameObject currentItem;

    void Start()
    {
        if (minigamePanel != null) minigamePanel.SetActive(false);
        if (crystal != null) crystal.SetActive(false);
        if (questHintText != null) questHintText.gameObject.SetActive(false);

        
        if (holdPoint == null && Camera.main != null)
        {
            GameObject point = new GameObject("ItemHoldPoint");
            point.transform.SetParent(Camera.main.transform);
            point.transform.localPosition = new Vector3(0.5f, -0.3f, 0.8f);
            point.transform.localRotation = Quaternion.identity;
            holdPoint = point.transform;          
        }
    }

    void Update()
    {
        if (!questStarted || questCompleted) return;

        if (isRemembering)
        {
            rememberTimer -= Time.deltaTime;
            if (rememberTimer <= 0f)
            {
                isRemembering = false;
                isPlaying = true;
                sequenceText.text = "ПОВТОРИ ПОСЛЕДОВАТЕЛЬНОСТЬ!";
            }
        }

        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                CheckInput("Q");
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                CheckInput("W");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CheckInput("E");
            }
        }
    }

    void CheckInput(string key)
    {
        if (!isPlaying) return;
        if (currentStep >= currentSequence.Length) return;

        if (key == currentSequence[currentStep])
        {
            currentStep++;
            sequenceText.text = $" {currentStep}/{currentSequence.Length}";

            if (currentStep >= currentSequence.Length)
            {

                currentRound++;

                if (currentRound > maxRounds)
                {
                    ForgingComplete();
                }
                else
                {
                    sequenceText.text = $"РАУНД {currentRound} ГОТОВ!";
                    Invoke("GenerateSequence", 1f);
                }
            }
        }
        else
        {
            currentAttempt++;

            if (attemptsText != null)
                attemptsText.text = $"Попытка: {currentAttempt}/{maxAttempts}";

            if (currentAttempt > maxAttempts)
            {
                currentRound = 1;
                currentAttempt = 0;
                minigamePanel.SetActive(false);
                PlayerMovement.SetMovement(true);

                if (questHintText != null)
                    questHintText.text = "Не вышло... Попробуй снова у наковальни.";
            }
            else
            {
                GenerateSequence();
            }
        }
    }

    void GenerateSequence()
    {
        targetLength = baseLength + (currentRound - 1) * roundLengthIncrease;

        string[] possible = { "Q", "W", "E" };
        currentSequence = new string[targetLength];

        for (int i = 0; i < targetLength; i++)
            currentSequence[i] = possible[Random.Range(0, 3)];

        string display = "";
        foreach (string s in currentSequence)
        {
            if (s == "Q") display += "Z ";
            else if (s == "W") display += "X ";
            else if (s == "E") display += "C ";
        }

        sequenceText.text = $"РАУНД {currentRound}/{maxRounds}\nЗАПОМНИ: {display}";

        isRemembering = true;
        isPlaying = false;
        rememberTimer = rememberTime;
        currentStep = 0;

        currentAttempt = 1;

        if (attemptsText != null)
            attemptsText.text = $"Попытка: {currentAttempt}/{maxAttempts}";

    }


    void HideItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }

    void ShowIngot()
    {
        HideItem();

        currentItem = Instantiate(ingotPrefab, holdPoint.position, holdPoint.rotation);
        currentItem.transform.SetParent(holdPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

    }

    void ShowHorseshoe()
    {
        HideItem();

        if (horseshoePrefab == null)
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.transform.SetParent(holdPoint);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            temp.GetComponent<Renderer>().material.color = Color.magenta;
            currentItem = temp;
            return;
        }

        if (holdPoint == null && Camera.main != null)
        {
            GameObject point = new GameObject("HoldPoint");
            point.transform.SetParent(Camera.main.transform);
            point.transform.localPosition = new Vector3(0.5f, -0.3f, 0.8f);
            holdPoint = point.transform;
        }


        currentItem = Instantiate(horseshoePrefab, holdPoint.position, holdPoint.rotation);
        currentItem.transform.SetParent(holdPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

    }


    public void StartQuest()
    {

        questStarted = true;
        hasIngot = false;
        forgingSuccess = false;

        if (questHintText != null)
        {
            questHintText.gameObject.SetActive(true);
            questHintText.text = "ШАГ 1: Возьми заготовку у ГОРНА (F)";
        }
    }

    public void TakeIngot()
    {
        hasIngot = true;
        ShowIngot();

        if (questHintText != null)
            questHintText.text = "ШАГ 2: Иди к НАКОВАЛЬНЕ и нажми F";
    }

    public void StartForging()
    {

        HideItem();  

        
        currentRound = 1;
        currentAttempt = 0;
        minigamePanel.SetActive(true);
        PlayerMovement.SetMovement(false);

        GenerateSequence();
    }

    void ForgingComplete()
    {
        forgingSuccess = true;
        isPlaying = false;
        minigamePanel.SetActive(false);

        ShowHorseshoe(); 

        PlayerMovement.SetMovement(true);

        if (questHintText != null)
            questHintText.text = "ШАГ 3: Остуди подкову в БОЧКЕ (F)";
    }

    public void CoolHorseshoe()
    {

        HideItem();  

        CompleteQuest();
    }

    void CompleteQuest()
    {
        questCompleted = true;

        if (crystal != null)
            crystal.SetActive(true);

        if (questHintText != null)
            questHintText.text = "ЗАДАНИЕ ВЫПОЛНЕНО! Поговори с кузнецом.";

        PlayerPrefs.SetInt("RussianQuestDone", 1);

        BlacksmithDialogue smith = FindObjectOfType<BlacksmithDialogue>();
        if (smith != null) smith.OnQuestCompleted();
    }

    public bool IsQuestCompleted()
    {
        return questCompleted;
    }
}