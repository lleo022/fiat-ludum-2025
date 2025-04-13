using UnityEngine;
using System.Collections;

public class FlowerProjectile : MonoBehaviour
{
    //public GameObject target;
    public float flowerSpeed = 5f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == 8)
        {//if hit player
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gameObject.name = "Flower(Copy)";
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector3(0, Vector3.up.y*flowerSpeed, 0);
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
