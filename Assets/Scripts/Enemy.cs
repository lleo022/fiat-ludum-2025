using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;  
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer;

    private Animator anim;
    private GameObject GameLogic;

    private EnemyPatrol enemyPatrol;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameLogic = GameObject.Find("GameLogic");
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Attack only when player is in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("melee");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }

    }

    private bool PlayerInSight()
    {
        Vector3 castOrigin = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector3 castSize = new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
        Vector2 castDirection = Vector2.right * transform.localScale.x; // correct direction based on facing

        RaycastHit2D hit = Physics2D.BoxCast(
            castOrigin,
            castSize,
            0,
            castDirection,
            0.1f, // small non-zero distance
            playerLayer
        );

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            boxCollider.bounds.size);
    }

    private void damagePlayer()
    {
        if (PlayerInSight())
        {
            GameLogic.GetComponent<GameLogic>().hurtPlayer(1);
        }
    }


}

