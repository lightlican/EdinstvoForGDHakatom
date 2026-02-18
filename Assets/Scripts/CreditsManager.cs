using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("UI")]
    public Canvas creditsCanvas;
    public Image blackOverlay;
    public Text creditsText;

    [Header("Настройки")]
    public float fadeInTime = 2f;
    public float displayTime = 8f;
    public float fadeOutTime = 2f;

    [Header("Текст титров")]
    [TextArea(3, 10)]
    public string[] creditsLines;

    void Start()
    {
        if (creditsCanvas != null)
            creditsCanvas.gameObject.SetActive(false);
    }

    public void StartCredits()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        creditsCanvas.gameObject.SetActive(true);


        string fullText = "";
        foreach (string line in creditsLines)
        {
            fullText += line + "\n\n";
        }
        creditsText.text = fullText;

        float elapsedTime = 0f;
        Color overlayColor = blackOverlay.color;

        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            overlayColor.a = alpha;
            blackOverlay.color = overlayColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        overlayColor.a = 1f;
        blackOverlay.color = overlayColor;

 
        yield return new WaitForSeconds(displayTime);

        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            overlayColor.a = alpha;
            blackOverlay.color = overlayColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        overlayColor.a = 0f;
        blackOverlay.color = overlayColor;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}