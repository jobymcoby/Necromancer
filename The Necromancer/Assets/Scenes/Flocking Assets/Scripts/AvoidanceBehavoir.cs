using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavoir : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // No neighbors stay straight
        if (context.Count == 0)
            return agent.transform.up;

        // Add the neighbors transforms together and average
        Vector2 avoidMove = Vector2.zero;
        int nAvoid = 0;
       
        foreach (Transform item in context)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidMove += (Vector2)(agent.transform.position - item.position);
            }
                
        }
        if (nAvoid > 0)
        {
            avoidMove /= nAvoid;
        }

        return avoidMove;
    }
}
