using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Rigidbody2D rb;
    public Camera cam;
    public Vector2 movement;
    public Vector2 mousePosition;
    
    public float graspingHandsRadius = 2.5f;
    public float graspingHandsCooldown = 2.0f;
    private float graspingHandsAttackTimer = 0.0f;
    public GameObject graspingHandsArea;

    private bool hoverCorpse = false;       //Is the mouse hovering over a corpse

    public float lifeDrainRadius = 2.5f;
    public float drainSpeed = .01f;
    public float drainConversion = 0.60f;
    public float lifeDrained;
    public float drainCooldown = 0.8f;
    private float drainAttackTimer = 0.0f;
    public GameObject drainShadow;
    public GameObject drainOutline;

    public float currentHealth;
    public float maxHealth = 30f;

    public EnemyController _enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Player Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        
        // Grasping Hands Input
        if (Input.GetMouseButtonDown(0) && hoverCorpse == false) 
        {
            GraspingHands(mousePosition);
        }

        // Life Drain Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("LifeDrain", rb.position);
        }
        
        // idk about this attack
        if (Input.GetMouseButton(1))
        {
            //Fire Bolt
        }

        //Keep max health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    void GraspingHands(Vector2 mousePosition)
    {
        // Cool Down Check
        if (Time.time >= graspingHandsAttackTimer)
        {
            // Starts coroutine for the grasping hands animation
            graspingHandsArea.SetActive(true);
            // Find enemies to grapple
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, graspingHandsRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.tag == "Enemy" && hoverCorpse == false)
                    {
                        colliders[i].gameObject.GetComponent<EnemyController>().Grappled = true;
                    }
                }
            //Start Cool Down Timer
            graspingHandsAttackTimer = Time.time + graspingHandsCooldown;
        }
    }

    private IEnumerator LifeDrain(Vector2 selfPosition)
    {
        drainShadow.GetComponent<SpriteRenderer>().enabled = true;
        drainOutline.GetComponent<SpriteRenderer>().enabled = true;

        while (Input.GetKey(KeyCode.Space))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(selfPosition, lifeDrainRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Enemy")
                {
                    //This should start health stealing coroutines while enemies are in the sphere
                    colliders[i].gameObject.GetComponent<EnemyController>().health.Damage(drainSpeed);   
                }
            }

            if (currentHealth < maxHealth)
            {
                currentHealth += lifeDrained * drainConversion;
            }

            yield return null;
        }

        drainShadow.GetComponent<SpriteRenderer>().enabled = false;
        drainOutline.GetComponent<SpriteRenderer>().enabled = false;

        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            currentHealth -= 1;
        }
    }

    private void OnDrawGizmos()
    {
        //Raise Corpse Visual
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePosition, graspingHandsRadius);
        // Life Drain radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rb.position, lifeDrainRadius);
    }
}
