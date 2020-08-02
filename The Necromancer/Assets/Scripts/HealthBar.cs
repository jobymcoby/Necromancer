using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        this.transform.localScale = new Vector3(healthSystem.CurrentPercent() * 10, 1);
    }
}
