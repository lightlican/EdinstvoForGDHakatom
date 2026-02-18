using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CaucasusQuestManager questManager = FindObjectOfType<CaucasusQuestManager>();
            if (questManager != null)
                questManager.PlayerFell();
        }
    }
}