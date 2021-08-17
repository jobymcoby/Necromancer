using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Attack Stats")]
public class NPCAttackData : ScriptableObject
{
    public float innerRange;
    public float outerRange;
    public float damage;
    public float whileAttackingRangeBoost;
    public float force;
}
