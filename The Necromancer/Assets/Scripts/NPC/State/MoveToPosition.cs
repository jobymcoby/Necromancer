using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MoveToPosition : IState
{
    private Rigidbody2D rb;
    private NPCData enemyData;
    private NPCController npc;

    public MoveToPosition(NPCController npc, Rigidbody2D rb, NPCData enemyData)
    {
        this.rb = rb;
        this.enemyData = enemyData;
        this.npc = npc;
    }

    public void OnEnter()
    {
        // Creates a path to the taget
        npc.targeter.CreatePath();
    }

    public void Tick()
    {
        // Update Path
        npc.targeter.CreatePath();

        // Check for new target
        npc.targeter.FindTarget( cooldown: 2f );
    }

    public void FixedTick()
    {
        Move(npc.targeter.WalkDirection());

        // Face the same direction that you are moving
        npc.facingDirection = rb.velocity.normalized;
    }

    private void Move(Vector2 direction)
    {
        Vector2 velocity = direction * enemyData.moveSpeed;
        ApplyForceToReachVelocity(rb, velocity);
    }

    public static void ApplyForceToReachVelocity(Rigidbody2D rb, Vector2 velocity, float force = 1, ForceMode2D mode = ForceMode2D.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rb.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rb.velocity.magnitude == 0)
        {
            rb.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector2.Dot(velocity, rb.velocity) / velocity.magnitude);
            rb.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }

    public void OnExit()
    {

    }
}
