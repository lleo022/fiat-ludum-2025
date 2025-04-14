using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class existentialCrisis1 : MonoBehaviour
{
    [SerializeField] GameLogic gameLogic;
    [SerializeField] string[] monologue;
    private bool done = false;

    bool playerInRange = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(interact());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueScript dialogueScript = gameLogic.GetComponent<DialogueScript>();
        if (dialogueScript == null)
        {
            Debug.LogError("DialogueScript component not found on GameLogic!");
            return;
        }
    }
    private IEnumerator interact()
    {
        if (!done)
        {
            done = true;
            DialogueScript dialogueScript = gameLogic.GetComponent<DialogueScript>();

            gameLogic.current_player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            gameLogic.current_player.GetComponent<Movement>().enabled = false;

            dialogueScript.dialogue(monologue, "You");
            yield return new WaitUntil(() => gameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false);
            gameLogic.GetComponent<GameLogic>().current_player.GetComponent<Movement>().enabled = true;
        }
    }
}
