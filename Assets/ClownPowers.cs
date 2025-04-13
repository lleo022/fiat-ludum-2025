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

    private InputAction balloons;

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

        balloons.Enable();
        balloons.performed += Balloons;
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        balloons.Disable();
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

    private void Balloons(InputAction.CallbackContext context)
    {
        if (ballooning == false)
        {
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
