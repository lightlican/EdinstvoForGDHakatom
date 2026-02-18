using UnityEngine;
using UnityEngine.UI;

public class SiberianQuestManager : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject shamanNPC;
    public GameObject crystal;
    public Text questHintText;
    public FishingSpot fishingSpot;

    public static SiberianQuestManager Instance { get; private set; }

    
    private bool questStarted = false;
    private bool questCompleted = false;

    void Awake()
    {
        
        Instance = this;
    }

    void Start()
    {
        if (crystal != null) crystal.SetActive(false);
        if (questHintText != null) questHintText.gameObject.SetActive(false);
    }

    public void StartQuest()
    {
        questStarted = true;
        questCompleted = false;

        if (questHintText != null)
        {
            questHintText.gameObject.SetActive(true);
            questHintText.text = "ЗАДАНИЕ: Умилостивь духа реки\n" +
                                "Подойди к озеру и нажми F чтобы начать рыбалку\n" +
                                "Удерживай рыбу в зелёной зоне 30 секунд!";
        }

        if (fishingSpot != null)
        {
            fishingSpot.gameObject.SetActive(true);
        }

    }

    
    public static void FishCaught()
    {      
        if (Instance != null)
        {
            Instance.CompleteQuest();
        }
    }

    void CompleteQuest()
    {
        questCompleted = true; 

        if (crystal != null)
            crystal.SetActive(true);

        ShamanDialogue shaman = FindObjectOfType<ShamanDialogue>();
        if (shaman != null)
            shaman.OnQuestCompleted();  

        PlayerPrefs.SetInt("SiberianQuestDone", 1);
    }
    public bool IsQuestCompleted()
    {
        return questCompleted;
    }
}