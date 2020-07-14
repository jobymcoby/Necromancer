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
    [Range(0,5)]
    public float drainRadius = 1.8f;
    private const float drainSpeed = .1f;
    private const float drainConversion = 0.60f;
    #endregion

    #region Enemies Affected
    private List<GameObject> enemies = new List<GameObject>();
    #endregion

    void Awake()
    {
        // Set each color together
        image = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer part in image)
        {
            part.color = new Color(color.r, color.g, color.b, part.color.a);
        }

        // Set radius of all objects
        transform.localScale = new Vector3(drainRadius / 1.8f, drainRadius / 1.8f, 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Damage each enemy in range
            collision.gameObject.GetComponent<EnemyController>().health.Damage(drainSpeed);
            // Heal Player in proportion to damage to enemies 
            GetComponentInParent<PlayerMovement>().health.Heal(drainSpeed * drainConversion);
        }
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
