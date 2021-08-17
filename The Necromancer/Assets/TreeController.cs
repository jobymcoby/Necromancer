using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : ControllerBase
{
    private NPCHealth npcHealth;
    [SerializeField] private float currentHealth;


    // Start is called before the first frame update
    void Awake()
    {
        this.tag = "Plant";

        npcHealth = GetComponent<NPCHealth>();
        npcHealth.Startup(npcData.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = npcHealth.health.Current();
    }
}
