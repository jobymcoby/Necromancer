using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Pathfinding;

public class RangedNPCController : NPCController
{
    private RangedAttackBase attackAction;

    private float innerAttackRange;

    protected override void Awake()
    {
        base.Awake();
        attackAction = attackComp as RangedAttackBase;
        innerAttackRange = npcData.attack1.innerRange;

        #region State Machine Setup
        // States
        stateMachine = new StateMachine();
        var idle = new Idle(this, rb);
        var move = new MoveToRangedPosition(this, rb, npcData, feet);
        var attack = new Attack(this, rb);
        var corpse = new Corpse(gameObject, rb, hitBox, body, feet);

        // State transition conditions
        Func<bool> OnTarget() => () => targeter.Target != null;                                                 // When the npc finds a target                         
        Func<bool> OffTarget() => () => !OnTarget().Invoke();                                                   // When the npc loses a target
        Func<bool> OnDeath() => () => npcHealth.health.Current() == 0;                                          // NPC health is below zero

        Func<bool> InRange() => () => innerAttackRange * innerAttackRange <= targeter.Distance.sqrMagnitude     // Target is within attack range
                                    && targeter.Distance.sqrMagnitude <= attackRange * attackRange;         

        Func<bool> TargetOutRange() => () => !InRange().Invoke()                                                // Target is not in range but instantiated and npc is not mid attack
                                            && OnTarget().Invoke() 
                                            && !attackAction.attacking;                           

        // Helper for adding state  machine transitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        // State Transitions
        At(idle, move, OnTarget());                         // When idling, move when you have a target
        At(move, attack, InRange());                        // When moving to target, attack when you are in range
        At(attack, move, TargetOutRange());                 // When attacking, and target is out of range, move
        At(attack, idle, OffTarget());                      // When attacking and no targets, idle
        At(move, idle, OffTarget());                        // When moving and no targets, idle
        stateMachine.AddAnyTransition(corpse, OnDeath());   // Anytime health is below 0, die

        // Set first state to alive
        stateMachine.SetState(idle);
        #endregion

        // Connect logic to Graphics here
        // These are sent by the state machine to the graphics script
        attack.AttackAnimation += npcGFX.AttackSwitch;
        corpse.DeathAnimation += npcGFX.Die;
        ResurrectAnimation += npcGFX.Resurrect;
    }

    public Vector3 FindRangedPosition()
    {
        // Creates the path to the target 
        if (targeter.Distance.sqrMagnitude > attackRange * attackRange)
        {
            return targeter.Target.position;
        }

        // Creates the path directly away from the target times a mutiple
        else
        {
            Vector3 runPos = rb.position - targeter.Distance.normalized*5;
            return runPos;
        }    
    }

    private void OnDrawGizmos()
    {
        // Display Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
