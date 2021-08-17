using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour, IDamagable
{
    public HealthSystem health;
    public HealthBar healthBar;
    private NPCData npcData;

    public void Startup(float maxHealth)
    {
        health = new HealthSystem(maxHealth);
        
    }

    public void HealthBar()
    {
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
