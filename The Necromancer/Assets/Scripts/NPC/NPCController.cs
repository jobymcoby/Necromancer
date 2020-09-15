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
    // make trigger
    public bool rez = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D hitBox;
    private StateMachine stateMachine;
    private NPCTargeting tageter;

    private void Awake()
    {
        // Set target to player fix until i figure out why it changes stae when the target is still null

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        tageter = GetComponentInChildren<NPCTargeting>();
        npcHealth = GetComponent<NPCHealth>();

        #region State Machine Setup
        // States
        stateMachine = new StateMachine();
        var idle = new Idle(this, rb);
        var move = new MoveToPosition(this, rb, npcData, seeker, TargetInSight);
        var attack = new Attack();
        var corpse = new Corpse(gameObject, rb, hitBox);
        var undead = new Undead(gameObject, rb, hitBox);

        // State transition conditions
        Func<bool> OnTarget() => () => TargetInSight() != null;
        Func<bool> OffTarget() => () => !OnTarget().Invoke();
        Func<bool> OnDied() => () => npcHealth.health.Current() == 0 && tag == "Enemy";
        Func<bool> OnRez() => () => rez == true;
        // Func<bool> InRange() => () => hit enemy && cooldown up;
        // Func<bool> InCoolDown() => () => !cooldown up.

        // Set State transitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        At(idle, move, OnTarget());
        At(move, idle, OffTarget());

        // At(move, attack, InRange());
        // At(move, attack, InRange());
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

    public Transform TargetInSight()
    {
        return tageter.FindTarget();
    }

    public IEnumerator Grappled()
    {
        // Freeze Enemy position 
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(npcData.holdTime);
        // If they are still alive unfreeze
        if (this != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDestroy()
    {
        
    }

}
