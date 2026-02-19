using UnityEngine;
using UnityEngine.UI;

public class MasterDialogue : MonoBehaviour
{
    [Header("UI")]
    public string speakerName = "Мастер";
    public Sprite icon;

    [Header("Диалоги")]
    public string[] questDialogue;
    public string[] finalDialogue;

    [Header("Озвучка")]
    public AudioSource audioSource;
    public AudioClip[] questVoiceLines;
    public AudioClip[] finalVoiceLines;

    [Header("Артефакты")]
    public GameObject[] activeArtifacts;

    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLine = 0;
    private string[] currentDialogue;
    private AudioClip[] currentVoiceLines;

    private bool finalTriggered = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        
        finalTriggered = AllArtifactsActive();

        if (finalTriggered)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = Color.yellow;
        }
    }

    void Update()
    {
        if (!finalTriggered && AllArtifactsActive())
        {
            finalTriggered = true;

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = Color.yellow;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            StartDialogue();
        }

        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }

    bool AllArtifactsActive()
    {
        bool tatar = PlayerPrefs.GetInt("TatarQuestDone", 0) == 1;
        bool russian = PlayerPrefs.GetInt("RussianQuestDone", 0) == 1;
        bool siberian = PlayerPrefs.GetInt("SiberianQuestDone", 0) == 1;
        bool northern = PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1;
        bool caucasus = PlayerPrefs.GetInt("CaucasusQuestDone", 0) == 1;

        return tatar && russian && siberian && northern && caucasus;
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;

        if (finalTriggered)
        {
            currentDialogue = finalDialogue;
            currentVoiceLines = finalVoiceLines;
            Time.timeScale = 0.5f;
        }
        else
        {
            currentDialogue = questDialogue;
            currentVoiceLines = questVoiceLines;
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
        DialogueUI.Instance.HideDialogue();
        audioSource.Stop();

        if (finalTriggered)
        {
            Time.timeScale = 1f;
            CreditsManager credits = FindObjectOfType<CreditsManager>();
            if (credits != null)
                credits.StartCredits();
        }

        if (finalTriggered && Journal.Instance != null)
        {
            Journal.Instance.AddFact("Мастер: Единство народов — великая сила");
        }
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