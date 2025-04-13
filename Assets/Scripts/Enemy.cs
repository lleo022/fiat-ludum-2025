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

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameLogic = GameObject.Find("GameLogic");
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
    }

    private bool PlayerInSight()
    {
         RaycastHit2D hit  = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
             0, Vector2.left, 0, playerLayer);
         
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

