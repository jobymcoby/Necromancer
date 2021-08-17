using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MoveToTarget : IState
{
    private Rigidbody2D rb;
    private NPCData enemyData;
    private NPCController npc;
    private GameObject feet;
    private Vector2 groupDirection;
    private float pathW = 2;
    private float cohesionW = 1;
    private float alignW = 1;
    private float avoidW = 4;
    private Vector2 currentVelocity;
    private float agentSmoothTime = 1.5f;

    public MoveToTarget(NPCController npc, Rigidbody2D rb, NPCData enemyData, GameObject feet)
    {
        this.rb = rb;
        this.enemyData = enemyData;
        this.npc = npc;
        this.feet = feet;
    }

    public void OnEnter()
    {
        // Creates a path to the taget
        npc.targeter.CreatePath(npc.targeter.Target.position);
    }

    public void Tick()
    {
        // Update Path
        npc.targeter.CreatePath(npc.targeter.Target.position);

        Vector2 cohesionMove = Vector2.SmoothDamp(groupDirection, npc.targeter.CohesionAllies(feet.transform), ref currentVelocity, agentSmoothTime);

        groupDirection = (
            npc.targeter.PathDirection() * enemyData.pathWeight +
            cohesionMove * enemyData.cohesionWeight +
            npc.targeter.AlignAllies() * enemyData.alignWeight +
            npc.targeter.AvoidAllies(feet.transform) * enemyData.avoidWeight
        ).normalized;

        // Check for new target
        npc.targeter.FindTarget( cooldown: 2f );
    }

    public void FixedTick()
    {
        npc.Move(groupDirection, rb, enemyData);
        // Display group direction
        Debug.DrawRay(feet.transform.position, (Vector3)groupDirection, Color.magenta);

        // Face the same direction that you are moving
        npc.facingDirection = rb.velocity.normalized;
    }

    public void OnExit()
    {

    }
}
