using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class NPCTargeting : DynamicTriggerListener
{
    #region Targeting
    private enum WhoToTarget { Strongest, Weakest, Closest }
    [SerializeField] private WhoToTarget targetMethod;
    [SerializeField] private bool playerPrefered;
    private List<GameObject> enemies = new List<GameObject>();
    private CircleCollider2D range;
    private float detectionRate;
    #endregion

    #region Pathfinding
    private Path path;
    private Seeker seeker;
    private int currentWaypoint = 0;
    private float nextWapointDistance = .75f;
    private float pathResetTimer = 0;
    private bool reachEndofPath = false;
    private Vector2 direction;
    #endregion

    #region Aggression Matrix
    private string enemyTag;
    #endregion

    public Transform Target;
    private float targetResetTimer;

    public Vector2 Distance
    {
        get
        {
            if (Target == null)
                return Vector2.positiveInfinity;
            else
                return (Vector2)(Target.position - transform.position);
        }
    }

    private void Awake()
    {
        detectionRate = GetComponentInParent<NPCController>().npcData.detectionRate;
        float detectionRange = GetComponentInParent<NPCController>().npcData.detectionRange;
        range = GetComponent<CircleCollider2D>();
        range.isTrigger = true;
        range.radius = detectionRange;

        seeker = GetComponent<Seeker>();

        // Choose aggressor
        // Create an Agression matrix
        if (gameObject.tag == "Enemy")
        {
            enemyTag = "Undead";
        }
        else if (gameObject.tag == "Undead")
        {
            enemyTag = "Enemy";
        }
    }

    public  void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {
        //
        GameObject temp = collision.gameObject;
        Debug.Log("This: " + transform.parent + " found: " + collision.gameObject.name);


        if (collision.gameObject.tag == enemyTag)
            enemies.Add(temp);
        if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player") enemies.Add(temp);
    }

    public override void OnDynamicTriggerExit2D(Collider2D collision)
    {
        Debug.Log("This: " + transform.parent + " lost: " + collision.gameObject.name);
        if (collision.gameObject != null)
        {
            GameObject temp = collision.gameObject;
            if (enemies.Contains(temp)) enemies.Remove(temp);
            if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player") enemies.Remove(temp);
        }
    }

    public void CreatePath()
    {
        // Create a path on a fixed interval
        if (Time.time >= pathResetTimer)
        {
            // Make sure one path is calculated at a time
            if (seeker.IsDone())
            {
                // The path is started by the delegate OnPathComplete
                seeker.StartPath((Vector2)transform.position, Target.position, OnPathComplete);
                currentWaypoint = 0;
            }
            // Reset Cooldown 
            pathResetTimer = Time.time + detectionRate;
        }
    }

    public Vector2 WalkDirection()
    {
        if (path == null)
            return Vector2.zero;
        // When you reach a point move one to the next
        // Need to handle index out of range exception
        float waypointDistance;

        try
        {
            waypointDistance = Vector2.Distance((Vector2)transform.position, path.vectorPath[currentWaypoint]);
        }
        catch (ArgumentOutOfRangeException)
        {
            currentWaypoint--;
            waypointDistance = Vector2.Distance((Vector2)transform.position, path.vectorPath[currentWaypoint]);
        }

        if (waypointDistance < nextWapointDistance)
        {
            currentWaypoint++;
        }

        // Get Direction after path creation
        try
        {
            return direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        }
        catch (ArgumentOutOfRangeException)
        {
            currentWaypoint--;
            return direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void FindTarget(float cooldown)
    {
        Transform tempTarget = null;
        if (enemies.Count == 0) Target = tempTarget;

        // Create a path on a fixed interval
        if (Time.time >= targetResetTimer)
        {
            if (targetMethod == WhoToTarget.Closest)
            {
                // Furthest target is at infinty
                float closestDistanceSqr = Mathf.Infinity;
                foreach (GameObject enemy in enemies)
                {
                    if (playerPrefered && enemy.tag == "Player")
                    {
                        tempTarget = enemy.transform;
                        break;
                    }

                    Vector2 directionToTarget = enemy.transform.position - gameObject.transform.parent.gameObject.transform.position;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        tempTarget = enemy.transform;
                    }
                }
                Target = tempTarget;
            }
            else if (targetMethod == WhoToTarget.Strongest)
            {
                // Furthest target is at infinty
                float largestHealth = 0;
                foreach (GameObject enemy in enemies)
                {
                    if (playerPrefered && enemy.tag == "Player")
                    {
                        tempTarget = enemy.transform;
                        break;
                    }

                    float enemyHealth = enemy.GetComponent<NPCHealth>().health.Current();
                    if (enemyHealth > largestHealth)
                    {
                        largestHealth = enemyHealth;
                        tempTarget = enemy.transform;
                    }
                }
                Target = tempTarget;
            }
            else             //(targetMethod == WhoToTarget.Strongest)
            {
                // Furthest target is at infinty
                float lowestHealth = Mathf.Infinity;
                foreach (GameObject enemy in enemies)
                {
                    if (playerPrefered && enemy.tag == "Player")
                    {
                        tempTarget = enemy.transform;
                        break;
                    }

                    float enemyHealth = enemy.GetComponent<NPCHealth>().health.Current();
                    if (enemyHealth < lowestHealth)
                    {
                        lowestHealth = enemyHealth;
                        tempTarget = enemy.transform;
                    }
                }
                Target = tempTarget;
            }
            targetResetTimer = Time.time + cooldown;
        }
    }
}
