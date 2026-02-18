using UnityEngine;

public class TotemInteraction : MonoBehaviour
{
    public int totemIndex;

    private bool playerInRange = false;
    private NorthernQuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<NorthernQuestManager>();

        if (GetComponent<Collider>() == null)
        {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = Vector3.one * 3;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (questManager != null)
            {
                questManager.TotemActivated(totemIndex);
                gameObject.SetActive(false);
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