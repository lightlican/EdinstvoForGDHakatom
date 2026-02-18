using UnityEngine;
using UnityEngine.UI;

public class ShamanDialogue : MonoBehaviour
{
    [Header("UI")]
    public string speakerName = "Шаман";
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

    [Header("Удочка")]
    public GameObject fishingRod;
    public Transform rodHoldPoint;

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool questCompleted = false;
    private bool isTalking = false;
    private int currentLine = 0;
    private string[] currentDialogue;
    private AudioClip[] currentVoiceLines;

    private SiberianQuestManager questManager;
    private Renderer rodRenderer;

    void Start()
    {
        questManager = FindObjectOfType<SiberianQuestManager>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (fishingRod != null)
        {
            rodRenderer = fishingRod.GetComponent<Renderer>();
            if (rodRenderer != null)
                rodRenderer.enabled = false;
            fishingRod.SetActive(false);
        }

        if (rodHoldPoint == null && Camera.main != null)
        {
            GameObject holdPoint = new GameObject("RodHoldPoint");
            holdPoint.transform.SetParent(Camera.main.transform);
            holdPoint.transform.localPosition = new Vector3(0.5f, -0.3f, 0.8f);
            holdPoint.transform.localRotation = Quaternion.Euler(0, 90, 0);
            rodHoldPoint = holdPoint.transform;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
            StartDialogue();

        if (isTalking && Input.GetKeyDown(KeyCode.Space))
            ShowNextLine();
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

        DialogueUI.Instance?.ShowDialogue(speakerName, currentDialogue[0], icon);
        PlayVoice(0);
    }

    void ShowNextLine()
    {
        currentLine++;

        if (currentLine < currentDialogue.Length)
        {
            DialogueUI.Instance?.UpdateText(currentDialogue[currentLine]);
            PlayVoice(currentLine);

            if (!questCompleted && currentLine == 2 && currentDialogue == firstDialogue)
            {
                ShowFishingRod();
            }
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowFishingRod()
    {
        if (fishingRod == null) return;

        if (rodHoldPoint == null && Camera.main != null)
        {
            GameObject holdPoint = new GameObject("RodHoldPoint");
            holdPoint.transform.SetParent(Camera.main.transform);
            holdPoint.transform.localPosition = new Vector3(0.5f, -0.3f, 0.8f);
            holdPoint.transform.localRotation = Quaternion.Euler(0, 90, 0);
            rodHoldPoint = holdPoint.transform;
        }

        if (rodHoldPoint != null)
        {
            fishingRod.SetActive(true);
            fishingRod.transform.SetParent(rodHoldPoint);
            fishingRod.transform.localPosition = Vector3.zero;
            fishingRod.transform.localRotation = Quaternion.identity;

            if (rodRenderer != null)
                rodRenderer.enabled = true;
        }
    }

    public void HideFishingRod()
    {
        if (fishingRod != null)
            fishingRod.SetActive(false);
    }

    void PlayVoice(int index)
    {
        if (currentVoiceLines == null || index >= currentVoiceLines.Length) return;

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
        DialogueUI.Instance?.HideDialogue();
        audioSource.Stop();

        if (!questGiven && questManager != null)
        {
            questGiven = true;
            questManager.StartQuest();
        }
    }

    public void OnQuestCompleted()
    {
        questCompleted = true;
        questGiven = true;
        HideFishingRod();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
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