using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobo : EnemyController
{
    public string title = "Blobo";
    [SerializeField] private readonly float maxHealth = 10f;
    [SerializeField] private float moveSpeed = 3f;

    private float holdTime = 1.8f;

    public override void FindMoveDirections(GameObject target)
    {
        // Find the vector between the enemy and player and normalize it
        moveDir = target.GetComponent<Rigidbody2D>().position - rb.position;
        moveDir.Normalize();
    }

    public override void Move()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
    }

    public override void SetMaxHealth()
    {
        health = new HealthSystem(maxHealth);
    }

    public override void SetHoldTime()
    {
        base.holdTime = holdTime;
    }
}
