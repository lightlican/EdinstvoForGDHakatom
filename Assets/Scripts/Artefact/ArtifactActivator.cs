using UnityEngine;

public class ArtifactActivator : MonoBehaviour
{
    public int artifactID; 
    public GameObject inactiveModel;
    public GameObject activeModel;

    private bool isActive = false;
    private bool playerInRange = false;

    void Start()
    {
        
        bool questDone = false;

        switch (artifactID)
        {
            case 1:
                questDone = PlayerPrefs.GetInt("TatarQuestDone", 0) == 1;
                break;
            case 2:
                questDone = PlayerPrefs.GetInt("SiberianQuestDone", 0) == 1;
                break;
            case 3:
                questDone = PlayerPrefs.GetInt("RussianQuestDone", 0) == 1;
                break;
            case 4:
                questDone = PlayerPrefs.GetInt("NorthernQuestDone", 0) == 1;
                break;
            case 5:
                questDone = PlayerPrefs.GetInt("CaucasusQuestDone", 0) == 1;
                break;
        }

        if (questDone)
        {
            ActivateArtifact();
        }
    }

    void Update()
    {
        if (playerInRange && !isActive && Input.GetKeyDown(KeyCode.F))
        {

            bool hasCrystal = false;

            switch (artifactID)
            {
                case 1:
                    hasCrystal = PlayerPrefs.GetInt("TatarCrystal", 0) == 1;
                    break;
                case 2:
                    hasCrystal = PlayerPrefs.GetInt("SiberianCrystal", 0) == 1;
                    break;
                case 3:
                    hasCrystal = PlayerPrefs.GetInt("RussianCrystal", 0) == 1;
                    break;
                case 4:
                    hasCrystal = PlayerPrefs.GetInt("NorthernCrystal", 0) == 1;
                    break;
                case 5:
                    hasCrystal = PlayerPrefs.GetInt("CaucasusCrystal", 0) == 1;
                    break;
            }

            if (hasCrystal)
            {
                ActivateArtifact();

                
                switch (artifactID)
                {
                    case 1: PlayerPrefs.SetInt("TatarCrystal", 0); break;
                    case 2: PlayerPrefs.SetInt("SiberianCrystal", 0); break;
                    case 3: PlayerPrefs.SetInt("RussianCrystal", 0); break;
                    case 4: PlayerPrefs.SetInt("NorthernCrystal", 0); break;
                    case 5: PlayerPrefs.SetInt("CaucasusCrystal", 0); break;
                }

            }
        }
    }

    void ActivateArtifact()
    {
        isActive = true;

        if (inactiveModel != null)
            inactiveModel.SetActive(false);

        if (activeModel != null)
            activeModel.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}