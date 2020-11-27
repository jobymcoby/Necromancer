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

    public Attack(NPCController npc, Rigidbody2D rb, IAttack attackType)
    {
        this.npc = npc;
        this.rb = rb;
        this.attackType = attackType;
    }

    public void OnEnter()
    {
        AttackAnimation?.Invoke(true);
    }

    public void Tick() { }

    public void FixedTick()
    {
        npc.facingDirection = npc.targeter.Distance.normalized;
    }

    public void OnExit()
    {
        AttackAnimation?.Invoke(false);
        npc.targeter.FindTarget(0);
    }
}
