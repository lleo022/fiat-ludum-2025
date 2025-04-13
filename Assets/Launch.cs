using UnityEngine;
using System.Collections;
using System.Data;

public class Launch : MonoBehaviour
{
    //public Vector2 faceDirection = new Vector2(0,0);
    public Movement movement; //should be set by spawner
    public float launchVelocity = 5f;
    private float selfDestructTime = 2f;
    private Rigidbody2D rb;
    public bool boss_fight;

    [SerializeField] private LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.name = "LegalDocument(Copy)";
        rb = GetComponent<Rigidbody2D>();
        Vector2 facingDirection = new Vector2(-1, 1); //left
        if (boss_fight)
        {
            launchVelocity = launchVelocity * 1.5f;
            facingDirection = new Vector2(0, 1);
            rb.gravityScale = 0;
            selfDestructTime = 3f;
        }
        else
        {
            if (movement.goingRight)
            {
                facingDirection = new Vector2(1, 1); //right
            }
        }

        Debug.Log("Legal document start");

        rb.linearVelocity = new Vector2(2 * facingDirection.x * launchVelocity, facingDirection.y * launchVelocity);
        StartCoroutine(SelfDestruct());
        
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime); // destroy projectile after 2 seconds
        Destroy(gameObject); // "gameObject" refers to itself
    }

   // private trigger2d collisions did not work
   // i had to use this way to detect collisions between projectile and the child :(
    private void Update()
    {
        Collider2D myCollider = GetComponent<BoxCollider2D>();
        if (myCollider != null)
        {
            // Get all colliders overlapping with this projectile
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                transform.position,
                myCollider.bounds.size,
                0f,
                enemyLayer
            );

            // Process each hit
            foreach (Collider2D hit in hits)
            {
                // Skip if null
                if (hit == null)
                    continue;

                Debug.Log("Projectile hit: " + hit.gameObject.name);

                // Find enemy component
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy == null)
                {
                    enemy = hit.GetComponentInParent<Enemy>();
                }

                if (enemy != null)
                {
                    // Get the patrol component
                    EnemyPatrol patrol = enemy.GetComponentInParent<EnemyPatrol>();

                    if (patrol != null)
                    {
                        // Disable the patrol script first to prevent it from accessing the enemy
                        patrol.enabled = false;

                        // Notify EnemyPatrol that the enemy is gone (we'll create this method next)
                        patrol.EnemyDestroyed();
                    }

                    // Now it's safe to destroy the enemy
                    Destroy(enemy.gameObject);
                    Debug.Log("Enemy destroyed: " + enemy.gameObject.name);

                    // Destroy this projectile
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

}
