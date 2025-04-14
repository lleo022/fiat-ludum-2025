using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class clownDialogue : MonoBehaviour
{
    [SerializeField] GameLogic gameLogic;
    [SerializeField] GameObject popUp;
    bool playerInRange = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            popUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popUp.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popUp.SetActive(false);
        DialogueScript dialogueScript = gameLogic.GetComponent<DialogueScript>();
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
                popUp.SetActive(false);
                StartCoroutine(interact());
                playerInRange = true;
                popUp.SetActive(true);
            }
        }
    }
    private IEnumerator interact()
    {
        var dialogueScript = gameLogic.GetComponent<DialogueScript>();

        gameLogic.current_player.GetComponent<Movement>().enabled = false;
        string[] dialogue_Trophy = { "20XX Junior Jester League MVP" };
        string[] dialogue_Player = { "Sure, I’ve got clown powers. But these days I mostly just use them to skip the traffic.","It all went downhill after my funnybone injury.",  "Ha.", "Ha."};

        dialogueScript.dialogue(dialogue_Trophy, "Statue");
        yield return new WaitUntil(() => gameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false);
        dialogueScript.dialogue(dialogue_Player, "Player");
        yield return new WaitUntil(() => gameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false); //wait till dialogue box is closed
        gameLogic.GetComponent<GameLogic>().current_player.GetComponent<Movement>().enabled = true;
    }
}
