using UnityEngine;

public class MasterVoice : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Обычный диалог")]
    public AudioClip[] questLines;    // 3 строки обычного диалога

    [Header("Финальный диалог")]
    public AudioClip[] finalLines;     // 12 строк финальной речи

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Вызывается из диалога при показе строки
    public void PlayQuestLine(int lineIndex)
    {
        if (questLines == null || lineIndex >= questLines.Length)
        {
            Debug.Log($"?? Нет звука для обычной строки {lineIndex}");
            return;
        }

        PlayClip(questLines[lineIndex]);
    }

    public void PlayFinalLine(int lineIndex)
    {
        if (finalLines == null || lineIndex >= finalLines.Length)
        {
            Debug.Log($"?? Нет звука для финальной строки {lineIndex}");
            return;
        }

        PlayClip(finalLines[lineIndex]);
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.Stop(); // Останавливаем предыдущий звук
        audioSource.clip = clip;
        audioSource.Play();

        Debug.Log($"?? Мастер говорит: {clip.name}");
    }

    public void StopVoice()
    {
        audioSource.Stop();
    }
}