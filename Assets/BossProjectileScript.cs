using UnityEngine;
using System.Collections;

public class BossProjectileScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    public float projectileSpeed = 3f;

    public GameObject GameLogic;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //transform.forward is local forward vector
        rb.linearVelocity = new Vector3(transform.right.x * projectileSpeed, transform.right.y * projectileSpeed, 0);
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject); // "gameObject" refers to itself
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9) 
        {
            GameLogic.GetComponent<GameLogic>().hurtPlayer(1);
        }
    }
}
