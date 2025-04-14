using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class trophyDialogue : MonoBehaviour
{
    [SerializeField] GameLogic gameLogic;
    bool playerInRange = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var dialogueScript = gameLogic.GetComponent<DialogueScript>();
        if (dialogueScript == null)
        {
            Debug.LogError("DialogueScript component not found on GameLogic!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerInRange = false;
                StartCoroutine(interact());
            }
        }
    }
    private IEnumerator interact()
    {
        var dialogueScript = gameLogic.GetComponent<DialogueScript>();

        gameLogic.current_player.GetComponent<Movement>().enabled = false;
        string[] dialogue_Trophy = { "Mr. Boss...", "I quit!" };
        string[] dialogue_Player = { "Quit?", "...", "HAH HA HA", "You can't quit or else I'll FIRE YOU!!" };

        dialogueScript.dialogue(dialogue_Trophy, "Trophy");
        yield return new WaitUntil(() => gameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false);
        dialogueScript.dialogue(dialogue_Player, "Player");
        yield return new WaitUntil(() => gameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false); //wait till dialogue box is closed
        gameLogic.GetComponent<GameLogic>().current_player.GetComponent<Movement>().enabled = true;
    }
}
