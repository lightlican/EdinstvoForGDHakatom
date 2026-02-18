using UnityEngine;

public class VoiceDialogue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] voiceLines; // Звуки для каждой строки диалога

    private int currentLine = 0;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Вызывай из диалога при показе строки
    public void PlayLine(int lineIndex)
    {
        if (voiceLines == null || voiceLines.Length <= lineIndex)
        {
            Debug.Log($"?? Нет звука для строки {lineIndex}");
            return;
        }

        AudioClip clip = voiceLines[lineIndex];
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log($"?? Играет: {clip.name}");
        }
    }

    // Остановить звук
    public void StopVoice()
    {
        audioSource.Stop();
    }
}