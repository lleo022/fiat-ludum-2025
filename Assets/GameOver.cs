using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //private Button restartButton;
    public UnityEngine.UI.Button restartButton;
    //private GameLogic GameLogic_;
    //private GameObject current_player;
    private void RestartGame()
    {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads scene
        //current_player.SetActive(true);
        //gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameLogic_ = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        //current_player = GameLogic_.current_player;
        Debug.Log("Added restart button items, start");
        //restartButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>();
    }
    void OnEnable()
    {
        restartButton.onClick.AddListener(RestartGame);
        Debug.Log("Added restart button onClick");
    }
}
