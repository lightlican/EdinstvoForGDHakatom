using UnityEngine;

public class CupInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private CaucasusQuestManager questManager;
    private Renderer cupRenderer;

    void Start()
    {
        questManager = FindObjectOfType<CaucasusQuestManager>();
        cupRenderer = GetComponent<Renderer>();

        if (cupRenderer == null)
            Debug.LogError("renderer -");
        else
        {
            cupRenderer.enabled = false;
            Debug.Log("Cup renderer -");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (questManager != null)
                questManager.TakeCup();
        }
    }

    public void ShowCup()
    {
        if (cupRenderer != null)
        {
            cupRenderer.enabled = true;
        }
        else
        {
            cupRenderer = GetComponent<Renderer>();
            if (cupRenderer != null)
            {
                cupRenderer.enabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (questManager != null && questManager.questStarted && !questManager.hasCup)
            {
                Renderer cupRenderer = GetComponent<Renderer>();
                if (cupRenderer != null)
                {
                    cupRenderer.enabled = true;
                }
            }

            if (questManager != null && questManager.questHintText != null)
            {
                questManager.questHintText.text = "Нажми F чтобы взять чашу";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}