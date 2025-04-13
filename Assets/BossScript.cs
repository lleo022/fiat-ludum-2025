using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BossScript : MonoBehaviour
{
    private float bossHealth = 100f;
    public float maxBossHealth = 100f;
    public float bossMoveSpeed = 5f;
    public float smashSpeed = 10f;
    public float timeBetweenDirections = 2f;
    public float attackPeriod = 1f;
    private GameObject GameLogic;

    public Slider healthSlider;

    public GameObject projectile;
    public int projectileCount;
    public float radius;

    private Rigidbody2D rb;
    public int stage = 1;

    private bool smashing = false;
    private bool smashing_moving = false;
    private Vector3 original_position;
    private Vector3 target = new Vector3(0f,0f,0f);

    public float stunTime = 3f;

    private bool stunned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
        rb = GetComponent<Rigidbody2D>();
        original_position = transform.position;
        //healthSlider.SetActive(true);
        returnToNormal();

        bossHealth = maxBossHealth;
        healthSlider.maxValue = maxBossHealth;
        healthSlider.value = bossHealth;
        
    }

    private void returnToNormal()
    {
        StartCoroutine(MoveSideToSide());
        if (stage == 1)
        {
            StartCoroutine(Stage1());
        } else if (stage == 2)
        {
            StartCoroutine(Stage2());
        }
    }

    private IEnumerator MoveSideToSide()
    {
        while (smashing == false && stunned == false)
        {
            rb.linearVelocity = new Vector3(1 * bossMoveSpeed, 0, 0);
            yield return new WaitForSeconds(timeBetweenDirections);
            rb.linearVelocity = new Vector3(-1 * bossMoveSpeed, 0, 0);
            yield return new WaitForSeconds(timeBetweenDirections);
        }


    }

    private IEnumerator Stage1()
    {
        while (stage == 1 && stunned == false)
        {
            ProjectileAttack();
            yield return new WaitForSeconds(attackPeriod);
            if ((bossHealth <= 50f) && (stage == 1))
            {
                stage = 2;
                StartCoroutine(Stage2());
                break;
            }
        }


    }

    private IEnumerator Stage2()
    {
        Debug.Log("Stage 2 start");
        // every random amount of second, it will smash
        while (stage == 2 && stunned == false)
        {
            target = GameLogic.GetComponent<GameLogic>().current_player.transform.position;
            StartCoroutine(SmashAttack());
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
            if (bossHealth <= 0)
            {
                GameLogic.GetComponent<GameLogic>().Victory();
                Destroy(gameObject);
            }

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
            newProjectile.GetComponent<BossProjectileScript>().GameLogic = GameLogic;
        }
    }

    private IEnumerator SmashAttack()
    {
        if (smashing == false)
        {
            smashing = true;
            smashing_moving = true;
            //rb.linearVelocity = new Vector3(0, -1 * smashSpeed, 0);
            while (smashing_moving == true)
            {
                yield return new WaitForSeconds(.05f);

            }
            yield return new WaitForSeconds(1f);
            //change taregt back to original position
            target = original_position;
            smashing_moving = true;
            while (smashing_moving == true)
            {
                yield return new WaitForSeconds(.1f);

            }
            smashing = false;
            returnToNormal(); //go back to moving from side to side
        }

    }

    private bool approachPosition()
    {
        var step = smashSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            // Swap the position of the cylinder.
            return false;
        } else
        {
            return true;
        }
    }

    private IEnumerator stun()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        returnToNormal();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision: " + col.gameObject.name + " | " + col.gameObject.layer);
        if (col.gameObject.layer == 10)
        {
            Debug.Log("Collision is layer 10");
            //if hit by player
            if (col.gameObject.name == "Flower(Copy)")
            {
                Debug.Log("Hit boss- boss script");
                bossHealth = bossHealth - 10f;
            } else if (col.gameObject.name == "LegalDocument(Copy)")
            {
                StartCoroutine(stun());
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (smashing_moving)
        {
            smashing_moving = approachPosition(); //will eventually become false
        }
        if (bossHealth >= 0)
        {
            healthSlider.value = bossHealth;
        }
        
    }
}
