using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{

    public float speed = 2f;
    public float jumpSpeed = 5f;
    public float maxJumpTime = 1f;
    public float waitBetweenJumps = 1f;

    private Rigidbody2D rb;
    private Vector2 movement_direction = Vector2.zero;

    public PlayerInputActions playerControls;

    private InputAction move;
    private InputAction jump;
    //private InputAction fire;

    public bool jumping = false;
    private float original_gravity_scale = 0f;

    public GameObject GameLogic;

    public bool goingRight = true;

    private void Awake() //gets called as game starts up
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        move.Enable();
        move.performed += MoveSpecial;
        move.canceled += MoveSpecialFinished;

        jump.Enable();
        jump.performed += Jump;
        jump.canceled += JumpFinished;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        original_gravity_scale = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {

        //OLD SYSTEM: movement_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement_direction = move.ReadValue<Vector2>();
        if (goingRight)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement_direction.x * speed, rb.linearVelocity.y);
    }

    private void MoveSpecial(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.name);
        if (context.control.name == "s")
        {
            JumpFinished(context); //cancel jump
            if (GameLogic.GetComponent<GameLogic>().is_clown == true)
            {
                GetComponent<ClownPowers>().CancelBalloons(); //if ballooning, stop
            }
        } else if (context.control.name == "a")
        {
            goingRight = false;
        } else if (context.control.name == "d")
        {
            goingRight = true;
        }
    }
    private void MoveSpecialFinished(InputAction.CallbackContext context)
    {
        //Debug.Log("Movespecialfinished " + context.control.name);
        if (context.control.name == "w" || context.control.name == "space") //on w or space up, cancel jump
        {
            //JumpFinished(context); //cancel jump
        }

    }
    private void Jump(InputAction.CallbackContext context)
    {
        
        StartCoroutine(JumpCoroutine(context));
    }
    private IEnumerator JumpCoroutine(InputAction.CallbackContext context)
    {
        if (GameLogic.GetComponent<GameLogic>().is_clown == true)
        {
            //Debug.Log("Is clown");
            if (GetComponent<ClownPowers>().ballooning == true)
            {
                yield return null; //can't jump while using balloons power
            }
        }
        //make sure it is actually on the ground
        float maxDistance = 100;
        LayerMask mask = LayerMask.GetMask("Ground"); //only use ground layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, maxDistance, mask);
        if (hit)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance < .5f)
            {
                jumping = true;
                    
                rb.linearVelocity += new Vector2(0, jumpSpeed); //small jump
                yield return new WaitForSeconds(waitBetweenJumps);
                if (jumping) //extend jump
                {
                    //Debug.Log("Extending jump");
                    //rb.linearVelocity += new Vector2(0, jumpSpeed*1/4);
                    original_gravity_scale = rb.gravityScale;
                    rb.gravityScale = original_gravity_scale*.5f;
                }
                //Debug.Log("Jump!");
                yield return new WaitForSeconds(maxJumpTime-waitBetweenJumps);
                JumpFinished(context);
                //rb.linearVelocity += new Vector2(0, jumpSpeed);
            }
            
        }
    }

    public void JumpFinished(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump Finished");
        if (jumping) // if currently jumping
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.gravityScale = original_gravity_scale;
            jumping = false;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision from player " + col);
        if (col.collider.gameObject.layer == 8 && col.collider.gameObject.name == "MrBoss")
        {
            GameLogic.GetComponent<GameLogic>().hurtBoss();
            GameLogic.GetComponent<GameLogic>().hurtPlayer(1);
        }
    }

}
