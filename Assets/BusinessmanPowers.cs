using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BusinessmanPowers : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private Rigidbody2D rb;

    private InputAction legalbinding;

    public GameObject legal_documents;

    private GameObject GameLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        GameLogic = GameObject.Find("GameLogic");
    }
    private void OnEnable()
    {
        //playerControls.Enable();
        legalbinding = playerControls.Player.LegalBinding;

        legalbinding.Enable();
        legalbinding.performed += LegalBinding;
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        legalbinding.Disable();
    }

    private void LegalBinding(InputAction.CallbackContext context)
    {
        GameObject new_docs = Instantiate(legal_documents, transform.position, Quaternion.identity);
        new_docs.GetComponent<Launch>().GameLogic = GameLogic;
        Vector2 facingDirection = new Vector2(-1, 0); //left
        if (GetComponent<Movement>().goingRight)
        {
            facingDirection = new Vector2(1, 0); //right
        }
        new_docs.GetComponent<Launch>().faceDirection = facingDirection;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
