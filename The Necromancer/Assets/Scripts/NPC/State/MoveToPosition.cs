using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MoveToPosition : IState
{
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private NPCData enemyData;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private float nextWapointDistance = .75f;
    private bool reachEndofPath = false;
    private Func<Transform> FindPlayer;
    private NPCController npc;
    private Transform target;
    private float pathResetTimer;
    public Vector2 direction;

    public MoveToPosition(NPCController npc, Rigidbody2D rb, NPCData enemyData, Seeker seeker, Func<Transform> FindPlayer)
    {
        this.rb = rb;
        this.enemyData = enemyData;
        this.seeker = seeker;
        this.FindPlayer = FindPlayer;
        this.npc = npc;
    }

    public void OnEnter()
    {
        // Start Path
        target = FindPlayer();
        seeker.StartPath(rb.position, target.position, OnPathComplete);
        // Reset Cooldown
        pathResetTimer = Time.time + enemyData.detectionRate;
    }

    public void Tick()
    {
        if (path == null)
            return;

        // Cool down on Path Creation
        if (Time.time >= pathResetTimer)
        {
            // Start Path
            target = FindPlayer();
            // Make sure one path is calculated at a time
            if (seeker.IsDone())
            { 
                seeker.StartPath(rb.position, target.position, OnPathComplete);
                currentWaypoint = 0;
            }
            // Reset Cooldown 
            pathResetTimer = Time.time + enemyData.detectionRate;
        }

        // Get Direction after path creation
        try
        {
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        }
        catch (ArgumentOutOfRangeException)
        {
            currentWaypoint--;
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        }

        // End of path check
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndofPath = true;
        }
        else
        {
            reachEndofPath = false;
        }
    }

    public void FixedTick()
    {
        // no path dont move
        if (path == null)
            return;

        // End of path dont move
        if (reachEndofPath)
            return;

        Move();

        // Face the same direction that you are moving
        npc.facingDirection = rb.velocity;
    }

    private void Move()
    {

        Vector2 velocity = direction * enemyData.moveSpeed;

        ApplyForceToReachVelocity(rb, velocity);

        // When you reach a point move one to the next
        // Need to handle index out of range exception
        float distance;

        try
        {
            distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        }
        catch (ArgumentOutOfRangeException)
        {
            currentWaypoint--;
            distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        }

        if (distance < nextWapointDistance)
        {
            currentWaypoint++;
        }
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

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void OnExit()
    {
        path = null;
    }
}
