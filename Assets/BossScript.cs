using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour
{
    public int bossHealth = 100;
    public float bossMoveSpeed = 5f;
    public float smashSpeed = 10f;
    public float timeBetweenDirections = 2f;
    public float attackPeriod = 1f;

    public GameObject projectile;
    public int projectileCount;
    public float radius;

    private Rigidbody2D rb;
    public int stage = 1;

    private bool smashing = false;
    private bool smashing_moving = false;
    private Vector3 target = new Vector3(0f,0f,0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveSideToSide());
        StartCoroutine(AttackFrequently());
    }

    private IEnumerator MoveSideToSide()
    {
        while (stage == 1 && smashing == false)
        {
            rb.linearVelocity = new Vector3(1 * bossMoveSpeed, 0, 0);
            yield return new WaitForSeconds(timeBetweenDirections);
            rb.linearVelocity = new Vector3(-1 * bossMoveSpeed, 0, 0);
            yield return new WaitForSeconds(timeBetweenDirections);
        }


    }

    private IEnumerator AttackFrequently()
    {
        while (stage == 1)
        {
            ProjectileAttack();
            yield return new WaitForSeconds(attackPeriod);
        }


    }

    private void ProjectileAttack()
    {
        float angleStep = (Mathf.PI/2) / projectileCount; // 90 degrees/# projectiles --> shoot out of lower end
        float initialAngle = (((3 * Mathf.PI / 2) - (Mathf.PI / 4))* Mathf.Rad2Deg + 10) * (1/ Mathf.Rad2Deg);
        for (int i = 0; i < projectileCount; i++)
        {
            // Calculate the angle for the current item
            float angle = initialAngle + angleStep * i;

            // Calculate the x and y coordinates based on the angle and radius
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            float rotation_angle = (Mathf.Atan2(x, y) + (Mathf.PI / 2)) * Mathf.Rad2Deg; //faces away from center
            Quaternion rotation = Quaternion.Euler(0f, 0f, rotation_angle);

            // Instantiate the item and set its position
            GameObject newProjectile = Instantiate(projectile, new Vector3(transform.position.x + x, transform.position.y + y, 0), rotation);
        }
    }

    private void SmashAttack()
    {
        smashing = true;
        smashing_moving = true;
        rb.linearVelocity = new Vector3(0, -1 * smashSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (smashing_moving)
        {
            var step = smashSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                // Swap the position of the cylinder.
                smashing_moving = false;
            }
        }
    }
}
