using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Pathfinding;

public class NPCController : MonoBehaviour
{
    public NPCData npcData;
    public NPCHealth npcHealth;
    public Vector2 facingDirection;
    public NPCTargeting targeter;
    public string state;
    
    // make trigger
    public bool rez = false;

    private NPCGFX npcGFX;
    private Rigidbody2D rb;
    private Collider2D hitBox;
    private StateMachine stateMachine;
    private MeleeAttackController meleeAttack;
    public AggressionMatrix aggressionMatrix;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        npcHealth = GetComponent<NPCHealth>();

        targeter = GetComponentInChildren<NPCTargeting>();
        npcGFX = GetComponentInChildren<NPCGFX>();

        meleeAttack = GetComponentInChildren<MeleeAttackController>();
        float attackRangeSqr = Mathf.Pow(meleeAttack.attackRange,2f);

        aggressionMatrix = new AggressionMatrix(this.tag);

        #region State Machine Setup
        // States
        stateMachine = new StateMachine();
        var idle = new Idle(this, rb);
        var move = new MoveToPosition(this, rb, npcData);
        var attack = new Attack(this, rb, npcGFX);
        var corpse = new Corpse(gameObject, rb, hitBox);
        var undead = new Undead(gameObject, rb, hitBox);

        // State transition conditions
        Func<bool> OnTarget() => () => targeter.Target != null;
        Func<bool> OffTarget() => () => !OnTarget().Invoke();
        Func<bool> OnDied() => () => npcHealth.health.Current() == 0;
        Func<bool> OnRez() => () => rez == true;
        Func<bool> InRange() => () => targeter.Distance.sqrMagnitude <= attackRangeSqr;
        Func<bool> NewTarget() => () => !InRange().Invoke() && OnTarget().Invoke();
        // Func<bool> InCoolDown() => () => !cooldown up.

        // Set State transitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        At(idle, move, OnTarget());
        At(move, attack, InRange());
        At(attack, move, NewTarget());
        At(attack, idle, OffTarget());
        At(move, idle, OffTarget());

        At(idle, corpse, OnDied());
        At(move, corpse, OnDied());
        At(attack, corpse, OnDied());

        At(corpse, undead, OnRez());

        // Set first state to alive
        stateMachine.SetState(idle);
        #endregion
    }

    private void Update()
    {
        stateMachine.Tick();
        state = stateMachine.currentState;

        // death check? probably make this an event
        if (npcHealth.health.Current() <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    private void LateUpdate()
    {
        // reset rez after transition
        if (rez == true) rez = !rez;
    }

    public IEnumerator Grappled()
    {
        // Freeze Enemy position 
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(npcData.holdTime);
        // If they are still alive unfreeze
        if (this != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}

