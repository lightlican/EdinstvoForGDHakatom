using UnityEngine;
using UnityEngine.UI;

public class TatarQuestManager : MonoBehaviour
{
    public GameObject tatarElder; 
    public GameObject[] cartParts; 
    public GameObject cart; 
    public GameObject crystal; 
    public Text questHintText; 

    private int partsCollected = 0;
    private bool questStarted = false;
    private bool questCompleted = false;

    public string[] afterQuestDialogue; 
    private bool canTalkAfterQuest = false; 

    void Start()
    {
        
        if (questHintText != null) questHintText.gameObject.SetActive(false);
        if (crystal != null) crystal.SetActive(false);

        
        if (cartParts.Length == 0)
        {
            cartParts = GameObject.FindGameObjectsWithTag("CartPart");
        }
    }

    void Update()
    {
        
    }

    
    public void StartQuest()
    {
        questStarted = true;
        if (questHintText != null)
        {
            questHintText.gameObject.SetActive(true);
            questHintText.text = "Задание: Собери 3 детали для телеги (Подойди и нажми F). Найдено: 0/3";
        }
        Debug.Log("Квест начался: Собери детали для Сабантуя!");
    }

    
    public void CollectPart()
    {
        if (!questStarted || questCompleted) return;

        partsCollected++;
        questHintText.text = "Задание: Собери 3 детали для телеги. Найдено: " + partsCollected + "/3";

        if (partsCollected >= cartParts.Length)
        {
            CompleteQuest();
        }
    }

    void CompleteQuest()
    {
        questCompleted = true;
        questHintText.text = "Задание выполнено! Вернись к старейшине за кристаллом.";
        canTalkAfterQuest = true; 

        
        if (crystal != null)
        {
            crystal.SetActive(true);
        }

        
        if (cart != null)
        {
            Renderer cartRenderer = cart.GetComponent<Renderer>();
            if (cartRenderer != null) cartRenderer.material.color = Color.green;
        }


        PlayerPrefs.SetInt("TatarQuestDone", 1);

        TatarElderDialogue elderDialogue = FindObjectOfType<TatarElderDialogue>();
        if (elderDialogue != null)
        {
            elderDialogue.OnQuestCompleted();
        }
    }
}