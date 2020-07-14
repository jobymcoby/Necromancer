using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LifeDrain : MonoBehaviour
{
    public Color color;
    private SpriteRenderer[] image;
    [Range(0,5)]
    public float lifeDrainRadius = 1.8f;
    private float drainSpeed = .1f;
    private float drainConversion = 0.60f;
    private float lifeDrained;

    private Collider2D[] enemies;




    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer part in image)
        {
            part.color = new Color(color.r, color.g, color.b, part.color.a);
        }
        transform.localScale = new Vector3(lifeDrainRadius / 1.8f, lifeDrainRadius / 1.8f, 1);

    }

    // Update is called once per frame
    private void Update()
    {
        enemies = Physics2D.OverlapCircleAll(transform.position, lifeDrainRadius);
    }

    void FixedUpdate()
    {
        Debug.Log(enemies.Count());

        if (enemies.Count() != 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.tag == "Enemy") enemy.gameObject.GetComponent<EnemyController>().health.Damage(drainSpeed);
            }
        }
    }

    private void OnDisable()
    {
        
    }

    private void OnDrawGizmos()
    {
        // Life Drain radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lifeDrainRadius);
    }
}
