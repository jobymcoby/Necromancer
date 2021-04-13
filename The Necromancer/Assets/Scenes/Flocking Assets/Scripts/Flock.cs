using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = .08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplyer = .5f;

    // makes math faster and easier
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplyer * avoidanceRadiusMultiplyer;

        // Create the Arrows in a random circle
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newagent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newagent.name = "Agent " + i;

            Color background = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            SpriteRenderer sprite = newagent.GetComponentInChildren<SpriteRenderer>();
            sprite.color = background;
            agents.Add(newagent);
        }
    }

    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            // Finds all the colliders within the neighbor radius
            List<Transform> context = GetNearbyObjects(agent);
            // Gets Movement from behavoir
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    // Finds the nearest neighbors 
    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        // For gameObjects a Transform and move direction needs to be available. 
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
