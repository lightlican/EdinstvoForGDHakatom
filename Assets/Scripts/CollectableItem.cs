using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public string itemType; 
    public int questID; 

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {

            TatarQuestManager manager = FindObjectOfType<TatarQuestManager>();
            if (manager != null)
            {
                manager.CollectPart();
            }

            Destroy(gameObject);
        }
    }
}