using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthDisplay : MonoBehaviour
{
    private PlayerController player;
    private HealthSystem health;
    private float curr_health;
    private SkullHeart skullHeart;
    private int health_steps;



    void Start()
    {
        player = PlayerManager.instance.player.GetComponent<PlayerController>();
        health = player.health;
        health.OnHealthChanged += OnHealthChanged;
        // Multiple hearts, make hearts an interface and grab all of the heart type scripts
        skullHeart = GetComponentInChildren<SkullHeart>();
        health_steps = skullHeart.total;
    }


    private void OnHealthChanged(object sender, System.EventArgs e)
    {
        skullHeart.SetHealth((int) Math.Ceiling(health.CurrentPercent() * health_steps));
    }
}
