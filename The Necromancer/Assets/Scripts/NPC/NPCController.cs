using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Pathfinding;

public class NPCController : ControllerBase
{
    public float attackRange;
    public NPCHealth npcHealth;
    public NPCTargeting targeter;
    protected NPCGraphicsBase npcGFX;
    public string currentState;

    [SerializeField] protected GameObject body;
    [SerializeField] protected GameObject feet;

    // make trigger
    public delegate void UndeathHappens(int undeadMode,GameObject undead);
    public event UndeathHappens ResurrectAnimation;


    protected Rigidbody2D rb;
    protected Collider2D hitBox;
    protected StateMachine stateMachine;
    protected Component attackComp;
    public AggressionMatrix aggressionMatrix;

    protected virtual void Awake()
    {
        // Used to check if other entities has and enemy or ally tag
        aggressionMatrix = new AggressionMatrix(this.tag);

        #region Access components that are affected by the state
        rb = GetComponent<Rigidbody2D>(); 
        hitBox = body.GetComponent<Collider2D>();
        npcHealth = body.GetComponent<NPCHealth>();
        npcHealth.Startup(npcData.maxHealth);
        npcHealth.HealthBar();
        targeter = GetComponentInChildren<NPCTargeting>();
        npcGFX = GetComponentInChildren<NPCGraphicsBase>();
        attackRange = npcData.attack1.outerRange;
        attackComp = gameObject.GetComponentInChildren(typeof(IAttack));
        #endregion
    }

    private void Update()
    {
        stateMachine.Tick();

        // Display to Inspector
        currentState = stateMachine.currentState;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    public void Move(Vector2 direction, Rigidbody2D rb, NPCData stats, float force = 1, ForceMode2D mode = ForceMode2D.Force)
    {
        // not really sure what going on but this averages the flocking 
        Vector2 velocity = direction * stats.moveSpeed;

        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rb.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rb.velocity.magnitude == 0)
        {
            rb.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector2.Dot(velocity, rb.velocity) / velocity.magnitude);
            rb.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
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
