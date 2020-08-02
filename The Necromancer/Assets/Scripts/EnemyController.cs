using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D hitBox;
    public Animator animator;
    public HealthSystem health;
    public HealthBar healthBar;
    private readonly float oneHealth = 1;

    public float holdTime;

    #region AI Abilities
    protected GameObject target;
    protected Vector2 moveDir;
    #endregion

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); 
        healthBar = GetComponentInChildren<HealthBar>();

        SetMaxHealth();
        SetHoldTime();

        healthBar.Setup(health);

        target = GameObject.FindGameObjectWithTag("Player");    // Set target to player
    }

    // Update is called once per frame
    protected void Update()
    {
        FindMoveDirections(target);

        // Die
        if (health.Current() == 0) Die();
    }

    protected void FixedUpdate()
    {
        //Move player towards player
        Move(); 
    }

    public void Die()
    {
        // Set Tag
        tag = "Corpse";

        // Stop movement, remove collisions
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = true;
        animator.enabled = false;

        // Doesnt Work (FIX Lights)
        // Set color a shade darker and grey/green scale 
        Color shade = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(shade.r - 30f, shade.g - 30f, shade.b - 30f, shade.a);
    }

    public void Resurrect()
    {
        // Restore Health
        health.Resurrect();
    }

    public abstract void FindMoveDirections(GameObject target);
    public abstract void Move();
    public abstract void SetMaxHealth();
    public abstract void SetHoldTime();

}
