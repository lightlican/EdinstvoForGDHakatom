using UnityEngine;
using UnityEngine.UI;

public class ElderDialogue : MonoBehaviour
{
    [Header("UI")]
    public string speakerName = "Старейшина";
    public Sprite icon;

    [Header("Диалоги")]
    public string[] finalDialogue;

    [Header("Озвучка")]
    public AudioSource audioSource;
    public AudioClip[] finalVoiceLines;

    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLine = 0;

    private CaucasusQuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<CaucasusQuestManager>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            if (questManager != null && questManager.HasCup())
            {
                questManager.DeliverCup();
                StartDialogue();
            }
            else if (questManager != null && questManager.IsQuestCompleted())
            {
                StartDialogue();
            }
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

        if (finalDialogue == null || finalDialogue.Length == 0)
        {
            EndDialogue();
            return;
        }

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.ShowDialogue(speakerName, finalDialogue[0], icon);
            PlayVoice(0);
        }
    }

    void ShowNextLine()
    {
        currentLine++;

        if (currentLine < finalDialogue.Length)
        {
            if (DialogueUI.Instance != null)
                DialogueUI.Instance.UpdateText(finalDialogue[currentLine]);

            PlayVoice(currentLine);
        }
        else
        {
            EndDialogue();

            if (questManager != null)
                questManager.CompleteQuest();
        }
    }

    void PlayVoice(int index)
    {
        if (finalVoiceLines == null || index >= finalVoiceLines.Length)
            return;

        AudioClip clip = finalVoiceLines[index];
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