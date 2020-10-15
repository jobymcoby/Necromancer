using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    private Rigidbody2D rb;
    private NPCController npc;


    public Idle(NPCController npc, Rigidbody2D rb)  
    {
        this.rb = rb;
        this.npc = npc;
    }

    public void OnEnter()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Tick()
    {
        npc.facingDirection = PlayerManager.instance.PlayerDirection(rb);
        // Search for a target
        npc.targeter.FindTarget(0f);
    }

    public void FixedTick()
    {
        rb.constraints =  RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnExit()
    {
        
    }
}
