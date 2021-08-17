using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attack : IState
{
    private NPCController npc;
    private Rigidbody2D rb;
    private IAttack attackType;

    public delegate void AttackInitiate(bool val);
    public event AttackInitiate AttackAnimation;

    public Attack(NPCController npc, Rigidbody2D rb)
    {
        this.npc = npc;
        this.rb = rb;
    }

    public void OnEnter()
    {
        rb.velocity = Vector2.zero;

        if (npc.npcData.attack1.whileAttackingRangeBoost != 0)
        {
            npc.attackRange = npc.attackRange + npc.npcData.attack1.whileAttackingRangeBoost;
        }
    }

    public void Tick()
    {
        AttackAnimation?.Invoke(true);

        // Check for new target
        npc.targeter.FindTarget(cooldown: 5f);
    }

    public void FixedTick()
    {
        npc.facingDirection = npc.targeter.Distance.normalized;
    }

    public void OnExit()
    {
        if (npc.npcData.attack1.whileAttackingRangeBoost != 0)
        {
            npc.attackRange = npc.attackRange - npc.npcData.attack1.whileAttackingRangeBoost;
        }

        AttackAnimation?.Invoke(false);
        npc.targeter.FindTarget(0);
    }
}
