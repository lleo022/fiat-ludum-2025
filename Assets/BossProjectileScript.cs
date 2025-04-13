using UnityEngine;
using System.Collections;

public class BossProjectileScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    public float projectileSpeed = 3f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //transform.forward is local forward vector
        //Debug.Log("New projectile: forward direction = " + transform.up);
        rb.linearVelocity = new Vector3(transform.right.x * projectileSpeed, transform.right.y * projectileSpeed, 0);
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject); // "gameObject" refers to itself
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
