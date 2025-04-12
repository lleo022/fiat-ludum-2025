using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BusinessmanPowers : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private Rigidbody2D rb;

    private InputAction legalbinding;

    public GameObject legal_documents;

    //private GameObject GameLogic;

    public float documentSpawnOffset = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        //GameLogic = GameObject.Find("GameLogic");
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
        Debug.Log("LegalBinding");
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0, documentSpawnOffset, 0);
        Debug.Log("Pawn position: " + gameObject.transform.position + " | " + spawnPos);
        GameObject new_docs = Instantiate(legal_documents, spawnPos, Quaternion.identity);
        new_docs.GetComponent<Launch>().movement = GetComponent<Movement>();
        
       // new_docs.GetComponent<Launch>().faceDirection = facingDirection;
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
