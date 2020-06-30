using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 movement;
    Vector2 mousePosition;

    public float raiseCorpseRadius = 2.5f;
    private float waitTime = 2.0f;
    private float timer = 0.0f;

    public float lifeDrainRadius = 2.5f;
    public float drainSpeed = 3f;
    public float drainConversion = 0.60f;

    public float currentHealth;
    public float maxHealth = 30f;

    private EnemyController _enemyMovement;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        // Polling Left Click
        if (Input.GetMouseButtonDown(0))
        {
            //Raise Corpse
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, raiseCorpseRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Enemy")
                {
                    RaiseCorpse(colliders[i].gameObject);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            // Drain life
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.position, lifeDrainRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Enemy")
                {
                    LifeDrain(colliders[i].gameObject);
                }
            }
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void RaiseCorpse(GameObject Enemy)
    {
        // At level 1 raise corpse will raise grasping hand to hold the enemy in place for a set amount of time based on the enemy
        _enemyMovement = Enemy.GetComponent<EnemyController>();
        float prevSpeed = _enemyMovement.moveSpeed;
        _enemyMovement.moveSpeed = 0;
    }

    private void LifeDrain(GameObject Enemy)
    {
        //might want to make this a coroutine
        _enemyMovement = Enemy.GetComponent<EnemyController>();
        _enemyMovement.health -= drainSpeed * Time.deltaTime;
        if (currentHealth < maxHealth)
        {
            currentHealth += drainSpeed * Time.deltaTime * drainConversion;
        }
        Debug.Log("Take Health: " + currentHealth);
        Debug.Log(_enemyMovement.health);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            currentHealth -= 1;
            Debug.Log("Take Damage, Health: " + currentHealth);
        }
    }

    private void OnDrawGizmos()
    {
        //Raise Corpse Visual
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePosition, raiseCorpseRadius);
        // Life Drain radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rb.position, lifeDrainRadius);


    }
}
