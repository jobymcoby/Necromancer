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
    public string currentState;
    
    // make trigger
    public bool rez = false;
    public delegate void UndeathHappens(int undeadMode,GameObject undead);
    public event UndeathHappens ResurrectAnimation;

    private NPCGraphicsBase npcGFX;

    private Rigidbody2D rb;
    private Collider2D hitBox;
    private StateMachine stateMachine;
    private IAttack attackAction;
    public AggressionMatrix aggressionMatrix;

    private void Awake()
    {
        #region Access components that are affected by the state
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        npcHealth = GetComponent<NPCHealth>();
        targeter = GetComponentInChildren<NPCTargeting>();
        npcGFX = GetComponentInChildren<NPCGraphicsBase>();

        Component attackComp = gameObject.GetComponentInChildren(typeof(IAttack));
        Debug.Log(attackComp.GetType());
        attackAction = attackComp as IAttack;
        float attackRangeSqr = attackAction.AttackRangeSqr();

        aggressionMatrix = new AggressionMatrix(this.tag);
        #endregion

        #region State Machine Setup
        // States
        stateMachine = new StateMachine();
        var idle = new Idle(this, rb);
        var move = new MoveToPosition(this, rb, npcData);
        var attack = new Attack(this, rb, attackAction);
        var corpse = new Corpse(gameObject, rb, hitBox);
        var undead = new Undead(gameObject, rb, hitBox);

        // State transition conditions
        Func<bool> OnTarget() => () => targeter.Target != null;                         // When the npc finds a target                         
        Func<bool> OffTarget() => () => !OnTarget().Invoke();                           // When the npc loses a target
        Func<bool> OnDied() => () => npcHealth.health.Current() == 0;                   // NPC health is below zero,
        Func<bool> OnRez() => () => rez == true;
        Func<bool> InRange() => () => targeter.Distance.sqrMagnitude <= attackRangeSqr; // Target is within attack range
        Func<bool> NewTarget() => () => !InRange().Invoke() && OnTarget().Invoke();     // Target is not in range but instantiated
        // Func<bool> InCoolDown() => () => !cooldown up.


        // Helper for adding state  machine transitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        
        // State Transitions
        At(idle, move, OnTarget());                         // When idling, move when you have a target
        At(move, attack, InRange());                        // When moving to target, attack when you are in range
        At(attack, move, NewTarget());                      // When attacking, and target is out of range, move
        At(attack, idle, OffTarget());                      // When attacking and no targets, idle
        At(move, idle, OffTarget());                        // When moving and no targets, idle
        stateMachine.AddAnyTransition(corpse, OnDied());    // Anytime health is below 0, die

        // Set first state to alive
        stateMachine.SetState(idle);

        // Connect logic to Graphics here
        // These are sent by the state machine to the graphics script
        attack.AttackAnimation += npcGFX.AttackSwitch;
        corpse.DeathAnimation += npcGFX.Die;
        ResurrectAnimation += npcGFX.Resurrect;
        #endregion
    }

    private void Update()
    {
        stateMachine.Tick();

        // Debug to Inspector
        currentState = stateMachine.currentState;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    public void Resurrect(int undeadMode, GameObject undeadPrefab)
    {
        ResurrectAnimation?.Invoke(undeadMode, undeadPrefab);
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

