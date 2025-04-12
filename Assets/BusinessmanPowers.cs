using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BusinessmanPowers : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private Rigidbody2D rb;

    private InputAction legalbinding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerControls = new PlayerInputActions();
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
