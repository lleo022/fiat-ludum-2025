using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class coin : MonoBehaviour
{
    private GameLogic gameLogic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameLogic.addCoin();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        Debug.Log("Found gameLogic " + gameLogic);
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
