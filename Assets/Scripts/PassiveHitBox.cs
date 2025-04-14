using UnityEngine;
using System.Collections;

public class PassiveHitBox : MonoBehaviour
{
    [SerializeField] GameLogic gameLogic;

    private Coroutine running;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(running == null) running = StartCoroutine(loseHealth());
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator loseHealth()
    {
        gameLogic.hurtPlayer(1);
        yield return new WaitForSeconds(0.3f);
        running = null;
    }
}
