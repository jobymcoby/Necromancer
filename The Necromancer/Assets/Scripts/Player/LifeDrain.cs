using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LifeDrain : MonoBehaviour
{
    #region Drain Image
    public Color color;
    private SpriteRenderer[] image;
    #endregion

    #region Drain Constants
    [Range(0,5)] public float drainRadius = 1.8f;
    private const float drainSpeed = .1f;
    private const float drainConversion = 0.60f;
    #endregion

    #region Enemies Affected
    private Dictionary<GameObject, EnemyController> enemies = new Dictionary<GameObject, EnemyController>();
    #endregion

    #region Player to Heal
    private PlayerController player;
    #endregion

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        // Set each color together
        image = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer part in image)
        {
            part.color = new Color(color.r, color.g, color.b, part.color.a);
        }

        // Set radius of all objects
        transform.localScale = new Vector3(drainRadius / 1.8f, drainRadius / 1.8f, 1);
    }

    private void OnEnable()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") enemies.Add(collision.gameObject, collision.gameObject.GetComponent<EnemyController>());
    }

    private void FixedUpdate()
    {
        // no enemies, do nothing
        if (enemies.Count() == 0) return;

        foreach (IDamagable enemy in enemies.Values)
        {
            enemy.Damage(drainSpeed);                           // Damage each enemy in range
            player.health.Heal(drainSpeed * drainConversion);   // Heal Player in proportion to damage to enemies  
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") enemies.Remove(collision.gameObject);
    }

    private void OnDrawGizmos()
    {
        // Life Drain radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, drainRadius);
        // Set the  color and radius values in the editor
        Awake();
    }
}
