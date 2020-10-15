using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attack : IState
{
    private NPCController npc;
    private Rigidbody2D rb;
    private NPCGFX npcGFX;

    public Attack(NPCController npc, Rigidbody2D rb, NPCGFX npcGFX)
    {
        this.npc = npc;
        this.rb = rb;
        this.npcGFX = npcGFX;
    }

    public void OnEnter()
    {
        npcGFX.animator.SetBool("Attacking",true);
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {
        npc.facingDirection = npc.targeter.Distance.normalized;
    }

    public void OnExit()
    {
        npcGFX.animator.SetBool("Attacking", false);
        npc.targeter.FindTarget(0);
    }
}
