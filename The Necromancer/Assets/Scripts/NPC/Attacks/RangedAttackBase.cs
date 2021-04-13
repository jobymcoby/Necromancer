using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackBase : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public GameObject firePos;

    private NPCController npc;
    private float projectileMagnitude;
    private float attackRange;
    private float attackDamage;

    private void Start()
    {
        npc = GetComponentInParent<NPCController>();

        attackDamage = npc.npcData.attack1.attackDamage;
        projectileMagnitude = npc.npcData.attack1.projectileForce;
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

}
