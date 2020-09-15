using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeleeAttackController : MonoBehaviour
{
    public bool AttackReady;
    private NPCController npc;
    private List<NPCHealth> enemies = new List<NPCHealth>();
    private NPCHealth targetEnemy = null;


    private void Awake()
    {
        npc = GetComponentInParent<NPCController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            NPCHealth enemy = collision.gameObject.GetComponent<NPCHealth>();
            enemies.Add(enemy);
            Debug.Log(collision.gameObject.name);
            if (targetEnemy == null)
            {
                targetEnemy = enemy;
                // Send Event target Aquired

                AttackReady = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
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
}
