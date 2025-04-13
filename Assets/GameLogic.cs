using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    public int maxPlayerHealth = 3;
    public int playerHealth;

    public GameObject businessman;
    public GameObject clown;
    public GameObject current_player;
    public GameObject current_camera;

    public GameObject boss;

    public bool is_clown = false; // true = clown, false = businessman

    public PlayerInputActions playerControls;

    //three hearts
    public UIDocument healthBarUI;
    private VisualElement healthbar;

    public GameObject gameOverUI;
    public GameObject victoryUI;

    public GameObject bossFightUI;
    public UnityEngine.UI.Slider bossFightFightSlider;
    private InputAction switch_persona;

    private List<VisualElement> currentHearts;

    public Vector2 bossFightCameraOffset = new Vector2(0, 3);
    public float bossFightCameraZoom = 6f;

    public bool CreativeMode = false;

    private void Awake() //gets called as game starts up
    {
        playerControls = new PlayerInputActions();
        playerHealth = maxPlayerHealth;
        healthbar = healthBarUI.rootVisualElement.Q<VisualElement>("Healthbar");
        currentHearts = healthbar.Query("Heart").ToList();

        bossFightFightSlider = bossFightUI.GetComponent<UnityEngine.UI.Slider>();

        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
        bossFightUI.SetActive(false);

        currentHearts.Reverse(); // it comes out in the wrong order
    }
    private void OnEnable()
    {
        switch_persona = playerControls.Player.Switch;

        switch_persona.Enable();
        switch_persona.performed += SwitchCallback;
    }

    private void OnDisable()
    {
        switch_persona.Disable();
    }

    public void Victory()
    {
        victoryUI.SetActive(true);
    }
    private void Death()
    {
        current_player.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public IEnumerator BossFight()
    {
        current_camera.GetComponent<FollowPlayer>().zoom = bossFightCameraZoom;
        current_camera.GetComponent<FollowPlayer>().offset = bossFightCameraOffset;
        
        yield return new WaitForSeconds(5); //5 second delay for testing purposes
        Instantiate(boss, current_player.transform.position+ new Vector3(-2f,5f,0f), Quaternion.identity);
        boss.GetComponent<BossScript>().healthSlider = bossFightFightSlider;
        bossFightUI.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //restartButton.RegisterCallback<ClickEvent>(onClick);
        Switch();
        StartCoroutine(BossFight());
    }
    private void SwitchCallback(InputAction.CallbackContext context)
    {
        Switch();
    }
    private void Switch()
    {
        is_clown = !is_clown;
        //Debug.Log("Isclown: " + is_clown);
        GameObject new_player;
        if (is_clown) //clown
        {
            new_player = Instantiate(clown, current_player.transform.position, Quaternion.identity);
        } else
        {
            new_player = Instantiate(businessman, current_player.transform.position, Quaternion.identity);
        }
        SpriteRenderer oldSR = current_player.GetComponent<SpriteRenderer>();
        SpriteRenderer newSR = new_player.GetComponent<SpriteRenderer>();
        if (oldSR != null && newSR != null)
        {
            newSR.sortingLayerID = oldSR.sortingLayerID;
            newSR.sortingOrder = oldSR.sortingOrder;
        }
        Debug.Log("Destroying : " + current_player.name + " Creating: " + current_player.name + " With transform: " + current_player.transform.position);
        Destroy(current_player);
        current_player = new_player;
        //Debug.Log("New current player: " + current_player);
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
            //Debug.Log("Heart display: " + heart + " | " + heart.style.opacity);
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
        if (CreativeMode) // for testing purposes, you can't get hurt
        {
            return;
        }
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
        if (current_player.transform.position.y < -20)
        {
            Death();
        }
    }
}
