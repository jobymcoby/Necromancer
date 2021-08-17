using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float damage;
    public GameObject shooter;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Undead")
        {
            NPCHealth enemy = collision.gameObject.GetComponentInChildren<NPCHealth>();
            enemy?.Damage(damage);
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        
    } 
}