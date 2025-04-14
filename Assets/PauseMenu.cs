using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button Resume_;
    public Button MainMenu_;
    public Button QuitGame_;
    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (Resume_)
        {
            Resume_.onClick.AddListener(Resume);
        }
        if (MainMenu_)
        {
            MainMenu_.onClick.AddListener(MainMenu);
        }
        if (QuitGame_)
        {
            QuitGame_.onClick.AddListener(Quit);
        }
    }
}
