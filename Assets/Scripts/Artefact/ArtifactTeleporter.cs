using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactTeleporter : MonoBehaviour
{
    public string sceneToLoad; 
    public int artifactID; 

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F) && IsPlayerNear())
        {
            
            PlayerPrefs.SetInt("CurrentArtifact", artifactID);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    bool IsPlayerNear()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance < 3f; 
        }
        return false;
    }
}