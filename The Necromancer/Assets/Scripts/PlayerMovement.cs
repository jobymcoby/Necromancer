using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Rigidbody2D rb;
    public Camera cam;
    private Vector2 movement;
    public Vector2 mousePosition;
    
    public float graspingHandsRadius = 1f;
    public float graspingHandsCooldown = 2.0f;
    private float graspingHandsAttackTimer = 0.0f;
    public GameObject graspingHandsArea;

    public GameObject lifeDrainCircle;

    private bool hoverCorpse = false;       //Is the mouse hovering over a corpse

    private List<GameObject> enemiesDraining = new List<GameObject>();


    [SerializeField]
    private float maxHealth = 30f;
    private float currentHealth;

    


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
        if (Input.GetMouseButtonDown(0) && hoverCorpse == false) GraspingHands(mousePosition);

        // Life Drain Input
        lifeDrainCircle.SetActive(Input.GetKey(KeyCode.Space));

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
    }
}
