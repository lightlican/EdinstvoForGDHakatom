using UnityEngine;

public class StartFishing : MonoBehaviour
{
    public FishGame fishingGame;

    void Update()
    {
        // ЗАПУСК ПО КНОПКЕ F В ЛЮБОМ МЕСТЕ СЦЕНЫ (для теста)
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("ЗАПУСК РЫБАЛКИ ПО КНОПКЕ F!");

            if (fishingGame == null)
            {
                fishingGame = FindObjectOfType<FishGame>();
            }

            if (fishingGame != null)
            {
                fishingGame.StartGame();
            }
            else
            {
                Debug.LogError("SimpleFishingGame не найден!");
            }
        }

        // Перезапуск по R
        if (Input.GetKeyDown(KeyCode.R) && fishingGame != null)
        {
            fishingGame.RetryGame();
        }

        // Выход по ESC
        if (Input.GetKeyDown(KeyCode.Escape) && fishingGame != null)
        {
            fishingGame.ExitGame();
        }
    }
}