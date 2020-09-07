using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MoveToPosition : IState
{
    private Vector2 moveDir;
    private Rigidbody2D rb;
    private float moveSpeed;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    public float nextWapointDistance = 3f;
    private bool reachEndofPath = false;
    private Func<Transform> FindPlayer;
    private Transform target;
    private float pathResetTimer;
    private float pathResetCoolDown = 1.3f;

    public MoveToPosition(Rigidbody2D rb, float moveSpeed, ref Seeker seeker, Func<Transform> FindPlayer)
    {
        this.rb = rb;
        this.moveSpeed = moveSpeed;
        this.seeker = seeker;
        this.FindPlayer = FindPlayer;
    }

    public void OnEnter()
    {
        // Start Path
        target = FindPlayer();
        seeker.StartPath(rb.position, target.position, OnPathComplete);
        // Reset Cooldown
        pathResetTimer = Time.time + pathResetCoolDown;
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
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            // Reset Cooldown 
            pathResetTimer = Time.time + pathResetCoolDown;
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
    }

    private void Move()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWapointDistance)
        {
            currentWaypoint++;
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
