// ll

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameter")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Start()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (enemy == null)
            return;

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            { 
                MoveInDirection(-1); 
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
        
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, 
            initScale.y, initScale.z);

        // Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, 
            enemy.position.y, enemy.position.z);
    }

    public void EnemyDestroyed()
    {
        enemy = null;
        this.enabled = false;
        Debug.Log("EnemyPatrol notified that enemy is destroyed");
    }
}
