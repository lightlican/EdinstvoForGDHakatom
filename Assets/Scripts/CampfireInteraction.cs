using UnityEngine;

public class CampfireInteraction : MonoBehaviour
{
    private bool playerInRange = false;
    private NorthernQuestManager questManager;
    private WarmMinigame warmMinigame;

    void Start()
    {
        questManager = FindObjectOfType<NorthernQuestManager>();
        warmMinigame = FindObjectOfType<WarmMinigame>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (questManager != null)
            {
                NorthernQuestManager.QuestState state = questManager.GetState();

                if (state == NorthernQuestManager.QuestState.WarmthQuest)
                {
                    if (warmMinigame != null)
                    {
                        warmMinigame.StartMinigame();
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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