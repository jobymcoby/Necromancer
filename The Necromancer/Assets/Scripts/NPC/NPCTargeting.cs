using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTargeting : MonoBehaviour
{

    private List<GameObject> enemies = new List<GameObject>();
    private CircleCollider2D range;
    private float detectionRange;
    private string enemyTag;
    [SerializeField] private bool playerPrefered;
    public enum WhoToTarget { Strongest, Weakest, Closest }

    public WhoToTarget targetMethod;

    private void Awake()
    {
        detectionRange = GetComponentInParent<NPCController>().npcData.detectionRange;
        range = GetComponent<CircleCollider2D>();
        range.isTrigger = true;
        range.radius = detectionRange;

        // Choose aggressor
        // Create an Agression matrix
        if (gameObject.tag == "Enemy")
        {
            enemyTag = "Undead";
        }
        else if(gameObject.tag == "Undead")
        {
            enemyTag = "Enemy";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyTag) enemies.Add(collision.gameObject);
        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player") enemies.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyTag) enemies.Remove(collision.gameObject);
        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player") enemies.Remove(collision.gameObject);
    }

    public Transform FindTarget()
    {
        Transform target = null;
        if (enemies.Count == 0) return target;

        else if (targetMethod == WhoToTarget.Closest)
        {
            // Furthest target is at infinty
            float closestDistanceSqr = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                if (playerPrefered && enemy.tag == "Player") return enemy.transform;

                Vector2 directionToTarget = enemy.transform.position - gameObject.transform.parent.gameObject.transform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    target = enemy.transform;
                }
            }
            return target;
        }
        else if (targetMethod == WhoToTarget.Strongest)
        {
            // Furthest target is at infinty
            float largestHealth = 0;
            foreach (GameObject enemy in enemies)
            {
                if (playerPrefered && enemy.tag == "Player") return enemy.transform;

                float enemyHealth = enemy.GetComponent<NPCHealth>().health.Current();
                if (enemyHealth > largestHealth)
                {
                    largestHealth = enemyHealth;
                    target = enemy.transform;
                }
            }
            return target;
        }
        else             //(targetMethod == WhoToTarget.Strongest)
        {
            // Furthest target is at infinty
            float lowestHealth = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                if (playerPrefered && enemy.tag == "Player") return enemy.transform;

                float enemyHealth = enemy.GetComponent<NPCHealth>().health.Current();
                if (enemyHealth < lowestHealth)
                {
                    lowestHealth = enemyHealth;
                    target = enemy.transform;
                }
            }
            return target;
        }
    }
}
