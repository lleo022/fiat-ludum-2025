using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public Button PlayGame_;
    public Button Credits_;
    public Button Back_;
    public Button QuitGame_;
    public void PlayGame()
    {
    SceneManager.LoadSceneAsync(1);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (PlayGame_)
        {
            PlayGame_.onClick.AddListener(PlayGame);
        }
        if (Credits_)
        {
            Credits_.onClick.AddListener(Credits);
        }
        if (QuitGame_)
        {
            QuitGame_.onClick.AddListener(QuitGame);
        }
        if (Back_)
        {
            Back_.onClick.AddListener(Back);
        }
        //restartButton.onClick.AddListener(RestartGame);
        //restartButton.onClick.AddListener(RestartGame);
        //restartButton.onClick.AddListener(RestartGame);
    }
}
