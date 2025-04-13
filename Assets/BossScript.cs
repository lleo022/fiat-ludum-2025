using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using NUnit.Framework;
using System.Collections.Generic;

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
    private bool move_back_to_original = false;
    private Vector3 original_position;
    private Vector3 target = new Vector3(0f,0f,0f);

    public float stunTime = 3f;

    private bool stunned = false;

    public float flowerDamage = 5;

    public void hurtBoss(float amount)
    {
        Debug.Log("HurtBoss");
        bossHealth -= amount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
        rb = GetComponent<Rigidbody2D>();
        original_position = transform.position;
        bossHealth = maxBossHealth;
        healthSlider.maxValue = maxBossHealth;
        healthSlider.value = bossHealth;
        //healthSlider.SetActive(true);

        StartCoroutine(beginFight());
    }

    private IEnumerator beginFight()
    {
        string[] dialoguearr = { "I am here to annihilate you", "Stupid clown" };
        GameLogic.GetComponent<DialogueScript>().dialogue(dialoguearr, "Mr. Boss");
        while (GameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == true)
        {
            yield return new WaitForSeconds(.1f);
        }
        returnToNormal();
    }

    private void returnToNormal()
    {
        Debug.Log("returnToNormal called");
        move_back_to_original = true;
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
        if (stunned == false && smashing == false)
        {
            yield return new WaitUntil(() => move_back_to_original == false); //make sure boss is back at original position
            while (smashing == false && stunned == false)
            {
                rb.linearVelocity = new Vector3(1 * bossMoveSpeed, 0, 0);
                yield return new WaitForSeconds(timeBetweenDirections);
                rb.linearVelocity = new Vector3(-1 * bossMoveSpeed, 0, 0);
                yield return new WaitForSeconds(timeBetweenDirections);
            }
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
        while (stage == 2 && stunned == false && smashing == false)
        {
            target = GameLogic.GetComponent<GameLogic>().current_player.transform.position;
            yield return new WaitForSeconds(.5f);
            StartCoroutine(SmashAttack());
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 8f));
            if (bossHealth <= 0)
            {
                GameLogic.GetComponent<GameLogic>().Victory();
                Destroy(gameObject);
            }

        }
    }

    private void ProjectileAttack()
    {
        if (stunned == false)
        {
            float angleStep = (Mathf.PI / 2) / projectileCount; // 90 degrees/# projectiles --> shoot out of lower end
            float initialAngle = (((3 * Mathf.PI / 2) - (Mathf.PI / 4)) * Mathf.Rad2Deg + 10) * (1 / Mathf.Rad2Deg);
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
        
    }

    private IEnumerator SmashAttack()
    {
        if (smashing == false && stunned == false)
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

    private bool approachPosition(Vector3 target_)
    {
        var step = smashSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target_, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target_) < 0.001f)
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
        if (stunned == false)
        {
            stunned = true;
            rb.linearVelocity = Vector3.zero;
            yield return new WaitForSeconds(stunTime);
            stunned = false;
            returnToNormal();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision: " + col.gameObject.name + " | " + col.gameObject.layer);
        if (col.gameObject.layer == 10)
        {
            //if hit by player
            if (col.gameObject.name == "Flower(Copy)")
            {
                hurtBoss(flowerDamage);
            } else if (col.gameObject.name == "LegalDocument(Copy)")
            {
                StartCoroutine(stun());
            }

        } else if (col.gameObject.layer == 9)
        {
            //hit by player themselves
            hurtBoss(3 * flowerDamage);
            //bossHealth = bossHealth - 3*flowerDamage; //more damage
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned == false)
        {
            if (smashing_moving)
            {
                smashing_moving = approachPosition(target); //will eventually become false
            }
            else if (move_back_to_original)
            {
                move_back_to_original = approachPosition(original_position);
            }
        }
        
        if (bossHealth >= 0)
        {
            if (healthSlider != null)
            {
                healthSlider.value = bossHealth;
            }
        }
        
    }
}
