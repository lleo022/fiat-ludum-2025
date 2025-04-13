using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ClownPowers : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerInputActions playerControls;

    public bool ballooning = false;
    public float balloonTimer = 3f;
    public float balloonSpeed = 2f;

    public GameObject flowers_obj;
    public float flowersSpawnOffset = .3f;

    private InputAction balloons;
    private InputAction flowers;

    private float original_gravity_scale = 2f;

    private Vector3 original_scale;

    public Sprite flying_sprite;
    public Sprite walking_sprite;

    private void Awake() //gets called as game starts up
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        //playerControls.Enable();
        balloons = playerControls.Player.Balloons;
        flowers = playerControls.Player.Flowers;

        balloons.Enable();
        balloons.performed += Balloons;

        flowers.Enable();
        flowers.performed += Flowers;
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        balloons.Disable();
        flowers.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Flowers(InputAction.CallbackContext context)
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0, flowersSpawnOffset, 0);
        GameObject new_flowers = Instantiate(flowers_obj, spawnPos, Quaternion.identity);
    }

    private void Balloons(InputAction.CallbackContext context)
    {
        if (ballooning == false)
        {
            GetComponent<Movement>().JumpFinished(context); //if jumping, stop jumping
            StartCoroutine(BalloonsCoroutine());
        }
    }
    private IEnumerator BalloonsCoroutine()
    {
        ballooning = true;

        //change sprite
        GetComponent<SpriteRenderer>().sprite = flying_sprite;


        original_scale = transform.localScale;
        transform.localScale = new Vector3(original_scale.x * 1f, original_scale.y * 1f, original_scale.z);



        original_gravity_scale = rb.gravityScale;
        rb.gravityScale = 0; //disable gravity
        rb.linearVelocity += new Vector2(0, balloonSpeed);
        yield return new WaitForSeconds(balloonTimer); //3 seconds of flight
        CancelBalloons();
    }

    public void CancelBalloons()
    {
        rb.gravityScale = original_gravity_scale; //re-enable gravity
        ballooning = false;
        GetComponent<SpriteRenderer>().sprite = walking_sprite;
        transform.localScale = original_scale;

    }
}
