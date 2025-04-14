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
    public float smashSpeed = 20f;
    public float xRange = 4f;
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
    private Vector3 start_position;
    private Vector3 original_position;
    private Vector3 target = new Vector3(0f,0f,0f);

    public float stunTime = 3f;

    private bool stunned = false;
    private bool sideToSide = false;
    private bool invulnerable = false;

    public float flowerDamage = 5;

    private Vector2 point1;
    private Vector2 point2;

    public void hurtBoss(float amount)
    {
        //Debug.Log("HurtBoss");
        if (smashing == false && invulnerable == false)
        {
            bossHealth -= amount;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
        rb = GetComponent<Rigidbody2D>();
        original_position = transform.position;
        start_position = transform.position;
        point1 = new Vector2(start_position.x + xRange, start_position.y);
        point2 = new Vector2(start_position.x - xRange, start_position.y);

        bossHealth = maxBossHealth;
        healthSlider.maxValue = maxBossHealth;
        healthSlider.value = bossHealth;
        //healthSlider.SetActive(true);
        invulnerable = true;
        StartCoroutine(beginFight());
    }

    private IEnumerator beginFight()
    {
        GameLogic.GetComponent<GameLogic>().current_player.GetComponent<Movement>().enabled = false;
        string[] dialogue_Player = { "Mr. Boss...", "I quit!" };
        string[] dialogue_Villain = { "Quit?", "...", "HAH HA HA", "You can't quit or else I'll FIRE YOU!!" };
        string[] dialogue_Player2 = { "[Press J to shoot pies as Clown.]", "He has too many lawyers! My legal documents won't work on him." };

        GameLogic.GetComponent<DialogueScript>().dialogue(dialogue_Player, "You");
        yield return new WaitUntil(() => GameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false);
        GameLogic.GetComponent<DialogueScript>().dialogue(dialogue_Villain, "Mr. Boss");
        yield return new WaitUntil(() => GameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false); //wait till dialogue box is closed
        GameLogic.GetComponent<DialogueScript>().dialogue(dialogue_Player2, "You");
        yield return new WaitUntil(() => GameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false); //wait till dialogue box is closed
        invulnerable = false;
        GameLogic.GetComponent<GameLogic>().current_player.GetComponent<Movement>().enabled = true;
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
            sideToSide = true;
            while (smashing == false && stunned == false)
            {
                target = point1;
                sideToSide = true;
                yield return new WaitUntil(() => sideToSide == false);
                target = point2;
                sideToSide = true;
                yield return new WaitUntil(() => sideToSide == false);
            }
        } else
        {
            Debug.Log("Cant't return to normal: " + stunned + smashing);
        }
        
    }

    private IEnumerator Stage2Custcene()
    {
        stunned = true;
        invulnerable = true;

        string[] dialogue_Villain = { "Your attire is so... unprofessional." };
        GameLogic.GetComponent<DialogueScript>().dialogue(dialogue_Villain, "Mr. Boss");
        yield return new WaitUntil(() => GameLogic.GetComponent<DialogueScript>().dialogueUI.activeSelf == false); //wait till dialogue box is closed

        stunned = false;
        invulnerable = false;
        returnToNormal();

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
                StartCoroutine(Stage2Custcene());
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
            stunned = true;
            yield return new WaitForSeconds(.5f);
            stunned = false;
            StartCoroutine(SmashAttack());
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
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
                newProjectile.GetComponent<BossProjectileScript>().boss_script = GetComponent<BossScript>();
            }
        }
        
    }

    private IEnumerator SmashAttack()
    {
        if (smashing == false && stunned == false)
        {
            original_position = transform.position;
            smashing = true;
            smashing_moving = true;
            //rb.linearVelocity = new Vector3(0, -1 * smashSpeed, 0);
            yield return new WaitUntil(() => smashing_moving == false);
            yield return new WaitForSeconds(1f);
            move_back_to_original = true;
            yield return new WaitUntil(() => move_back_to_original == false);
            smashing = false;
            StartCoroutine(MoveSideToSide()); //go back to moving from side to side
        }

    }

    private bool approachPosition(Vector3 target_, float speed_)
    {
        var step = speed_ * Time.deltaTime; // calculate distance to move
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
            Debug.Log("Stun over");
            stunned = false;
            returnToNormal();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //debugDebug.Log("Collision: " + col.gameObject.name + " | " + col.gameObject.layer);
        if (col.collider.gameObject.layer == 10)
        {
            //if hit by player
            if (col.collider.gameObject.name == "Flower(Copy)")
            {
                hurtBoss(flowerDamage);
            } else if (col.collider.gameObject.name == "LegalDocument(Copy)")
            {
                StartCoroutine(stun());
            }

        } else if (col.collider.gameObject.layer == 9)
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
                smashing_moving = approachPosition(target, smashSpeed); //will eventually become false
            }
            else if (move_back_to_original)
            {
                move_back_to_original = approachPosition(original_position, bossMoveSpeed);
            } else if (sideToSide)
            {
                sideToSide = approachPosition(target, bossMoveSpeed);
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
