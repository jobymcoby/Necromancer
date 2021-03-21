using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Stats")]
public class NPCData : ScriptableObject
{
    public string title;
    public float maxHealth;

    // Movement
    public float moveSpeed;
    public float holdTime;

    // Enemy Detection
    public float detectionRange;
    public float detectionRate;

    // Animator
    public RuntimeAnimatorController animator;

    public NPCAttackData attack1;

}
