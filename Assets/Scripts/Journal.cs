using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Journal : MonoBehaviour
{
    public static Journal Instance { get; private set; }

    [Header("UI")]
    public GameObject journalCanvas;
    public Button closeButton;

    [Header("Вкладки")]
    public Button factsTabButton;
    public Button mapTabButton;
    public GameObject factsPanel;
    public GameObject mapPanel;

    [Header("Факты")]
    public Text factsText;  

    [Header("Карта")]
    public Image[] locationIcons;
    public Sprite locationUnlocked;
    public Sprite locationLocked;

    private bool isOpen = false;
    private List<string> collectedFacts = new List<string>();

    void Awake()
    {
        Instance = this;

        if (journalCanvas != null)
            journalCanvas.SetActive(false);

        if (factsTabButton != null)
            factsTabButton.onClick.AddListener(ShowFacts);

        if (mapTabButton != null)
            mapTabButton.onClick.AddListener(ShowMap);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseJournal);

        ShowFacts();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
                CloseJournal();
            else
                OpenJournal();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseJournal();
        }
    }

    void OpenJournal()
    {
        if (journalCanvas == null) return;

        journalCanvas.SetActive(true);
        isOpen = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateFactsFromArtifacts();

        UpdateMap();
    }

    void CloseJournal()
    {
        if (journalCanvas == null) return;

        journalCanvas.SetActive(false);
        isOpen = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateJournal()
    {
        UpdateFacts();
        UpdateMap();
    }

    void UpdateFacts()
    {

        string allFacts = "";
        foreach (string fact in collectedFacts)
        {
            allFacts += "• " + fact + "\n\n";
        }

        if (string.IsNullOrEmpty(allFacts))
        {
            allFacts = "Факты пока не собраны...\nПроходите квесты и узнавайте традиции!";
        }

        factsText.text = allFacts;


        Canvas.ForceUpdateCanvases();

    }

    void UpdateMap()
    {
        bool tatar = PlayerPrefs.GetInt("TatarQuestDone", 0) == 1;
        bool russian = PlayerPrefs.GetInt("RussianQuestDone", 0) == 1;
        bool siberian = PlayerPrefs.GetInt("SiberianQuestDone", 0) == 1;
        bool northern = PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1;
        bool caucasus = PlayerPrefs.GetInt("CaucasusQuestDone", 0) == 1;

        SetLocationIcon(0, tatar);
        SetLocationIcon(1, russian);
        SetLocationIcon(2, siberian);
        SetLocationIcon(3, northern);
        SetLocationIcon(4, caucasus);
    }

    void SetLocationIcon(int index, bool unlocked)
    {
        if (index < locationIcons.Length && locationIcons[index] != null)
        {
            locationIcons[index].sprite = unlocked ? locationUnlocked : locationLocked;
        }
    }

    void ShowFacts()
    {
        if (factsPanel != null) factsPanel.SetActive(true);
        if (mapPanel != null) mapPanel.SetActive(false);
    }

    void ShowMap()
    {
        if (factsPanel != null) factsPanel.SetActive(false);
        if (mapPanel != null) mapPanel.SetActive(true);
    }

    public void AddFact(string fact)
    {

        if (!collectedFacts.Contains(fact))
        {
            collectedFacts.Add(fact);
            UpdateFacts();

        }

    }

    void UpdateFactsFromArtifacts()
    {

        collectedFacts.Clear();


        if (PlayerPrefs.GetInt("TatarQuestDone", 0) == 1)
        {
            collectedFacts.Add("Татары: Сабантуй — праздник плуга, внесён в список наследия ЮНЕСКО. Это праздник силы, ловкости и единства общины.");
        }

        if (PlayerPrefs.GetInt("RussianQuestDone", 0) == 1)
        {
            collectedFacts.Add("Русские: Подкова — символ счастья, вешали концами вверх, чтобы счастье не вытекало. Кузнецов считали колдунами, умеющими укрощать огонь.");
        }

        if (PlayerPrefs.GetInt("SiberianQuestDone", 0) == 1)
        {
            collectedFacts.Add("Буряты: Шаманский бубен — это целая вселенная: обод символизирует мироздание, рукоять — ось мира, а рисунки изображают духов-помощников.");
        }

        if (PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1)
        {
            collectedFacts.Add("Ненцы: Олень для северных народов — не просто животное, а основа жизни: транспорт, пища, одежда и жильё. Тотемные столбы — связь между миром людей и миром духов.");
        }

        if (PlayerPrefs.GetInt("CaucasusQuestDone", 0) == 1)
        {
            collectedFacts.Add("Осетины: Кавказское гостеприимство — закон. За одним столом решаются споры, мирятся враги, заключаются союзы. Чаша с айраном — символ уважения к старшим.");
        }

        UpdateFacts();
    }
}