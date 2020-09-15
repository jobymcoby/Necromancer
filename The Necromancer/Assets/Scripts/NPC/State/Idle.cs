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

    }

    public void Tick()
    {
        Debug.Log("im idle");
    }

    public void FixedTick()
    {
        npc.facingDirection = PlayerManager.instance.PlayerDirection(rb);
    }

    public void OnExit()
    {
        
    }
}
