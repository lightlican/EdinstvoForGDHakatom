using UnityEngine;
using UnityEngine.UI;

public class TatarElderController : MonoBehaviour
{
    [Header("Диалоги")]
    public string[] firstDialogue;    // При первом подходе (даём квест)
    public string[] secondDialogue;   // После выполнения квеста (спасибо + факт)

    [Header("Ссылки")]
    public Text dialogueText;         // UI Text для показа диалога
    public GameObject questHint;      // Подсказка квеста (опционально)

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool isTalking = false;
    private int currentLine = 0;
    private string[] currentDialogue;

    void Start()
    {
        // Скрываем текст при старте
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Если игрок в зоне и нажал E, и мы не в диалоге
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            StartDialogue();
        }

        // Продолжение диалога по Space
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueText.gameObject.SetActive(true);
        currentLine = 0;

        // Решаем, какой диалог показывать
        if (!questGiven)
        {
            // Первый диалог - даём квест
            currentDialogue = firstDialogue;
            questGiven = true;
        }
        else if (questCompleted)
        {
            // Второй диалог - после выполнения квеста
            currentDialogue = secondDialogue;
        }
        else
        {
            // Если квест взят, но не выполнен
            currentDialogue = new string[] {
                "Ты уже получил задание. Собери 3 деревянные детали для телеги к Сабантую!",
                "Детали разбросаны по площади. Ищи оранжевые деревянные бруски."
            };
        }

        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLine < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[currentLine];
            currentLine++;
        }
        else
        {
            EndDialogue();

            // Если это был первый диалог - запускаем квест
            if (!questCompleted && FindObjectOfType<TatarQuestManager>() != null)
            {
                FindObjectOfType<TatarQuestManager>().StartQuest();
            }
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueText.gameObject.SetActive(false);
    }

    // Эту функцию будет вызывать TatarQuestManager, когда квест выполнен
    public void MarkQuestCompleted()
    {
        questCompleted = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isTalking)
            {
                EndDialogue();
            }
        }
    }
}