using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public int maxPlayerHealth = 3;
    public int playerHealth;

    public GameObject businessman;
    public GameObject clown;
    public GameObject current_player;

    public GameObject current_camera;

    public bool is_clown = false; // true = clown, false = businessman

    public PlayerInputActions playerControls;
    //public int heart1, heart2, heart3;

    public UIDocument healthBarUI;
    private VisualElement healthbar;

    public UIDocument gameOverUI;
    public Button restartButton;
    //private VisualElement currentHeart;

    private InputAction switch_persona;

    private List<VisualElement> currentHearts;

    private void Awake() //gets called as game starts up
    {
        playerControls = new PlayerInputActions();
        playerHealth = maxPlayerHealth;
        healthbar = healthBarUI.rootVisualElement.Q<VisualElement>("Healthbar");
        currentHearts = healthbar.Query("Heart").ToList();

        restartButton = gameOverUI.rootVisualElement.Q<Button>();
        Debug.Log("Restart button name: " + restartButton.name);

        gameOverUI.enabled = false;

        currentHearts.Reverse(); // it comes out in the wrong order
    }
    private void OnEnable()
    {
        //playerControls.Enable();
        switch_persona = playerControls.Player.Switch;

        switch_persona.Enable();
        switch_persona.performed += SwitchCallback;

        //restartButton.clickable.clicked += RestartGame;
        restartButton.RegisterCallback<PointerDownEvent>(onClick);
        //PointerDownEvent 
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        switch_persona.Disable();

        //restartButton.Disable();
    }

    private void onClick(PointerDownEvent evt)
    {
        RestartGame();
    }

    private void RestartGame()
    {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads scene
        gameOverUI.enabled = false;
        //current_player.SetActive(true);
    }
    private void Death()
    {
        current_player.SetActive(false);
        gameOverUI.enabled = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Switch();
    }
    private void SwitchCallback(InputAction.CallbackContext context)
    {
        Switch();
    }
    private void Switch()
    {
        is_clown = !is_clown;
        Debug.Log("Isclown: " + is_clown);
        GameObject new_player;
        if (is_clown) //clown
        {
            new_player = Instantiate(clown, current_player.transform.position, Quaternion.identity);
        } else
        {
            new_player = Instantiate(businessman, current_player.transform.position, Quaternion.identity);
        }
        Debug.Log("Destroying : " + current_player.name + " Creating: " + current_player.name + " With transform: " + current_player.transform.position);
        Destroy(current_player);
        current_player = new_player;
        Debug.Log("New current player: " + current_player);
        current_camera.GetComponent<FollowPlayer>().player = current_player.transform;
    }

    private void addHeart()
    {
        //List<VisualElement> currentHearts = healthbar.Query("Heart").ToList();//healthbar.Q<VisualElement>("Heart");
        foreach (VisualElement heart in currentHearts)
        {
            // go until you find one that is not visible
            if (heart.resolvedStyle.opacity == 0f)
            {
                heart.style.opacity = 1f; //reappear
                break;
            }
        }
        // currentHeart = healthbar.Q<VisualElement>("Heart");
        // currentHeart.style.display = DisplayStyle.Flex; //reappear
    }

    private void removeHeart()
    {
        // .Q() = .Query().First()
        //List<VisualElement> currentHearts = healthbar.Query("Heart").ToList();//healthbar.Q<VisualElement>("Heart");
        foreach (VisualElement heart in currentHearts)
        {
            Debug.Log("Heart display: " + heart + " | " + heart.style.opacity);
            // go until you find one that is still visible
            //if (heart.style.display == StyleKeyword.Null)
            if (heart.resolvedStyle.opacity == 1f)
            {
                heart.style.opacity = 0f; //disappear
                break;
            }
        }
        //healthbar.Remove(currentHeart);
        
    }
    public void hurtPlayer(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Death();
        }
        for (int i = 0; i < amount; i++)
        {
            removeHeart(); //might animate in the future
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
