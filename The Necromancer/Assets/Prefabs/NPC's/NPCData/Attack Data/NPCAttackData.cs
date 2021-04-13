using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Attack Stats")]
public class NPCAttackData : ScriptableObject
{
    public float attackRange;
    public float attackDamage;

    // projectile stuff, either sub or sibling class
    public float projectileForce;
}
