using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour, IDamagable
{
    public HealthSystem health;
    private HealthBar healthBar;
    private NPCData npcData;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        npcData = GetComponent<NPCController>().npcData;
    }
    
    void Start()
    {
        health = new HealthSystem(npcData.maxHealth);
        healthBar.Setup(health);
    }

    // IDamagable
    public void Damage(float dmg)
    {
        health.Damage(dmg);
    }

    public void Heal(float heal)
    {
        health.Heal(heal);
    }
}
