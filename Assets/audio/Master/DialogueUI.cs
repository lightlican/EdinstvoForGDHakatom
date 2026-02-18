using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text speakerNameText;
    public Image speakerIcon;

    private bool isVisible = false;

    void Awake()
    {
        Instance = this;
        HideDialogue();
    }

    public void ShowDialogue(string speaker, string text, Sprite icon = null)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = speaker;

        if (dialogueText != null)
            dialogueText.text = text;

        if (speakerIcon != null && icon != null)
            speakerIcon.sprite = icon;

        isVisible = true;
    }

    public void UpdateText(string text)
    {
        if (dialogueText != null)
            dialogueText.text = text;
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        isVisible = false;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}