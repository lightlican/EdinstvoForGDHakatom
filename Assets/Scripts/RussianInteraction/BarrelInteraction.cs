using UnityEngine;

public class BarrelInteraction : MonoBehaviour
{
    public RussianQuestManager questManager;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (questManager == null)
            {
                questManager = FindObjectOfType<RussianQuestManager>();
            }

            if (questManager != null)
            {
                questManager.CoolHorseshoe();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}