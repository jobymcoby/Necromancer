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

    private AggressionMatrix aggressionMatrix;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> allies = new List<GameObject>();
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

    public Transform Target;
    private float targetResetTimer;
    private float SquareAvoidanceRadius = 4.2f;

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

    private void Start()
    {
        aggressionMatrix = GetComponentInParent<NPCController>().aggressionMatrix;
        detectionRate = GetComponentInParent<NPCController>().npcData.detectionRate;
        
        //Setup Sight trigger
        float detectionRange = GetComponentInParent<NPCController>().npcData.detectionRange;
        range = GetComponent<CircleCollider2D>();
        range.isTrigger = true;
        range.radius = detectionRange;

        seeker = GetComponent<Seeker>();
    }

    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("This: " + transform.parent + " found: " + collision.gameObject.name);
        Debug.Log(aggressionMatrix.CheckAggression(collision.gameObject.tag));
        GameObject temp = collision.gameObject;

        // if the object has a tag that make me angry add them to the enemy list
        if (aggressionMatrix.CheckAggression(collision.gameObject.tag))
            enemies.Add(temp);
        else
            allies.Add(temp);
    }

    public override void OnDynamicTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("This: " + transform.parent + " lost: " + collision.gameObject.name);

        // if is to make sure noe errors if they died and cant get removed
        if (collision.gameObject != null)
        {
            GameObject temp = collision.gameObject;
            if (enemies.Contains(temp)) enemies.Remove(temp);
            if (allies.Contains(temp)) allies.Remove(temp);
        }
    }

    public Vector2 AlignAllies()
    {
        Vector2 alignMove = Vector2.zero;

        if (allies.Count == 0)
            return alignMove;

        foreach (GameObject ally in allies)
            alignMove += ally.GetComponentInParent<NPCController>().facingDirection;

        alignMove /= allies.Count;
        return alignMove;
    }

    public Vector2 CohesionAllies(Transform agent)
    {
        Vector2 cohesionMove = Vector2.zero;

        if (allies.Count == 0)
            return cohesionMove;

        foreach (GameObject ally in allies)
            cohesionMove += (Vector2)ally.transform.position;

        cohesionMove /= allies.Count;
        cohesionMove -= (Vector2)agent.position;

        return cohesionMove;
    }

    public Vector2 AvoidAllies(Transform agent)
    {
        Vector2 avoidMove = Vector2.zero;

        if (allies.Count == 0)
            return avoidMove;

        int nAvoid = 0;

        foreach (GameObject ally in allies)
        {
            if (Vector2.SqrMagnitude(ally.transform.position - agent.position) < SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidMove += (Vector2)(agent.position - ally.transform.position);
            }
        }

        if (nAvoid > 0)
            avoidMove /= nAvoid;

        return avoidMove;
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

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public Vector2 PathDirection()
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

    public void FindTarget(float cooldown)
    {
        Transform tempTarget = null;
        if (enemies.Count == 0) Target = tempTarget;

        // Cool down loop
        if (Time.time >= targetResetTimer)
        {
            // Switch 3 way to find a taget, usually closest or player prefered
            switch (targetMethod)
            {
                case WhoToTarget.Closest:

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
                    break;

                case WhoToTarget.Strongest:

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
                    break;

                default:

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

                    break;
            }
            
            targetResetTimer = Time.time + cooldown;
        }
    }
}
