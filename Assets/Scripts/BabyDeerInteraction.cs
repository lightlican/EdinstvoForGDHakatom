using UnityEngine;

public class BabyDeerInteraction : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {

            NorthernQuestManager nqm = FindObjectOfType<NorthernQuestManager>();
            if (nqm != null)
            {
                nqm.PickupBabyDeer();
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {

            NorthernQuestManager nqm = FindObjectOfType<NorthernQuestManager>();
            if (nqm != null)
            {
                nqm.PickupBabyDeer();
            }

            Destroy(gameObject);
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