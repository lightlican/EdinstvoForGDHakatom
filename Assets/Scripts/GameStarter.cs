using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("NewGameStarted", 0) == 1)
        {
            PlayerPrefs.DeleteKey("NewGameStarted");
        }
    }
}