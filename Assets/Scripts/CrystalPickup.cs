using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalPickup : MonoBehaviour
{
    public string crystalName; 

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            
            PlayerPrefs.SetInt(crystalName + "Crystal", 1);
            SceneManager.LoadScene("MasterScene");
        }
    }
}