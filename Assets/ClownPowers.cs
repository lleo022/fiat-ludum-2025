using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ClownPowers : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerInputActions playerControls;

    public bool ballooning = false;
    public float balloonTimer = 3f;
    public float balloonSpeed = 2f;

    private InputAction balloons;

    private float original_gravity_scale = 2f;

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
    }
}
