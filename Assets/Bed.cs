using UnityEngine;
using UnityEngine.InputSystem;

public class Bed : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    private GameObject playerInBed = null;
    private float og_gravityScale;

    public Vector2 bed_offset;
    public bool flipBed = false;
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        move.performed += GetOutOfBed;

    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void GetOutOfBed(InputAction.CallbackContext context)
    {
        if (playerInBed)
        {
            if (!flipBed)
            {
                playerInBed.transform.Rotate(Vector3.forward, -90);
                playerInBed.transform.position = transform.position + new Vector3(1f, 0, 0);
            } else
            {
                playerInBed.transform.Rotate(Vector3.forward, 90);
                playerInBed.transform.position = transform.position + new Vector3(-1f, 0, 0);
            }
            
            playerInBed.GetComponent<Movement>().enabled = true;
            playerInBed.GetComponent<Rigidbody2D>().gravityScale = og_gravityScale;
            playerInBed = null;
        }
    }

    private void EnterBed()
    {
        if (playerInBed)
        {
            og_gravityScale = playerInBed.GetComponent<Rigidbody2D>().gravityScale;
            playerInBed.GetComponent<Rigidbody2D>().gravityScale = 0;
            playerInBed.GetComponent<Movement>().enabled = false;
            
            //playerInBed.transform.Rotate(Vector3.forward, 90);
            playerInBed.transform.position = transform.position + new Vector3(bed_offset.x, bed_offset.y, 0);

            if (!flipBed)
            {
                playerInBed.GetComponent<SpriteRenderer>().flipX = false;
                playerInBed.transform.Rotate(Vector3.forward, 90);
            } else
            {
                playerInBed.GetComponent<SpriteRenderer>().flipX = true;
                playerInBed.transform.Rotate(Vector3.forward, -90);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (playerInBed == null)
        {
            playerInBed = col.gameObject;
            EnterBed();
        }
    }
}
