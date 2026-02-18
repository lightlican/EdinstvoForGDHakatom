using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    public FishGame fishingGame;
    public GameObject fishingRodPrefab;

    public GameObject currentFishingRod;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {

            if (fishingGame != null)
            {
                
                if (fishingRodPrefab != null && currentFishingRod == null)
                {
                    Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
                    currentFishingRod = Instantiate(fishingRodPrefab, spawnPos, Quaternion.Euler(0, 0, 45));
                }

                
                fishingGame.currentSpot = this;

                
                fishingGame.StartGame();
            }
        }
    }

    public void RemoveFishingRod()
    {
        if (currentFishingRod != null)
        {
            Destroy(currentFishingRod);
            currentFishingRod = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            SiberianQuestManager sqm = FindObjectOfType<SiberianQuestManager>();
            if (sqm != null && sqm.questHintText != null)
            {
                sqm.questHintText.text = "Нажми F чтобы закинуть удочку";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
               
            RemoveFishingRod();

            
            SiberianQuestManager sqm = FindObjectOfType<SiberianQuestManager>();
            if (sqm != null && sqm.questHintText != null)
            {
                sqm.questHintText.text = "Вернись к точке у озера";
            }
        }
    }
}