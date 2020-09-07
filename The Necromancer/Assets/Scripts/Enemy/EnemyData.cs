using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy")]
public class EnemyData : EnemyBase
{
    public string title;
    public float moveSpeed;
    public float maxHealth;
    public float holdTime;
    public float detectionRange;

    public override void SayHi()
    {
        
    }

}
