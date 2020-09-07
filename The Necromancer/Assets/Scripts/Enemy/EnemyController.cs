using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Pathfinding;

public class EnemyController : MonoBehaviour, IDamagable
{
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D hitBox;
    public EnemyData enemyData;
    public bool rez = false;

    private HealthSystem health;
    private HealthBar healthBar;
    private StateMachine stateMachine;

    #region AI Abilities
    private Path path;
    private int currentWaypoint = 0;
    public float nextWapointDistance = 3f;
    public Transform target;
    public Color deadColor;
    #endregion

    private void Awake()
    {
        // Set target to player fix until i figure out why it changes stae when the target is still null
        // target = GameObject.FindGameObjectWithTag("Player").transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); 
        healthBar = GetComponentInChildren<HealthBar>();

        #region State Machine Setup
        // States
        stateMachine = new StateMachine();
        var idle = new Idle();
        var move = new MoveToPosition(rb, enemyData.moveSpeed, ref seeker, FindPlayer);
        var attack = new Attack();
        var corpse = new Corpse(gameObject, rb, hitBox, animator);
        var undead = new Undead(gameObject, rb, hitBox, animator);

        // State transition conditions
        Func<bool> OnTarget() => () => FindPlayer() != null;
        Func<bool> OffTarget() => () => !OnTarget().Invoke();
        Func<bool> OnDied() => () => health.Current() == 0 && tag == "Enemy";
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

    private void Start()
    {
        health = new HealthSystem(enemyData.maxHealth);
        healthBar.Setup(health);
        target = GameObject.FindGameObjectWithTag("Player").transform;
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

    // IDamagable
    public void Damage(float dmg)
    {
        health.Damage(dmg);
    }

    public Transform FindPlayer()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float sightRange = 5f;
        if (Vector3.Distance(transform.position, player.position) < sightRange)
        {
            return player;
        }
        return null;
    }

    public IEnumerator Grappled()
    {
        // Freeze Enemy position 
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(enemyData.holdTime);
        // If they are still alive unfreeze
        if (this != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDestroy()
    {
        
    }

}
