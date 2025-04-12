using UnityEngine;
using System.Collections;

public class Launch : MonoBehaviour
{
    //public Vector2 faceDirection = new Vector2(0,0);
    public Movement movement; //should be set by spawner

    public int launchVelocity = 5;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 facingDirection = new Vector2(-1, 0); //left
        if (movement.goingRight)
        {
            facingDirection = new Vector2(1, 0); //right
        }
        Debug.Log("Legal document start");
        rb.linearVelocity = new Vector2(facingDirection.x*launchVelocity, launchVelocity);
        StartCoroutine(SelfDestruct());
    }
    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject); // "gameObject" refers to itself
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == 8) 
        {
            //if it collides with an enemy, also destroy
            //enemy: layer 8
            //projectiles: layer 7
            Destroy(gameObject);
        }
        
    }
}
