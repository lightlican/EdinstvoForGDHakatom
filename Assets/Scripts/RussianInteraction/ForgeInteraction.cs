using UnityEngine;

public class ForgeInteraction : MonoBehaviour
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
                questManager.TakeIngot();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (questManager != null && questManager.questHintText != null)
            {
                questManager.questHintText.text = "Нажми F чтобы взять заготовку";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}