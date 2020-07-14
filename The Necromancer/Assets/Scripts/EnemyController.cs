using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float holdTime = 1f;

    private bool _grappled;
    public bool Grappled
    {
        set
        {
            if (value == _grappled)
            {
                return;
            }
            if (value && !_grappled)
            {
                StartCoroutine("Grapple");
                _grappled = value;
            }
        }
    }

    private GameObject player;
    private Vector2 moveDir;

    public HealthSystem health;
    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private readonly float maxHealth = 10f;

    private void Awake()
    {
        health = new HealthSystem(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        healthBar.Setup(health);
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayerDirections();

        //die
        if (health.Current() == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        //Move player towards player
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
    }

    private void FindPlayerDirections()
    {
        // Find the vector between the enemy and player and normalize it
        moveDir = player.GetComponent<Rigidbody2D>().position - rb.position;
        moveDir.Normalize();
    }

    public IEnumerator Grapple()
    {
        float prevSpeed = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(holdTime);
        moveSpeed = prevSpeed;
        _grappled = false;
    }
}
