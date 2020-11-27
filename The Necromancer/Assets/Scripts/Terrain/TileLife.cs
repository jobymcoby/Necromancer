using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLife : MonoBehaviour
{
    // Will get health info info from scriptable object. 
    public HealthSystem health;
    private float grass_health = 1;

    private void Awake()
    {
        health = new HealthSystem(grass_health);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
