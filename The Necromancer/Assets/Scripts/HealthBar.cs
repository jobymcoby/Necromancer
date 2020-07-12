using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = GetComponentInParent<EnemyController>().maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(healthSystem.CurrentPercent() * 10, 1);
    }
}
