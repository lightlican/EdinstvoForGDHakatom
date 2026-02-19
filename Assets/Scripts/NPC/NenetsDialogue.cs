using UnityEngine;
using UnityEngine.UI;

public class NenetsDialogue : MonoBehaviour
{
    [Header("UI")]
    public string speakerName = "Оленевод";
    public Sprite icon;

    [Header("Диалоги")]
    public string[] firstDialogue;
    public string[] warmthDialogue;
    public string[] afterMinigameDialogue;
    public string[] finalDialogue;
    public string[] waitingDialogue;

    [Header("Озвучка")]
    public AudioSource audioSource;
    public AudioClip[] firstVoiceLines;
    public AudioClip[] warmthVoiceLines;
    public AudioClip[] afterMinigameVoiceLines;
    public AudioClip[] finalVoiceLines;
    public AudioClip[] waitingVoiceLines;

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool isTalking = false;
    private int currentLine = 0;
    private string[] currentDialogue;
    private AudioClip[] currentVoiceLines;

    private NorthernQuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<NorthernQuestManager>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1)
        {
            questCompleted = true;
            questGiven = true;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            StartDialogue();
        }

        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;

        NorthernQuestManager.QuestState state = questManager.GetState();

        if (!questGiven)
        {
            currentDialogue = firstDialogue;
            currentVoiceLines = firstVoiceLines;
        }
        else if (questCompleted)
        {
            currentDialogue = finalDialogue;
            currentVoiceLines = finalVoiceLines;
        }
        else if (state == NorthernQuestManager.QuestState.WarmthQuest)
        {
            currentDialogue = warmthDialogue;
            currentVoiceLines = warmthVoiceLines;
        }
        else if (state == NorthernQuestManager.QuestState.WarmthCompleted)
        {
            currentDialogue = afterMinigameDialogue;
            currentVoiceLines = afterMinigameVoiceLines;
        }
        else
        {
            currentDialogue = waitingDialogue;
            currentVoiceLines = waitingVoiceLines;
        }

        if (currentDialogue == null || currentDialogue.Length == 0)
        {
            EndDialogue();
            return;
        }

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.ShowDialogue(speakerName, currentDialogue[0], icon);
            PlayVoice(0);
        }
    }

    void ShowNextLine()
    {
        currentLine++;

        if (currentLine < currentDialogue.Length)
        {
            if (DialogueUI.Instance != null)
                DialogueUI.Instance.UpdateText(currentDialogue[currentLine]);

            PlayVoice(currentLine);
        }
        else
        {
            EndDialogue();
        }
    }

    void PlayVoice(int index)
    {
        if (currentVoiceLines == null || index >= currentVoiceLines.Length)
            return;

        AudioClip clip = currentVoiceLines[index];
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void EndDialogue()
    {
        isTalking = false;

        if (DialogueUI.Instance != null)
            DialogueUI.Instance.HideDialogue();

        audioSource.Stop();

        
        if (!questGiven && questManager != null)
        {
            questGiven = true;
            questManager.StartQuest();
        }
        else if (questManager != null)
        {
            NorthernQuestManager.QuestState state = questManager.GetState();

            if (state == NorthernQuestManager.QuestState.BabyDeerFound)
            {
                questManager.NeedToWarm();
            }
            else if (state == NorthernQuestManager.QuestState.WarmthCompleted)
            {
                questManager.FinalDialogue();
            }
        }
        if (questManager != null && questManager.IsQuestCompleted())
        {
            if (Journal.Instance != null)
            {
                Journal.Instance.AddFact("Ненцы: Олень для северных народов — не просто животное, а основа жизни: транспорт, пища, одежда и жильё. Тотемные столбы — связь между миром людей и миром духов.");
            }
        }

    }

    public void OnQuestCompleted()
    {
        questCompleted = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isTalking) EndDialogue();
        }
    }
}