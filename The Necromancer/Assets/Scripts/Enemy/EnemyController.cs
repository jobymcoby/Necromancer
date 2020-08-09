using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IDamagable
{
    public Rigidbody2D rb;
    public Collider2D hitBox;
    public Animator animator;
    public HealthSystem health;
    public HealthBar healthBar;
    public float holdTime;

    #region AI Abilities
    protected GameObject target;
    protected Vector2 moveDir;
    public Color deadColor;
    #endregion

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); 
        healthBar = GetComponentInChildren<HealthBar>();
    }

    protected void Start()
    {
        SetMaxHealth();
        SetHoldTime();
        healthBar.Setup(health);

        // Set target to player
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        FindMoveDirections(target);
        if (health.Current() == 0) Die();
    }

    protected void FixedUpdate()
    {
        Move(); 
    }

    public void Damage(float dmg)
    {
        health.Damage(dmg);
    }

    public void Die()
    {
        // Set Tag
        tag = "Corpse";

        // Stop movement, remove collisions
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = true;
        animator.enabled = false;

        // Set death shader
        GetComponent<SpriteRenderer>().color = deadColor;
    }

    public void Resurrect()
    {
        // Switch State 
        tag = "Undead";
        // Switch State 
        health.Resurrect();
        Debug.Log("Full Rez");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = false;
        animator.enabled = true;
        target = gameObject;

    }

    public abstract void FindMoveDirections(GameObject target);
    public abstract void Move();
    public abstract void SetMaxHealth();
    public abstract void SetHoldTime();

}
