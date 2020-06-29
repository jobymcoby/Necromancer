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

    public float clickRadius = 2.5f;

    private EnemyController _enemyMovement;
    private float waitTime = 2.0f;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        // Polling
        if (Input.GetMouseButtonDown(0))
        {
            //Raise Corpse
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, clickRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Enemy")
                {
                    RaiseCorpse(colliders[i].gameObject);
                }
            }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Take Damage");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePosition, clickRadius);
    }
}
