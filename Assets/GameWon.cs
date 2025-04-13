using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWon : MonoBehaviour
{

    public UnityEngine.UI.Button mainMenuButton;
    private void MainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadSceneAsync(0); //goes to main menu
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
    }
}
