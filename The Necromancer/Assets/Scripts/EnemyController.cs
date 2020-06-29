using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public float moveSpeed = 3f;

    private GameObject player;
    private Vector2 moveDir;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindMoveDir();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
    }

    private void FindMoveDir()
    {
        // Find the vector between the enemy and player and normalize it
        moveDir = player.GetComponent<Rigidbody2D>().position - rb.position;
        moveDir.Normalize();
    }
}
