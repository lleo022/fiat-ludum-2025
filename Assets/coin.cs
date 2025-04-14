using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class coin : MonoBehaviour
{
    [SerializeField] GameLogic gameLogic;
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
        
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
