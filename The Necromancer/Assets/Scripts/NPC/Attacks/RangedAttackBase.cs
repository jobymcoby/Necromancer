using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackBase : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public GameObject firePos;
    public bool attacking = true;

    private NPCController npc;
    private float projectileMagnitude;
    private float attackRange;
    private float attackDamage;

    private void Start()
    {
        npc = GetComponentInParent<NPCController>();

        attackDamage = npc.npcData.attack1.damage;
        projectileMagnitude = npc.npcData.attack1.force;
    } 

    // Called from an animation event when the character looks like they are firing
    public void FireArrow()
    {
        // Vector from shooter to target
        Vector2 projectileDirection = npc.targeter.Distance.normalized;

        // Projectile to instatiate
        GameObject arrow = Instantiate(projectilePrefab, firePos.transform.position, Quaternion.identity);

        // Give projectile stats
        ProjectileController projController = arrow.GetComponent<ProjectileController>();
        projController.damage = attackDamage;

        // Add force
        Rigidbody2D rb = arrow.GetComponentInChildren<Rigidbody2D>();
        rb.AddForce(projectileDirection * projectileMagnitude, ForceMode2D.Impulse);
    }

    public void StartAttack()
    {
        // Allow npc to switch states when attacking is false
        attacking = true;
    }

    public void Reload()
    {
        // Lock the attack state while this is true
        attacking = false;
    }
}
