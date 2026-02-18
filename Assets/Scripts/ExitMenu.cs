using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    public GameObject exitMenuCanvas;
    public Button yesButton;
    public Button noButton;

    private bool isMenuActive = false;

    void Start()
    {
        if (exitMenuCanvas != null)
            exitMenuCanvas.SetActive(false);

        if (yesButton != null)
            yesButton.onClick.AddListener(ExitToMenu);

        if (noButton != null)
            noButton.onClick.AddListener(CloseMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuActive)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    void OpenMenu()
    {
        if (exitMenuCanvas != null)
        {
            exitMenuCanvas.SetActive(true);
            isMenuActive = true;


            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void CloseMenu()
    {

        if (exitMenuCanvas != null)
        {
            exitMenuCanvas.SetActive(false);
            isMenuActive = false;


            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}