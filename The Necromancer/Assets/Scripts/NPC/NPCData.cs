using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Stats")]
public class NPCData : EnemyBase
{
    public string title;
    public float moveSpeed;
    public float maxHealth;
    public float holdTime;
    public float detectionRange;
    public float detectionRate;
    public RuntimeAnimatorController animator;

    public override void SayHi()
    {
        
    }

}
