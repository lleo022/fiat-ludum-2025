using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    //private Button restartButton;
    public UnityEngine.UI.Button restartButton;
    public GameLogic GameLogic_;
    private GameObject current_player;
    private void OnEnable()
    {
        //restartButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>();
        Debug.Log("Restart button: " + restartButton);
        //restartButton.clicked += RestartGame;
        //restartButton.RegisterCallback<ClickEvent>(onClick);
        //restartButton.onClick.AddListener(RestartGame);

    }
    private void RestartGame()
    {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads scene
        gameObject.SetActive(false);
        current_player.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic_ = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        current_player = GameLogic_.current_player;
        restartButton.onClick.AddListener(RestartGame);
        //restartButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
