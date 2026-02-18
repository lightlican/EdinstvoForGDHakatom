using UnityEngine;
using UnityEngine.UI;

public class TatarElderDialogue : MonoBehaviour
{
    [Header("UI")]
    public string speakerName = "Татарский старейшина";
    public Sprite icon;

    [Header("Диалоги")]
    public string[] firstDialogue;
    public string[] afterQuestDialogue;
    public string[] waitingDialogue;

    [Header("Озвучка")]
    public AudioSource audioSource;
    public AudioClip[] firstVoiceLines;
    public AudioClip[] afterQuestVoiceLines;
    public AudioClip[] waitingVoiceLines;

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool isTalking = false;
    private int currentLine = 0;
    private string[] currentDialogue;
    private AudioClip[] currentVoiceLines;

    private TatarQuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<TatarQuestManager>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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

        
        if (!questGiven)
        {
            currentDialogue = firstDialogue;
            currentVoiceLines = firstVoiceLines;
        }
        else if (questCompleted)
        {
            currentDialogue = afterQuestDialogue;
            currentVoiceLines = afterQuestVoiceLines;
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

        
        DialogueUI.Instance.ShowDialogue(speakerName, currentDialogue[0], icon);
        PlayVoice(0);
    }

    void ShowNextLine()
    {
        currentLine++;

        if (currentLine < currentDialogue.Length)
        {
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
        DialogueUI.Instance.HideDialogue();
        audioSource.Stop();

        // Запускаем квест после первого диалога
        if (!questGiven && questManager != null)
        {
            questGiven = true;
            questManager.StartQuest();

        }
    }

    public void OnQuestCompleted()
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
            if (isTalking) EndDialogue();
        }
    }
}