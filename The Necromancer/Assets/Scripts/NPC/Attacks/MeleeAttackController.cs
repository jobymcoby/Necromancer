using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeleeAttackController : DynamicTriggerListener, IAttack
{
    public float attackRange = .9f;
    private NPCController npc;
    private List<NPCHealth> enemies = new List<NPCHealth>();
    private NPCHealth targetEnemy = null;

    public delegate void DamageDealer(float dmg);
    public static event DamageDealer DealDamage;


    private void Awake()
    {
        npc = GetComponentInParent<NPCController>();
    }

    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {
        if (npc.aggressionMatrix.CheckAggression(collision.gameObject.tag))
        {
            NPCHealth enemy = collision.gameObject.GetComponent<NPCHealth>();
            enemy?.Damage(.25f);
            enemies.Add(enemy);
            if (targetEnemy == null)
            {
                targetEnemy = enemy;
                // Send Event target Aquired
            }
        }
    }

    public override void OnDynamicTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (targetEnemy == collision.gameObject.GetComponent<NPCHealth>())
            {
                if (enemies.Count == 0)
                {
                    targetEnemy = null;
                    // Send Event target de-aquired
                }
                else targetEnemy = enemies.First();
            }
        }
    }

    public float AttackRangeSqr()
    {
        // Sends Attack Range sqred to the NPC controller
        return Mathf.Pow(attackRange, 2f);
    }
}
