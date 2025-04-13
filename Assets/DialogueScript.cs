using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dialogueUI;
    public GameObject textBox;
    public GameObject nameBox;
    

    public float charDelay;

    private PlayerInputActions playerControls;

    private InputAction turnOffDialogue;

    private int dialogueIndex = 0;
    //private List<string> messages_list;
    private string[] messages_list = new string[0];

    private bool currentlyTyping = false;

    public void dialogue(string [] messages, string name)
    {
        dialogueUI.SetActive(true);
        Debug.Log("Called dialogue box" + messages[0] + messages.Length);
        nameBox.GetComponent<TextMeshProUGUI>().text = name;
        Debug.Log("Name box set");
        messages_list = new string[messages.Length];
        for (int i = 0; i < messages.Length; i++) //copy array
        {
            Debug.Log("For loop " + messages[i]);
            messages_list[i] = messages[i];
        }
        Debug.Log("messages_list" + messages_list[0]);
        StartCoroutine(dialogueCoroutine(messages_list[dialogueIndex]));
    }
    private IEnumerator dialogueCoroutine(string message)
    {
        currentlyTyping = true;
        string partial_message = "";
        foreach (char c in message)
        {
            if (currentlyTyping == false) //if player hits enter midway
            {
                break;
            }
            partial_message = partial_message + c;
            textBox.GetComponent<TextMeshProUGUI>().text = partial_message;
            yield return new WaitForSeconds(charDelay);
        }
        textBox.GetComponent<TextMeshProUGUI>().text = message;
        currentlyTyping = false;
    }
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        dialogueUI.SetActive(false);
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        turnOffDialogue = playerControls.Player.TurnOffDialogue;
        turnOffDialogue.Enable();
        turnOffDialogue.performed += TurnOffDialogue;
    }

    private void OnDisable()
    {
        turnOffDialogue.Disable();
    }

    private void TurnOffDialogue(InputAction.CallbackContext context)
    {
        if (currentlyTyping == true)
        {
            //interrupt for loop
            currentlyTyping = false;
            return;
        }
        dialogueIndex += 1;
        if (dialogueIndex >= messages_list.Length)
        {
            dialogueUI.SetActive(false);
            dialogueIndex = 0;
        } else
        {
            StartCoroutine(dialogueCoroutine(messages_list[dialogueIndex]));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
