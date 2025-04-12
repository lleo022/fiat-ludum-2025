using UnityEngine;

public class FunguyEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;  
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer;
    
    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Attack only when player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                // Attack
            }
        }
    }

    private bool PlayerInSight()
    {
        // RaycastHit2D hit  = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x, 
        //     boxCollider.bounds.size, 0, Vector2.left, 0, playerLayer);
        // return hit.collider != null;

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
      //  Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
       //     boxCollider.bounds.size);
    }

}


